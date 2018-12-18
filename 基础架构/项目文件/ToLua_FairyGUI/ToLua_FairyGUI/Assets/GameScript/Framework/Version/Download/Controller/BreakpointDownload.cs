using System.IO;
using System.Net;
using UnityEngine;
using System.Threading;
using System.Collections;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

// 断点续传
public class BreakpointDownload
{
    long fileLength = 0;    // 记录已完成的大小
    long maxFileLength = 0; // 记录文件的大小

    Stream stream;
    FileStream fileStream; 	//打开上次下载的文件或新建文件

    const int ReadSize = 262144;

    /// <summary>
    /// 以断点续传方式下载文件
    /// </summary>
    /// <param name="strFileName">下载文件的保存路径</param>
    /// <param name="strUrl">文件下载地址</param>
    public bool CheckDownload(string _fileName, string _url)
    {
        Close();

        if (File.Exists(_fileName))
        {
            fileStream = File.OpenWrite(_fileName);
            fileLength = fileStream.Length;
            fileStream.Seek(fileLength, SeekOrigin.Current);//移动文件流中的当前指针
        }
        else
        {
            fileStream = new FileStream(_fileName, FileMode.Create);
            fileLength = 0;
        }

        //打开网络连接
        try
        {
            HttpWebRequest tempRequest = (HttpWebRequest)HttpWebRequest.Create(_url);
            if (fileLength > 0)
            {
                tempRequest.AddRange((int)fileLength);//设置Range值
            }

            // 向服务器请求，获得服务器的回应数据流
            HttpWebResponse webResponse = (HttpWebResponse)tempRequest.GetResponse();
            stream = webResponse.GetResponseStream();
            maxFileLength = webResponse.ContentLength + fileLength;

            return true;
        }
        catch
        {
            Close();
            return false;
        }
    }

    /// <summary>
    /// 获取断点下载的大小
    /// </summary>
	public float Progress
    {
        get
        {
            if (null == stream)
            {
                Debug.LogError("stream value null");
                return 1;
            }

            byte[] tempContent = new byte[ReadSize];
            int tempReadSize = stream.Read(tempContent, 0, ReadSize);
            if (tempReadSize > 0)
            {
                fileStream.Write(tempContent, 0, tempReadSize);
                fileLength += tempReadSize;
            }

            return ((float)fileLength / (float)maxFileLength);
        }
    }

    /// <summary>
    /// 获取最大文件的长度
    /// </summary>
    public int GetMaxFileLength()
    {
        return (int)maxFileLength;
    }

    public void Close()
    {
        if (stream != null)
        {
            stream.Close();
            stream = null;
        }

        if (fileStream != null)
        {
            fileStream.Close();
            fileStream = null;
        }
    }
}