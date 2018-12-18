using System.Collections.Generic;

public class BuildBundleLibrary
{
    List<BuildBundleInfo> buildBundleInfoList = new List<BuildBundleInfo>();

    public void AddBundleInfo(string _path, string _prefix, params string[] _suffixArray)
    {
        buildBundleInfoList.Add(new BuildBundleInfo(_path, _prefix, _suffixArray));
    }

    public List<BuildBundleInfo> GetBundleInfoList()
    {
        return buildBundleInfoList;
    }
}