ModulesManager = {};

local function RemoveAll()
    ModulesManager.ClearAllData();
    ModulesManager.RemoveAllListener();
end

local function OnDisconnected()
    RemoveAll();
end

--[Comment]
-- 游戏初始化  PS：只有登录的时候才会触发一次
function ModulesManager.Init()

end

--[Comment]
-- 添加网络监听事件
function ModulesManager.AddListener()
    logError("全部模块监听事件添加");
end

--[Comment]
-- 移除全部的网络监听事件
function ModulesManager.RemoveAllListener()
    logError("全部模块监听事件清除");
end

--[Comment]
-- 清除全部缓存数据  PS：只有退出重新登录的时候才会触发
function ModulesManager.ClearAllData()
    logError("全部模块缓存数据清除");
end

-- 退出某个模块的时候会触发一次事件
function ModulesManager.GameStateExit_Callback(_gameState)
--    if _gameState == EnumGameState.GS_LEAGUE then
--        -- 退出了某个模块
--    end
end

--[Comment]
-- 退出游戏回到登录界面
function ModulesManager.ExitGame()
    RemoveAll();
    NetworkManager.DisconnectedLogin();
	GameState.SetGameState(EnumGameState.GS_LOGIN);
end