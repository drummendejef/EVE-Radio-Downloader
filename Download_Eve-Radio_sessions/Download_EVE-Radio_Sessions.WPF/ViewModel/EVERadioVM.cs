using Download_EVE_Radio_Sessions.WPF.Models;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Media;
using System.Runtime.Remoting.Contexts;
using Download_EVE_Radio_Sessions.ClassLibrary;
using Download_EVE_Radio_Sessions.ClassLibrary.Models;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using System.Windows;
using MessageBox = System.Windows.MessageBox;

namespace Download_EVE_Radio_Sessions.WPF.ViewModel
{
    public class EVERadioVM : MainViewModel
    {
        #region Properties
        //List of all the found EVE Radio sessions
        private List<EVERadioSession> _everadiosessions;
        public List<EVERadioSession> EVERadioSessions
        {
            get { return _everadiosessions; }
            set { _everadiosessions = value; RaisePropertyChanged("EVERadioSessions"); }
        }

        //Do we want to use the proxy in case of a firewall somewhere?
        private bool _useproxy;
        public bool UseProxy
        {
            get { return _useproxy; }
            set { _useproxy = value; RaisePropertyChanged("UseProxy"); }
        }

        //Feedback and color
        private ObservableCollection<string> _feedbacklist = new ObservableCollection<string>();
        public ObservableCollection<string> FeedbackList
        {
            get { return _feedbacklist; }
            set { _feedbacklist = value; RaisePropertyChanged("FeedbackList"); }
        }

        private string _feedbackcolor;
        public string FeedbackColor
        {
            get { return _feedbackcolor; }
            set { _feedbackcolor = value; RaisePropertyChanged("FeedbackColor"); }
        }

        //Location of where the downloads should go to
        private string _downloadfolder = "C:\\Users\\Admin\\Music\\";
        public string DownloadFolder
        {
            get { return _downloadfolder; }
            set { _downloadfolder = value; RaisePropertyChanged("DownloadFolder"); }
        }

        //List of all downloadspeeds
        private Dictionary<int, int> _downloadingspeeds;
        public Dictionary<int, int> DownloadingSpeeds
        {
            get { return _downloadingspeeds; }
            set { _downloadingspeeds = value; RaisePropertyChanged("DownloadingSpeeds"); }
        }



        #endregion

        #region Commands
        //Button "Download All" in main screen
        public ICommand DownloadAllCommand
        {
            get { return new RelayCommand(DownloadAll); }
        }

        //Button "Download Selected" in main screen
        public ICommand DownloadSelectedCommand
        {
            get { return new RelayCommand(DownloadSelected); }
        }

        //Button "Choose Download Folder" in main screen
        public ICommand ChooseDownloadFolderCommand
        {
            get { return new RelayCommand(ChooseDownloadFolder); }
        }

        //Button "Open Download Folder" in main screen
        public ICommand OpenDownloadFolderCommand
        {
            get { return new RelayCommand(OpenDownloadFolder); }
        }

        //Button "Open Filezilla" in main screen
        public ICommand OpenFilleZillaCommand
        {
            get { return new RelayCommand(OpenFileZilla); }
        }

        //Opvangen Window sluit event
        public ICommand WindowClosingCommand
        {
            get { return new RelayCommand<CancelEventArgs>(WindowClosing); }
        }

        #endregion

        #region Methods
        public EVERadioVM()
        {
            EVERadioSessions = new List<EVERadioSession>();

            //Alle sessies ophalen en tonen
            GetAllSessions();

            //Feedback invullen
            FeedbackList.Add("Hello I'm feedback. Please press 'Download All' to start");
            FeedbackColor = "White";

        }

