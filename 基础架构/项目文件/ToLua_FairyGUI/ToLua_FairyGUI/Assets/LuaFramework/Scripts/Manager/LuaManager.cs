using UnityEngine;
using LuaInterface;

namespace LuaFramework
{
    public class LuaManager : MonoBehaviour
    {
        LuaState lua;
        LuaLooper loop = null;
        public static LuaManager mInst;

        void Awake()
        {
            mInst = this;

            lua = new LuaState();
            // 初始化加载第三方库
            lua.OpenLibs(LuaDLL.luaopen_pb);
            lua.OpenLibs(LuaDLL.luaopen_sproto_core);
            lua.OpenLibs(LuaDLL.luaopen_protobuf_c);
            lua.OpenLibs(LuaDLL.luaopen_lpeg);
            lua.OpenLibs(LuaDLL.luaopen_bit);
            lua.OpenLibs(LuaDLL.luaopen_socket_core);

            //cjson 比较特殊，只new了一个table，没有注册库，这里注册一下
            lua.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
            lua.OpenLibs(LuaDLL.luaopen_cjson);
            lua.LuaSetField(-2, "cjson");
            lua.OpenLibs(LuaDLL.luaopen_cjson_safe);
            lua.LuaSetField(-2, "cjson.safe");
            lua.LuaSetTop(0);

            LuaBinder.Bind(lua);
            DelegateFactory.Init();
            LuaCoroutine.Register(lua, this);
        }

        public void InitStart()
        {
            AddSearchBundle();

            lua.Start();        //启动LUAVM
            StartLooper();
            InitLuaBundle();
        }

        /// <summary>
        /// 初始化LuaBundle
        /// </summary>
        void InitLuaBundle()
        {
            DoFile("Logic/Game");               // 加载游戏
            CallMethod("Game", "OnInitOK");   // 初始化完成
        }

        void AddSearchBundle()
        {
            if (!Application.isMobilePlatform)
            {
                return;
            }

            if (lua != null)
            {
                // 加载Lua文件的AB包
                lua.AddSearchBundle(Util.LuaPath);
            }
        }

        void StartLooper()
        {
            loop = gameObject.AddComponent<LuaLooper>();
            loop.luaState = lua;
        }

        public void DoFile(string _fileName)
        {
            lua.DoFile(_fileName.ToLower());
        }

        public void CallMethod(string _module, string _funcName, params object[] _args)
        {
            LuaFunction func = lua.GetFunction(_module + "." + _funcName);
            if (func != null)
            {
                func.Call(_args);
            }
        }

        public void LuaGC()
        {
            lua.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
        }

        public void Close()
        {
            loop.Destroy();
            loop = null;

            lua.Dispose();
            lua = null;
        }
    }
}