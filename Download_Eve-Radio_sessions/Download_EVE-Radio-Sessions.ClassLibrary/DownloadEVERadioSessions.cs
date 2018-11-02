using Download_EVE_Radio_Sessions.ClassLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Download_EVE_Radio_Sessions.ClassLibrary
{
    public class DownloadEVERadioSessions
    {
        public GetProxyModel GetRandomProxy()
        {
            //Opvragen willekeurige proxy
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.getproxylist.com/proxy");

            try
            {
                WebResponse response = request.GetResponse();
                using(Stream responseStream = response.GetResponseStream())
                {
                    //Lezen stream en omzetten naar 
                    StreamReader reader = new StreamReader(responseStream);
                    string readresult = reader.ReadToEnd();

                    GetProxyModel result = JsonConvert.DeserializeObject<GetProxyModel>(readresult);

                    return result;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Foutboodschap: " + ex.Message);
            }

            return null;
        }
    }
}
