using FairyGUI;
using UnityEngine;
using LuaInterface;
using System.Collections.Generic;

public class FairyManager : MonoBehaviour
{
    public static FairyManager mInst;
    void Awake()
    {
        mInst = this;
    }

    int instanceID = 0;
    Dictionary<string, Dictionary<string, GObject>> packageList = new Dictionary<string, Dictionary<string, GObject>>();
    Dictionary<string, Dictionary<GObject, string>> packageObjectList = new Dictionary<string, Dictionary<GObject, string>>(); 

    #region ////////// 生成 //////////
    /// <summary>
    /// 添加页面
    /// </summary>
    public void AddPackage(AssetBundle _bundle)
    {
        if (_bundle == null)
        {
            LogError("_bundle value null");
            return;
        }

        UIPackage.AddPackage(_bundle);
    }

    /// <summary>
    /// 添加页面对象
    /// </summary>
    public string AddPackageObject(AssetBundle _bundle, string _packageName, string _viewName)
    {
        AddPackage(_bundle);
        return CreateObject(_packageName, _viewName);
    }

    /// <summary>
    /// 创建页面对象
    /// </summary>
    public string CreateObject(string _packageName, string _viewName)
    {
        GObject tempMainView = UIPackage.CreateObject(_packageName, _viewName).asCom;
        tempMainView.SetSize(GRoot.inst.width, GRoot.inst.height);
        tempMainView.AddRelation(GRoot.inst, RelationType.Size);
        GRoot.inst.AddChild(tempMainView);

        string tempPackageViewName = GetPackageViewName(_packageName, _viewName);
        string tempObjectId = GetObjectId(tempPackageViewName, tempMainView);
        if (packageList.ContainsKey(tempPackageViewName))
        {
            packageList[tempPackageViewName].Add(tempObjectId, tempMainView);
        }
        else
        {
            Dictionary<string, GObject> tempList = new Dictionary<string, GObject>();
            tempList.Add(tempObjectId, tempMainView);
            packageList.Add(tempPackageViewName, tempList);
        }

        return tempPackageViewName + "/" + tempObjectId;
    }

    public string AddChildObject(string _comName, string _childComName)
    {
        GObject tempComObj = GetFairyObject(_comName);
        if (tempComObj == null)
        {
            LogError("tempComObj value nil");
            return "";
        }

        GObject tempChildObj = GetFairyObject(_childComName);
        if (tempChildObj == null)
        {
            LogError("tempChildObj value nil");
            return "";
        }

        if (tempComObj.asCom == null)
        {
            LogError("tempComObj.asCom value nil");
            return "";
        }

        tempComObj.asCom.AddChild(tempChildObj);
        return _childComName;
    }

    /// <summary>
    /// 移除页面
    /// </summary>
    public void RemovePackage(string _comName)
    {
        GObject tempObj = GetFairyObject(_comName);
        if (tempObj == null)
        {
            LogError("tempObj value nil");
            return;
        }

        string tempPackageName = GetPackageName(_comName);
        if (string.IsNullOrEmpty(tempPackageName))
        {
            LogError("包资源为空，无法卸载");
            return;
        }

        string[] temp = tempPackageName.Split('#');
        if (temp == null || temp.Length < 2)
        {
            LogError("无法解析包名 --> " + tempPackageName);
            return;
        }

        tempObj.Dispose();
        packageList.Remove(tempPackageName);
        packageObjectList.Remove(tempPackageName);
        UIPackage.RemovePackage(temp[0], true);
    }

    /// <summary>
    /// 移除页面对象
    /// </summary>
    public void RemovePackageObject(string _comName)
    {
        string tempPackageName = GetPackageName(_comName);
        GObject tempObj = GetFairyObject(_comName);
        if (tempObj != null)
        {
            if (packageObjectList.ContainsKey(tempPackageName) && packageObjectList[tempPackageName].ContainsKey(tempObj))
            {
                packageObjectList[tempPackageName].Remove(tempObj);
            }
            tempObj.Dispose();
        }

        if (!packageList.ContainsKey(tempPackageName))
        {
            return;
        }

        packageList[tempPackageName].Remove(GetFairyObjectName(_comName));
    }
    #endregion

