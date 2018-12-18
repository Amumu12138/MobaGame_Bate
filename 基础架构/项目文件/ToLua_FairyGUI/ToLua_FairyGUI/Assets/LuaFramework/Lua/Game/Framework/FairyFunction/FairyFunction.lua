--[Comment]
-- 加载资源包并返回
function GetFairyAssetBundle(_bundleName)
    return UnityEngine.AssetBundle.LoadFromFile(Util.GetTargetAssetBundlesPath("fairy_assets/fairy_" .. tostring(_bundleName)));
end

--------------------------------------------- 添加组件 ---------------------------------------------
--[Comment]
-- 添加Fairy组件，加载包名并且加载组件，一般在打开新模块的时候用
-- _packageName ：包名
-- _viewName    ：组件名
function AddFairyPackage(_packageName)
    return FairyManager:AddPackage(GetFairyAssetBundle(_packageName));
end

--[Comment]
-- 添加Fairy组件，加载包名并且加载组件，一般在打开新模块的时候用
-- _packageName ：包名
-- _viewName    ：组件名
function AddPackageObject(_packageName, _viewName)
    return FairyManager:AddPackageObject(GetFairyAssetBundle(_packageName), _packageName, _viewName);
end

--[Comment]
-- 添加Fairy组件，加载一个组件
-- _packageName ：包名
-- _viewName    ：组件名
function CreateObject(_packageName, _viewName)
    return FairyManager:CreateObject(_packageName, _viewName);
end

--[Comment]
-- 创建一个新对象，并作为目标对象的子对象
-- _asCom       ：Fairy组件
-- _packageName ：包名
-- _viewName    ：组件名
function AddFairyChildObject(_asCom, _packageName, _viewName)
    return FairyManager:AddChildObject(_asCom, CreateObject(_packageName, _viewName));
end

--------------------------------------------- 移除组件 ---------------------------------------------
--[Comment]
-- 移除组件对象
-- _asCom       ：Fairy组件
function RemovePackageObject(_asCom) FairyManager:RemovePackageObject(_asCom); end
--[Comment]
-- 移除资源包名
-- _asCom       ：Fairy组件
function RemoveFairyPackage(_packageName) FairyManager:RemovePackage(_packageName); end

----------------------------------------------- 字体 -----------------------------------------------
--[Comment]
-- 注册Fairy使用的字体
-- _fontName  ：字体名
-- _aliasName ：字体别名
function RegisterFairyFont(_fontName, _aliasName) FairyManager:RegisterFont(_fontName, _aliasName); end
--[Comment]
-- 设置Fairy的默认字体
-- _fontName  ：字体名
function SetFairyDefaultFont(_fontName) FairyManager:SetDefaultFont(_fontName); end

--------------------------------------------- 获取对象 ---------------------------------------------
--[Comment]
-- 获取Unity对象
-- _asCom       ：Fairy组件
function GetCachedTransform(_asCom)
    return FairyManager:GetCachedTransform(_asCom);
end

--[Comment]
-- 获取 Fairy 子对象
-- _asCom      ：Fairy组件对象
-- _childName  ：子对象名字
function GetFairyChild(_asCom, _childName)
    if _asCom == nil then
        logError("GetChild _asCom value nil");
        return nil;
    end
    
    return FairyManager:GetFairyChildObject(_asCom, _childName);
end

--------------------------------------------- 获取文本 ---------------------------------------------
--[Comment]
-- 获取Fairy对象的文本信息
-- _asCom      ：Fairy组件对象
function GetFairyText(_asCom) return FairyManager:GetFairyText(_asCom); end
--[Comment]
-- 获取Fairy对象的文本信息
-- _asCom     ：Fairy组件对象
-- _childName ：子对象的名字
function GetFairyChildText(_asCom, _childName) return GetFairyText(GetFairyChild(_asCom, _childName)); end

--------------------------------------------- 获取宽高 ---------------------------------------------
--[Comment]
-- 获取Fairy对象的宽度
-- _asCom     ：Fairy组件对象
function GetFairyWidth(_asCom) return FairyManager:GetFairyWidth(_asCom); end
--[Comment]
-- 获取Fairy对象的宽度
-- _asCom     ：Fairy组件对象
-- _childName ：子对象的名字
function GetFairyChildWidth(_asCom, _childName) return GetFairyWidth(GetFairyChild(_asCom, _childName)); end

--[Comment]
-- 获取Fairy对象的高度
-- _asCom     ：Fairy组件对象
function GetFairyHeigh(_asCom) return FairyManager:GetFairyHeigh(_asCom); end
--[Comment]
-- 获取Fairy对象的高度
-- _asCom     ：Fairy组件对象
-- _childName ：子对象的名字
function GetFairyChildHeigh(_asCom, _childName) return GetFairyHeigh(GetFairyChild(_asCom, _childName)); end

--[Comment]
-- 获取Fairy滑动块的高度
-- _asCom     ：Fairy组件对象
function GetFairySliderContentHeight(_asCom) return FairyManager:GetFairySliderContentHeightOrWidth(_asCom, 1); end
--[Comment]
-- 获取Fairy滑动块组件的高度
-- _asCom     ：Fairy组件对象
-- _childName ：子对象的名字
function GetFairyChildSliderContentHeight(_asCom,_childName) return GetFairySliderContentHeight(GetFairyChild(_asCom,_childName)); end

--[Comment]
-- 获取Fairy滑动块组件的宽度
-- _asCom     ：Fairy组件对象
function GetFairySliderContentWidth(_asCom) return FairyManager:GetFairySliderContentHeightOrWidth(_asCom, 2); end
--[Comment]
-- 获取Fairy滑动块组件的宽度
-- _asCom     ：Fairy组件对象
-- _childName ：子对象的名字
function GetFairyChildSliderContentWidth(_asCom,_childName) return GetFairySliderContentWidth(GetFairyChild(_asCom,_childName)); end

