public delegate void GameTimerCallBack();
public delegate void GameCallBack(object param);
public delegate void GameEventCallBack(object param);

public class NetState
{
    public const string DISCONNECTED = "DISCONNECTED";          // 断开连接
    public const string CONNECT_FAILED = "CONNECT_FAILED";       // 连接失败
	public const string CONNECT_SUCCEED = "CONNECT_SUCCEED";    // 连接成功
}

public class SDKEvent
{
    public const string Init = "SDK_Init";
    public const string Login = "SDK_Login";
    public const string Logout = "SDK_LoginOut";
    public const string ExitGame = "SDK_ExitGame";
    public const string Purchase = "SDK_Purchase";
}