    #region ////////// 字体 ////////// 
    public void RegisterFont(string _fontName, string _aliasName)
    {
        FontManager.RegisterFont(FontManager.GetFont(_fontName), _aliasName);
    }

    public void SetDefaultFont(string _defaultFont)
    {
        UIConfig.defaultFont = _defaultFont;
    }
    #endregion

    #region ////////// 坐标 ////////// 
    public void SetFairyObjectRotation(string _comName, float _rotation)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        tempCom.rotation = _rotation;
    }

    public void SetFairyObjectX(string _comName, int _x)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        tempCom.x = _x;
    }

    public void SetFairyObjectY(string _comName, int _y)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        tempCom.y = _y;
    }

    public void SetFairyObjectXY(string _comName, int _x, int _y)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        tempCom.x = _x;
        tempCom.y = _y;
    }

    public float GetFairyObjectX(string _comName)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return 0;
        }

        return tempCom.x;
    }

    public float GetFairyObjectY(string _comName)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return 0;
        }

        return tempCom.y;
    }

    public Vector2 GetFairyObjectXY(string _comName)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return new Vector2(0, 0);
        }

        return tempCom.xy;
    }
    #endregion

    #region ////////// 列表 //////////
    /// <summary>
    /// 设置列表下标索引
    /// </summary>
    public void SetFairyItemSelectedIndex(string _comName, int _selectedIndex)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        GList tempList = tempCom.asList;
        if (tempList == null)
        {
            LogError("tempList value nil");
            return;
        }

        tempList.selectedIndex = _selectedIndex;
    }

    public void SetFairyItemSelected(string _comName, int _selectedIndex)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        GList tempList = tempCom.asList;
        if (tempList == null)
        {
            LogError("tempList value nil");
            return;
        }

        if (_selectedIndex != -1)
        {
            tempList.AddSelection(_selectedIndex, true);
        }
        else
        {
            tempList.ClearSelection();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_comName"></param>
    /// <param name="_numItems">这个值不为空，会执行一次 numItems</param>
    /// <param name="_isVirtual">false 不复用列表</param>
    /// <param name="_selectedIndex">指定显示的下标，滚动显示并且至底层</param>
    public void SetFairyListNumItems(string _comName, int _numItems, bool _isVirtual, int _selectedIndex)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        GList tempList = tempCom.asList;
        if (tempList == null)
        {
            LogError("tempList value nil");
            return;
        }

        if (_isVirtual)
        {
            tempList.SetVirtual();
        }

        tempList.numItems = _numItems;
        if (_numItems > 0 && _selectedIndex != -1)
        {
            tempList.AddSelection(_selectedIndex, true);
        }
        else
        {
            tempList.ClearSelection();
        }
    }

    /// <summary>
    /// 设置列表点击
    /// </summary>
    /// <param name="_comName">对象查找路径</param>
    /// <param name="_callback">点击回调的函数</param>
    /// <param name="_selectedIndex">触发的事件索引</param>
    /// <param name="_activeReturnIndex">激活函数会返回单选列表的点击下标</param>
    public void SetFairyListClickItem(string _comName, LuaFunction _callback, int _selectedIndex, bool _activeReturnIndex)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        GList tempList = tempCom.asList;
        if (tempList == null)
        {
            LogError("tempList value nil");
            return;
        }

        if (_selectedIndex != -1)
        {
            tempList.AddSelection(_selectedIndex, true);
        }

        if (_activeReturnIndex)
        {
            tempList.onClickItem.Add(() => { if (_callback != null) { _callback.Call(tempList.selectedIndex); } });
        }
        else
        {
            tempList.onClickItem.Add((EventContext _context) => { if (_callback != null) { _callback.Call(_context); } });
        }
    }

    /// <summary>
    /// 设置列表渲染
    /// </summary>
    /// <param name="_callback">回调的函数</param>
    /// <param name="_numItems">这个值不为空，会执行一次 numItems</param>
    /// <param name="_isVirtual">虚拟列表会重复使用列表，不会一次性加载完</param>
    public void SetFairyListRenderer(string _comName, LuaFunction _callback, int _numItems, bool _isVirtual)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        GList tempList = tempCom.asList;
        if (tempList == null)
        {
            LogError("tempList value nil");
            return;
        }

        if (_isVirtual)
        {
            tempList.SetVirtual();
        }

        string tempPackageName = GetPackageName(_comName);
        tempList.itemRenderer = (int _index, GObject _item) => { if (_callback != null) { _callback.Call(_index, AddPackageChildObject(tempPackageName, _item)); } };
        if (_numItems >= 0)
        {
            SetFairyListNumItems(_comName, _numItems, _isVirtual, -1);
        }
    }
    #endregion

    #region ////////// 动画 //////////
    // _transitionObj ：播放动画的对象
    // _isPlayer      : 是否播放动画
    public void SetFairyTransitionPlay(string _comName, string _transitionName, bool _isPlayer, LuaFunction _playCallbackFun, LuaFunction _reverseCallbackFun, float _timeScale = 1)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        Transition tempTransition = tempCom.asCom.GetTransition(_transitionName);
        if (tempTransition == null)
        {
            LogError("tempTransition value nil");
            return;
        }

        if (tempTransition.playing)
        {
            return;
        }

        tempTransition.timeScale = _timeScale;
        if (_isPlayer)
        {
            tempTransition.Play(() => { if (_playCallbackFun != null) { _playCallbackFun.Call(); } });
        }
        else
        {
            tempTransition.PlayReverse(() => { if (_reverseCallbackFun != null) { _reverseCallbackFun.Call(); } });
        }
    }

    // _transitionObj ：播放动画的对象
    // _isPlayer      : 是否播放动画
    public void SetFairyTransitionTimeScale(string _comName, string _transitionName, float _timeScale = 1)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        Transition tempTransition = tempCom.asCom.GetTransition(_transitionName);
        if (tempTransition == null)
        {
            LogError("tempTransition value nil");
            return;
        }

        tempTransition.timeScale = _timeScale;
    }

    // _asCom ：停止动画的对象
    public void SetFairyTransitionStop(string _comName, string _transitionName)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        Transition tempTransition = tempCom.asCom.GetTransition(_transitionName);
        if (tempTransition == null)
        {
            LogError("tempTransition value nil");
            return;
        }

        tempTransition.Stop();
    }
    #endregion

    #region ////////// 按钮 //////////
    public void SetKeyDown(LuaFunction _callback)
    {
        Stage.inst.onKeyDown.Add((EventContext _context) => 
        {
            if (_callback != null)
            {
                _callback.Call(_context.inputEvent.keyCode);
            }
        });
    }
    #endregion

    #region ////////// 按钮 //////////
    public void SetFairySelected(string _comName, bool _isSelected)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        tempCom.asButton.selected = _isSelected;
    }

    // _asCom     : Fairy组件对象
    // _callback  ：点击回调的函数
    public void SetFairyClick(string _comName, LuaFunction _callback,bool _stopPropagation)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        tempCom.onClick.Clear();
        tempCom.onClick.Add((EventContext obj) => {
            if (_stopPropagation) obj.StopPropagation(); 
            if (_callback != null) { _callback.Call(); }
        });
    }

    // _asCom     : Fairy组件对象
    // _callback  ：点击回调的函数，返回参数 true 和 false
    public void SetFairyTouchEnd(string _comName, LuaFunction _callback)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        tempCom.onTouchEnd.Clear();
        tempCom.onTouchEnd.Add((EventContext _context) => { if (_callback != null) { _callback.Call(_context); } });
    }

    // _asCom     : Fairy组件对象
    // _callback  ：点击回调的函数，返回参数 true 和 false
    public void SetFairyTouchBegin(string _comName, LuaFunction _callback)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        tempCom.onTouchBegin.Clear();
        tempCom.onTouchBegin.Add((EventContext _context) => { if (_callback != null) { _callback.Call(_context); } });
    }

    // _asCom     : Fairy组件对象
    // _callback  ：点击回调的函数，返回参数 true 和 false
    public void SetFairyGlobalTouchEnd(string _comName, LuaFunction _callback)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        Stage.inst.onTouchEnd.Clear();
        Stage.inst.onTouchEnd.Add((EventContext _context) => { if (_callback != null) { _callback.Call(_context); } });
    }

    // _asCom     : Fairy组件对象
    // _callback  ：点击回调的函数，返回参数 true 和 false
    public void SetFairyGlobalTouchBegin(string _comName, LuaFunction _callback)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        Stage.inst.onTouchBegin.Clear();
        Stage.inst.onTouchBegin.Add((EventContext _context) => { if (_callback != null) { _callback.Call(_context); } });
    }

    // 设置按钮按下事件
    public void SetFairyBtnOnTouchBegainOrOut(string _comName, LuaFunction _callback, bool _isOver)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        if (_isOver)
        {
            tempCom.onTouchBegin.Clear();
            tempCom.onTouchBegin.Add(() => { if (_callback != null) { _callback.Call(); } });
        }
        else
        {
            tempCom.onTouchEnd.Clear();
            tempCom.onTouchEnd.Add(() => { if (_callback != null) { _callback.Call(); } });
        }
    }
    #endregion

    #region ////////// 宽高 //////////
    // _asCom     ：Fairy组件对象
    // _width     ：宽度
    public void SetFairyWidth(string _comName, int _width)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        tempCom.width = _width;
    }

    // _asCom     ：Fairy组件对象
    // _heigh     ：高度
    public void SetFairyHeigh(string _comName, int _height)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        tempCom.height = _height;
    }

    public float GetFairyWidth(string _comName)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return 0;
        }

        return tempCom.width;
    }

    // _asCom     ：Fairy组件对象
    // _heigh     ：高度
    public float GetFairyHeigh(string _comName)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return 0;
        }

        return tempCom.height;
    }
    #endregion

    #region ////////// 图标 //////////
    public void SetFairyIcon(string _comName, string _icon)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        tempCom.icon = _icon;
    }

    public void SetFairyItemIcon(string _comName, string _packageName, string _itemName)
    {
        SetFairyIcon(_comName, UIPackage.GetItemURL(_packageName, _itemName));
    }
    #endregion

    #region ////////// 文本 //////////
    // _asCom : Fairy组件对象
    // _value ：显示的值，值会自动转换 string 值
    public void SetFairyTitle(string _comName, string _value)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        tempCom.asCom.text = _value;
    }

    // _asCom : Fairy组件对象
    // _value ：显示的值，值会自动转换 string 值
    public void SetFairyText(string _comName, string _value)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        tempCom.text = _value;
    }

    public string GetFairyText(string _comName)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return "";
        }

        return tempCom.text;
    }
    #endregion

    #region ////////// 拖动 //////////
    // _asCom   ：Fairy组件对象
    // _callback   ：回包
    public void SetFairyOnDragEnd(string _comName, LuaFunction _callback)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }
        tempCom.onDragEnd.Add(() => { if (_callback != null) { _callback.Call(); } });
    }
    // _asCom   ：Fairy组件对象
    // _callback   ：回包
    public void SetFairyOnDragMove(string _comName, LuaFunction _callback)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }
        tempCom.onDragMove.Add(() => { if (_callback != null) { _callback.Call(); } });
    }
    // _asCom   ：Fairy组件对象
    // _callback   ：回包
    public void SetFairyOnDragStart(string _comName, LuaFunction _callback)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }
        tempCom.onDragStart.Add(() => { if (_callback != null) { _callback.Call(); } });
    }
    #endregion

    #region ////////// 组件 //////////
    // _asCom   ：Fairy组件对象
    // _alpha   ：透明度
    public void SetFairyAlpha(string _comName, float _alpha)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        tempCom.alpha = _alpha == 0 ? 0 : (_alpha / 100);
    }

    // _asCom    ：Fairy组件对象
    // _grayed   ：是否设置对象为灰度图
    // _canClick: 是否同时设置不可点击
    public void SetFairyGrayed(string _comName, bool _grayed)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        tempCom.grayed = _grayed;
    }

    // _asCom     : Fairy组件对象
    // _visible   ：是否隐藏组件
    public void SetFairyVisible(string _comName, bool _visible)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        if (tempCom.visible == _visible)
        {
            return;
        }

        tempCom.visible = _visible;
    }

    // _asCom       ：Fairy组件对象
    // _touchable   ：组件是否可触摸
    public void SetFairyTouchable(string _comName, bool _touchable)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        tempCom.touchable = _touchable;
    }

    // 设置 UI 层级
    // _asCom        ：Fairy组件对象
    // _sortingOrder ：目标值
    public void SetFairySortingOrder(string _comName, int _sortingOrder)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        tempCom.sortingOrder = _sortingOrder;
    }

    public void SetFairyControllerSelectedIndex(string _comName, string _controllerName, int _selectedIndex)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        FairyGUI.Controller tempController = tempCom.asCom.GetController(_controllerName);
        if (tempController == null)
        {
            LogError("tempController value nil");
            return;
        }

        tempController.selectedIndex = _selectedIndex;
    }
    #endregion

    #region ////////// 输入 //////////
    // 设置输入文本聚焦回调
    public void SetFairyInputTextOnFocusIn(string _comName, LuaFunction _callback)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        if (_callback == null)
        {
            LogError("_callback value nil");
            return;
        }

        tempCom.asTextInput.onFocusIn.Clear();
        tempCom.asTextInput.onFocusIn.Add(() => { _callback.Call(); });
    }

    // 设置输入文本离开聚焦回调
    public void SetFairyInputTextOnFocusOut(string _comName, LuaFunction _callback)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        if (_callback == null)
        {
            LogError("_callback value nil");
            return;
        }

        tempCom.asTextInput.onFocusOut.Clear();
        tempCom.asTextInput.onFocusOut.Add(() => { _callback.Call(); });
    }

    // 设置输入文本变动回调
    public void SetFairyInputOnChange(string _comName, LuaFunction _callback)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        if (_callback == null)
        {
            LogError("_callback value nil");
            return;
        }

        tempCom.asTextInput.onChanged.Clear();
        tempCom.asTextInput.onChanged.Add(() => { _callback.Call(); });
    }
    // 下拉框选择
    public void SetFairyComboBoxOnChange(string _comName, LuaFunction _callback)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        if (_callback == null)
        {
            LogError("_callback value nil");
            return;
        }
        tempCom.asComboBox.onChanged.Clear();
        tempCom.asComboBox.onChanged.Add(() => { _callback.Call(tempCom.asComboBox.selectedIndex); });
    }
    #endregion

    #region ////////// 滑动 //////////
    // _asCom     : 面板实例对象
    // _minValue  ：左值，用于除法运算
    // _maxValue  ：右值，用于除法运算
    public void SetFairyProgressValue(string _comName, float _minValue, float _maxValue)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        tempCom.asProgress.max = _maxValue;
        tempCom.asProgress.value = _minValue;
    }

    // _asCom     : 面板实例对象
    // _minValue  ：左值，用于除法运算
    // _maxValue  ：右值，用于除法运算
    public void SetFairySliderValue(string _comName, float _minValue, float _maxValue)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        if (tempCom.asSlider != null)
        {
            tempCom.asSlider.max = _maxValue;
            tempCom.asSlider.value = _minValue;
            return;
        }

        if (tempCom.asProgress != null)
        {
            tempCom.asProgress.max = _maxValue;
            tempCom.asProgress.value = _minValue;
        }
    }

    public void SetFairySliderScroll(string _comName, LuaFunction _callback)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }
        tempCom.asCom.scrollPane.onScroll.Clear();
        tempCom.asCom.scrollPane.onScroll.Add(() => { if (_callback != null) { _callback.Call(); } });
    }

    public void SetFairySliderScrollEnd(string _comName, LuaFunction _callback)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }
        tempCom.asCom.scrollPane.onScrollEnd.Clear();
        tempCom.asCom.scrollPane.onScrollEnd.Add(() => { if (_callback != null) { _callback.Call(); } });
    }

    public void SetFairySliderScrollPosX(string _comName, float _scrollPosX, bool _ani)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        tempCom.asCom.scrollPane.SetPosX(_scrollPosX, _ani);
    }

    public void SetFairySliderScrollPosY(string _comName, float _scrollPosY, bool _ani)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        tempCom.asCom.scrollPane.SetPosY(_scrollPosY, _ani);
    }

    public void SetFairySliderScrollPosXY(string _comName, float _scrollPosX, float _scrollPosY, bool _ani)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        tempCom.asCom.scrollPane.SetPosX(_scrollPosX, _ani);
        tempCom.asCom.scrollPane.SetPosY(_scrollPosY, _ani);
    }

    public void SetFairySliderScrollPageX(string _comName, int _scrollPosX)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        tempCom.asList.scrollPane.currentPageX = _scrollPosX;
    }

    public void SetFairySliderScrollPageY(string _comName, int _scrollPosY)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        tempCom.asList.scrollPane.currentPageY = _scrollPosY;
    }

    public void SetFairySliderScrollPageXY(string _comName, int _scrollPosX, int _scrollPosY)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        tempCom.asList.scrollPane.currentPageY = _scrollPosY;
        tempCom.asList.scrollPane.currentPageY = _scrollPosY;
    }

    public float GetFairySliderScrollX(string _comName)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return 0;
        }

        return tempCom.asCom.scrollPane.scrollingPosX;
    }

    public float GetFairySliderScrollY(string _comName)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return 0;
        }

        return tempCom.asCom.scrollPane.scrollingPosY;
    }

    public float GetFairySliderContentHeightOrWidth(string _comName,int _type)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return 0;
        }

        if (_type == 1)
        {
            return tempCom.asCom.scrollPane.contentHeight;
        }
        else
        {
            return tempCom.asCom.scrollPane.contentWidth;
        }
    }

    public float GetFairySliderViewHeightOrWidth(string _comName, int _type)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return 0;
        }
        if (_type == 1)
            return tempCom.asCom.scrollPane.viewHeight;
        else
            return tempCom.asCom.scrollPane.viewWidth;
    }
    public void SetFairySliderGripTouchEnd(string _comName,LuaFunction _callBack) {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }
        tempCom.asSlider.onGripTouchEnd.Clear();
        tempCom.asSlider.onGripTouchEnd.Add(()=> { if(_callBack!=null) _callBack.Call(tempCom.asSlider.value); });
    }
    public void SetFairySliderOnChange(string _comName, LuaFunction _callBack)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }
        tempCom.asSlider.onChanged.Clear();
        tempCom.asSlider.onChanged.Add(() => { if (_callBack != null) _callBack.Call(tempCom.asSlider.value); });
    }
    #endregion

    #region ////////// 填充 //////////
    // _asCom        ：Fairy组件对象
    // _fillAmount   ：目标值
    public void SetFairyFillAmount(string _comName, float _fillAmount, float _fillTotalAmount)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }
        
        tempCom.asImage.fillAmount = _fillAmount == 0 ? 0 : (_fillAmount / _fillTotalAmount);
    }
    #endregion

    #region ////////// 颜色 //////////
    // _asCom : Fairy组件对象
    // _color ：颜色值
    public void SetFairyLoaderColor(string _comName, byte _r, byte _g, byte _b, byte _a)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        tempCom.asLoader.color = new Color32(_r, _g, _b, _a);
    }
    public void SetFairyImageColor(string _comName, float _r, float _g, float _b, float _a)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        tempCom.asImage.color = new Color(_r, _g, _b, _a);
    }
    #endregion

    #region ////////// 加载 //////////
    public void SetFairyNativeObject(string _comName, GameObject _go)
    {
        GObject tempCom = GetFairyObject(_comName);
        if (tempCom == null)
        {
            LogError("_asCom value nil");
            return;
        }

        GameObject tempGo = Object.Instantiate(_go);
        tempCom.asGraph.SetNativeObject(new GoWrapper(tempGo));
    }
    #endregion

    #region ////////// 缩放 //////////
    public void SetFairyObjectScale(string _comName, float _x, float _y)
    {
        GObject tempObj = GetFairyObject(_comName);
        if (tempObj == null || tempObj.asCom == null)
        {
            LogError("tempObj value nil");
            return;
        }

        tempObj.SetScale(_x, _y);
    }
    #endregion

    #region ////////// 返回 //////////
    public string GetFairyItemURL(string _packageName, string _itemName)
    {
        return UIPackage.GetItemURL(_packageName, _itemName);
    }

    public string GetFairyChildObject(string _comName, string _childName)
    {
        GObject tempObj = GetFairyObject(_comName);
        if (tempObj == null || tempObj.asCom == null)
        {
            LogError("tempObj value nil");
            return "";
        }

        GObject tempChildObj = tempObj.asCom.GetChild(_childName);
        if (tempChildObj == null)
        {
            LogError("tempChildObj value nil");
            return "";
        }

        return AddPackageChildObject(GetPackageName(_comName), tempChildObj);
    }

    string AddPackageChildObject(string _packageName, GObject _childObj)
    {
        if (!packageList.ContainsKey(_packageName))
        {
            LogError(_packageName + " 不存在包列表");
            return "";
        }

        string tempObjectId = GetObjectId(_packageName, _childObj);
        if (!packageList[_packageName].ContainsKey(tempObjectId))
        {
            packageList[_packageName].Add(tempObjectId, _childObj);
        }

        return _packageName + "/" + tempObjectId;
    }

    GObject GetFairyObject(string _comName)
    {
        if (string.IsNullOrEmpty(_comName))
        {
            LogError("字符串为空");
            return null;
        }

        string[] tempSplit = _comName.Split('/');
        if (tempSplit == null || tempSplit.Length < 2)
        {
            return null;
        }

        if (!packageList.ContainsKey(tempSplit[0]))
        {
            return null;
        }

        Dictionary<string, GObject> tempList = packageList[tempSplit[0]];
        return tempList[tempSplit[1]];
    }

    string GetFairyObjectName(string _comName)
    {
        string[] tempSplit = _comName.Split('/');
        if (tempSplit == null || tempSplit.Length < 2)
        {
            return "";
        }

        return tempSplit[1];
    }

    string GetPackageName(string _comName)
    {
        string[] tempSplit = _comName.Split('/');
        if (tempSplit == null || tempSplit.Length < 2)
        {
            return "";
        }

        return tempSplit[0];
    }

    string GetObjectId(string _packageName, GObject _obj)
    {
        if (_obj == null)
        {
            Debug.LogError("_obj value nil");
            return "";
        }

        if (!packageObjectList.ContainsKey(_packageName))
        {
            Dictionary<GObject, string> tempList = new Dictionary<GObject, string>();
            packageObjectList.Add(_packageName, tempList);
        }

        if (packageObjectList[_packageName].ContainsKey(_obj))
        {
            return packageObjectList[_packageName][_obj];
        }

        instanceID += 1;
        string tempPackageObjectCount = instanceID.ToString();
        packageObjectList[_packageName].Add(_obj, tempPackageObjectCount);
        return tempPackageObjectCount;
    }

    string GetPackageViewName(string _packageName, string _viewName)
    {
        return _packageName + "#" + _viewName;
    }
    #endregion

    #region ////////// 对象 //////////
    public Transform GetCachedTransform(string _comName)
    {
        GObject tempObj = GetFairyObject(_comName);
        if (tempObj == null || tempObj.asCom == null)
        {
            LogError("tempObj value nil");
            return null;
        }

        return tempObj.displayObject.cachedTransform;
    }
    #endregion

    #region ////////// 合并 //////////
    public void SetFairyBatching(string _comName, bool _fairyBatching)
    {
        GObject tempObj = GetFairyObject(_comName);
        if (tempObj == null || tempObj.asCom == null)
        {
            LogError("tempObj value nil");
            return;
        }

        tempObj.asCom.fairyBatching = _fairyBatching;
    }
    #endregion

    void LogError(string _str)
    {
        // Debug.LogError(_str);
    }
}