--[Comment]
-- 获取Fairy滑动块页面的高度
-- _asCom     ：Fairy组件对象
function GetFairySliderViewHeight(_asCom) return FairyManager:GetFairySliderViewHeightOrWidth(_asCom, 1); end
--[Comment]
-- 获取Fairy滑动块页面的高度
-- _asCom     ：Fairy组件对象
-- _childName ：子对象的名字
function GetFairyChildSliderViewHeight(_asCom, _childName) return GetFairySliderViewHeightOrWidth(GetFairyChild(_asCom,_childName)); end

--[Comment]
-- 获取Fairy滑动块页面的宽度
-- _asCom     ：Fairy组件对象
function GetFairySliderViewWidth(_asCom,_type) return FairyManager:GetFairySliderViewHeightOrWidth(_asCom, 2); end
--[Comment]
-- 获取Fairy滑动块页面的宽度
-- _asCom     ：Fairy组件对象
-- _childName ：子对象的名字
function GetFairyChildSliderViewWidth(_asCom, _childName) return GetFairySliderViewHeightOrWidth(GetFairyChild(_asCom,_childName)); end

--------------------------------------------- 获取滑块 ---------------------------------------------
--[Comment]
-- 获取滑动块的X轴
-- _asCom     ：Fairy组件对象
function GetFairySliderScrollX(_asCom) return FairyManager:GetFairySliderScrollX(_asCom) end
--[Comment]
-- 获取滑动块的X轴
-- _asCom     ：Fairy组件对象
-- _childName ：子对象的名字
function GetFairyChildSliderScrollX(_asCom, _childName) return GetFairySliderScrollX(GetFairyChild(_asCom, _childName)); end

--[Comment]
-- 获取滑动块的Y轴
-- _asCom     ：Fairy组件对象
function GetFairySliderScrollY(_asCom) return FairyManager:GetFairySliderScrollY(_asCom) end
--[Comment]
-- 获取滑动块的Y轴
-- _asCom     ：Fairy组件对象
-- _childName ：子对象的名字
function GetFairyChildSliderScrollY(_asCom, _childName) return GetFairySliderScrollY(GetFairyChild(_asCom, _childName)); end

--------------------------------------------- 获取位置 ---------------------------------------------
--[Comment]
-- 获取Fairy组件X轴
-- _asCom     ：Fairy组件对象
function GetFairyObjectX(_asCom) return FairyManager:GetFairyObjectX(_asCom); end
--[Comment]
-- 获取Fairy组件X轴
-- _asCom     ：Fairy组件对象
-- _childName ：子对象的名字
function GetFairyChildObjectX(_asCom,_childName) return GetFairyObjectX(GetFairyChild(_asCom,_childName)); end

--[Comment]
-- 获取Fairy组件Y轴
-- _asCom     ：Fairy组件对象
function GetFairyObjectY(_asCom) return FairyManager:GetFairyObjectY(_asCom); end
--[Comment]
-- 获取Fairy组件Y轴
-- _asCom     ：Fairy组件对象
-- _childName ：子对象的名字
function GetFairyChildObjectY(_asCom,_childName) return GetFairyObjectY(GetFairyChild(_asCom,_childName)); end

--[Comment]
-- 获取Fairy组件XY轴（PS：返回一个Vector2，性能消耗比单个取值要大很多，慎用）
-- _asCom     ：Fairy组件对象
function GetFairyObjectXY(_asCom) return FairyManager:GetFairyObjectXY(_asCom); end
--[Comment]
-- 获取Fairy组件XY轴（PS：返回一个Vector2，性能消耗比单个取值要大很多，慎用）
-- _asCom     ：Fairy组件对象
-- _childName ：子对象的名字
function GetFairyChildObjectXY(_asCom,_childName) return GetFairyObjectXY(GetFairyChild(_asCom,_childName)); end

--------------------------------------------- 材质合并 ---------------------------------------------
--[Comment]
-- 设置Fairy对象材质合并
-- _asCom         ：Fairy组件对象
-- _isBatching    ：是否合并
function SetFairyBatching(_asCom, _isBatching) FairyManager:SetFairyBatching(_asCom, _isBatching); end
--[Comment]
-- 获取Fairy对象的高度
-- _asCom         ：Fairy组件对象
-- _childName     ：子对象的名字
-- _isBatching    ：是否合并
function SetFairyChildBatching(_asCom, _childName, _isBatching) SetFairyBatching(GetFairyChild(_asCom, _childName), _isBatching); end

----------------------------------------------- 列表 -----------------------------------------------
--[Comment]
-- Fairy列表组件点击事件
-- _asCom               ：Fairy组件对象
-- _callback            ：点击回调的函数
-- _selectedIndex       ：触发的事件索引
-- _activeReturnIndex   ：激活函数会返回单选列表的点击下标
function SetFairyListClickItem(_asCom, _callback, _selectedIndex, _activeReturnIndex)
	if(_selectedIndex == nil) then 
        _selectedIndex = -1; 
    end

	if(_activeReturnIndex == nil) then 
        _activeReturnIndex = false; 
    end
    FairyManager:SetFairyListClickItem(_asCom, _callback, _selectedIndex, _activeReturnIndex)
end
--[Comment]
-- Fairy列表组件点击事件
-- _asCom               : Fairy组件对象
-- _childName           ：子对象的名字
-- _callback            ：点击回调的函数
-- _selectedIndex       ：触发的事件索引
-- _activeReturnIndex   ：激活函数会返回单选列表的点击下标
function SetFairyChildListClickItem(_asCom, _childName, _callback, _selectedIndex, _activeReturnIndex)
    SetFairyListClickItem(GetFairyChild(_asCom, _childName), _callback, _selectedIndex, _activeReturnIndex); 
