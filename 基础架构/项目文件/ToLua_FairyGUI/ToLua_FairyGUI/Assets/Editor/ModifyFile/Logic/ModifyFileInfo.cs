
public class ModifyFileInfo
{
    string filePath = "";
    string fileDate = "";
    string fileData = "";
    string assetsFilePath = "";

    public ModifyFileInfo(string _fileData)
    {
        string[] tempArray = _fileData.Split('#');
        if (tempArray == null || tempArray.Length < 2)
        {
            return;
        }

        fileData = _fileData;
        filePath = tempArray[0];
        fileDate = tempArray[1];
        assetsFilePath = filePath.Replace("\\", "/");
    }

    public string FilePath { get { return filePath; } }
    public string FileDate { get { return fileDate; } }
    public string AssetsFilePath { get { return assetsFilePath; } }
    public ModifyFileInfo Clone() { return new ModifyFileInfo(fileData); }
}