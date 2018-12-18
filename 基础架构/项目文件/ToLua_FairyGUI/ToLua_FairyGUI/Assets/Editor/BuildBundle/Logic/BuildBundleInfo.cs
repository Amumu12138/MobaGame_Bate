public class BuildBundleInfo
{
    string path = "";
    string prefix = "";
    string[] suffixArray;

    public BuildBundleInfo(string _path, string _prefix, string[] _suffixArray)
    {
        path = _path;
        prefix = _prefix;
        suffixArray = _suffixArray; 
    }

    /// <summary>
    /// 资源路径
    /// </summary>
    public string Path { get { return path; } }
    /// <summary>
    /// 资源前缀
    /// </summary>
    public string Prefix { get { return prefix; } }
    /// <summary>
    /// 资源后缀
    /// </summary>
    public string[] SuffixArray { get { return suffixArray; } }
}