end

--[Comment]
-- 复位单选按钮列表的下标索引
-- _asCom : Fairy组件对象
function ResetFairyItemSelectedIndex(_asCom)
    FairyManager:SetFairyItemSelectedIndex(_asCom, -1);
end
--[Comment]
-- 复位单选按钮列表的下标索引
-- _asCom     ：Fairy组件对象
-- _childName ：子对象的名字
function ResetFairyChildItemSelectedIndex(_asCom, _childName) ResetFairyItemSelectedIndex(GetFairyChild(_asCom, _childName)); end

--[Comment]
-- 设置GList的selectIndex的值，这个函数不会跳转到目标下标位置
-- _asCom         ：Fairy组件对象
-- _selectedIndex : 下标值
function SetFairyItemSelectedIndex (_asCom, _selectedIndex) FairyManager:SetFairyItemSelectedIndex(_asCom, _selectedIndex) end
--[Comment]
-- 设置GList的selectIndex的值，这个函数不会跳转到目标下标位置
-- _asCom         ：Fairy组件对象
-- _childName     ：子对象的名字
-- _selectedIndex : 下标值
function SetFairyChildItemSelectedIndex(_asCom,_childName, _selectedIndex) 
    SetFairyItemSelectedIndex(GetFairyChild(_asCom,_childName),_selectedIndex);
end

--[Comment]
-- 设置GList的selectIndex的值，这个函数会直接跳转到目标下标位置
-- _asCom         ：Fairy组件对象
-- _selectedIndex : 下标值
function SetFairyItemSelected(_asCom,_index) FairyManager:SetFairyItemSelected(_asCom,_index); end
--[Comment]
-- 设置GList的selectIndex的值，这个函数会直接跳转到目标下标位置
-- _asCom         ：Fairy组件对象
-- _childName     ：子对象的名字
-- _selectedIndex : 下标值
function SetFairyChildItemSelected(_asCom,_childName,_index) SetFairyItemSelected(GetFairyChild(_asCom,_childName),_index); end

--[Comment]
-- 渲染Fairy列表
-- _asCom     : Fairy组件对象
-- _callback  ：回调的函数
-- _numItems  ：这个值不为空，会执行一次 numItems
function SetFairyListRenderer(_asCom, _callback, _numItems, _isVirtual)
    if _numItems == nil then
        _numItems = -1;
    end

    if _isVirtual == nil then
        _isVirtual = false;
    end

    FairyManager:SetFairyListRenderer(_asCom, _callback, _numItems, _isVirtual);
end

--[Comment]
-- 渲染Fairy列表
-- _asCom     : Fairy组件对象
-- _childName ：子对象的名字
-- _callback  ：点击回调的函数
-- _numItems  ：这个值不为空，会执行一次 numItems
-- _isVirtual ：是否自动复用
function SetFairyChildListRenderer(_asCom,_childName,_callback,_numItems,_isVirtual) 
    SetFairyListRenderer(GetFairyChild(_asCom,_childName),_callback,_numItems,_isVirtual); 
end

--[Comment]
-- 设置渲染Fairy列表数量
-- _asCom           ：Fairy组件对象
-- _numItems        ：这个值不为空，会执行一次 numItems
-- _isVirtual       ：false 不复用列表
-- _selectedIndex   ：指定显示的下标，滚动显示并且至底层
function SetFairyListNumItems(_asCom, _numItems, _isVirtual, _selectedIndex)	
	if(_isVirtual == nil) then 
        _isVirtual = false; 
    end

	if(_selectedIndex == nil) then 
        _selectedIndex = -1; 
    end
	FairyManager:SetFairyListNumItems(_asCom, _numItems,_isVirtual, _selectedIndex)
end
--[Comment]
-- 设置渲染Fairy列表数量
-- _asCom           ：Fairy组件对象
-- _childName       ：子对象的名字
-- _numItems        ：这个值不为空，会执行一次 numItems
-- _isVirtual       ：false 不复用列表
-- _selectedIndex   ：指定显示的下标，滚动显示并且至底层
function SetFairyChildListNumItems(_asCom, _childName, _numItems, _isVirtual, _selectedIndex)
    SetFairyListNumItems(GetFairyChild(_asCom, _childName), _numItems, _isVirtual, _selectedIndex);
end

----------------------------------------------- 动画 -----------------------------------------------
--[Comment]
-- 播放动画
-- _asCom               ：Fairy组件对象
-- _transitionName      ：动画名字
-- _isPlayer            ：是否播放动画
-- _playCallbackFun     ：顺播回调
-- _reverseCallbackFun  ：倒播回调
-- _timeScale           ：动画速度
function SetFairyChildTransitionPlay(_asCom, _transitionName, _isPlayer, _playCallbackFun, _reverseCallbackFun, _timeScale)
    if _timeScale == nil then
        _timeScale = 1;
    end

    FairyManager:SetFairyTransitionPlay(_asCom, _transitionName, _isPlayer, _playCallbackFun, _reverseCallbackFun, _timeScale)
end

--[Comment]
-- 设置动画速度
-- _asCom               ：Fairy组件对象
-- _transitionName      ：动画名字
-- _timeScale           ：动画速度
function SetFairyTransitionTimeScale(_asCom, _transitionName, _timeScale)
    FairyManager:SetFairyTransitionTimeScale(_asCom, _transitionName, _timeScale);
end

--[Comment]
-- 停止动画
-- _asCom               ：Fairy组件对象
-- _transitionName      ：动画名字
function SetFairyTransitionStop(_asCom, _transitionName)
    FairyManager:SetFairyTransitionStop(_asCom, _transitionName);
