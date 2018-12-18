public class VersionEvent
{
    public const string VERSION_UPDATE = "VERSION_UPDATE";              // 有版本更新

    public const string UNPACC_SIZE = "UNPACC_SIZE";                    // 解压资源大小
    public const string UNPACC_PROGRESS = "UNPACC_PROGRESS";            // 解压资源进度
    public const string UNPACC_COMPLETION = "UNPACC_COMPLETION";        // 解压资源完成

    public const string DOWNLOAD_SIZE = "DOWNLOAD_SIZE";                // 下载的总文件大小
	public const string DOWNLOAD_UPDATE = "DOWNLOAD_UPDATE";            // 下载的文件进度
	public const string DOWNLOAD_NUMBER = "DOWNLOAD_NUMBER";            // 下载文件的数量
    public const string DOWNLOAD_FILE_SIZE = "DOWNLOAD_FILE_SIZE";      // 下载的单个文件大小
    public const string DOWNLOAD_COMPLETION = "DOWNLOAD_COMPLETION";    // 下载完成
}