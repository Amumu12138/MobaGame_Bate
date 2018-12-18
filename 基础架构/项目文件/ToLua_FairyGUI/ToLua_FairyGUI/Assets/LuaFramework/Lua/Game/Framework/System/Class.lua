-- metatable是Lua中的重要概念，每一个table都可以加上metatable，以改变相应的table的行为。让我们看一个例子：
-- __index
-- other = {}
-- t = setmetatable({}, { __index = other })
-- t.foo = 3
-- t.foo -- 3
-- other.foo -- nil

-- __newindex
-- other = {}
-- t = setmetatable({}, { __newindex = other })
-- t.foo = 3
-- other.foo -- 3
-- t.foo -- nil

local _class = { }

--[Comment]
-- 实例化类
-- _super ：继承的父对象
function Class(_super)
    local class_type = { };
    -- 构造函数
    -- 当类被实例化的时候，会调用该类 ctor 函数来进行初始化的操作
    class_type.ctor = false;
    class_type._super = _super;
    class_type.new = function(...)
        -- 初始化obj，如果没有这句，那么类所建立的对象改变，其他对象都会改变
        local obj = { };
        do
            local create;
            create = function(c, ...)
                if c._super then
                    -- 设置父对象，这里是一个循环，会所有父对象都存起来
                    create(c._super, ...)
                end

                if c.ctor then
                    -- 调用初始化函数
                    c.ctor(obj, ...)
                end
            end

            create(class_type, ...)
        end

        -- 将 obj 的元表设定为 Class
        setmetatable(obj, { __index = _class[class_type] });
        obj.super = _class[_super];

        return obj;
    end

    local vtbl = { };
    _class[class_type] = vtbl;
    setmetatable(class_type, { __newindex = function(t, k, v) vtbl[k] = v end } );

    if _super then
        setmetatable(vtbl, { __index =
            function(t, k) 
                local ret = _class[_super][k];
                vtbl[k] = ret;
                return ret;
            end } )
    end

    return class_type
end