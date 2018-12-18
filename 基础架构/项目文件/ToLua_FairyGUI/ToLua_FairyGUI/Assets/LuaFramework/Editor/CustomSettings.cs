using System;
using UnityEditor;
using UnityEngine;
using LuaInterface;
using LuaFramework;
using UnityEngine.UI;
using System.Collections.Generic;
using BindType = ToLuaMenu.BindType;

public static class CustomSettings
{
    public static string FrameworkPath = AppConst.FrameworkRoot;
    public static string saveDir = FrameworkPath + "/ToLua/Source/Generate/";
    public static string luaDir = FrameworkPath + "/Lua/";
    public static string toluaBaseType = FrameworkPath + "/ToLua/BaseType/";
    public static string baseLuaDir = FrameworkPath + "/ToLua/Lua";
    public static string injectionFilesPath = Application.dataPath + "/ToLua/Injection/";

    //导出时强制做为静态类的类型(注意customTypeList 还要添加这个类型才能导出)
    //unity 有些类作为sealed class, 其实完全等价于静态类
    public static List<Type> staticClassTypes = new List<Type>
    {
        typeof(GL),
        typeof(Time),
        typeof(Input),
        typeof(Screen),
        typeof(Physics),
        typeof(Graphics),
        typeof(Resources),
        typeof(Application),
        typeof(SleepTimeout),
        typeof(RenderSettings),
        typeof(QualitySettings),
    };

    //附加导出委托类型(在导出委托时, customTypeList 中牵扯的委托类型都会导出， 无需写在这里)
    public static DelegateType[] customDelegateList =
    {
        _DT(typeof(Action)),
        _DT(typeof(Action<int>)),
        _DT(typeof(Predicate<int>)),
        _DT(typeof(Func<int, int>)),
        _DT(typeof(Comparison<int>)),
        _DT(typeof(UnityEngine.Events.UnityAction)),
    };

    //在这里添加你要导出注册到lua的类型列表
    public static BindType[] customTypeList =
    {
        _GT(typeof(WWW)),
        _GT(typeof(Time)),
        _GT(typeof(Text)),
        _GT(typeof(Color)),
        _GT(typeof(Light)),
        _GT(typeof(Space)),
        _GT(typeof(Camera)),
        _GT(typeof(Screen)),
        _GT(typeof(Shader)),
        _GT(typeof(Color32)),
        _GT(typeof(Physics)),
        _GT(typeof(Texture)),
        _GT(typeof(Animator)),
        _GT(typeof(Collider)),
        _GT(typeof(Material)),
        _GT(typeof(PlayMode)),
        _GT(typeof(Renderer)),
        _GT(typeof(WrapMode)),
        _GT(typeof(Animation)),
        _GT(typeof(AudioClip)),
        _GT(typeof(Behaviour)),
        _GT(typeof(Component)),
        _GT(typeof(LightType)),
        _GT(typeof(Rigidbody)),
        _GT(typeof(Resources)),
        _GT(typeof(Transform)),
        _GT(typeof(Texture2D)),
        _GT(typeof(QueueMode)),
        _GT(typeof(GameObject)),
        _GT(typeof(InjectType)),
        _GT(typeof(AudioSource)),
        _GT(typeof(Application)),
        _GT(typeof(AssetBundle)),
        _GT(typeof(BoxCollider)),
        _GT(typeof(BlendWeights)),
        _GT(typeof(MeshCollider)),
        _GT(typeof(MeshRenderer)),
        _GT(typeof(SleepTimeout)),
        _GT(typeof(MonoBehaviour)),
        _GT(typeof(RectTransform)),
        _GT(typeof(RenderTexture)),
        _GT(typeof(AnimationState)),
        _GT(typeof(SphereCollider)),
        _GT(typeof(ParticleSystem)),
        _GT(typeof(RenderSettings)),
        _GT(typeof(CapsuleCollider)),
        _GT(typeof(ParticleEmitter)),
        _GT(typeof(QualitySettings)),
        _GT(typeof(CameraClearFlags)),
        _GT(typeof(TrackedReference)),
        _GT(typeof(ParticleRenderer)),
        _GT(typeof(ParticleAnimator)),
        _GT(typeof(AnimationBlendMode)),
        _GT(typeof(AssetBundleManifest)),
        _GT(typeof(CharacterController)),
        _GT(typeof(LuaInjectionStation)),
        _GT(typeof(SkinnedMeshRenderer)),
        _GT(typeof(Debugger)).SetNameSpace(null),
        _GT(typeof(AsyncOperation)).SetBaseType(typeof(System.Object)),
        _GT(typeof(AnimationClip)).SetBaseType(typeof(UnityEngine.Object)),

        //LuaFramework
        _GT(typeof(Util)),
        _GT(typeof(AppConst)),
        _GT(typeof(LuaManager)),

        // ==================== 自定义模块 ====================
        // 网络模块
        _GT(typeof(GameFramework.SocketTcp)),
        _GT(typeof(GameFramework.ByteBuffer)),
        _GT(typeof(GameFramework.NetManager)),
        _GT(typeof(GameFramework.SessionData)),
        _GT(typeof(GameFramework.SocketClient)),
        _GT(typeof(GameFramework.SocketClientProxy)),

        // Fairy模块
        _GT(typeof(MyGLoader)),
        _GT(typeof(IconManager)),
        _GT(typeof(FairyManager)),
        _GT(typeof(MyLoaderExtension)),

        // 定时器
        _GT(typeof(TimeManager)),

        // 音频
        _GT(typeof(AudioManager)),
    };

    public static List<Type> dynamicList = new List<Type>()
    {
        // 粒子
        typeof(MeshRenderer),
        typeof(ParticleEmitter),
        typeof(ParticleRenderer),
        typeof(ParticleAnimator),

        // 碰撞
        typeof(BoxCollider),
        typeof(MeshCollider),
        typeof(SphereCollider),
        typeof(CharacterController),
        typeof(CapsuleCollider),

        // 动画
        typeof(Animation),
        typeof(AnimationClip),
        typeof(AnimationState),

        // 物理
        typeof(Rigidbody),
        typeof(BlendWeights),
        typeof(RenderTexture),
    };

    //重载函数，相同参数个数，相同位置out参数匹配出问题时, 需要强制匹配解决
    public static List<Type> outList = new List<Type>() { };
    //ngui优化，下面的类没有派生类，可以作为sealed class
    public static List<Type> sealedList = new List<Type>() { };
    public static BindType _GT(Type t) { return new BindType(t); }
    public static DelegateType _DT(Type t) { return new DelegateType(t); }

    // [MenuItem("Lua/Attach Profiler", false, 151)]
    static void AttachProfiler()
    {
        if (!Application.isPlaying)
        {
            EditorUtility.DisplayDialog("警告", "请在运行时执行此功能", "确定");
            return;
        }

        LuaClient.Instance.AttachProfiler();
    }

    // [MenuItem("Lua/Detach Profiler", false, 152)]
    static void DetachProfiler()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        LuaClient.Instance.DetachProfiler();
    }
}