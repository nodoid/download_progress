using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace download_progress
{
    public class App : Application, INotifyPropertyChanged
    {
        public static App Self { get; private set; }

        public string Url { get; private set; } = "https://s-media-cache-ak0.pinimg.com/originals/7e/a0/30/7ea0300be3d56a04bc3d00fccdbaf5d8.jpg";

        public event PropertyChangedEventHandler PropertyChanged;

        int downloadProgress;

        bool downloadCompleted;

        public long DownloadTotal { get; set; } = 1;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public int DownloadProgress
        {
            get { return downloadProgress; }
            set
            {
                downloadProgress = value;
                OnPropertyChanged("DownloadProgress");
            }
        }

        public bool DownloadCompleted
        {
            get
            {
                return downloadCompleted;
            }
            set
            {
                downloadCompleted = value;
                OnPropertyChanged("DownloadCompleted");
            }
        }

        public App()
        {
            App.Self = this;

            MainPage = new DownloadProgress();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
