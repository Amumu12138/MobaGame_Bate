using System.IO;
using System.Net;
using System.Text;

public class NetworkRequest
{
    public static string GetWebBuff(string _url)
    {
        int byteRead = 0;
        string strBuff = "";
        char[] cbuffer = new char[256];

        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(_url);
        req.Method = "GET";
        using (WebResponse wr = req.GetResponse())
        {
            Stream respStream = wr.GetResponseStream();
            StreamReader respStreamReader = new StreamReader(respStream, Encoding.UTF8);

            byteRead = respStreamReader.Read(cbuffer, 0, 256);

            while (byteRead != 0)
            {
                string strResp = new string(cbuffer, 0, byteRead);
                strBuff = strBuff + strResp;
                byteRead = respStreamReader.Read(cbuffer, 0, 256);
            }

            respStream.Close();
            return strBuff;
        }
    }
}