end

----------------------------------------------- 按键 -----------------------------------------------
function SetFairyKeyDown(_callback)
    FairyManager:SetKeyDown(_callback);
end

----------------------------------------------- 按钮 -----------------------------------------------
--[Comment]
-- 按钮点击事件
-- _asCom             ：Fairy组件对象
-- _callback          ：点击回调的函数
-- _isPlaySound       ：是否播放音效
-- _isStopPropagation ：是否屏蔽下一层的点击事件，同一个按钮有两个点击事件要设置
function SetFairyClick(_asCom, _callback, _isPlaySound, _isStopPropagation)
	if(_isStopPropagation == nil) then 
        _isStopPropagation = false; 
    end

    local tempFun = function()
        if(_isPlaySound ~= false) then 
            MusicManager.PlayEffectSound(); 
        end
         
		if(_callback ~= nil) then 
            _callback();    
        end
    end
    FairyManager:SetFairyClick(_asCom, tempFun, _isStopPropagation);
end
--[Comment]
-- 按钮点击事件
-- _asCom             ：Fairy组件对象
-- _childName         ：子对象的名字
-- _callback          ：点击回调的函数
-- _isPlaySound       ：是否播放音效
-- _isStopPropagation ：是否屏蔽下一层的点击事件，同一个按钮有两个点击事件要设置
function SetFairyChildClick(_asCom, _childName, _callback, _isPlaySound, _isStopPropagation) 
    SetFairyClick(GetFairyChild(_asCom, _childName), _callback,_isPlaySound,_isStopPropagation); 
end

--[Comment]
-- 按钮弹起
-- _asCom     : Fairy组件对象
-- _callback  ：点击回调的函数，返回参数 true 和 false
function SetFairyTouchEnd(_asCom, _callback) FairyManager:SetFairyTouchEnd(_asCom, _callback) end
--[Comment]
-- 按钮弹起
-- _asCom     : Fairy组件对象
-- _childName ：子对象的名字
-- _callback  ：点击回调的函数
function SetFairyChildTouchEnd(_asCom, _childName, _callback) SetFairyTouchEnd(GetFairyChild(_asCom, _childName), _callback) end

--[Comment]
-- 按钮压下
-- _asCom     : Fairy组件对象
-- _callback  ：点击回调的函数，返回参数 true 和 false
function SetFairyTouchBegin(_asCom, _callback) FairyManager:SetFairyTouchBegin(_asCom, _callback); end
--[Comment]
-- 按钮压下
-- _asCom     : Fairy组件对象
-- _childName ：子对象的名字
-- _callback  ：点击回调的函数
function SetFairyChildTouchBegin(_asCom,_childName,_callback) SetFairyTouchBegin(GetFairyChild(_asCom,_childName),_callback) end

--[Comment]
-- 设置按钮选中状态
-- _asCom       : Fairy组件对象
-- _isSelected  ：是否选中
function SetFairySelected (_asCom, _isSelected) FairyManager:SetFairySelected(_asCom, _isSelected) end
--[Comment]
-- 设置按钮选中状态
-- _asCom       : Fairy组件对象
-- _childName   ：子对象的名字
-- _isSelected  ：是否选中
function SetFairyChildSelected (_asCom,_childName, _isSelected) SetFairySelected(GetFairyChild(_asCom,_childName), _isSelected); end

--------------------------------------------- 设置宽高 ---------------------------------------------
--[Comment]
-- 设置Fairy组件宽度
-- _asCom     ：Fairy组件对象
-- _width     ：宽度
function SetFairyWidth(_asCom, _width) FairyManager:SetFairyWidth(_asCom, _width); end
--[Comment]
-- 设置Fairy组件宽度
-- _asCom     ：Fairy组件对象
-- _childName ：子对象的名字
-- _width     ：宽度
function SetFairyChildWidth(_asCom, _childName, _width) SetFairyWidth(GetFairyChild(_asCom, _childName), _width); end

--[Comment]
-- 设置Fairy组件高度
-- _asCom     ：Fairy组件对象
-- _heigh     ：高度
function SetFairyHeigh(_asCom, _height) FairyManager:SetFairyHeigh(_asCom, _height); end
--[Comment]
-- 设置Fairy组件高度
-- _asCom     ：Fairy组件对象
-- _childName ：子对象的名字
-- _width     ：宽度
function SetFairyChildHeigh(_asCom, _childName, _height) SetFairyHeigh(GetFairyChild(_asCom, _childName), _height); end

----------------------------------------------- 颜色 -----------------------------------------------
--[Comment]
-- 设置组件透明度（PS：慎用，这个货性能消耗有点大）
-- _asCom   ：Fairy组件对象
-- _alpha   ：透明度
function SetFairyAlpha(_asCom, _alpha) FairyManager:SetFairyAlpha(_asCom, _alpha); end
--[Comment]
-- 设置组件透明度（PS：慎用，这个货性能消耗有点大）
-- _asCom     ：Fairy组件对象
-- _childName ：子对象的名字
-- _alpha     ：透明度
function SetFairyChildAlpha(_asCom, _childName, _alpha) SetFairyAlpha(GetFairyChild(_asCom, _childName), _alpha) end

--[Comment]
-- 设置Fairy组件为灰度图
-- _asCom    ：Fairy组件对象
-- _grayed   ：是否设置对象为灰度图
-- _canClick ：是否同时设置不可点击
function SetFairyGrayed(_asCom, _grayed, _canClick)
    FairyManager:SetFairyGrayed(_asCom, _grayed);

	if(_canClick == true) then 
        SetFairyTouchable(_asCom, not _grayed); 
    end
