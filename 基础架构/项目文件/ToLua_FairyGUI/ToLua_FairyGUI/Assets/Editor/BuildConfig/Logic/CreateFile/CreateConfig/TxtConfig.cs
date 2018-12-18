using System.IO;
using System.Collections.Generic;

public class TxtConfig : BaseConfig
{
    protected override void HorizontalTableList(string _dir, List<CreateFileGroupLibrary> _dataList)
    {
        string tempValue = "";
        for (int j = 0; j < _dataList.Count; j++)
        {
            // 获取单行数据
            List<CreateFileInfo> tempInfo = _dataList[j].FileInfoList;
            if (tempInfo.Count == 0)
            {
                continue;
            }

            string temp = tempInfo[0].ParamValue;
            for (int i = 1; i < tempInfo.Count; i++)
            {
                temp += ("#" + tempInfo[i].ParamValue);
            }

            tempValue += (temp + "\n");
        }

        File.WriteAllText(_dir + ".txt", tempValue);
    }
}