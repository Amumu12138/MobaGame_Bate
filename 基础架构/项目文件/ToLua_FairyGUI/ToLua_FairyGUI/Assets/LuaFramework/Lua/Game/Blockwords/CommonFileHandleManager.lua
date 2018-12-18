local this;

CommonFileHandleManager = {
	blockWords = "";
	blockWordArr = nil;
	path = tostring(Util.LocalGameAssetsPath) .. "Blockwords.txt";
};

this = CommonFileHandleManager;

function CommonFileHandleManager.New ()
	if (this.FileExists(this.path)) then
		this.ReadFile(this.path);
	end

	this.blockWordArr = Split(this.blockWords, ",");
end

function CommonFileHandleManager.FileExists (_path)
	local tmpFile = io.open(_path, "r");
	if tmpFile then 
		tmpFile:close();
	end
	return tmpFile ~= nil;
end

function CommonFileHandleManager.ReadFile (_path)
	if not this.FileExists(_path) then 
		return;
	end

	local tmpLines = {};
	local tmpFile = io.open(_path, "r");
	local tmpList = io.lines(_path);

	for line in tmpList do
		tmpLines[#tmpLines + 1] = line;
	end

	this.blockWords = tmpLines[1];
	
	tmpFile:close();
	tmpFile = nil;
end

function CommonFileHandleManager.CheckFirstBlockwords (_srcStr)
	local tempWord;
    local tempIndex = 0;
	local tempIsFind = false;
    local tempSrcStr = _srcStr or "";

	for i = 1, #this.blockWordArr do
		tempWord = this.blockWordArr[i];
		if (nil ~= string.find(_srcStr, tempWord)) then
			tempIsFind = true;
			tempIndex = i;
			break ;
		end
	end

	if (not tempIsFind) then
		tempWord = "";
	end
	return { tempWord, tempIndex };
end

function CommonFileHandleManager.DealBlockwords (_srcStr)
	local tempWord;
	local tempSrcStr = _srcStr or "";

	for i = 1, #this.blockWordArr do
		tempWord = this.blockWordArr[i];
		tempSrcStr = string.gsub(tempSrcStr, tempWord, "***");
	end

	return tempSrcStr;
end