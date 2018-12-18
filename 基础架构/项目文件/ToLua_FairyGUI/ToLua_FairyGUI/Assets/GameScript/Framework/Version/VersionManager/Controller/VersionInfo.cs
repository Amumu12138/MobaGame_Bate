public class VersionInfo
{
    int size;
    int versionNum;
    string versionName;
    string versionPath;

    public VersionInfo(string _size, string _versionNum, string _versionPath, string _versionName)
    {
        size = ToNumber(_size);
        versionNum = ToNumber(_versionNum);
        versionPath = _versionPath;
        versionName = _versionName;
    }

    public int Size { get { return size; } }
    public int VersionNum { get { return versionNum; } }
    public string VersionPath { get { return versionPath; } }
    public string VersionName { get { return versionName; } }

    int ToNumber(string _str)
    {
        int tempStr = 0;
        int.TryParse(_str, out tempStr);
        return tempStr;
    }
}