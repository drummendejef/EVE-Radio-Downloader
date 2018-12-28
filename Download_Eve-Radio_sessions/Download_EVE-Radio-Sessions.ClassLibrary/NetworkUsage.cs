using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using SharpPcap;


namespace Download_EVE_Radio_Sessions.ClassLibrary
{
    public class ProcessPerformanceInfo
    {
        public int ProcessID { get; set; }
        public long NetSendBytes { get; set; }
        public long NetRecvBytes { get; set; }
        public long NetTotalBytes { get; set; }

    }
    public class NetworkUsage
    {
        static ProcessPerformanceInfo _PROC_INFO;
        //static void Main(string[] args)
        public void Main()
        {
            _PROC_INFO = new ProcessPerformanceInfo()
            {
                ProcessID = 3684
            };

            int pid = _PROC_INFO.ProcessID;
            List<int> ports = new List<int>();

            #region get ports for certain process through executing the netstat -ano command  in cmd
            Process pro = new Process();
            pro.StartInfo.FileName = "cmd.exe";
            pro.StartInfo.UseShellExecute = false;
            pro.StartInfo.RedirectStandardInput = true;
            pro.StartInfo.RedirectStandardOutput = true;
            pro.StartInfo.RedirectStandardError = true;
            pro.StartInfo.CreateNoWindow = true;
            pro.Start();
            pro.StandardInput.WriteLine("netstat -ano");
            pro.StandardInput.WriteLine("exit");
            Regex reg = new Regex("\\s+", RegexOptions.Compiled);
            string line = null;
            ports.Clear();
            while((line = pro.StandardOutput.ReadLine()) != null)
            {
                line = line.Trim();
                if(line.StartsWith("TCP", StringComparison.OrdinalIgnoreCase))
                {
                    line = reg.Replace(line, ",");
                    string[] arr = line.Split(',');
                    if(arr[4] == pid.ToString())
                    {
                        string soc = arr[1];
                        int pos = soc.LastIndexOf(':');
                        int pot = int.Parse(soc.Substring(pos + 1));
                        ports.Add(pot);
                    }
                }
                else if(line.StartsWith("UDP", StringComparison.OrdinalIgnoreCase))
                {
                    line = reg.Replace(line, ",");
                    string[] arr = line.Split(',');
                    if(arr[3] == pid.ToString())
                    {
                        string soc = arr[1];
                        int pos = soc.LastIndexOf(':');
                        int pot = int.Parse(soc.Substring(pos + 1));
                        ports.Add(pot);
                    }
                }
            }
            pro.Close();
            #endregion

            //get ip address
            IPAddress[] addrList = Dns.GetHostByName(Dns.GetHostName()).AddressList;
            string IP = addrList[0].ToString();


            //var devices = SharpPcap.WinPcap.WinPcapDeviceList.Instance;
            var devices = CaptureDeviceList.Instance;

            // differentiate based upon types

            int count = devices.Count;
            if(count < 1)
            {
                Console.WriteLine("No device found on this machine");
                return;
            }

            for(int i = 0; i < count; ++i)
            {
                for(int j = 0; j < ports.Count; ++j)
                {
                    CaptureFlowRecv(IP, ports[j], i);
                    CaptureFlowSend(IP, ports[j], i);
                }
            }
            while(true)
            {
                Console.WriteLine("proc NetTotalBytes : " + _PROC_INFO.NetTotalBytes);
                Console.WriteLine("proc NetSendBytes : " + _PROC_INFO.NetSendBytes);
                Console.WriteLine("proc NetRecvBytes : " + _PROC_INFO.NetRecvBytes);

                //Call refresh function every 1s 
                RefershInfo();
            }

        }

        private static void CaptureFlowSend(string IP, int portID, int deviceID)
        {
            ICaptureDevice device = CaptureDeviceList.New()[deviceID];

            device.OnPacketArrival += new PacketArrivalEventHandler(Device_OnPacketArrivalSend);
            int readTimeoutMilliseconds = 1000;
            device.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);
            string filter = "src host " + IP + " and src port " + portID;
            device.Filter = filter;
            device.StartCapture();

        }

        private static void Device_OnPacketArrivalSend(object sender, CaptureEventArgs e)
        {
            //DateTime time = e.Packet.Timeval.Date;
            //int len = e.Packet.Data.Length;
            //Console.WriteLine("{0}:{1}:{2},{3} Len={4}",
            //    time.Hour, time.Minute, time.Second, time.Millisecond, len);
            var len = e.Packet.Data.Length;
            _PROC_INFO.NetSendBytes += len;

        }

        private static void CaptureFlowRecv(string IP, int portID, int deviceID)
        {
            ICaptureDevice device = CaptureDeviceList.New()[deviceID];
            device.OnPacketArrival += new PacketArrivalEventHandler(Device_OnPacketArrivalRecv);

            int readTimeoutMilliseconds = 1000;
            device.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);

            string filter = "dst host " + IP + " and dst port " + portID;
            device.Filter = filter;
            device.StartCapture();

        }
        private static void Device_OnPacketArrivalRecv(object sender, CaptureEventArgs e)
        {
            var len = e.Packet.Data.Length;
            _PROC_INFO.NetRecvBytes += len;
        }
        public static void RefershInfo()
        {
            _PROC_INFO.NetRecvBytes = 0;
            _PROC_INFO.NetSendBytes = 0;
            _PROC_INFO.NetTotalBytes = 0;
            Thread.Sleep(1000);
            _PROC_INFO.NetTotalBytes = _PROC_INFO.NetRecvBytes + _PROC_INFO.NetSendBytes;
        }
    }
}
