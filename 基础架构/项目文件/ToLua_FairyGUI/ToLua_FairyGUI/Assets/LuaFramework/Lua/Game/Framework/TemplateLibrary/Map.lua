Map = Class()

function Map:ctor()
	local object = {}
	setmetatable(object, self)
	self.__index = self
	self.__keyList = {}
end

--插入元素[如果已存在该key,则用新的value覆盖原有的value，与STL的“[key] = value 类似”]
function Map:Add(key, value)
	if(self[key] == nil) then
		self[key] = value
		table.insert(self.__keyList, key)
	else
		self[key] = nil
		self[key] = value
	end
	return self
end

--返回map中元素的个数
function Map:Count()
    return #self.__keyList
end

--如果map为空则返回true
function Map:Empty()
    if (self:size() == 0) then
		return true
	else
		return false
	end
end

--删除一个元素,根据key删除value【只实现stl的其中一个重载】
function Map:Remove(key)
	if self[key] == nil then
		return self
	else
        for i = 1, #self.__keyList do
			if(self.__keyList[i] == key) then
				table.remove(self.__keyList, i)
				break;
			end
		end
		self[key] = nil
	end
end

--删除所有元素
function Map:Clear()
	for i = 1, self:size() do
		self[self.__keyList[1]] = nil
		table.remove(self.__keyList, 1)
	end
end

--获取key
function Map:Key(index)
	return self.__keyList[index]
end

function Map:Keys()
	return self.__keyList;
end

function Map:Values()
	local _values = {};

	for i = 1, self:size() do
		_values[i] = self:value(self.__keyList[i]);
	end	

	return _values;
end

--根据key返回对应的value[lua特有，stl无此功能]
function Map:Value(key)
	return self[key]
end

-- 返回指定下标的的值
function Map:ValueIndex(index)
	return self[self.__keyList[index]]
end

--检查是否包含
function Map:ContainsKey(key)
	if(self[key] == nil) then
		return false;
	else
		return true;
	end
end

--返回迭代器函数[lua特有，stl无此功能]
function Map:Iter()
	local i = 0
	local n = self:size()
	return function()
		i = i + 1
		if(i <= n) then
			return self.__keyList[i]
		else
			return nil
		end
	end
end

--传入key，返回key所在的迭代函数[lua特有，stl无此功能]
function Map:Find(key)
	local idx = 0
	local n = self:size()
	for i = 1, n do
		if(self.__keyList[i] == key) then
			idx = i - 1
			break
		end
	end

	return function()
		idx = idx + 1
		if(idx <= n) then
			return self.__keyList[idx]
		else
			return nil
		end
	end
end

function Map:ToString()
	local str = ("[Map size:");
	str = str .. self:size() .. "\n";
	for i = 1, self:size() do
		local key = self:key(i);
		local value = self:value(key);
		if type(value) ~= "userdata" and type(value) ~= "table" then
			str = str .. key .. "=" .. tostring(value) .. "\n";
		else      
			str = str .. key .. "=" .. type(value) .. "\n"
		end
	end

	str = str .. "]";
	return str;
end