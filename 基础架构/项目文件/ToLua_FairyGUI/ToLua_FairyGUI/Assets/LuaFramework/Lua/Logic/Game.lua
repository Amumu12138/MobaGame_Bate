-- local WWW = UnityEngine.WWW;
-- coroutine.start(this.test_coroutine);
-- -- 测试协同--
-- function Game.test_coroutine()
--     logWarn("1111");
--     coroutine.wait(1);
--     logWarn("2222");

--     local www = WWW("http://bbs.ulua.org/readme.txt");
--     coroutine.www(www);
--     logWarn(www.text);
-- end

require("Common/define");
require("Common/functions");
require("3rd/pbc/protobuf");
json = require("3rd/json/json");

require("Game/Framework/Manager/FrameworkManager");      -- 所有架构模块管理
require("Game/Configs/Manager/ConfigManager");           -- 所有配置文件管理
require("Game/Modules/Manager/ModulesManager");          -- 所有功能模块管理
require("Game/Common/ViewFunction/ViewFunction");        -- FairyGUI通用模块

Game = { };

-- 初始化完成
function Game.OnInitOK()
    logError("------> 33333 <------");

    RegisterFairyFont("MicrosoftYaHeiGB", "微软雅黑");
    SetFairyDefaultFont("Microsoft YaHei");

    -- 加载公共的资源，这个是优先加载的
--    AddFairyPackage("Com");
--    AddFairyPackage("Polygon");
--    AddFairyPackage("RaceCom");
--    AddFairyPackage("FontTilte");

    ConfigManager.Init();
    ModulesManager.Init();
    FrameworkManager.Init();

    if IsWindows() then
        local tempKeyDownFun = function(_keyCode)
            if tostring(_keyCode) == "Return" then
                GameState.SetGameState(EnumGameState.GS_GM);
            end
        end

        SetFairyKeyDown(tempKeyDownFun);
    end

    GameState.SetGameState(EnumGameState.GS_LOGIN);
    Dispatcher.AddListener(NetworkName.ReConnectFailed, function() GameState.SetGameState(EnumGameState.GS_LOGIN); end);
end

-- SDK 登录
function Game.SDKLoginIn()
	
end

-- SDK 登出
function Game.SKDLoginOut()
	ModulesManager.ClearAllData();
	NetworkManager.DisconnectedLogin();
	GameState.SetGameState(EnumGameState.GS_LOGIN);
end

function Game.SDKExitGame()
	if GameState.GetGameState() ~= EnumGameState.GS_LOGIN then
		
	end
end

function Game.OnApplicationPause(_pauseStatus)
    Dispatcher.Brocast(EventName.GamePauseChange, _pauseStatus);
end

function Game.OnApplicationFocus(_focusStatus)
    Dispatcher.Brocast(EventName.GameFocusChange, _focusStatus);
end