using System.IO;
using System.Text;
using System.Collections.Generic;

public class WriteText
{
	public void WriteAll(string _path, string _data, bool _isUseDefault = false)
	{
		FileStream fs = new FileStream(_path, FileMode.Create);
		StreamWriter sw = new StreamWriter (fs, _isUseDefault ? Encoding.Default : Encoding.UTF8);
		// 开始写入
		sw.Write(_data);
		// 清空缓冲区
		sw.Flush();
		// 关闭流
		sw.Close();
		fs.Close();
	}
}