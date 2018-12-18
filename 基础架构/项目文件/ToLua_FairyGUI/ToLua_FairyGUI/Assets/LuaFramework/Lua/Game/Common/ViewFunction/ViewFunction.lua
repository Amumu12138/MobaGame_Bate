--[[
功能：所有跟游戏UI有关系的逻辑集中写在这个类
]]

-------------------------------------------------- 通用函数 --------------------------------------------------
--[Comment]
-- 转换颜色
-- _param ：转换的值
-- _isReb ：是否转换成红色
function ToColor(_param, _isReb)
    if _isReb == true then
        return ToRedColor(_param);
    end

    return ToGreenColor(_param);
end

--[Comment]
-- 转换红色
function ToRedColor(_param)
    return "[color=#ff0000]" .. tostring(_param) .. "[/color]";
end

--[Comment]
-- 转换绿色
function ToGreenColor(_param)
    return "[color=#18cc00]" .. tostring(_param) .. "[/color]";
end

-------------------------------------------------- 加载对象 --------------------------------------------------
--[Comment]
-- 加载Prefab对象并保存在Fairy的Graph组件里面
-- _asGraph    ：Fairy组件，Prefab对象要加载到Fairy中只能使用此组件
-- _bundleName ：AB资源包的名字
-- _objectName ：需要加载的对象名字
function SetFairyNativeObject(_asGraph, _bundleName, _objectName)
    if _asGraph == nil then
        logError("SetFairyNativeObject -----> _asGraph value nil");
        return;
    end

    local tempFun = function(_go)
        if _go == nil then
            logError("SetFairyNativeObject -----> _go value nil");
            return;
        end

        PackageManager.mInst:SetFairyNativeObject(_asGraph, _go);
    end

    ResourceManager.LoadPrefab(_bundleName, _objectName, tempFun);
end

--[Comment]
-- _asGraph ：Fairy组件，Prefab对象要加载到Fairy中只能使用此组件
-- _objectName ：需要加载的特效对象名字
function SetFairyNativeEffect(_asGraph, _objectName)
    SetFairyNativeObject(_asGraph, "effect_" .. tostring(_objectName), _objectName)
end

--[Comment]
-- _asGraph    ：Fairy组件，Prefab对象要加载到Fairy中只能使用此组件
-- _childName  ：子对象名字
-- _objectName ：需要加载的特效对象名字
function SetFairyChildNativeEffect(_asGraph, _childName, _objectName)
    SetFairyNativeEffect(GetFairyChild(_asGraph, _childName), _objectName);
end