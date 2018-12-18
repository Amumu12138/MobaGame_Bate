TimerHelper = {};

--[Comment]
-- 添加触发一次的事件
-- _eventName ：事件名字
-- _delay     ：间隔时间
-- _fun       ：回调函数
function TimerHelper.AddOnceEvent(_eventName, _delay, _fun)
    TimeManager.mInst:AddOnceEvent(_eventName, _delay, _fun);
end

--[Comment]
-- 添加循环触发的事件
-- _eventName ：事件名字
-- _delay     ：间隔时间
-- _fun       ：回调函数
function TimerHelper.AddLoopEvent(_eventName, _delay, _fun)
    TimeManager.mInst:AddLoopEvent(_eventName, _delay, _fun);
end

--[Comment]
-- 添加触发的事件
-- _eventName ：事件名字
-- _delay     ：间隔时间
-- _loopNum   ：循环次数
-- _fun       ：回调函数
function TimerHelper.AddEvent(_eventName, _delay, _loopNum, _fun)
    TimeManager.mInst:AddEvent(_eventName, _delay, _loopNum, _fun);
end

--[Comment]
-- 移除事件名字
function TimerHelper.RemoveEvent(_eventName)
    TimeManager.mInst:RemoveEvent(_eventName)
end

--[Comment]
-- 是否存在事件名字
function TimerHelper.IsExistEventName(_eventName) return TimeManager.mInst:IsExistEventName(_eventName); end