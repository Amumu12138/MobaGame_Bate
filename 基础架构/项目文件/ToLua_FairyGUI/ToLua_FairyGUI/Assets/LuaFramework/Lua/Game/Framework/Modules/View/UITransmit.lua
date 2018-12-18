UITransmit = {}

local loginView;

local function SetBaseModuleView(_statePhase, _gameState, _moduleView)
    if(_statePhase == EnumGameStatePhase.Enter) then
        if _moduleView ~= nil then
            _moduleView:Init();
        end

    elseif(_statePhase == EnumGameStatePhase.Exit) then
        if _moduleView ~= nil then
            _moduleView:ExitView();
            _moduleView = nil;
        end
    end
end

local function OnLogin_Callback(_statePhase, _gameState)
    if loginView == nil then
        -- loginView = LoginView.new("Login");
    end

    SetBaseModuleView(_statePhase, _gameState, loginView);
end

function UITransmit.Init()
    GameState.AddStateListener (EnumGameState.GS_LOGIN, OnLogin_Callback);
end

function UITransmit.AddListener()

end