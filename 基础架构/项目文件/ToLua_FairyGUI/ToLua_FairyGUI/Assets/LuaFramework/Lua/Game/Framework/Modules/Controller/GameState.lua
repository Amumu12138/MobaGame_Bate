require("Game/Framework/Modules/Controller/Params")

GameState = { };

-- @value
-- 0: can't set "row" state 
-- 1: can set "row" state, no state exit 
-- 2: can set "row" state, and all state of [row, col] will exit 
--[[
-1：模块共存
-2：模块互斥
]]
local mutexArray = {
    --[[LOGIN               登录]]            { -1 },
   };

local curGameState;
local lastState = EnumGameState.GS_NONE;

local mutex = nil;
local stateList = nil;
local gameStateListeners = nil;
local existsStateList = function() return stateList:Count() > 1; end

function GameState.Init()
    curGameState = EnumGameState.GS_MAIN;
    stateList = List.new();
    stateList:Add(curGameState);

    gameStateListeners = { };
    for i = 1, (EnumGameState.GS_NUM - 1) do
        gameStateListeners[i] = List.new();
    end

    mutex = {};
    local tempCount = EnumGameState.GS_NUM - 1;
    for i = 1, tempCount do
        local tempArray = {};                   -- 记录模块状态
        local tempStateArray = mutexArray[i];   -- 查询模块状态，-1为共存，-2为互斥

        for j = 1, tempCount do
            if i == j then
                -- 模块相等的话，即为共存
                tempArray[j] = 1;
            else
                local tempIsExistState = false;
                for k = 2, #tempStateArray do
                    if j == tempStateArray[k] then
                        tempIsExistState = true;
                    end
                end

                local tempState = tempStateArray[1];
                if tempState == -1 then
                    -- 模块状态为共存
                    tempArray[j] = tempIsExistState and 1 or 2;

                elseif tempState == -2 then
                    -- 模块状态为互斥
                    tempArray[j] = tempIsExistState and 2 or 1;
                end
            end
        end

        mutex[i] = tempArray;
    end
end

--[Comment]
-- 添加状态机事件
-- _inState : EnumGameState
-- _inCallBack : GameStateCallBack(EnumGameStatePhase, EnumGameState);
function GameState.AddStateListener(_inState, _inCallBack)
    gameStateListeners[_inState]:Add(_inCallBack);
end

--[Comment]
-- 移除状态机事件
-- _inState : EnumGameState
-- _inCallBack : GameStateCallBack(EnumGameStatePhase, EnumGameState);
function GameState.RemoveStateListener(_inState, _inCallBack)
    gameStateListeners[_inState]:Remove(_inCallBack);
end

--[Comment]
-- 设置当前游戏状态
-- _gs : EnumGameState
function GameState.SetGameState(_gs, ...)
    lastState = curGameState;
    local tCall = nil;
    local tList = nil;
    
    for i = stateList:Count(), 1, -1 do
    	local tempGs = stateList[i];
    	if (mutex[_gs][tempGs] == 0) then
    		return;
    	
        elseif (mutex[_gs][tempGs] == 1) then
            
        end
    
    	if (mutex[_gs][tempGs] == 2) then
    		tList = gameStateListeners[tempGs];
    		for k = 1, tList:Count() do
    			tCall = tList[k];
    			tCall(EnumGameStatePhase.Exit, tempGs, ...);
                Dispatcher.Brocast(EventName.GameStateExit, tempGs)
    		end

    		stateList:RemoveRange(i, 1);
    	end
    end
    
    if (stateList:Contains (_gs) == false) then
    	stateList:Add (_gs);
    else
    	stateList:Remove(_gs);
    	stateList:Add(_gs);
    end

    tList = gameStateListeners[_gs];
    for i = 1, tList:Count() do
    	tCall = tList[i];
    	tCall(EnumGameStatePhase.Enter, _gs, ...);
    end

    curGameState = _gs;
    Dispatcher.Brocast(EventName.GameStateChange, curGameState)
end

--[Comment]
-- 退出状态
-- _st : EnumGameState
-- _removeOne : bool
-- _isLast : bool
function GameState.ExitState(_st, _removeOne, _isLast)
    lastState = curGameState;
    local tCall = nil;
    local tList = nil;
    if _isLast == nil then
        _isLast = true;
    end

    for i = 1, stateList:Count() do
    	if (stateList[i] == _st) then
    		if (_removeOne) then
    			local tst = _st;
    
    			tList = gameStateListeners[tst];
    			for k = 1, tList:Count() do
    				tCall = tList[k];
    				tCall(EnumGameStatePhase.Exit, tst);
                    Dispatcher.Brocast(EventName.GameStateExit, tst)
    			end

    			stateList:RemoveRange(i, 1);
    		else
    			for j = stateList:Count(), 1, -1 do
    				local tst = stateList[j];
    
    				tList = gameStateListeners[tst];
    				for k = 1, tList:Count() do
    					tCall = tList[k];
    					tCall(EnumGameStatePhase.Exit, tst);
                        Dispatcher.Brocast(EventName.GameStateExit, tst)
    				end
    			end

    			stateList:RemoveRange(i, (stateList:Count() + 1) - i);
    		end
    		break;
    	end
    end

    if (_isLast) then
    	if stateList:Count() > 0 then
    		curGameState = stateList [stateList:Count()];
    
    		tList = gameStateListeners [curGameState];
    		for i = 1, tList:Count() do
    			tCall = tList [i];
    			tCall(EnumGameStatePhase.Enter, curGameState);
    		end

    		Dispatcher.Brocast(EventName.GameStateChange, curGameState)
    	end
    end
end

--[Comment]
-- 返回到上一级状态
-- 触发当前状态的 Exit 事件
function GameState.PopState()
    if (existsStateList()) then
        GameState.ExitState(stateList[stateList:Count()], true, false);
    end
end

--[Comment]
-- 返回到上一级的状态
-- 全部触发 Exit 事件，再触发上一级的 Enter 事件
function GameState.PopLastStateList()
    if (existsStateList()) then
        GameState.ExitState(stateList[stateList:Count()], false);
    end
end

--[Comment]
-- 最后一个游戏状态
function GameState.LastState() return lastState; end
--[Comment]
-- 当前游戏状态
function GameState.GetGameState() return curGameState; end
--[Comment]
-- 是否可以返回上一级
function GameState.CanPopState() return stateList:Count() > 1; end

--[Comment]
-- 目标游戏状态是否存在，已经打开的状态才会保存
function GameState.Contains(_enumGameState) return stateList:Contains (_enumGameState); end
--[Comment]
-- 目标游戏状态是否存在，所有的状态
function GameState.ContainsState(_enumGameState) return gameStateListeners[_enumGameState] ~= nil end
function GameState.LastStateList() return existsStateList() and stateList [stateList:Count() - 1] or EnumGameState.GS_NONE; end