using System;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;

public class ReadXmlFile
{
    #region ///// 私有变量 /////
    /// <summary>
    /// XML 加密
    /// </summary>
    XmlCryptography xmlCryptography = new XmlCryptography();
    /// <summary>
    /// XML 文档实例
    /// </summary>
    XmlDocument xmlDocument = new XmlDocument();
    /// <summary>
    /// 根节点
    /// </summary>
    XmlNode rootNode = null;
    /// <summary>
    /// 根节点的数据都保存在这个类
    /// </summary>
    List<NodeAttribute> nodeAttribute = new List<NodeAttribute>();
    #endregion

    #region ///// 更新加载 /////
    /// <summary>
    /// 加载指定路径的xml文件
    /// </summary>
    public bool Load(string filePath)
    {
        if (null == xmlDocument)
        {
            throw new ArgumentNullException("xmlDocument");
        }

        if (filePath == null || filePath.Length == 0)
        {
            throw new ArgumentNullException("filePath");
        }

        if (File.Exists(filePath))
        {
            xmlDocument.Load(filePath);
            rootNode = xmlDocument.DocumentElement;
            nodeAttribute = ReadAllNode();

            return true;
        }

        return false;
    }

    /// <summary>
    /// 加载 XML的文本文件
    /// </summary>
    public bool LoadXml(string xmlFile)
    {
        if (xmlDocument == null)
        {
            throw new ArgumentNullException("xmlDocument");
        }

        if (xmlFile == null || xmlFile.Length == 0)
        {
            throw new ArgumentNullException("xmlFile");
        }

        xmlDocument.LoadXml(xmlFile);
        rootNode = xmlDocument.DocumentElement;
        nodeAttribute = ReadAllNode();

        return true;
    }
    #endregion

    #region ///// 查询节点 /////
    /// <summary>
    /// 查询节点，节点存在返回 true，否则 false
    /// </summary>
    /// <param name="nodeName">查询的节点的名字</param>
    public bool HasNode(string nodeName)
    {
        if (null == nodeAttribute)
        {
            throw new UnityException("nodeAttribute 值为空");
        }

        for (int i = 0; i < nodeAttribute.Count; i++)
        {
            if (nodeAttribute[i].NodeKey == nodeName)
            {
                return true;
            }
        }

        return false;
    }
    #endregion

    #region ////// 字符串 //////
    /// <summary>
    /// 读取一个字符串
    /// </summary>
    public string ReadString(string nodeName, string elementName)
    {
        if (null == nodeAttribute)
        {
            throw new UnityException("nodeAttribute 值为空");
        }

        for (int i = 0; i < nodeAttribute.Count; i++)
        {
            if (nodeAttribute[i].NodeKey == nodeName)
            {
                return nodeAttribute[i].ElementValue(elementName);
            }
        }

        return "";
    }

    /// <summary>
    /// 读取一个整型的数组，没有找到相关的数据就返回异常，以“,”符号区分开
    /// </summary>
    public string[] ReadStringArray(string nodeName, string elementName)
    {
        string strValue = ReadString(nodeName, elementName);
        string[] strValueArray = strValue.Split(',');

        if (null == strValueArray || strValueArray.Length <= 0)
        {
            throw new UnityException("strValueArray 值为空");
        }

        return strValueArray;
    }
    #endregion

    #region /////// 整型 ///////
    /// <summary>
    /// 读取一个整型，没有找到相关的数据就返回异常
    /// </summary>
    public int ReadInt(string nodeName, string elementName)
    {
        int elementValue = 0;
        string readString = ReadString(nodeName, elementName);

        if (!int.TryParse(readString, out elementValue))
        {
            throw new UnityException(readString + "  无法转换整型");
        }

        return elementValue;
    }

    /// <summary>
    /// 读取一个整型的数组，没有找到相关的数据就返回异常 “,”符号区分开
    /// </summary>
    public int[] ReadIntArray(string nodeName, string elementName)
    {
        string[] strValueArray = ReadStringArray(nodeName, elementName);
        int[] intValue = new int[strValueArray.Length];

        for (int i = 0; i < strValueArray.Length; i++)
        {
            if (!int.TryParse(strValueArray[i], out intValue[i]))
            {
                throw new UnityException(strValueArray[i] + "  无法转换整型");
            }
        }

        return intValue;
    }
    #endregion

    #region ////// 浮点型 //////
    /// <summary>
    /// 读取一个浮点型，没有找到相关的数据就返回 0
    /// </summary>
    public float ReadFloat(string nodeName, string elementName)
    {
        return StringToFloat(ReadString(nodeName, elementName));
    }

    /// <summary>
    /// 读取一个浮点型数组
    /// </summary>
    public float[] ReadFloatArray(string nodeName, string elementName)
    {
        string[] strValue = ReadStringArray(nodeName, elementName);
        float[] floatValue = new float[strValue.Length];

        for (int i = 0; i < strValue.Length; i++)
        {
            floatValue[i] = StringToFloat(strValue[i]);
        }

        return floatValue;
    }

    float StringToFloat(string str)
    {
        float value = 0;
        float.TryParse(str, out value);
        return value;
    }
    #endregion

    #region /// 读取全部数据 ///
    /// <summary>
    /// 返回全部数据
    /// </summary>
    public List<NodeAttribute> ReadAllNode()
    {
        if (null == rootNode)
        {
            throw new XmlException("查找不到根节点");
        }

        List<NodeAttribute> tempNodeAttribute = new List<NodeAttribute>();

        for (int i = 0; i < rootNode.ChildNodes.Count; i++)
        {
            List<ElementAttribute> elementAttribute = new List<ElementAttribute>();

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

    #region ///// 数据解密 /////
    /// <summary>
    /// 数据解密
    /// </summary>
    public void Decrypt()
    {
        if (xmlDocument == null)
        {
            throw new ArgumentNullException("xmlDocument");
        }

        if (xmlCryptography == null)
        {
            throw new ArgumentNullException("xmlCryptography");
        }

        xmlCryptography.Decrypt(xmlDocument);
    }
    #endregion
}