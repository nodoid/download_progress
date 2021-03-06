﻿using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using download_progress.iOS;
using Foundation;
using Xamarin.Forms;

[assembly: Dependency(typeof(DownloadProgress))]
namespace download_progress.iOS
{
    public class DownloadProgress : IDownload
    {
        public string ContentPath
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            }
        }

        public void DownloadFile()
        {
            ThreadPool.QueueUserWorkItem((object state) =>
                {

                    var fullFilename = Path.Combine(ContentPath, App.Self.Url.Split('/').Last());

                    if (File.Exists(fullFilename))
                        File.Delete(fullFilename);

                    if (!File.Exists(fullFilename))
                    {
                        var wc = new WebClient();
                        wc.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                        wc.DownloadFileAsync(new Uri(App.Self.Url), fullFilename);
                    }
                    else
                    {
                        using (var pool = new NSAutoreleasePool())
                            pool.InvokeOnMainThread(() => App.Self.DownloadProgress++);
                    }
                });
        }

        void Completed(object sender, AsyncCompletedEventArgs e)
        {
            using (var pool = new NSAutoreleasePool())
                pool.InvokeOnMainThread(() => App.Self.DownloadProgress++);
        }

        public long GetDownloadSize()
        {
            var client = new WebClient();
            client.OpenRead(App.Self.Url);
            return Convert.ToInt64(client.ResponseHeaders["Content-Length"]);
        }

        public string GetFilename()
        {
            return Path.Combine(ContentPath, App.Self.Url.Split('/').Last());
        }
    }
}
