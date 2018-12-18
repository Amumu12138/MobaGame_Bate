require "Common/functions"

List = Class()

function List:ctor()
    local object = {}
    setmetatable(object, self)
    self.__index = self
    self.key=1;
    self.__classtype = "List"
end

function List:ForEach(func)
	local len = self:getSize();
	for i=1,len do
		func(self[i]);
	end
end	

function List:Sort(func)
	table.sort( self, func )
end	

function List:Find(func) 
	for i=1,#self do
		if func(self[i]) then
			return self[i];
		end
	end				
end	

function List:FindAll(func) 
	local _values = List.new();
	for i = 1, #self do
		if func(self[i]) then
			_values:add(self[i]);
		end
	end				
	return _values;
end	

function List:Add(value)
    local key = self.key;
    self[key] = value
    self.key = self.key+1;
end

function List:AddRange(addList)
    if(addList == nil) then
        return;
    end

    for i = 1, #addList do
        self:add(addList[i]);
    end
end

function List:GetRange(index, count)
	if index > #self or index < 1 or count > #self then
		return nil;
	end

	local _values = List.new();

	local j = 1;
	for i = index, count do
		_values[j] = self[i];
		j = j + 1;
	end	

	return _values;
end

function List:Reverse() 
	local len = self:getSize();
	local lenLoop;

	if len % 2 == 0 then
		lenLoop = len / 2;
	else 
		lenLoop = GetIntPart(len / 2);
	end		

	for i = 1 , lenLoop do
		local tmp = self[len - i + 1];
		self[len - i + 1] = self[i];
		self[i] = tmp;
	end
end	

function List:Contains(value) 
    for i = 1, #self do
        if self[i] == value then
            return true;
        end
    end

    return false;
end

function List:IndexOf(value)
    for i = 1, #self do
        if self[i] == value then
            return i;
        end
    end

    return -1;
end

function List:Count()
    return self.key - 1;
end 

function List:Clear()
	for i = 1, #self do
		self[i] = nil;
	end

	self.key = 1;
end

--替换
function List:Replace(index, newValue)
    self[index] = newValue
end 

function List:Remove(value)
	local index = self:indexOf(value);

	if index ~= nil and index > 0 then
		self:removeAt(index);
	end	
end	

function List:Insert(v, iter)
	local len = self:getSize();
	if(v < 1 or v > len + 1) then
		error("is bug List:insert index:"..v);
		return;
	end

	if v == len then
		self[len + 1] = self[len];
	else

		local _p;
		for i = v, #self do
			if _p ~= nil then
				local __p = self[i];
				self[i] = _p;
				_p = __p;
			else 
				_p = self[i];
			end	
		end

		if _p ~= nil then
			self[len + 1] = _p;
		end	
	end	
	
	self[v] = iter;
	self.key = self.key + 1;
end	

--删除
function List:RemoveAt(index)
	local len = self:getSize();
	if(index < 1 or index > len) then
		error("is bug List:removeAt index:"..index..",self.key:"..self.key);
		return;
	end

	for i = index, #self do
		if(index + 1 <= len) then
			self[i] = self[i + 1];
		end
	end

	self[len] = nil;
	self.key = self.key - 1;
	if #self ~= len - 1 then
		error("is bug2 List:removeAt index:"..index..",self.key:"..self.key);
	end
end 

function List:ToString()
    local str = "[List size:" .. #self;
    for i = 1, #self do
        if type(self[i]) ~= "table" and type(self[i]) ~= "userdata" then
				str = str .. "self[" .. i .. "]=" .. self[i] .. " ";
			end
		end
    str = str .. "]"
    return str;
end