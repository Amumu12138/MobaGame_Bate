--------------------------------------- 运行平台 ---------------------------------------
PlatformType = {
	IOS = "iPhone",
	Android = "Android",
	Windows = "Windows"
}

local platformType = nil;
function GetPlatform()
    if (IsNilOrEmpty(platformType)) then 
        platformType = Util.GetGamePlatform(); 
    end
    return platformType;
end

function IsIos() return GetPlatform() == PlatformType.IOS end
function IsAndroid() return GetPlatform() == PlatformType.Android end
function IsWindows() return GetPlatform() == PlatformType.Windows end

--------------------------------------- 通用函数 ---------------------------------------
--[Comment]
-- 输出日志
function log(_str)
    Util.Log(tostring(_str));
end

--[Comment]
-- 错误日志
function logError(_str)
	Util.LogError("<color=#FF0000>" .. tostring(_str) .. "</color>" .. "\n\n\n" .. debug.traceback());
end

--[Comment]
-- 警告日志
function logWarn(_str)
	Util.LogWarning(tostring(_str));
end

----------------------------------------- JSON -----------------------------------------
--[Comment]
-- 解析 Json
function DecodeJson(_text)
    return json.decode(_text);
end

---------------------------------------- 随机数 ----------------------------------------
--[Comment]
-- 随机整数
function RandomInt(_leftValue, _rightValue)
    return Util.RandomInt(_leftValue, _rightValue)
end

--[Comment]
-- 随机浮点数
function RandomFloat(_leftValue, _rightValue)
    return Util.RandomFloat(_leftValue, _rightValue)
end

---------------------------------------- 文件操作 ----------------------------------------
--[Comment]
-- 判断文件是否存在
function IsExistFile(_fileName)
    return Util.IsExistAssetBundleFile(_fileName);
end

---------------------------------------- 字符串 ----------------------------------------
--[Comment]
-- 转换时间格式
-- 格式 ：大于10，转换字符串返回， 小于10，前面加个0，比如：05
function ToTimeFormat(_time)
    if _time == nil then
        return "00";
    end

    if _time < 10 then
        return "0" .. tostring(_time);
    end

    return tostring(_time);
end

--[Comment]
-- str 是否是空
function IsNilOrEmpty(_str)
    return _str == nil or _str == "";
end

--[Comment]
-- 是否存在非法字符
-- _str ：待检测的值
function IsLegalChar(_str)
    return Util.IsLegalChar(_str)
end

--[Comment]
-- 获取字符串长度
function GetStringLength(_str)
    -- 计算字符串宽度
    -- 可以计算出字符宽度，用于显示使用
   local i = 1
   local tempWidth = 0
   local tempLenInByte = #_str
    while (i <= tempLenInByte) do
        local tempByteCount = 1;
        local tempByte = string.byte(_str, i)
        
        if tempByte > 0 and tempByte <= 127 then
            tempByteCount = 1                                               --1字节字符
        elseif tempByte >= 192 and tempByte < 223 then
            tempByteCount = 2                                               --双字节字符
        elseif tempByte >= 224 and tempByte < 239 then
            tempByteCount = 3                                               --汉字
        elseif tempByte >= 240 and tempByte <= 247 then
            tempByteCount = 4                                               --4字节字符
        end

        i = i + tempByteCount                                              -- 重置下一字节的索引
        tempWidth = tempWidth + 1                                           -- 字符的个数（长度）
    end

    return tempWidth
end

--[Comment]
-- 获取字符字节数量
function GetByteCount(_str, _index)
    local tempByte = string.byte(_str, _index)
    local tempByteCount = 1;
    if tempByte >= 192 and tempByte < 223 then
        tempByteCount = 2                                               --双字节字符
    elseif tempByte >= 224 and tempByte < 239 then
        tempByteCount = 3                                               --汉字
    elseif tempByte >= 240 and tempByte <= 247 then
        tempByteCount = 4                                               --4字节字符
    end

    return tempByteCount;
end

--[Comment]
-- str : 待分割的字符串
-- split ：分割字符   
-- return : string []
function Split(str, split)
    if str == nil then
        logError("str value nil");
        return {};
    end

    if split == nil then
        logError("str value nil");
        return {};
    end

    local rt= {}
    string.gsub(str, '[^'.. split ..']+', function(w) table.insert(rt, w); end );
    return rt;
end

--[Comment]
-- _str     ：待替换的值
-- _oldChar ：需要替换的旧值
-- _newChar ：需要替换的新值
function Replace(_str, _oldChar, _newChar)
    if _str == nil then
        logError("_str value nil");
        return "";
    end

    if _oldChar == nil then
        logError("_oldChar value nil");
        return "";
    end

    if _newChar == nil then
        logError("_newChar value nil");
        return "";
    end

    return string.gsub(_str, _oldChar, _newChar) 
end

