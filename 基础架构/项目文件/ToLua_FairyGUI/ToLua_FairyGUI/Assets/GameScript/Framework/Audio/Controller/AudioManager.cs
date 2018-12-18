using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager mInst;

    AudioSource soundSource;            // 音效
    AudioSource sceneMusicSource;       // 场景音效
    AudioClip backgroundMusicClip;      // 背景音效剪辑
    AudioSource backgroundMusicSource;  // 背景音效

    bool enabledSound = true;
    bool enabledMusic = true;
    const string SoundTag = "Sound";
    const string MusicTag = "Music";
    const string SoundVolumeTag = "SoundVolume";
    const string MusicVolumeTag = "MusicVolume";

    /// <summary>
    /// 音频剪辑列表
    /// </summary>
    Dictionary<string, AudioClip> audioClipList = new Dictionary<string, AudioClip>();
    /// <summary>
    /// 音频剪辑列表
    /// </summary>
    Dictionary<string, AssetBundle> assetBundleList = new Dictionary<string, AssetBundle>();
    void Awake()
    {
        mInst = this;
    }

    void Start()
    {
        soundSource = GetAudioSource("Sound");
        sceneMusicSource = GetAudioSource("SceneMusic");
        backgroundMusicSource = GetAudioSource("BackgroundMusic");

        enabledSound = IsEnabledSound(SoundTag);
        enabledMusic = IsEnabledSound(MusicTag);
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="_clipName">音效名字，需要加上后缀</param>
    public void PlaySound_ClipName(string _clipName)
    {
        if (!enabledSound)
        {
            return;
        }
        StartCoroutine(LoadAudioClip(_clipName, PlaySound_Clip));
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="_clip">音效剪辑</param>
    public void PlaySound_Clip(AudioClip _clip)
    {
        if (!enabledSound)
        {
            return;
        }

        if (!soundSource)
        {
            return;
        }

        soundSource.clip = _clip;
        soundSource.volume = GetSoundVolume();
        soundSource.Play();
    }

    /// <summary>
    /// 播放场景音乐，某个功能模块需要的特殊音乐
    /// </summary>
    /// <param name="_clipName">音乐名字，需要加上后缀</param>
    public void PlaySceneMusic_ClipName(string _clipName)
    {
        if (!enabledMusic)
        {
            return;
        }

        StartCoroutine(LoadAudioClip(_clipName, PlaySceneMusic_Clip));
    }

    /// <summary>
    /// 播放场景音乐，某个功能模块需要的特殊音乐
    /// </summary>
    /// <param name="_clip">音乐剪辑</param>
    public void PlaySceneMusic_Clip(AudioClip _clip)
    {
        if (!enabledMusic)
        {
            return;
        }

        if (!sceneMusicSource)
        {
            return;
        }

        sceneMusicSource.loop = true;
        sceneMusicSource.clip = _clip;
        sceneMusicSource.volume = GetMusicVolume();
        sceneMusicSource.Play();

    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="_clipName">音乐名字，需要加上后缀</param>
    public void PlayBackgroundMusic_ClipName(string _clipName)
    {
        if (!enabledMusic)
        {
            return;
        }

        StartCoroutine(LoadAudioClip(_clipName, PlayBackgroundMusic_Clip));
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="_clip">音乐剪辑</param>
    public void PlayBackgroundMusic_Clip(AudioClip _clip)
    {
        if (!enabledMusic)
        {
            return;
        }

        if (!backgroundMusicSource)
        {
            return;
        }

        backgroundMusicClip = _clip;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.clip = _clip;
        backgroundMusicSource.volume = GetMusicVolume();
        backgroundMusicSource.Play();
    }

    public void SetMusicVolume(float _volume)
    {
        if (!sceneMusicSource)
        {
            return;
        }

        if (!backgroundMusicSource)
        {
            return;
        }

        float tempVolume = 0;
        if (_volume > 0)
        {
            tempVolume = _volume / 100;
        }

        sceneMusicSource.volume = tempVolume;
        backgroundMusicSource.volume = tempVolume;
        PlayerPrefs.SetFloat(MusicVolumeTag, tempVolume);
    }

    public void SetSoundVolume(float _volume)
    {
        if (!soundSource)
        {
            return;
        }

        float tempVolume = 0;
        if (_volume > 0)
        {
            tempVolume = _volume / 100;
        }

        soundSource.volume = tempVolume;
        PlayerPrefs.SetFloat(SoundVolumeTag, tempVolume);
    }

    public void SetAllVolume(float _volume)
    {
        SetMusicVolume(_volume);
        SetSoundVolume(_volume);
    }

    /// <summary>
    /// 获取音乐音量大小
    /// </summary>
    /// <returns></returns>
    public float GetMusicVolume()
    {
        if (!PlayerPrefs.HasKey(MusicVolumeTag))
        {
            PlayerPrefs.SetFloat(MusicVolumeTag, 1);
        }

        return PlayerPrefs.GetFloat(MusicVolumeTag);
    }

    /// <summary>
    /// 获取音效音量大小
    /// </summary>
    public float GetSoundVolume()
    {
        if (!PlayerPrefs.HasKey(SoundVolumeTag))
        {
            PlayerPrefs.SetFloat(SoundVolumeTag, 1);
        }

        return PlayerPrefs.GetFloat(SoundVolumeTag);
    }

    /// <summary>
    /// 设置音效是否关闭
    /// </summary>
    /// <param name="_enabledSound"></param>
    public void EnabledSound(bool _enabledSound)
    {
        enabledSound = _enabledSound;
        if (!enabledSound && soundSource != null)
        {
            soundSource.Stop();
        }

        PlayerPrefs.SetInt(SoundTag, enabledSound ? 1 : 0);
    }

    /// <summary>
    /// 设置背景音乐
    /// </summary>
    /// <param name="_enabledMusic"></param>
    public void EnabledMusic(bool _enabledMusic)
    {
        enabledMusic = _enabledMusic;
        if (enabledMusic)
        {
            if (backgroundMusicClip != null)
            {
                PlayBackgroundMusic_Clip(backgroundMusicClip);
            }
        }
        else
        {
            if (sceneMusicSource != null)
            {
                sceneMusicSource.Stop();
            }

            if (backgroundMusicSource != null)
            {
                backgroundMusicSource.Stop();
            }
        }

        PlayerPrefs.SetInt(MusicTag, enabledMusic ? 1 : 0);
    }

    public void EnabledAll(bool _enabledAll)
    {
        EnabledSound(_enabledAll);
        EnabledMusic(_enabledAll);
    }

    public void UnloadBundle(string _fileName)
    {
        if (assetBundleList.ContainsKey(_fileName))
        {
            assetBundleList[_fileName].Unload(false);
            assetBundleList.Remove(_fileName);
        }

        if (audioClipList.ContainsKey(_fileName))
        {
            audioClipList.Remove(_fileName);
        }
    }

    bool IsEnabledSound(string _keyTag)
    {
        if (!PlayerPrefs.HasKey(_keyTag))
        {
            PlayerPrefs.SetInt(_keyTag, 1);
        }

        return PlayerPrefs.GetInt(_keyTag) == 1;
    }

    AudioSource GetAudioSource(string _objectName)
    {
        GameObject tempSoundObj = new GameObject(_objectName);
        tempSoundObj.transform.parent = transform;

        return tempSoundObj.AddComponent<AudioSource>();
    }

    IEnumerator LoadAudioClip(string _fileNName, Action<AudioClip> _callback)
    {
        if (audioClipList.ContainsKey(_fileNName))
        {
            if (_callback != null)
            {
                _callback(audioClipList[_fileNName]);
            }
        }
        else
        {
            bool tempIsHttp = _fileNName.Contains("http://");
            if (tempIsHttp)
            {
                WWW tempWWW = new WWW(_fileNName);
                yield return tempWWW;

                if (!string.IsNullOrEmpty(tempWWW.error))
                {
                    Debug.LogError("load audioClip fail -----> " + tempWWW.error);
                    yield break;
                }
                else
                {
                    if (tempWWW.isDone)
                    {
                        AudioClip tempAudioClip = tempWWW.audioClip;
                        audioClipList.Add(_fileNName, tempAudioClip);
                        _callback(tempAudioClip);
                    }
                }

                tempWWW.Dispose();
            }
            else
            {
                AssetBundle tempBundle = AssetBundle.LoadFromFile(LuaFramework.Util.GetTargetAssetBundlesPath(_fileNName));
                yield return null;

                try
                {
                    AudioClip tempAudioClip = tempBundle.LoadAllAssets<AudioClip>()[0];
                    audioClipList.Add(_fileNName, tempAudioClip);
                    assetBundleList.Add(_fileNName, tempBundle);
                    _callback(tempAudioClip);
                }
                catch (Exception e)
                {
                    Debug.LogError("System.Exception---------------------e: " + e);
                }
            }
        }
    }
}