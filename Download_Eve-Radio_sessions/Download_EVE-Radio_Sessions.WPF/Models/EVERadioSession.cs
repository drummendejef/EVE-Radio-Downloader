using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Download_EVE_Radio_Sessions.WPF.Models
{
    public class EVERadioSession : ViewModelBase
    {
        public string FileName { get; set; }

        /// <summary>
        /// Download Progress, how much % of the file has already been downloaded
        /// </summary>
        private int _progress;
        public int Progress
        {
            get { return _progress; }
            set { _progress = value; RaisePropertyChanged("Progress"); }
        }

        /// <summary>
        /// The Location of the file online
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// The backgroundcolor of the item in the list
        /// </summary>
        private string _achtergrondkleur = "white";
        public string Achtergrondkleur
        {
            get { return _achtergrondkleur; }
            set { _achtergrondkleur = value; RaisePropertyChanged("Achtergrondkleur"); }
        }

        /// <summary>
        /// Is Item selected to download using "download selected files only"
        /// </summary>
        private bool _isselected;
        public bool IsSelected
        {
            get { return _isselected; }
            set { _isselected = value; RaisePropertyChanged("IsSelected"); }
        }


        /// <summary>
        /// Keep the stopwatch per session
        /// </summary>
        private Stopwatch _stopwatch;
        public Stopwatch StopWatch
        {
            get { return _stopwatch; }
            set { _stopwatch = value; RaisePropertyChanged("StopWatch"); }
        }
        /// <summary>
        /// Keep the downloadspeed per downloadsession
        /// </summary>
        private double _downloadspeed;
        public double DownloadSpeed
        {
            get { return _downloadspeed; }
            set { _downloadspeed = value; RaisePropertyChanged("DownloadSpeed"); }
        }

       
        //Extra information for on the tooltip.
        private long _filesize;
        public long FileSize 
        {
            get { return _filesize; }
            set { _filesize = value; RaisePropertyChanged("FileSize"); }
        }
        private DateTime _timestarted;
        public DateTime TimeStarted
        {
            get { return _timestarted; }
            set { _timestarted = value; RaisePropertyChanged("TimeStarted"); }
        }

        /// <summary>
        /// Keep status of file. Is it still downloading? (disable download all button/make sure it can't be downloaded again when already downloading)
        /// </summary>
        private bool _isdownloading = false;
        public bool IsDownloading
        {
            get { return _isdownloading; }
            set { _isdownloading = value; RaisePropertyChanged("IsDownloading"); }
        }
    }
}
