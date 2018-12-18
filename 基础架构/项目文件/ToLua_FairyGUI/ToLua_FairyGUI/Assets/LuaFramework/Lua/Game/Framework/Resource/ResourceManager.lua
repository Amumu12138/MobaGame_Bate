ResourceManager = {}

local manifest;
local prefabMap = Map.new();
local dependenciesMap = Map.new();
local assetBundleList = List.new();
local assetBundleSuffix = ".assetbundles";

function ResourceManager.RemoveAssetBundle()
    for i = 1, #assetBundleList do
        local tempInfo = assetBundleList[i];
        tempInfo:Unload(true);
    end

    prefabMap:clear();
    assetBundleList:clear();
    IconManager.mInst:UnloadIcon();
end

function ResourceManager.LoadPrefab(_bundleName, _prefabName, _callbackFun)
    if prefabMap:containsKey(_bundleName) then
        local tempPrefab = prefabMap[_bundleName];
        if tempPrefab == nil then
            logError("资源不存在，无法加载");
            return;
        end

        _callbackFun(tempPrefab);
    else
--        if (manifest == nil) then
--            -- 加载主资源文件，新版的打包方式需要依赖于这个文件
--            local tempManifestBundle = UnityEngine.AssetBundle.LoadFromFile(Util.GetTargetAssetBundlesPath("assetbundles"));
--            manifest = tempManifestBundle:LoadAsset("AssetBundleManifest");
--            tempManifestBundle:Unload(false);
--        end

--        if (manifest ~= nil) then
--            -- 加载目标资源的全部依赖文件
--            local tempDependencieArray = manifest:GetAllDependencies(_bundleName .. assetBundleSuffix);
--            for i = 0, tempDependencieArray.Length - 1 do
--                local tempDependencie = tempDependencieArray[i];
--                -- 资源包无法被重复加载，加载相同依赖包前先检查一下是否已经加载过了
--                if dependenciesMap:containsKey(tempDependencie) == false then
--                    dependenciesMap:put(tempDependencie, List.new());
--                    -- 加载依赖文件
--                    local tempBundle = UnityEngine.AssetBundle.LoadFromFile(Util.GetTargetAssetBundlesPath(tempDependencie));
--                    if tempBundle ~= nil then
--                        local tempNameArray = tempBundle:GetAllAssetNames();
--                        for i = 0, tempNameArray.Length - 1 do
--                            local tempName = tempNameArray[i];
--                            -- 依赖文件有可能会在多处被使用，所以加载子依赖文件的时候也需要判断一下
--                            if dependenciesMap[tempDependencie]:contains(tempName) == false then
--                                tempBundle:LoadAsset(tempName);
--                                dependenciesMap[tempDependencie]:add(tempName);
--                            end
--                        end

--                        -- tempBundle:Unload(false);
--                        assetBundleList:add(tempBundle);
--                    end
--                end
--            end
--        end

        local tempBundle = UnityEngine.AssetBundle.LoadFromFile(Util.GetTargetAssetBundlesPath(_bundleName .. assetBundleSuffix));
        if tempBundle == nil then
            logError("资源无法加载 --> " .. tostring(Util.GetTargetAssetBundlesPath(_bundleName .. assetBundleSuffix)));
            return;
        end

        local tempNameArray = tempBundle:GetAllAssetNames();
        for i = 0, tempNameArray.Length - 1 do
            local tempName = tempNameArray[i];
            local tempPrefab = tempBundle:LoadAsset(tempName);
            if tempPrefab ~= nil then
                local tempPrefabName = tempPrefab.name;
                prefabMap:put(_bundleName, tempPrefab);
                if tempPrefabName == _prefabName then
                    _callbackFun(tempPrefab);
                end
            end
        end

        assetBundleList:add(tempBundle);
    end
end