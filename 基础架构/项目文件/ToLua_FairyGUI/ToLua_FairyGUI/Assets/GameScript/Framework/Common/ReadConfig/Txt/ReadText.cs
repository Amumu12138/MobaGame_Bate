using System.IO;
using System.Collections.Generic;

public class ReadText
{
	List<string> readTextList = new List<string> ();
	Dictionary<string, string[]> configList = new Dictionary<string, string[]> ();

	public bool Load(string _path)
	{
		if(!File.Exists(_path))
		{
			return false;
		}

		FileStream tempStream = new FileStream(_path, FileMode.Open);
		StreamReader tempReader = new StreamReader(tempStream);
		
		while (!tempReader.EndOfStream)
		{
			readTextList.Add(tempReader.ReadLine());
		}

		for (int i = 0; i < readTextList.Count; i++) 
		{
			string[] tempConfig = readTextList[i].Split('#');
			if(null == tempConfig || tempConfig.Length <= 1)
			{
				continue;
			}

			configList.Add(tempConfig[0], tempConfig);
		}

		//清空缓冲区
		tempStream.Flush();
		//关闭流
		tempStream.Close();
		tempStream.Close();

		return true;
	}

	public bool LoadText(string _textAsset)
	{
		string[] tempLine = _textAsset.Split ('\n');
		if(null == tempLine || tempLine.Length == 0)
		{
			return false;
		}

		for (int i = 0; i < tempLine.Length; i++) 
		{
			string[] tempConfig = tempLine[i].Split('#');
			if(null == tempConfig || tempConfig.Length == 0)
			{
				continue;
			}

			configList.Add(tempConfig[0], tempConfig);
		}

		return true;
	}

	public string ReadString(string _id, int _index)
	{
		if (!configList.ContainsKey (_id)) 
		{
			return "";
		}

		if(_index < 0 || _index >= configList[_id].Length)
		{
			return "";
		}

		return configList [_id] [_index];
	}

	public string[] ReadStringArray(string _id, int _index)
	{
		string tempData = ReadString (_id, _index);
		if(string.IsNullOrEmpty(tempData))
		{
			return null;
		}

		return tempData.Split (',');
	}

	public int ReadInt(string _id, int _index)
	{
		string tempData = ReadString (_id, _index);
		if(string.IsNullOrEmpty(tempData))
		{
			return -1;
		}

		return IntParse (tempData);
	}

	public int[] ReadIntArray(string _id, int _index)
	{
		string[] tempData = ReadStringArray (_id, _index);
		if(null == tempData)
		{
			return null;
		}

		List<int> tempValue = new List<int> ();
		for (int i = 0; i < tempData.Length; i++) 
		{
			tempValue.Add(IntParse(tempData[i]));
		}

		return tempValue.ToArray ();
	}

	public float ReadFloat(string _id, int _index)
	{
		string tempData = ReadString (_id, _index);
		if(string.IsNullOrEmpty(tempData))
		{
			return -1;
		}

		return FloatParse (tempData);
	}

	public float[] ReadFloatArray(string _id, int _index)
	{
		string[] tempData = ReadStringArray (_id, _index);
		if(null == tempData)
		{
			return null;
		}
		
		List<float> tempValue = new List<float> ();
		for (int i = 0; i < tempData.Length; i++) 
		{
			tempValue.Add(FloatParse(tempData[i]));
		}
		
		return tempValue.ToArray ();
	}

	int IntParse(string _value)
	{
		int tempValue = -1;
		if (!int.TryParse (_value, out tempValue)) 
		{
			return -1;
		}
		return tempValue;
	}

	float FloatParse(string _value)
	{
		float tempValue = -1;
		if(!float.TryParse(_value, out tempValue))
		{
			return -1;
		}
		return tempValue;
	}

	public string ReadLineAll()
	{
		if(0 == readTextList.Count)
		{
			return "";
		}

		string tempReadText = readTextList[0];
		for (int i = 1; i < readTextList.Count; i++) 
		{
			tempReadText += "\r\n" + readTextList[i];
		}

		return tempReadText;
	}

	public string ReadTargetLine(string _targetText)
	{
		for (int i = 0; i < readTextList.Count; i++) 
		{
			if(readTextList[i].IndexOf(_targetText) >= 0)
			{
				return readTextList[i];
			}
		}
		
		return "";
	}

	public List<string> ReadLineList() { return readTextList; }
	public Dictionary<string, string[]> config { get { return configList; } }
}