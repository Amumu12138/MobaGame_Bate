using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

namespace EditorTool
{
	public class Utility
	{
		public static string BatchReplace(string _targetValue, string[] _oldValue, string[] _newValue)
		{
			string tempValue = _targetValue;
			for (int i = 0; i < _oldValue.Length; i++)
			{
				tempValue = tempValue.Replace(_oldValue[i], _newValue[i]);
			}
			return tempValue;
		}
		
		public static string BatchReplace(string _targetValue, params string[] _newValue)
		{
			string tempValue = _targetValue;
			for (int i = 0; i < _newValue.Length; i++)
			{
				int tempIndex = tempValue.IndexOf("xxx");
				tempValue = tempValue.Remove(tempIndex, 3);
				tempValue = tempValue.Insert(tempIndex, _newValue[i]);
			}
			return tempValue;
		}

		public static bool IsLetter(string _str)
		{
			for (int i = 0; i < _str.Length; i++) 
			{
				if(!char.IsLetter(_str[i]))
				{
					return false;
				}
			}

			return true;
		}

		public static string GetSystemTime()
		{
			return 	DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + 
					DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second;
		}

        public static int GetVersionNumber(string _serverName)
        {
            List<int> tempNumberList = new List<int>();

            string tempBuff = NetworkRequest.GetWebBuff(BuildConst.UploadServerPath + _serverName + "/" + BuildConst.VersionFile);
            string[] tempBuffs = tempBuff.Split('#');

            for (int i = 0; i < tempBuffs.Length; i++)
            {
                int tempIndex = 0;
                string tempNumber = tempBuffs[i].Split('_')[0];
                if (int.TryParse(tempNumber, out tempIndex))
                {
                    tempNumberList.Add(tempIndex);
                }
            }

            if (tempNumberList.Count == 0)
            {
                return 9999;
            }

            tempNumberList.Sort();
            return tempNumberList[tempNumberList.Count - 1];
        }

        public static void CopyFileOrDirectory(string _formPath, string _toPath)
		{
			if(!File.Exists(_formPath))
			{
				Debug.LogError(_formPath + " Path Not Exists");
				return;
			}
			
			DisposeDirectoryPath (_toPath);
			File.Copy (_formPath, _toPath, true);
		}
		
		public static void DeleteFileOrDirectory(string _path)
		{
			if(ExistsFile(_path))
			{
				File.Delete(_path);
				return;
			}
			
			Directory.Delete (_path, true);
		}
		
		public static bool ExistsFile(string _path)
		{
			return File.Exists (_path);
		}
		
		public static bool ExistsDirectory(string _path)
		{
			return Directory.Exists (_path);
		}
		
		public static void DisposeDirectoryPath(string _path)
		{
			string[] tempPath = _path.Split ('/');
			
			if(tempPath.Length <= 2)
			{
				return;
			}
			
			string tempDirPath = tempPath[0];
			for (int i = 1; i < tempPath.Length - 1; i++) 
			{
				tempDirPath += "/" + tempPath[i];
				if(!Directory.Exists(tempDirPath))
				{
					Directory.CreateDirectory(tempDirPath);
				}
			}
		}
	}
}