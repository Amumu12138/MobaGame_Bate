using LuaInterface;

public class TimeInfo
{
    /// <summary>
    /// 间隔时间
    /// </summary>
    float delay;
    /// <summary>
    /// 当前时间
    /// </summary>
    float time;
    /// <summary>
    /// 当前循环的次数
    /// </summary>
    double loopNum;
    /// <summary>
    /// 总循环的次数
    /// </summary>
    double totalLoopNum;
    /// <summary>
    /// Lua 回调函数
    /// </summary>
    LuaFunction luaFun;
    /// <summary>
    /// Lua 回调结束函数
    /// </summary>
    LuaFunction luaEndFun;

    public TimeInfo(float _delay, double _totalLoopNum, LuaFunction _luaFun, LuaFunction _luaEndFun)
    {
        loopNum = 0;
        delay = _delay;
        luaFun = _luaFun;
        luaEndFun = _luaEndFun;
        totalLoopNum = _totalLoopNum;
        time = UnityEngine.Time.time;
    }

    public bool UpdateTime(float _time)
    {
        if (totalLoopNum != 0 && loopNum >= totalLoopNum)
        {
            if (luaEndFun != null)
            {
                luaEndFun.Call();
            }

            return false;
        }

        if (time == 0)
        {
            time = _time;
            return true;
        }

        if (_time - time >= delay)
        {
            if (luaFun != null)
            {
                luaFun.Call();
            }
            
            time = _time;
            loopNum++;
        }

        return true;
    }
}