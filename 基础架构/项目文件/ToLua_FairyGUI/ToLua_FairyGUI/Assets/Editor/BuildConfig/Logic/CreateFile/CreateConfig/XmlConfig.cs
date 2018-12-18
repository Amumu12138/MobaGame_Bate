using System.Collections.Generic;

public class XmlConfig : BaseConfig
{
    protected override void HorizontalTableList(string _dir, List<CreateFileGroupLibrary> _dataList)
    {
        List<NodeAttribute> tempNodeList = new List<NodeAttribute>();
        for (int j = 0; j < _dataList.Count; j++)
        {
            // 获取单行数据
            List<CreateFileInfo> tempInfo = _dataList[j].FileInfoList;
            if (tempInfo.Count == 0)
            {
                continue;
            }

            List<ElementAttribute> tempElementList = new List<ElementAttribute>();
            for (int i = 0; i < tempInfo.Count; i++)
            {
                tempElementList.Add(new ElementAttribute(tempInfo[i].ParamName, tempInfo[i].ParamValue));
            }

            tempNodeList.Add(new NodeAttribute("NodeAttribute_" + j, tempElementList));
        }

        if (tempNodeList.Count == 0)
        {
            return;
        }

        WriteXmlFile tempXml = new WriteXmlFile(_dir + ".xml");
        for (int i = 0; i < tempNodeList.Count; i++)
        {
            tempXml.WriteXmlAttribute(tempNodeList[i]);
        }
        tempXml.Save();
    }
}