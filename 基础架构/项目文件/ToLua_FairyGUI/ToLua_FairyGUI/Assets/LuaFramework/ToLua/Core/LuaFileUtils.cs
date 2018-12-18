using System.IO;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

namespace LuaInterface
{
    public class LuaFileUtils
    {
        protected static LuaFileUtils instance = null;
        public static LuaFileUtils Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LuaFileUtils();
                }

                return instance;
            }

            protected set
            {
                instance = value;
            }
        }

        protected Dictionary<string, byte[]> fileByteMap = new Dictionary<string, byte[]>();
        protected Dictionary<string, string> searchPaths = new Dictionary<string, string>();

        public LuaFileUtils()
        {
            instance = this;
        }

        public void AddSearchPath(string _path)
        {
            List<string> tempFileList = new List<string>();
            SearchAllFile(_path, "lua", tempFileList);

            for (int i = 0; i < tempFileList.Count; i++)
            {
                string tempFile = tempFileList[i];
                tempFile = tempFile.Replace(@"\", "/");

                string tempFileKey = tempFile.Replace(_path + "/", "");
                tempFileKey = tempFileKey.Replace(".lua", "").ToLower();

                searchPaths.Add(tempFileKey, tempFile);
                fileByteMap.Add(tempFileKey, Encoding.UTF8.GetBytes(File.ReadAllText(tempFile)));
            }
        }

        public void AddSearchBundle(string _path)
        {
            List<string> tempFileList = new List<string>();
            SearchAllFile(_path, "lua", tempFileList);

            for (int i = 0; i < tempFileList.Count; i++)
            {
                string tempFile = tempFileList[i];
                tempFile = tempFile.Replace(@"\", "/");

                string tempFileKey = tempFile.Replace(_path, "");
                tempFileKey = tempFileKey.Replace("_", "/");
                tempFileKey = tempFileKey.Replace(".lua", "").ToLower();

                byte[] tempByte = null;
                AssetBundle tempBundle = AssetBundle.LoadFromFile(tempFile);
                if (tempBundle)
                {
                    TextAsset tempLuaCode = tempBundle.LoadAllAssets<TextAsset>()[0];
                    tempByte = tempLuaCode.bytes;
                    tempBundle.Unload(true);
                }

                searchPaths.Add(tempFileKey, tempFile);
                fileByteMap.Add(tempFileKey, tempByte);
            }
        }

        public byte[] ReadFile(string _fileName)
        {
            if (_fileName.EndsWith(".lua"))
            {
                _fileName = _fileName.Substring(0, _fileName.Length - 4);
            }

            string tempKey = _fileName.ToLower();
            if (fileByteMap.ContainsKey(tempKey))
            {
                return fileByteMap[tempKey];
            }

            return null;
        }

        public string FindFile(string _fileName)
        {
            if (searchPaths.ContainsKey(_fileName))
            {
                return searchPaths[_fileName];
            }

            return string.Empty;
        }

        public string FindFileError(string _fileName)
        {
            if (Path.IsPathRooted(_fileName))
            {
                return _fileName;
            }

            if (_fileName.EndsWith(".lua"))
            {
                _fileName = _fileName.Substring(0, _fileName.Length - 4);
            }

            using (CString.Block())
            {
                CString tempStr = CString.Alloc(512);
                int tempPos = _fileName.LastIndexOf('/');
                if (tempPos > 0)
                {
                    int tempIndex = tempPos + 1;
                    tempStr.Append("\n\tno file '").Append(_fileName, tempIndex, _fileName.Length - tempIndex).Append(".lua' in ").Append("lua_");
                    tempIndex = tempStr.Length;
                    tempStr.Append(_fileName, 0, tempPos).Replace('/', '_', tempIndex, tempPos).Append(".unity3d");
                }
                else
                {
                    tempStr.Append("\n\tno file '").Append(_fileName).Append(".lua' in ").Append("lua.unity3d");
                }

                return tempStr.ToString();
            }
        }

        void SearchAllFile(string _directoryPath, string _searchPattern, List<string> _fileInfoList)
        {
            DirectoryInfo tempFolder = new DirectoryInfo(_directoryPath);
            DirectoryInfo[] tempDirectoryInfoList = tempFolder.GetDirectories();
            foreach (DirectoryInfo directory in tempDirectoryInfoList)
            {
                SearchAllFile(directory.FullName, _searchPattern, _fileInfoList);
            }

            FileInfo[] tempFileList = null;
            if (string.IsNullOrEmpty(_searchPattern))
            {
                tempFileList = tempFolder.GetFiles();
            }
            else
            {
                tempFileList = tempFolder.GetFiles("*." + _searchPattern);
            }

            foreach (FileInfo file in tempFileList)
            {
                _fileInfoList.Add(file.FullName);
            }
        }
    }
}