        //Download all the sessions
        private void DownloadAll()
        {
            try
            {
                //Give feedback to user
                FeedbackList.Add("We started downloading ALL THE SONGS");
                FeedbackColor = "Green";


                //We overlopen alle EVE Radio sessies die we willen downloaden
                foreach(EVERadioSession sessie in EVERadioSessions)
                {
                    HandleSession(sessie);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Foutboodschap: " + ex.Message);
                FeedbackColor = "Red";
                FeedbackList.Add("DownloadAll failed: " + ex.Message);
            }
        }

        /// <summary>
        /// Controles of het een volledige radiosessie is
        /// </summary>
        /// <param name="localpath"></param>
        /// <returns></returns>
        private bool IsFullSession(string localpath)
        {
            try
            {
                //Lengte van setje opvragen.
                ulong ticks = 0;
                using(ShellObject shell = ShellObject.FromParsingName(localpath))
                {
                    IShellProperty prop = shell.Properties.System.Media.Duration;
                    ticks = (ulong)prop.ValueAsObject;
                }

                //Kijken of het een volledige sessie is
                if(ticks > 108000000000)//3 uur lengte
                {
                    return true;//Het is een volledige sessie.
                }

                return false;//De sessie is nog niet compleet

            }
            catch(Exception ex)
            {
                Console.WriteLine("GetLengthUsingShellObject failed: " + ex.Message);

                return false;//Er is iets misgelopen, de sessie is waarschijnlijk niet compleet
            }
        }

        //Download de geselecteerde sessies
        private void DownloadSelected()
        {
            try
            {
                //Sessies ophalen die geselecteerd zijn
                foreach(EVERadioSession sessie in EVERadioSessions.Where(item => item.IsSelected))
                {
                    HandleSession(sessie);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Foutboodschap: " + ex.Message);
                FeedbackColor = "Red";
                FeedbackList.Add("DownloadSelected failed: " + ex.Message);
            }
        }

        //Start downloaden van een bestand
        private async Task DownloadFileAsync(string url, string naam)
        {
            try
            {
                using(WebClient wc = new WebClient())
                {
                    wc.DownloadProgressChanged += Client_DownLoadProcessChanged;//Om tussentijdse feedback te kunnen geven.
                    wc.DownloadFileCompleted += Client_DownloadProcessComplete;//Om feedback te kunnen geven bij voltooing

                    //Make instance off class library
                    DownloadEVERadioSessions downloadEVERadioSessionsClassLib = new DownloadEVERadioSessions();

                    if(UseProxy)//Use a proxy if the user wants to
                    {
                        GetProxyModel proxyinfo = downloadEVERadioSessionsClassLib.GetRandomProxy();//Get a random proxy

                        wc.Proxy = new WebProxy(proxyinfo.Ip, proxyinfo.Port);//Proxy instellen
                    }

                    //timer per download starten
                    EVERadioSession evers = EVERadioSessions.Find(f => f.FileName == naam.Split('\\').Last());
                    evers.StopWatch.Start();

                    //Extra informatie per download bijhouden.
                    evers.TimeStarted = DateTime.Now;

                    await wc.DownloadFileTaskAsync(new Uri(url), naam);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);

                if(ex.Message == "Unable to connect to the remote server")
                    FeedbackList.Add("Error, try again with Proxy enabled");
                else
                {

                    FeedbackList.Add("Bestand: " + naam.Split('\\').Last() + " - Something went wrong, maybe this helps: " + ex.Message);
                }

                FeedbackColor = "Red";

            }
        }

        //When the download completed
        private void Client_DownloadProcessComplete(object sender, AsyncCompletedEventArgs e)
        {
            //De meegezonden sessie ophalen
            string sessiontolookfor = ((Uri)((TaskCompletionSource<object>)e.UserState).Task.AsyncState).Segments[4].ToString();

            //Opzoeken van de bestandsnaam in de sessielijst
            EVERadioSession evers = EVERadioSessions.Find(f => f.FileName == sessiontolookfor);

            //Resetten van downloadsnelheid stopwatch
            evers.StopWatch.Reset();

            //Klaar met downloaden
            evers.IsDownloading = false;

            //Als we iets gevonden hebben vullen we een percentage in.
            if(evers != null)
            {
                evers.Progress = 100;
                evers.Achtergrondkleur = "GreenYellow";
            }

        }

        //When the download is progressing.
        private void Client_DownLoadProcessChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //De meegezonden sessie ophalen
            string sessiontolookfor = ((Uri)((TaskCompletionSource<object>)e.UserState).Task.AsyncState).Segments[4].ToString();

            //Opzoeken van de bestandsnaam in de sessielijst
            EVERadioSession evers = EVERadioSessions.Find(f => f.FileName == sessiontolookfor);

            //TODO: Stopwatch per sessie ophalen en downloadsnelheid berekenen.
            //Console.WriteLine("Downloadsnelheid " + evers.FileName + " = " + (e.BytesReceived / 1024d / evers.StopWatch.Elapsed.TotalSeconds) + "kb/s");
            evers.DownloadSpeed = e.BytesReceived / 1024d / evers.StopWatch.Elapsed.TotalSeconds;
            evers.FileSize = e.TotalBytesToReceive / 1000000;


            //Als we iets gevonden hebben vullen we een percentage in.
            if(evers != null)
                evers.Progress = e.ProgressPercentage;
        }

        //Get all the sessions and show them in the overviewlist
        private void GetAllSessions()
        {
            try
            {
                using(WebClient client = new WebClient())
                {
                    //We downloaden de EVE-Radio pagina
                    string htmlCode = client.DownloadString("http://eve-radio.com/radio/rewind");

                    //We splitsen de herbeluister divs op basis van hun ID
                    string[] stringSeperators = new string[] { "<div id='erRW' style='float: left;'>" };
                    string[] rewinds = htmlCode.Split(stringSeperators, StringSplitOptions.None).Skip(1).ToArray();//Het eerste dat we eruit halen is rommel.

                    //Nu gaan we voor elk van de gevonden rewinds de starturl opzoeken
                    foreach(string rewind in rewinds)
                    {
                        //We vinden de startpositie van de tekst die we willen, de eindpositie, en halen daar de lengte uit.
                        int startPos = rewind.IndexOf("Listen from: <a href='#' onclick=\"javascript:doCmd({rewind:'") + "Listen from: <a href='#' onclick=\"javascript:doCmd({rewind:'".Length;
                        int length = rewind.IndexOf("'}); return false;\">Start") - startPos;

                        string downloadUrl = rewind.Substring(startPos, length);
                        string bestandsnaam = downloadUrl.Split('/').Last();

                        //Starten met downloadsnelheidsberekening
                        Stopwatch sw = new Stopwatch();

                        EVERadioSessions.Add(new EVERadioSession() { FilePath = downloadUrl, FileName = bestandsnaam, StopWatch = sw, IsDownloading = false });
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                FeedbackColor = "Red";
                FeedbackList.Add("Failed getting all sessions. Maybe this helps: " + ex.Message);
            }
        }

        /// <summary>
        /// Checks if a session is downloading
        /// Starts downloading session if not already downloading
        /// </summary>
        /// <param name="sessie"></param>
        private void HandleSession(EVERadioSession sessie)
        {
            string localpath = _downloadfolder + sessie.FileName;

            if(sessie.IsDownloading)//Als het bestand aan het downloaden is, gaan we het niet onderbreken
            {
                FeedbackList.Add("Song " + sessie.FileName + " is already downloading.");
            }
            else
            {
                if(!IsFullSession(localpath))//Als het een volledig bestand is moeten we het niet opnieuw downloaden
                {
                    sessie.IsDownloading = true;
                    sessie.Achtergrondkleur = "Orange";
                    DownloadFileAsync(sessie.FilePath, localpath);//Aanzetten om bestanden te downloaden
                }
                else
                {
                    sessie.IsDownloading = false;
                    sessie.Achtergrondkleur = "GreenYellow";
                    sessie.Progress = 100;
                }
            }
        }

        //Open and choose a folder where the downloads will go to
        private void ChooseDownloadFolder()
        {
            try
            {
                using(FolderBrowserDialog fbd = new FolderBrowserDialog())
                {
                    //Venster openen
                    DialogResult result = fbd.ShowDialog();

                    //Nakijken of dat venster succesvol is geopend en pad is gekozen
                    if(result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        //Pad opslaan
                        DownloadFolder = fbd.SelectedPath;

                        Console.WriteLine("Downloadpad veranderd naar: " + fbd.SelectedPath);
                        FeedbackColor = "White";
                        FeedbackList.Add("Downloadpad veranderd naar " + fbd.SelectedPath);
                    }
                }
            }
            catch(Exception ex)
            {
                FeedbackColor = "Red";
                FeedbackList.Add("Failed to open folder: " + ex.Message);
            }
        }

        //Open the download folder in windows explorer
        private void OpenDownloadFolder()
        {
            //Folder openen
            Process.Start(DownloadFolder);
        }

        //Open Filezilla
        private void OpenFileZilla()
        {
            try
            {
                //Gaat in regedit zoeken naar het pad aan de hand van onderstaande waarde 
                //(onderstaande waarde heb ik zelf in regedit opgezocht om dit te weten voor filezilla)
                //Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\filezilla.exe
                Process pcss = new Process();

                pcss.StartInfo.Arguments = "--local=" + _downloadfolder + " --site=0/Thuis";
                pcss.StartInfo.FileName = "Filezilla.exe";
                pcss.Start();
                //Process.Start("Filezilla.exe");
            }
            catch(Exception ex)
            {
                FeedbackColor = "Red";
                FeedbackList.Add("Filezilla niet geopend: " + ex.Message);
            }
        }

        //If window is closing, check if downloads would be stopped and warn
        private void WindowClosing(CancelEventArgs e)
        {
            //Check if there is a file still beeing downloaded, if so, show warning message
            if(EVERadioSessions.Any(s => s.IsDownloading))
            {
                MessageBoxResult result = MessageBox.Show("Download(s) still busy, sure you want to stop?", "Warning", MessageBoxButton.YesNo);

                if(result != MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }

        #endregion

    }
}
