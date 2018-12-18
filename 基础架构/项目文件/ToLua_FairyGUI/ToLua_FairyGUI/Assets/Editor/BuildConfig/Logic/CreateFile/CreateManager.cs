using System.IO;
using System.Collections.Generic;

/**
 * 管理各项文件生成
 * 所有生成文件的类都只能这里进行处理，不能在外部处理，这样写的好处在于
 * 1、各项管理之间比较方便
 * 2、很好移植，要把这部分逻辑分离到别的地方实现的话，只要把UI处理好之后，直接调用相关接口就行
 */

public class CreateManager
{
    List<CreateLibrary> libList = new List<CreateLibrary>();

    public CreateManager(string _dirPath)
    {
        libList.Clear();

        List<FileInfo> tempFileList = UtilityFile.GetDirectoryAllFile(_dirPath, "xlsx");
        for (int i = 0; i < tempFileList.Count; i++)
        {
            libList.Add(new CreateLibrary(tempFileList[i].FullName));
        }
    }

    /// <summary>
    /// 创建客户端的 Lua 代码
    /// </summary>
    /// <param name="_writePath">生成地址</param>
    public void GenerateLua_Client(string _writePath, bool _isShowIndex = false)
    {
        LuaScript tempLuaScript = new LuaScript(_isShowIndex);
        for (int i = 0; i < libList.Count; i++)
        {
            tempLuaScript.Generate(_writePath, libList[i].FileName, libList[i].ClientList);
        }
    }

    public void GenerateXml_Client(string _writePath)
    {
        XmlConfig tempXmlConfig = new XmlConfig();
        for (int i = 0; i < libList.Count; i++)
        {
            tempXmlConfig.Generate(_writePath, libList[i].FileName, libList[i].ClientList);
        }
    }

    public void GenerateTxt_Client(string _writePath)
    {
        TxtConfig tempTxtConfig = new TxtConfig();
        for (int i = 0; i < libList.Count; i++)
        {
            tempTxtConfig.Generate(_writePath, libList[i].FileName, libList[i].ClientList);
        }
    }
}