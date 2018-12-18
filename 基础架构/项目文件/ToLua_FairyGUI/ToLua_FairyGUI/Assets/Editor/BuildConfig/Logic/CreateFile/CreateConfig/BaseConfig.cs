using System.Collections.Generic;

public class BaseConfig
{
    string className = "";

    public void Generate(string _generateDir, string _fileName, Dictionary<string, List<CreateFileGroupLibrary>> _paramList)
    {
        string tempFile = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(_fileName);

        foreach (var item in _paramList)
        {
            string tempTablesName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(item.Key);
            string tempClassName = tempFile + tempTablesName +"Config";
            string tempDir = _generateDir + "/" + tempClassName;

            className = tempClassName;
            HorizontalTableList(tempDir, item.Value);
        }
    }

    protected string ClassName { get { return className; } }
    protected virtual void HorizontalTableList(string _dir, List<CreateFileGroupLibrary> _dataList) { }
}