ProtoRegister = { };

function ProtoRegister.Register()
    ProtoRegister.DoRegister("Mo.pb");
end	

function ProtoRegister.DoRegister(_pbName)
    local tempPath = Util.ProtoPath .. _pbName;
    local tempAddr = io.open(tempPath, "rb");

    if (tempAddr == nil) then
        logError(tostring(tempPath) .. "  path does not exist");
        return;
    end

    local tempBuffer = tempAddr:read "*a";
    tempAddr:close();

    protobuf.register(tempBuffer);
end	