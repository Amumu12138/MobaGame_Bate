-- 架构需要导出的脚本都要在这里实现
require "Game/Framework/System/Class"
require "Game/Framework/Event/Dispatcher"
require "Game/Framework/Proto/ProtoRegister"
require "Game/Framework/TemplateLibrary/Map"
require "Game/Framework/TemplateLibrary/List"
require "Game/Framework/Modules/View/BaseView"
require "Game/Framework/Network/NetworkManager"
require "Game/Framework/Modules/View/UITransmit"
require "Game/Framework/TimerHelper/TimerHelper"
require "Game/Framework/Resource/ResourceManager"
require "Game/Framework/AudioFunction/AudioFunction"
require "Game/Framework/FairyFunction/FairyFunction"
require "Game/Framework/Modules/Controller/GameState"

FrameworkManager = {};

--[Comment]
-- 初始化架构
function FrameworkManager.Init()
    ProtoRegister.Register();

    GameState.Init();
    UITransmit.Init();
end