using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;

/*
 * 作者：陈建滔
 * 日期：2014.6.24
 * 功能：写入 XML 配置文件
 */

public class WriteXmlFile
{
    #region ///// 私有变量 /////
    /// <summary>
    /// 数据加密
    /// </summary>
    XmlCryptography xmlCryptography = new XmlCryptography();
    /// <summary>
    /// XML 文档实例
    /// </summary>
    XmlDocument xmlDocument = new XmlDocument();
    /// <summary>
    /// 根节点
    /// </summary>
    XmlNode rootNode;
    /// <summary>
    /// 文件路径
    /// </summary>
    string mFilePath = "";
    /// <summary>
    /// 节点属性
    /// </summary>
    List<NodeAttribute> nodeAttributeList = new List<NodeAttribute>();
    #endregion

    #region ///// 构造函数 /////
    /// <summary>
    /// 实例化 WriteXmlFile
    /// </summary>
    /// <param name="filePath">要读取文件的路径</param>
    public WriteXmlFile(string filePath)
    {
        if (null == xmlDocument)
        {
            throw new ArgumentNullException("xmlDocument");
        }

        if (File.Exists(filePath))
        {
            // 文件存在，加载 XML 数据
            xmlDocument.Load(filePath);
            // 获取原有的数据
            nodeAttributeList = ReadAllNode();
        }

        mFilePath = filePath;
    }
    #endregion

    #region ////// 字符串 //////
    /// <summary>
    /// 写入属性
    /// </summary>
    public void WriteXmlAttribute(NodeAttribute nodeAttribute)
    {
        XmlIsNamingConventions(nodeAttribute.NodeKey);

        if (null == nodeAttributeList)
        {
            throw new ArgumentNullException("nodeAttributeList");
        }

        for (int i = 0; i < nodeAttributeList.Count; i++)
        {
            if (nodeAttributeList[i].NodeKey == nodeAttribute.NodeKey)
            {
                nodeAttributeList[i] = nodeAttribute;
                return;
            }
        }

        nodeAttributeList.Add(nodeAttribute);
    }
    #endregion

    #region ///// 移除数据 /////
    /// <summary>
    /// 移除指定的子节点
    /// </summary>
    public void RemoveChild(string nodeName)
    {
        if (null == nodeAttributeList)
        {
            throw new ArgumentNullException("nodeAttributeList");
        }

        for (int i = 0; i < nodeAttributeList.Count; i++)
        {
            if (nodeAttributeList[i].NodeKey == nodeName)
            {
                nodeAttributeList.RemoveAt(i);
                return;
            }
        }
    }
    /// <summary>
    /// 移除指定的子节点
    /// </summary>
    public void RemoveChild(string nodeName, string elementName)
    {
        if (null == nodeAttributeList)
        {
            throw new ArgumentNullException("nodeAttributeList");
        }

        for (int i = 0; i < nodeAttributeList.Count; i++)
        {
            if (nodeAttributeList[i].NodeKey == nodeName)
            {
                nodeAttributeList[i].RemoveChild(elementName);
            }
        }
    }
    #endregion

    #region ///// 创建声明 /////
    /// <summary>
    /// 创建声明
    /// </summary>
    void CreateXmlDeclaration()
    {
        // 这里一定要先清除数据，不然数据会重复写入
        xmlDocument.RemoveAll();

        // 创建一个声明
        XmlDeclaration dec = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);

        // 把声明添加到子节点下面去
        xmlDocument.AppendChild(dec);

        // 创建一个名字为：root的元素
        XmlElement createRoot = xmlDocument.CreateElement("root");

        // 把元素添加到子节点下面去
        xmlDocument.AppendChild(createRoot);

        rootNode = xmlDocument.DocumentElement;
    }
    #endregion

    #region ///// 数据保存 /////
    /// <summary>
    /// 保存 XML 数据
    /// 切记：这个函数不调用的话，数据是不会保存的
    /// </summary>
    public void Save()
    {
        if (null == nodeAttributeList)
        {
            throw new ArgumentNullException("nodeAttributeList");
        }

        if (null == xmlDocument)
        {
            throw new ArgumentNullException("xmlDocument");
        }

        if (null == mFilePath || mFilePath.Length == 0)
        {
            throw new ArgumentNullException("路径名为空");
        }

        // 创建声明
        CreateXmlDeclaration();

        XmlElement rootElement = rootNode as XmlElement;

        for (int i = 0; i < nodeAttributeList.Count; i++)
        {
            XmlElement keyElement = xmlDocument.CreateElement(nodeAttributeList[i].NodeKey);
            rootElement.AppendChild(keyElement);

            for (int j = 0; j < nodeAttributeList[i].NodeValue.Count; j++)
            {
                XmlElement valueElement = xmlDocument.CreateElement(nodeAttributeList[i].NodeValue[j].ElementKey);
                keyElement.AppendChild(valueElement);
                valueElement.InnerText = nodeAttributeList[i].NodeValue[j].ElementValue;
            }
        }

        // 保存数据到指定的路径
        xmlDocument.Save(mFilePath);
    }
    #endregion

    #region /// 读取全部数据 ///
    /// <summary>
    /// 返回全部数据
    /// </summary>
    List<NodeAttribute> ReadAllNode()
    {
        rootNode = xmlDocument.DocumentElement;
        List<NodeAttribute> tempNodeAttribute = new List<NodeAttribute>();
        List<ElementAttribute> elementAttribute = new List<ElementAttribute>();

        for (int i = 0; i < rootNode.ChildNodes.Count; i++)
        {
            elementAttribute.Clear();

            XmlNode node = rootNode.ChildNodes[i];

            for (int j = 0; j < node.ChildNodes.Count; j++)
            {
                XmlNode element = node.ChildNodes[j];

                elementAttribute.Add(new ElementAttribute(element.Name, element.InnerText));
            }

            tempNodeAttribute.Add(new NodeAttribute(node.Name, elementAttribute));
        }

        return tempNodeAttribute;
    }
    #endregion

    #region ///// 数据加密 /////
    /// <summary>
    /// 数据加密
    /// </summary>
    public void Encrypt()
    {
        if (xmlCryptography == null)
        {
            throw new ArgumentNullException("xmlCryptography");
        }

        if (xmlDocument == null)
        {
            throw new ArgumentNullException("xmlDocument");
        }

        xmlCryptography.Encrypt(xmlDocument, "root");
    }
    #endregion

    #region ///// 命名规范 /////
    /// <summary>
    /// Xml是否命名规范
    /// XML节点的首字母不能是非法字符只能是中文和字母
    /// </summary>
    void XmlIsNamingConventions(string paramValue)
    {
        if (paramValue == null || paramValue.Length == 0)
        {
            throw new ArgumentNullException("paramValue");
        }

        // 取第一个字符
        char ascll = paramValue[0];

        // //范围（0x4e00～0x9fff），判断字符是否为汉字
        int chfrom = Convert.ToInt32("4e00", 16);

        if (ascll <= 64
         && (ascll >= 91 && ascll <= 96)
         && (ascll >= 123 && ascll < chfrom))
        {
            throw new Exception("paramValue首字符不能为非法字符");
        }

        foreach (char item in paramValue)
        {
            if (item == ' ')
            {
                throw new Exception("paramValue有空格");
            }
        }
    }
    #endregion
}