end
--[Comment]
-- 设置Fairy组件为灰度图
-- _asCom     ：Fairy组件对象
-- _childName ：子对象的名字
-- _grayed    ：是否设置对象为灰度图
-- _canClick  ：是否同时设置不可点击
function SetFairyChildGrayed(_asCom, _childName, _grayed, _canClick)
    SetFairyGrayed(GetFairyChild(_asCom, _childName), _grayed, _canClick)
end

--[Comment]
-- 设置Fairy组件颜色
-- _asCom : Fairy组件对象
-- _color ：颜色值
-- _type 1 asImage 2 asLoader
function SetFairyColor(_asCom, _r, _g, _b, _a, _type)
	if(_type == nil) then 
        _type = 1; 
    end

	if(_type == 1) then
		FairyManager:SetFairyImageColor(_asCom, _r, _g, _b, _a)
	else
		FairyManager:SetFairyLoaderColor(_asCom, _r, _g, _b, _a)
	end
end
--[Comment]
-- _asCom     : Fairy组件对象
-- _childName ：子对象的名字
-- _color ：颜色值
-- _type 1 asImage 2 asLoader
function SetFairyChildColor(_asCom, _childName, _r, _g, _b, _a, _type)
    SetFairyColor(GetFairyChild(_asCom, _childName), _r, _g, _b, _a, _type)
end

----------------------------------------------- 位置 -----------------------------------------------
--[Comment]
-- 设置Fairy组件旋转
-- _asCom        ：Fairy组件对象
-- _rotation     ：旋转角度
function SetFairyRotation(_asCom, _rotation) FairyManager:SetFairyObjectRotation(_asCom, _rotation); end
--[Comment]
-- 设置Fairy组件旋转
-- _asCom        ：Fairy组件对象
-- _childName    ：子对象的名字
-- _rotation     ：旋转角度
function SetFairyChildRotation(_asCom, _childName, _rotation) SetFairyRotation(GetFairyChild(_asCom, _childName), _rotation) end

--[Comment]
-- 设置Fairy组件X轴
-- _asCom     ：Fairy组件对象
-- _x         ：X轴位置
function SetFairyObjectX(_asCom, _x) FairyManager:SetFairyObjectX(_asCom, _x); end
--[Comment]
-- 设置Fairy组件X轴
-- _asCom     ：Fairy组件对象
-- _childName ：子对象的名字
-- _x         ：X轴位置
function SetFairyChildObjectX(_asCom, _childName, _x) FairyManager:SetFairyObjectX(GetFairyChild(_asCom, _childName), _x); end

--[Comment]
-- 设置Fairy组件Y轴
-- _asCom     ：Fairy组件对象
-- _y         ：Y轴位置
function SetFairyObjectY(_asCom, _y) FairyManager:SetFairyObjectY(_asCom, _y); end
--[Comment]
-- 设置Fairy组件Y轴
-- _asCom     ：Fairy组件对象
-- _childName ：子对象的名字
-- _y         ：Y轴位置
function SetFairyChildObjectY(_asCom, _childName, _y) FairyManager:SetFairyObjectY(GetFairyChild(_asCom, _childName), _y); end

--[Comment]
-- 设置Fairy组件XY轴
-- _asCom     ：Fairy组件对象
-- _x         ：X轴位置
-- _y         ：Y轴位置
function SetFairyObjectXY(_comName,_x,_y) FairyManager:SetFairyObjectXY(_comName,_x,_y); end
--[Comment]
-- 设置Fairy组件XY轴
-- _asCom     ：Fairy组件对象
-- _childName ：子对象的名字
-- _x         ：X轴位置
-- _y         ：Y轴位置
function SetFairyChildObjectXY(_asCom,_childName,_x,_y) SetFairyObjectXY(GetFairyChild(_asCom,_childName),_x,_y); end

----------------------------------------------- 缩放 -----------------------------------------------
--[Comment]
-- 设置Fairy组件缩放
-- _asCom      ：Fairy组件对象
-- _rotation   ：旋转角度
function SetFairyScale(_asCom, _x, _y) FairyManager:SetFairyObjectScale(_asCom, _x, _y); end
--[Comment]
-- 设置Fairy组件缩放
-- _asCom      ：Fairy组件对象
-- _childName  ：子对象的名字
-- _rotation   ：旋转角度
function SetFairyChildScale(_asCom, _childName, _x, _y) SetFairyObjectScale(GetFairyChild(_asCom, _childName), _x, _y); end

----------------------------------------------- 填充 -----------------------------------------------
--[Comment]
-- 设置Fairy组件填充（比如：技能CD）
-- _asCom           ：Fairy组件对象
-- _fillAmount      ：目标值
-- _fillTotalAmount ：最大填充值（按照比例计算）
function SetFairyFillAmount(_asCom, _fillAmount, _fillTotalAmount)
    FairyManager:SetFairyFillAmount(_asCom, _fillAmount, _fillTotalAmount);
end
--[Comment]
-- 设置Fairy组件填充（比如：技能CD）
-- _asCom           ：Fairy组件对象
-- _childName       ：子对象的名字
-- _fillAmount      ：目标值
-- _fillTotalAmount ：最大填充值（按照比例计算）
function SetFairyChildFillAmount(_asCom, _childName, _fillAmount, _fillTotalAmount)
    SetFairyFillAmount(GetFairyChild(_asCom, _childName), _fillAmount, _fillTotalAmount);
end