--[Comment]
-- 去除字符串左右空格
function ToTrim(_string)
	if(type(_string)=="string") then
		_string = string.gsub(_string, "^%s*(.-)%s*$", "%1");
	end
	return _string;
end

--[Comment]
-- str 长度> len 后，后面字段换成 replaceStr
function GetStringSubToReplace(str, len, replaceStr)
	if(replaceStr==nil) then replaceStr = "..."; end
	local txt = "";
	if GetStringLen(str) > len then
		local lenInByte,width,i = #str,0,1;
		while (i <= lenInByte) do
			local curByte = string.byte(str, i)
			local byteCount = 1;
			if curByte > 0 and curByte <= 127 then byteCount = 1  --1字节字符
			elseif curByte >= 192 and curByte < 223 then byteCount = 2   --双字节字符
			elseif curByte >= 224 and curByte < 239 then byteCount = 3   --汉字
			elseif curByte >= 240 and curByte <= 247 then byteCount = 4  --4字节字符
			end
			txt = txt..string.sub(str, i, i + byteCount - 1);  
			i = i + byteCount                                              -- 重置下一字节的索引
			width = width + 1                                             -- 字符的个数（长度）
			if(width==len) then i = lenInByte+1; txt = txt..replaceStr; end
		end
    else
		txt = str;
	end
	return txt;
end
--------------------------------------- 数据操作 ---------------------------------------
--[Comment]
-- 排除小数点
function ToInt(_value)
    return math.modf(_value);
end

--[Comment]
-- 转换整型
-- 封转一层保护，一旦传进来的值为 nil，自动返回 -1，不至于程序异常
function ToNumber(_number)
    if _number == nil then
        return -1;
    end
    
    if type(_number) == "string" then
        return tonumber(_number);
    end

    return _number;
end

--[Comment]
-- 转换百分比
function ToPercent(_leftValue, _rightValue)
    local tempValue = ToDivision(_leftValue, _rightValue);
    if tempValue == 0 then
        return "0%";
    end

    return tostring(math.modf(tempValue * 100)) .. "%";
end

--[Comment]
-- 除法运算
-- 排除除法对0的运算，左值除以右值
function ToDivision(_left, _right)
    if _left == nil or _left == 0 or _right == nil or _right == 0 then
        return 0;
    end

    return _left / _right;
end

--[Comment]
-- 线性差值
function Lerp(_fromValue, _toValue, _t)
    if _fromValue == nil then
        logError("_fromValue value nil");
        return 0;
    end

    if _toValue == nil then
        logError("_toValue value nil");
        return _fromValue;
    end

    if _t == nil then
        return _fromValue;
    end

    return _fromValue + ((_toValue - _fromValue) * _t);
end

-- 根据当前时间t 返回路径  其中v0为起点 v1为终点 a为中间点
--[Comment]
-- 贝塞尔曲线
-- _time            当前时间
-- _startPoint      起点
-- _endPoint
-- _centerPoint
-- 公式为B(t) = (1 - t) ^ 2 * v0 + 2 * t * (1 - t) * a0 + t * t * v1 其中v0为起点 v1为终点 a为中间点
function GetBezierPoint(_time, _startPoint, _endPoint, _centerPoint)
    local a = Vector3.New(0, 0, 0);
    a.x = _time * _time * (_endPoint.x - 2 * _centerPoint.x + _startPoint.x) + _startPoint.x + 2 * _time * (_centerPoint.x - _startPoint.x); 
    a.y = _time * _time * (_endPoint.y - 2 * _centerPoint.y + _startPoint.y) + _startPoint.y + 2 * _time * (_centerPoint.y - _startPoint.y);  
    return a;  
end
----------------------------------------- 时间 -----------------------------------------
--[Comment]
-- 一星期中的第几天[1 ~ 7 = 星期天 ~ 星期六]
function GetWeeksNum(_time)
    return os.date("%w", _time);
end

--[Comment]
-- 时间，格式为 = 分:秒
-- _second    ：秒数
-- _split     ：分割符
function GetTimeFormat(_second, _split)
    if _split == nil then
        _split = ":";
    end

    if _second == nil or _second < 0 then
        return "00" .. tostring(_split) .. "00";
    end

    local tempArray = GetDateFormatArray(GetMinTimeStamp() + _second);
    return tempArray.minute .. tostring(_split) .. tempArray.second;
end

--[Comment]
-- 时间，格式为 = 时:分
-- _second    ：秒数
-- _split     ：分割符
function GetTimeFormat_Minute(_second, _split)
    if _split == nil then
        _split = ":";
    end

    if _second == nil or _second < 0 then
        return "00" .. tostring(_split) .. "00";
    end

    local tempArray = GetDateFormatArray(GetMinTimeStamp() + _second);
    return tempArray.hour .. tostring(_split) .. tempArray.minute;
end

