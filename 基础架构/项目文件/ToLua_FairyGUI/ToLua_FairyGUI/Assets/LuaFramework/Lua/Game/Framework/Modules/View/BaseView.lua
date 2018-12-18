BaseView = Class();

function BaseView:ctor(_packageName, _viewName)
    if _viewName == nil then
        _viewName = "Main";
    end

    self.selectIndex = 0;
    self.mainView = AddPackageObject(_packageName, _viewName);
end

-- 添加监听事件，在页面退出时，会自动关闭对应的事件，注意 --> 这个事件不能继承重写
function BaseView:AddListener(_eventName, _callbackFun)
    if _eventName == nil then
        logError("BaseView:AddListener -----> _eventName value nil");
        return;
    end

    if _callbackFun == nil then
        logError("BaseView:AddListener -----> _callbackFun value nil");
        return;
    end

    if self.eventNameList == nil then
        self.eventNameList = Map.new();
    end

    self.eventNameList:Add(_eventName,_callbackFun);
    Dispatcher.AddListener(_eventName, _callbackFun);
end

--[Comment]
-- 设置拖动切换 _childName 对象 _leftMoveFun 向左/向下 _rightMoveFun 向右/向上 _tempWidth 拖动距离 _isV == true 表示竖直滑动
function BaseView:SetFairyChildHorizontalDrag(_childName, _leftMoveFun, _rightMoveFun,_tempWidth,_isV,_asCom)
	if _asCom == nil then 
        _asCom = self.mainView; 
    end

    local tempTouchEndFun = function(_context)
        self.touchBeginTime = (self.touchBeginTime) or 0;
        if (self.touchBeginTime == nil or os.time() - self.touchBeginTime) > 1 then
            return;
        end

        local tempWidth = _tempWidth ~= nil and _tempWidth or GetFairyChildWidth(_asCom, _childName) * 0.1;
        local tempLeftDrag = _isV == true and (_context.inputEvent.y - self.touchBeginPosition.y) or (self.touchBeginPosition.x - _context.inputEvent.position.x);
        local tempRightDrag = _isV == true and (self.touchBeginPosition.y - _context.inputEvent.position.y) or (_context.inputEvent.position.x - self.touchBeginPosition.x);
        if tempLeftDrag > tempWidth then
            _leftMoveFun();

        elseif tempRightDrag > tempWidth then
            _rightMoveFun();
        end

        self.touchBeginTime = nil;
        self.touchBeginPosition = nil;
    end

    local tempTouchBeginFun = function(_context)
        self.touchBeginTime = os.time();
        self.touchBeginPosition = _context.inputEvent.position;
    end

    SetFairyChildTouchEnd(_asCom, _childName, tempTouchEndFun);
    SetFairyChildTouchBegin(_asCom, _childName, tempTouchBeginFun);
end

function BaseView:SetControllerView(_viewName)
    if _viewName == nil then
        _viewName = "c1";
    end

    self.viewController = _viewName;
end

function BaseView:SelectedIndex(_index)
    if self.viewController == nil then
        logError("viewController value nil");
        return;
    end

    self.selectIndex = _index;
    SetFairyControllerSelectedIndex(self.mainView, self.viewController, _index);
end

function BaseView:ExitView()
    if self.mainView == nil then
        return;
    end

    if self.eventNameList ~= nil then
        local keys = self.eventNameList:keys();
        for i = 1, #keys do
            local tempName = keys[i]; 
            local _callBack = self.eventNameList[tempName]
            if tempName ~= nil then
                Dispatcher.RemoveListener(tempName,_callBack);
            end
        end
        self.eventNameList:clear();
    end

    self:Clear();
    self:RemoveListener();
    RemoveFairyPackage(self.mainView);
end

function BaseView:Clear() end
function BaseView:RemoveListener() end