using LuaInterface;
using System.Collections.Generic;

public class TimeLibrary
{
    List<string> eventNameList = new List<string>();
    List<TimeInfo> timeInfoList = new List<TimeInfo>();

    public void AddEvent(string _eventName, float _delay, double _totalLoopNum, LuaFunction _luaFun, LuaFunction _luaEndFun)
    {
        if (eventNameList.Contains(_eventName))
        {
            return;
        }

        eventNameList.Add(_eventName);
        timeInfoList.Add(new TimeInfo(_delay, _totalLoopNum, _luaFun, _luaEndFun));
    }

    public void RemoveEvent(string _eventName)
    {
        RemoveTargetIndexEvent(eventNameList.IndexOf(_eventName));
    }

    public void RemoveAllEvent()
    {
        timeInfoList.Clear();
        eventNameList.Clear();
    }

    public void UpdateEvent(float _time)
    {
        if (timeInfoList.Count == 0)
        {
            return;
        }

        for (int i = timeInfoList.Count - 1; i >= 0; i--)
        {
            TimeInfo tempTimeInfo = timeInfoList[i];
            if (tempTimeInfo == null)
            {
                continue;
            }

            if (!tempTimeInfo.UpdateTime(_time))
            {
                RemoveTargetIndexEvent(i);
            }
        }
    }

    public bool IsExistEventName(string _eventName)
    {
        return eventNameList.Contains(_eventName);
    }

    void RemoveTargetIndexEvent(int _index)
    {
        if (_index < 0)
        {
            return;
        }

        timeInfoList.RemoveAt(_index);
        eventNameList.RemoveAt(_index);
    }
}