----------------------------------------------- 层级 -----------------------------------------------
--[Comment]
-- 设置 UI 层级
-- _asCom        ：Fairy组件对象
-- _sortingOrder ：组件层级
function SetFairySortingOrder(_asCom, _sortingOrder) FairyManager:SetFairySortingOrder(_asCom, _sortingOrder) end
--[Comment]
-- 设置 UI 层级
-- _asCom        ：Fairy组件对象
-- _childName    ：子对象的名字
-- _sortingOrder ：组件层级
function SetFairyChildSortingOrder(_asCom, _childName, _sortingOrder) 
    SetFairySortingOrder(GetFairyChild(_asCom, _childName), _sortingOrder)
end

--------------------------------------------- 隐藏组件 ---------------------------------------------
--[Comment]
-- 设置组件是否可见
-- _asCom     : Fairy组件对象
-- _visible   ：是否隐藏组件
function SetFairyVisible(_asCom, _visible) FairyManager:SetFairyVisible(_asCom, _visible == true) end
--[Comment]
-- _asCom     : Fairy组件对象
-- _childName ：子对象的名字
-- _visible   ：是否隐藏组件
function SetFairyChildVisible(_asCom, _childName, _visible) SetFairyVisible(GetFairyChild(_asCom, _childName), _visible) end

--------------------------------------------- 触摸组件 ---------------------------------------------
--[Comment]
-- _asCom       ：Fairy组件对象
-- _touchable   ：组件是否可触摸
function SetFairyTouchable(_asCom, _touchable) FairyManager:SetFairyTouchable(_asCom, _touchable == true); end
--[Comment]
-- _asCom       ： Fairy组件对象
-- _childName   ：子对象的名字
-- _touchable   ：组件是否可触摸
function SetFairyChildTouchable(_asCom, _childName, _touchable)
    SetFairyTouchable(GetFairyChild(_asCom, _childName), _touchable)
end

----------------------------------------------- 图标 -----------------------------------------------
--[Comment]
-- 设置组件图标
-- _asCom     : Fairy组件对象
-- _value     ：显示的图标
function SetFairyIcon(_asCom, _value) FairyManager:SetFairyIcon(_asCom, _value); end
--[Comment]
-- _asCom     : Fairy组件对象
-- _childName ：子对象的名字
-- _value     ：显示的值，值会自动转换 string 值
function SetFairyChildIcon(_asCom, _childName, _value) SetFairyIcon(GetFairyChild(_asCom, _childName), _value); end

--[Comment]
-- 设置组件图标，从导出的文件里面加载
-- _asCom        : Fairy组件对象
-- _packageName  ：包名
-- _itemName     ：图标名字
function SetFairyItemIcon(_asCom,_packageName,_itemName) FairyManager:SetFairyItemIcon(_asCom,_packageName,_itemName); end
--[Comment]
-- 设置组件图标，从导出的文件里面加载
-- _asCom        : Fairy组件对象
-- _childName    ：子对象的名字
-- _packageName  ：包名
-- _itemName     ：图标名字
function SetFairyChildItemIcon(_asCom, _childName, _packageName, _itemName)
    SetFairyItemIcon(GetFairyChild(_asCom, _childName), _packageName, _itemName);
end

----------------------------------------------- 文本 -----------------------------------------------
--[Comment]
-- 设置组件文本，这个函数只有在Fairy组件存在标识为：title的组件才会起作用
-- _asCom : Fairy组件对象
-- _value ：显示的值，值会自动转换 string 值
function SetFairyTitle(_asCom, _value) FairyManager:SetFairyTitle(_asCom, _value) end
--[Comment]
-- 设置组件文本，这个函数只有在Fairy组件存在标识为：title的组件才会起作用
-- _asCom     : Fairy组件对象
-- _childName ：子对象的名字
-- _value     ：显示的值，值会自动转换 string 值
function SetFairyChildTitle(_asCom, _childName, _value) SetFairyTitle(GetFairyChild(_asCom, _childName), _value); end

--[Comment]
--  设置组件文本
-- _asCom : Fairy组件对象
-- _value ：显示的值，值会自动转换 string 值
function SetFairyText(_asCom, _value) FairyManager:SetFairyText(_asCom, tostring(_value)) end
--[Comment]
--  设置组件文本
-- _asCom     : Fairy组件对象
-- _childName ：子对象的名字
-- _value     ：显示的值，值会自动转换 string 值
function SetFairyChildText(_asCom, _childName, _value) SetFairyText(GetFairyChild(_asCom, _childName), _value); end

----------------------------------------------- 滑动 -----------------------------------------------
--[Comment]
-- _asCom     : 面板实例对象
-- _minValue  ：左值，用于除法运算
-- _maxValue  ：右值，用于除法运算
function SetFairySliderValue(_asCom,_minValue,_maxValue) FairyManager:SetFairySliderValue(_asCom,_minValue,_maxValue); end
--[Comment]
-- _asCom     : 面板实例对象
-- _childName : 子对象的名字
-- _minValue  ：左值，用于除法运算
-- _maxValue  ：右值，用于除法运算
function SetFairyChildSliderValue(_asCom, _childName, _minValue, _maxValue)
    SetFairySliderValue(GetFairyChild(_asCom, _childName), _minValue, _maxValue);
end

--[Comment]
-- _asCom     : 面板实例对象
-- _minValue  ：左值，用于除法运算
-- _maxValue  ：右值，用于除法运算
function SetFairyProgressValue(_asCom, _minValue, _maxValue)
    if _minValue == nil or _minValue < 0 then
        _minValue = 0;
    end

	if (_maxValue == nil or _maxValue <= 0) then
        _maxValue = 1;
    end

    FairyManager:SetFairyProgressValue(_asCom, _minValue, _maxValue);
end
--[Comment]
-- _asCom     : 面板实例对象
-- _childName : 子对象的名字
-- _minValue  ：左值，用于除法运算
-- _maxValue  ：右值，用于除法运算
function SetFairyChildProgressValue(_asCom, _childName, _minValue, _maxValue)
    SetFairyProgressValue(GetFairyChild(_asCom, _childName), _minValue, _maxValue);
