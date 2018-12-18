require "Game/Framework/Event/EventName"
local EventLib = require "Game/Framework/Event/EventLib"

Dispatcher = {};
local events = {};

local eventArray = {};

function Dispatcher.AddListener(event, handler)
    if not event or type(event) ~= "string" then
        logError("EventDispatcher event parameter in addlistener function has to be string, " .. type(event) .. " not right.")
        return;
    end

    if not handler or type(handler) ~= "function" then
        logError("EventDispatcher handler parameter in addlistener function has to be function, " .. type(handler) .. " not right")
        return;
    end

    if not events[event] then
        events[event] = EventLib:new(event)
    end

    events[event]:connect(handler)
end

function Dispatcher.Brocast(event, ...)
    if not events[event] then
        logWarn("EventDispatcher brocast " .. event .. " has no event.")
    else
        events[event]:fire(...)
    end
end

function Dispatcher.RemoveListener(event, handler)
    if not events[event] then
        logWarn("EventDispatcher remove " .. event .. " has no event.")
    else
        events[event]:disconnect(handler)
    end
end

function Dispatcher.RemoveAllListener(event)
    if not events[event] then
        logWarn("EventDispatcher remove " .. event .. " has no event.")
    else
        events[event]:DisconnectAll()
    end
end