using System;
using UnityEngine;
using LuaInterface;

public class TimeManager : MonoBehaviour
{
    public static TimeManager mInst;

    long time = 0;
    TimeLibrary timeLib = new TimeLibrary();

    void Awake()
    {
        mInst = this;
    }

    void Update()
    {
        timeLib.UpdateEvent(Time.time);
    }

    public void AddOnceEvent(string _eventName, float _delay, LuaFunction _luaFun)
    {
        AddEvent(_eventName, _delay, 1, _luaFun, null);
    }

    public void AddLoopEvent(string _eventName, float _delay, LuaFunction _luaFun, LuaFunction _luaEndFun)
    {
        AddEvent(_eventName, _delay, 0, _luaFun, _luaEndFun);
    }

    public void AddEvent(string _eventName, float _delay, double _totalLoopNum, LuaFunction _luaFun, LuaFunction _luaEndFun)
    {
        timeLib.AddEvent(_eventName, _delay, _totalLoopNum, _luaFun, _luaEndFun);
    }

    public void RemoveEvent(string _eventName)
    {
        timeLib.RemoveEvent(_eventName);
    }

    public bool IsExistEventName(string _eventName)
    {
        return timeLib.IsExistEventName(_eventName);
    }
}