end

--[Comment]
-- _asCom       : 面板实例对象
-- _scrollPosX  ：X轴滑动
function SetFairySliderScrollPageX(_asCom, _scrollPosX)
    FairyManager:SetFairySliderScrollPageX(_asCom, _scrollPosX)
end
--[Comment]
-- _asCom       : 面板实例对象
-- _childName   : 子对象的名字
-- _scrollPosX  ：X轴滑动
function SetFairyChildSliderScrollPageX(_asCom, _childName, _scrollPosX)
    SetFairySliderScrollPageX(GetFairyChild(_asCom,_childName), _scrollPosX);
end

--[Comment]
-- _asCom       : 面板实例对象
-- _scrollPosX  ：Y轴滑动
function SetFairySliderScrollPageY(_asCom, _scrollPosY)
    FairyManager:SetFairySliderScrollPageY(_asCom, _scrollPosY)
end
--[Comment]
-- _asCom       : 面板实例对象
-- _childName   : 子对象的名字
-- _scrollPosX  ：Y轴滑动
function SetFairyChildSliderScrollPageY(_asCom, _childName, _scrollPosY)
    SetFairySliderScrollPageY(GetFairyChild(_asCom,_childName), _scrollPosY);
end

--[Comment]
-- _asCom       : 面板实例对象
-- _scrollPosX  ：Y轴滑动
-- _scrollPosX  ：Y轴滑动
function SetFairySliderScrollPageXY(_asCom, _scrollPosX, _scrollPosY)
    FairyManager:SetFairySliderScrollPageXY(_asCom, _scrollPosX, _scrollPosY)
end
--[Comment]
-- _asCom       : 面板实例对象
-- _childName   : 子对象的名字
-- _scrollPosX  ：Y轴滑动
-- _scrollPosX  ：Y轴滑动
function SetFairyChildSliderScrollPageXY(_asCom, _childName, _scrollPosX, _scrollPosY)
    SetFairySliderScrollPageXY(GetFairyChild(_asCom,_childName), _scrollPosX, _scrollPosY);
end

--[Comment]
-- 滑动块滑动的时候，会触发回调
-- _asCom       : 面板实例对象
-- _callFun     ：回调函数
function SetFairySliderScroll(_asCom, _callFun) FairyManager:SetFairySliderScroll(_asCom, _callFun); end
--[Comment]
-- 滑动块滑动的时候，会触发回调
-- _asCom       : 面板实例对象
-- _childName   : 子对象的名字
-- _callFun     ：回调函数
function SetFairyChildSliderScroll(_asCom, _childName, _callFun) SetFairySliderScroll(GetFairyChild(_asCom, _childName), _callFun); end

--[Comment]
-- 滑动块滑动结束的时候，会触发回调
-- _asCom       : 面板实例对象
-- _callFun     ：回调函数
function SetFairySliderScrollEnd(_asCom,_callFun) FairyManager:SetFairySliderScrollEnd(_asCom, _callFun); end
--[Comment]
-- 滑动块滑动结束的时候，会触发回调
-- _asCom       : 面板实例对象
-- _childName   : 子对象的名字
-- _callFun     ：回调函数
function SetFairyChildSliderScrollEnd(_asCom,_childName,_callFun) SetFairySliderScrollEnd(GetFairyChild(_asCom,_childName),_callFun); end

--[Comment]
-- 移动滑动块组件的X轴
-- _asCom       : 面板实例对象
-- _posX        ：滑动的X轴
-- _ani         ：是否显示动画
function SetFairySliderScrollPosX(_asCom, _posX, _ani) FairyManager:SetFairySliderScrollPosX(_asCom, _posX, _ani == true); end
--[Comment]
-- 移动滑动块组件的X轴
-- _asCom       : 面板实例对象
-- _childName   : 子对象的名字
-- _posX        ：滑动的X轴
-- _ani         ：是否显示动画
function SetFairyChildSliderScrollPosX(_asCom, _childName, _posX, _ani) SetFairySliderScrollPosX(GetFairyChild(_asCom, _childName), _posX, _ani); end

--[Comment]
-- 移动滑动块组件的Y轴
-- _asCom       : 面板实例对象
-- _posY        ：滑动的Y轴
-- _ani         ：是否显示动画
function SetFairySliderScrollPosY(_asCom, _posY, _ani) FairyManager:SetFairySliderScrollPosY(_asCom, _posY, _ani == true); end
--[Comment]
-- 移动滑动块组件的Y轴
-- _asCom       : 面板实例对象
-- _childName   : 子对象的名字
-- _posY        ：滑动的Y轴
-- _ani         ：是否显示动画
function SetFairyChildSliderScrollPosY(_asCom, _childName, _posY, _ani) 
    SetFairySliderScrollPosY(GetFairyChild(_asCom, _childName), _posY, _ani); 
end

--[Comment]
-- 滑动块值发生变化会触发，并且变化的值会通过回调函数回调回来
-- _asCom       ：面板实例对象
-- _callBack    ：回调函数，会返回一个浮点型的数值
function SetFairySliderOnChange(_asCom, _callBack) FairyManager:SetFairySliderOnChange(_asCom,_callBack); end
--[Comment]
-- 滑动块值发生变化会触发，并且变化的值会通过回调函数回调回来
-- _asCom       ：面板实例对象
-- _childName   : 子对象的名字
-- _callBack    ：回调函数，会返回一个浮点型的数值
function SetFairyChildSliderOnChange(_asCom,_childName,_callBack) SetFairySliderOnChange(GetFairyChild(_asCom,_childName),_callBack); end

