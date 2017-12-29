using Xamarin.Forms;
using System;
using System.Threading.Tasks;
namespace download_progress
{
    public class DownloadProgress : ContentPage
    {
        ProgressBar progress;
        Label lblProg;
        StackLayout progressStack;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.Self.PropertyChanged += (s, e) => PropertyChange(s, e);
        }

        public DownloadProgress()
        {
            CreateUI();
        }

        void CreateUI()
        {
            progress = new ProgressBar
            {
                WidthRequest = 200,
                Progress = 0
            };

            lblProg = new Label
            {
                Text = "Downloading : ",
                TextColor = Color.Blue,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = 10
            };

            progressStack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children = {lblProg, progress}
                    }
                }
            };

            if (Device.RuntimePlatform == Device.Android)
                progressStack.Padding = new Thickness(0, 4);

            Content = progressStack;

            DependencyService.Get<IDownload>().DownloadFile();
        }

        void PropertyChange(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if (e.PropertyName == "DownloadProgress")
            {
                var downloaded = (double)App.Self.DownloadProgress / (double)App.Self.DownloadTotal;
                progress.Progress = downloaded;
                if (!App.Self.DownloadCompleted)
                    Device.BeginInvokeOnMainThread(() => lblProg.Text = string.Format("Downloading {0}%", (int)(downloaded * 100)));
                if (App.Self.DownloadTotal == App.Self.DownloadProgress)
                    App.Self.DownloadCompleted = true;
            }
            if (e.PropertyName == "DownloadCompleted")
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    lblProg.Text = "Download completed";
                    var image = new Image
                    {
                        WidthRequest = 200,
                        HeightRequest = 250,
                        Source = DependencyService.Get<IDownload>().GetFilename()
                    };
                    progressStack.Children.Add(image);
                });
            }
        }
    }
}
