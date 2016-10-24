namespace download_progress
{
    public interface IDownload
    {
        void DownloadFile();

        long GetDownloadSize();

        string GetFilename();
    }
}
