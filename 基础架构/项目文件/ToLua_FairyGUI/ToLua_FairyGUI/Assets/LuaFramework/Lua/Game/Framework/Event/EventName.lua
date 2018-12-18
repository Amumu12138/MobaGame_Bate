-- 游戏事件，前三位数字代表模块，后三个数字代表数量，以此为唯一ID
EventName = {
    GameStateExit = "100001";                               -- 游戏退出
    GameStateChange = "100002";                             -- 游戏状态切换

    GamePauseChange = "200001";                             -- 游戏暂停状态
    GameFocusChange = "200002";                             -- 游戏失去焦点状态
}