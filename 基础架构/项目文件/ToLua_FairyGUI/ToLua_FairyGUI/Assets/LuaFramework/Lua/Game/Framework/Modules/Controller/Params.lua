-- 判断游戏状态
-- 状态只会有三种，不要进行修改
EnumGameStatePhase = {
	Enter = 0;
	In = 1;
	Exit = 2;
}

-- 游戏的状态类型
-- 这个是根据游戏情况来进行修改
EnumGameState = {
	GS_NONE = 0;                        -- 无
    GS_LOGIN = 1;                       -- 登录
    GS_NUM = 2;
}