--[Comment]
-- 播放音效
-- _clipName ：音效名字
function SetPlaySoundClipName(_clipName)
    AudioManager:PlaySound_ClipName(_clipName);
end

--[Comment]
-- 播放场景音乐，某个功能模块需要的特殊音乐
-- _clipName ：音乐名字
function SetPlaySceneMusicClipName(_clipName)
    AudioManager:PlaySceneMusic_ClipName(_clipName);
end

--[Comment]
-- 播放背景音乐
-- _clipName ：音乐名字
function SetPlayBackgroundMusicClipName(_clipName)
    AudioManager:PlayBackgroundMusic_ClipName(_clipName);
end

--[Comment]
-- 设置音乐音量（包含场音乐和背景音乐）
-- _volume ：音量
function SetMusicVolume(_volume)
    AudioManager:SetMusicVolume(_volume);
end

--[Comment]
-- 设置音效音量
-- _volume ：音量
function SetSoundVolume(_volume)
    AudioManager:SetSoundVolume(_volume);
end

--[Comment]
-- 设置音乐和音效的音量
-- _volume ：音量
function SetAllVolume(_volume)
    AudioManager:SetAllVolume(_volume);
end

--[Comment]
-- 设置音效是否关闭
-- _enabledSound ：是否关闭
function SetEnabledSound(_enabledSound)
    AudioManager:EnabledSound(_enabledSound == true);
end

--[Comment]
-- 设置音乐是否关闭
-- _enabledMusic ：是否关闭
function SetEnabledMusic(_enabledMusic)
    AudioManager:EnabledMusic(_enabledMusic == true);
end

--[Comment]
-- 设置音乐和音效是否关闭
-- _enabledAll ：是否关闭
function SetEnabledAll(_enabledAll)
    SetEnabledSound(_enabledAll);
    SetEnabledMusic(_enabledAll);
end

-- 移除加载的音乐或音效资源
function UnloadBundle(_fileName)
    AudioManager:UnloadBundle(_fileName);
end

--[Comment]
-- 获取音乐音量大小
function GetMusicVolume() return AudioManager:GetMusicVolume(); end
--[Comment]
-- 获取音效音量大小
function GetSoundVolume() return AudioManager:GetSoundVolume(); end