--[Comment]
-- 时间，格式为 = 时:分:秒
-- _second    ：秒数
-- _split     ：分割符
function GetTimeFormat_Second(_second, _split)
    if _split == nil then
        _split = ":";
    end

    if _second == nil or _second < 0 then
        return "00" .. tostring(_split) .. "00" .. tostring(_split) .. "00";
    end

    local tempArray = GetDateFormatArray(GetMinTimeStamp() + _second);
    return tempArray.hour .. tostring(_split) .. tempArray.minute .. tostring(_split) .. tempArray.second;
end

--[Comment]
-- 日期 默认格式为 = 年-月-日
-- _timeStamp  ：时间戳
-- _leftSplit  ：年月日的分隔符
function GetDateFormat_Day(_timeStamp, _leftSplit)
    if _leftSplit == nil then
        _leftSplit = "-";
    end

    local tempArray = GetDateFormatArray(_timeStamp);
    return tostring(tempArray.year) .. tostring(_leftSplit) .. tostring(tempArray.month) .. tostring(_leftSplit) .. tostring(tempArray.day);
end

--[Comment]
-- 日期 默认格式为 = 年-月-日 时
-- _timeStamp  ：时间戳
-- _leftSplit  ：年月日的分隔符
function GetDateFormat_Hour(_timeStamp, _leftSplit)
    if _leftSplit == nil then
        _leftSplit = "-";
    end

    local tempArray = GetDateFormatArray(_timeStamp);
    local tempLeftTime = tostring(tempArray.year) .. tostring(_leftSplit) .. tostring(tempArray.month) .. tostring(_leftSplit) .. tostring(tempArray.day);
    local tempRightTime = tostring(tempArray.hour);
    return tempLeftTime .. " " .. tempRightTime;
end

--[Comment]
-- 日期 默认格式为 = 年-月-日 时:分
-- _timeStamp  ：时间戳
-- _leftSplit  ：年月日的分隔符
-- _rightSplit ：时分秒的分隔符
function GetDateFormat_Minute(_timeStamp, _leftSplit, _rightSplit)
    if _leftSplit == nil then
        _leftSplit = "-";
    end

    if _rightSplit == nil then
        _rightSplit = ":";
    end

    local tempArray = GetDateFormatArray(_timeStamp);
    local tempLeftTime = tostring(tempArray.year) .. tostring(_leftSplit) .. tostring(tempArray.month) .. tostring(_leftSplit) .. tostring(tempArray.day);
    local tempRightTime = tostring(tempArray.hour) .. tostring(_rightSplit) .. tostring(tempArray.minute);
    return tempLeftTime .. " " .. tempRightTime;
end

--[Comment]
-- 日期 默认格式为 = 年-月-日 时:分:秒
-- _timeStamp  ：时间戳
-- _leftSplit  ：年月日的分隔符
-- _rightSplit ：时分秒的分隔符
function GetDateFormat_Second(_timeStamp, _leftSplit, _rightSplit)
    if _leftSplit == nil then
        _leftSplit = "-";
    end

    if _rightSplit == nil then
        _rightSplit = ":";
    end

    local tempArray = GetDateFormatArray(_timeStamp);
    local tempLeftTime = tostring(tempArray.year) .. tostring(_leftSplit) .. tostring(tempArray.month) .. tostring(_leftSplit) .. tostring(tempArray.day);
    local tempRightTime = tostring(tempArray.hour) .. tostring(_rightSplit) .. tostring(tempArray.minute) .. tostring(_rightSplit) .. tostring(tempArray.second);
    return tempLeftTime .. " " .. tempRightTime;
end

--[Comment]
-- 日期数组
function GetDateFormatArray(_timeStamp)
    local tempTime = GetTimeStampDate(_timeStamp);
    local tempYear = ToTimeFormat(tempTime.year);       -- 年
    local tempMonth = ToTimeFormat(tempTime.month);     -- 月
    local tempDay = ToTimeFormat(tempTime.day);         -- 日
    local tempHour = ToTimeFormat(tempTime.hour);       -- 时
    local tempMinute = ToTimeFormat(tempTime.minute);   -- 分
    local tempSecond = ToTimeFormat(tempTime.second);   -- 秒
    return { year = tempYear, month = tempMonth, day = tempDay, hour = tempHour, minute = tempMinute, second = tempSecond };
end

--[Comment]
-- 事件戳转日期，以秒为单位
function GetTimeStampDate(_timeStamp)
    return os.date("*t", _timeStamp);
end

--[Comment]
-- 获取最大时间戳
function GetMaxTimeStamp()
    return os.time({ day = 1, month = 1, year = 2030, hour = 0, minute = 0, second = 0 })
end

--[Comment]
-- 获取最小时间戳 以秒为单位
function GetMinTimeStamp()
    return os.time({ day = 1, month = 1, year = 1970, hour = 0, minute = 0, second = 0 });
end