--[Comment]
-- 滑动块值操作结束之后会触发，并且变化的值会通过回调函数回调回来
-- _asCom       ：面板实例对象
-- _callBack    ：回调函数，会返回一个浮点型的数值
function SetFairySliderGripTouchEnd(_asCom, _callBack) FairyManager:SetFairySliderGripTouchEnd(_asCom,_callBack); end
--[Comment]
-- 滑动块值操作结束之后会触发，并且变化的值会通过回调函数回调回来
-- _asCom       ：面板实例对象
-- _childName   : 子对象的名字
-- _callBack    ：回调函数，会返回一个浮点型的数值
function SetFairyChildSliderGripTouchEnd(_asCom,_childName,_callBack) SetFairySliderGripTouchEnd(GetFairyChild(_asCom,_childName),_callBack); end

----------------------------------------------- 输入 -----------------------------------------------
--[Comment]
-- 设置输入文本聚焦回调
-- _asCom     : 面板实例对象
-- _callback  : 回调函数
function SetFairyInputTextOnFocusIn(_asCom, _callback) FairyManager:SetFairyInputTextOnFocusIn(_asCom,_callback) end
--[Comment]
-- 设置输入文本聚焦回调
-- _asCom     : 面板实例对象
-- _childName : 子对象的名字
-- _callback  : 回调函数
function SetFairyChildInputTextOnFocusIn(_asCom,_childName,_callback) 
    SetFairyInputTextOnFocusIn(GetFairyChild(_asCom, _childName),_callback);
end

--[Comment]
-- 设置输入文本离开聚焦回调
-- _asCom     : 面板实例对象
-- _callback  : 回调函数
function SetFairyInputTextOnFocusOut(_asCom,_callback) FairyManager:SetFairyInputTextOnFocusOut(_asCom,_callback) end
--[Comment]
-- 设置输入文本离开聚焦回调
-- _asCom     : 面板实例对象
-- _childName : 子对象的名字
-- _callback  : 回调函数
function SetFairyChildInputTextOnFocusOut(_asCom,_childName,_callback)
	SetFairyInputTextOnFocusOut(GetFairyChild(_asCom, _childName),_callback);
end

--[Comment]
-- 设置输入文本变动回调
-- _asCom     : 面板实例对象
-- _callback  : 回调函数
function SetFairyInputOnChange(_asCom,_callback) FairyManager:SetFairyInputOnChange(_asCom,_callback) end
--[Comment]
-- 设置输入文本变动回调
-- _asCom     : 面板实例对象
-- _childName : 子对象的名字
-- _callback  : 回调函数
function SetFairyChildInputOnChange(_asCom,_childName,_callback)
	SetFairyInputOnChange(GetFairyChild(_asCom, _childName),_callback);
end

--[Comment]
-- 设置页面标签页切换
-- _asCom       : 面板实例对象
-- _controlName : 标签页名字
-- _index       ：标签页下标索引
function SetFairyControllerSelectedIndex(_asCom, _controlName, _index) 
    FairyManager:SetFairyControllerSelectedIndex(_asCom, _controlName, _index) 
end

---------------------------------------------- 下拉框 ----------------------------------------------
--[Comment]
-- 下拉框变化事件 返回选择的index
-- _asCom       : 面板实例对象
-- _callBack    ：回调函数，返回一个整型的下标索引
function SetFairyComboBoxOnChange(_asCom,_callBack) FairyManager:SetFairyComboBoxOnChange(_asCom,_callBack); end
--[Comment]
-- 下拉框变化事件 返回选择的index
-- _asCom       : 面板实例对象
-- _childName : 子对象的名字
-- _callBack    ：回调函数，返回一个整型的下标索引
function SetFairyChildComboBoxOnChange(_asCom, _childName, _callBack) SetFairyComboBoxOnChange(GetFairyChild(_asCom,_childName),_callBack); end

----------------------------------------------- 拖动 -----------------------------------------------
--[Comment]
-- 拖动组件完毕会触发该事件
-- _asCom       : 面板实例对象
-- _callBack    ：回调函数
function SetFairyOnDragEnd(_asCom, _callBack) FairyManager:SetFairyOnDragEnd(_asCom, _callBack); end
--[Comment]
-- 拖动组件完毕会触发该事件
-- _asCom       : 面板实例对象
-- _childName   : 子对象的名字
-- _callBack    ：回调函数
function SetFairyChildOnDragEnd(_asCom, _childName, _callBack) SetFairyOnDragEnd(GetFairyChild(_asCom, _childName), _callBack); end

--[Comment]
-- 拖动组件中会触发该事件
-- _asCom       : 面板实例对象
-- _callBack    ：回调函数
function SetFairyOnDragMove(_asCom, _callBack) FairyManager:SetFairyOnDragMove(_asCom, _callBack); end
--[Comment]
-- 拖动组件中会触发该事件
-- _asCom       : 面板实例对象
-- _childName   : 子对象的名字
-- _callBack    ：回调函数
function SetFairyChildOnDragMove(_asCom, _childName, _callBack) SetFairyOnDragMove(GetFairyChild(_asCom, _childName), _callBack); end

--[Comment]
-- 拖动组件开始会触发该事件
-- _asCom       : 面板实例对象
-- _callBack    ：回调函数
function SetFairyChildOnDragStart(_asCom, _childName, _callBack) SetFairyOnDragStart(GetFairyChild(_asCom, _childName), _callBack); end
--[Comment]
-- 拖动组件开始会触发该事件
-- _asCom       : 面板实例对象
-- _childName   : 子对象的名字
-- _callBack    ：回调函数
function SetFairyOnDragStart(_asCom, _callBack) FairyManager:SetFairyOnDragStart(GetFairyChild(_asCom, _childName), _callBack); end
