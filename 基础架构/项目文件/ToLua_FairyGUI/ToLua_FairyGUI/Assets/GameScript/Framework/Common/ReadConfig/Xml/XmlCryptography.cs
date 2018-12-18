using System;
using System.Xml;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;

/*
 * 作者：陈建滔
 * 日期：2014.6.24
 * 功能 加密 XML 配置文件
 */

public class XmlCryptography
{
    #region ///// 私有变量 /////
    RijndaelManaged mKey = new RijndaelManaged();
    #endregion

    #region ///// 构造函数 /////
    public XmlCryptography()
    {
        if (mKey == null)
        {
            throw new ArgumentNullException("mKey");
        }

        // 密钥值
        byte[] Key = { 169, 181, 206, 228, 43, 123, 98, 110, 247, 90, 131, 71, 17, 4, 49, 65, 83, 82, 164, 0, 2, 6, 87, 94, 55, 14, 240, 61, 200, 70, 146, 163 };

        mKey.Key = Key;
    }
    #endregion

    #region ///// 数据解密 /////
    /// <summary>
    /// 解密
    /// </summary>
    public void Decrypt(XmlDocument xmlDocument)
    {
        if (xmlDocument == null)
        {
            throw new ArgumentNullException("xmlDocument");
        }

        if (mKey == null)
        {
            throw new ArgumentNullException("mKey");
        }

        // 查找在XmlDocument里的EncryptedData元素。
        XmlElement encryptedElement = xmlDocument.GetElementsByTagName("EncryptedData")[0] as XmlElement;

        // 如果EncryptedData元素不存在,抛出一个异常。
        if (encryptedElement == null)
        {
            throw new XmlException("EncryptedData元素不能被发现。");
        }

        // 创建一个对象并填充EncryptedData。
        EncryptedData edElement = new EncryptedData();

        edElement.LoadXml(encryptedElement);

        // 创建一个新的EncryptedXml对象。
        EncryptedXml exml = new EncryptedXml();

        // 解密元素使用对称密钥。
        byte[] rgbOutput = exml.DecryptData(edElement, mKey);

        // 取代encryptedData元素与明文的XML元素。
        exml.ReplaceData(encryptedElement, rgbOutput);
    }
    #endregion

    #region ///// 数据加密 /////
    /// <summary>
    /// XML数据加密
    /// </summary>
    /// <param name="xmlDocument">XML 文档的实例</param>
    /// <param name="nodeName">需要加密的 XML 节点名字</param>
    /// <param name="key">密钥</param>
    public void Encrypt(XmlDocument xmlDocument, string nodeName)
    {
        // 检查参数
        if (xmlDocument == null)
        {
            throw new ArgumentNullException("xmlDocument");
        }

        if (nodeName == null || nodeName.Length == 0)
        {
            throw new ArgumentNullException("nodeName");
        }

        if (mKey == null)
        {
            throw new ArgumentNullException("mKey");
        }

        ////////////////////////////////////////////////
        // 在XmlDocument找到指定的元素对象，并创建一个新的XmlElemnt对象。
        ////////////////////////////////////////////////
        XmlElement elementToEncrypt = xmlDocument.GetElementsByTagName(nodeName)[0] as XmlElement;

        // 如果元素不能被发现，抛出一个XmlException。
        if (elementToEncrypt == null)
        {
            throw new XmlException("指定的元素不能被发现");
        }

        //////////////////////////////////////////////////
        // 创建一个新的实例的EncryptedXml类和使用它来加密XmlElement和对称密钥
        //////////////////////////////////////////////////
        EncryptedXml eXml = new EncryptedXml();

        byte[] encryptedElement = eXml.EncryptData(elementToEncrypt, mKey, false);

        ////////////////////////////////////////////////
        // 构造一个EncryptedData对象并填充所需的加密信息。
        ////////////////////////////////////////////////
        EncryptedData edElement = new EncryptedData();

        edElement.Type = EncryptedXml.XmlEncElementUrl;

        // 创建一个EncryptionMethod元素,以便接收方知道这算法用于解密。
        // 确定什么样的算法被使用和供应适当的URL到EncryptionMethod元素。
        string encryptionMethod = null;

        if (mKey is Rijndael)
        {
            switch (mKey.KeySize)
            {
                case 128:
                    encryptionMethod = EncryptedXml.XmlEncAES128Url;
                    break;
                case 192:
                    encryptionMethod = EncryptedXml.XmlEncAES192Url;
                    break;
                case 256:
                    encryptionMethod = EncryptedXml.XmlEncAES256Url;
                    break;
                default:
                    throw new CryptographicException("此算法只支持16位, 24位, 32位的密钥");
            }
        }
        else
        {
            // 如果变换不是在前面的类别，抛出一个异常
            throw new CryptographicException("指定的算法不支持XML加密");
        }

        edElement.EncryptionMethod = new EncryptionMethod(encryptionMethod);

        // 添加加密元素数据到EncryptedData对象。
        edElement.CipherData.CipherValue = encryptedElement;

        ////////////////////////////////////////////////////
        // 从原始XmlDocument对象与EncryptedData元素替换元素。
        ////////////////////////////////////////////////////
        EncryptedXml.ReplaceElement(elementToEncrypt, edElement, false);
    }
    #endregion
}