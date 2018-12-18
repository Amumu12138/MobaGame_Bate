using FairyGUI;
using UnityEngine;
using LuaFramework;
using System.Collections;
using System.Collections.Generic;

public delegate void LoadErrorCallback(string error);
public delegate void LoadCompleteCallback(NTexture texture);

public class IconManager : MonoBehaviour
{
    public static IconManager mInst;

    public const int MAX_POOL_SIZE = 10;
    public const int POOL_CHECK_TIME = 30;

	bool started;
    Hashtable pool = new Hashtable();
    List<LoadItem> items = new List<LoadItem>();
    Dictionary<string, AssetBundle> assetBundleList = new Dictionary<string, AssetBundle>();

    void Awake()
	{
        mInst = this;
	}

	public void LoadIcon(string _url, LoadCompleteCallback _onSuccess, LoadErrorCallback _onFail)
	{
		LoadItem tempItem = new LoadItem();
		tempItem.url = _url;
		tempItem.onSuccess = _onSuccess;
		tempItem.onFail = _onFail;
		items.Add(tempItem);
        if (!started)
        {
            StartCoroutine(Run());
        }
    }

    public void UnloadIcon()
    {
        if (assetBundleList == null)
        {
            return;
        }

        items.Clear();
        started = false;
        StopAllCoroutines();

        List<string> tempRemoveKey = new List<string>();
        foreach (DictionaryEntry de in pool)
        {
            string tempKey = (string)de.Key;
            NTexture tempTexture = (NTexture)de.Value;
            AssetBundle tempBundle = assetBundleList[tempKey];
            if (tempTexture.refCount == 0)
            {
                tempRemoveKey.Add(tempKey);
                tempTexture.Dispose();
                tempBundle.Unload(true);
            }
        }

        if (tempRemoveKey.Count != 0)
        {
            for (int i = 0; i < tempRemoveKey.Count; i++)
            {
                string tempKey = tempRemoveKey[i];
                pool.Remove(tempKey);
                assetBundleList.Remove(tempKey);
            }
        }
    }

    IEnumerator Run()
	{
		started = true;

		LoadItem tempItem = null;
		while (true)
		{
            if (items.Count > 0)
            {
                tempItem = items[0];
                items.RemoveAt(0);
            }
            else
            {
                break;
            }

            if (pool.ContainsKey(tempItem.url))
            {
                NTexture tempTexture = (NTexture)pool[tempItem.url];
                if (tempTexture == null)
                {
                    pool.Remove(tempItem.url);
                }
            }

			if (pool.ContainsKey(tempItem.url))
			{
                yield return null;
                NTexture tempTexture = (NTexture)pool[tempItem.url];
                tempTexture.refCount++;

                if (tempItem.onSuccess != null)
                {
                    tempItem.onSuccess(tempTexture);
                }

				continue;
			}

            AssetBundle bundle = AssetBundle.LoadFromFile(Util.GetTargetAssetBundlesPath(tempItem.url));
            yield return null;

            if (bundle != null)
			{
				NTexture texture = new NTexture(bundle.LoadAllAssets<Texture2D>()[0]);
                texture.refCount++;
                pool[tempItem.url] = texture;
                assetBundleList.Add(tempItem.url, bundle);
                
                try
                {
                    if (tempItem.onSuccess != null)
                    {
                        tempItem.onSuccess(texture);
                    }
                }
                catch
                {
                    started = false;
                }
            }
            else
            {
                Debug.LogError("<color=#FF0000>加载资源失败 -----> " + tempItem.url + "</color>");
            }
		}
		started = false;
    }

	IEnumerator FreeIdleIcons()
	{
        while (true)
        {
            yield return new WaitForSeconds(POOL_CHECK_TIME); //check the pool every 30 seconds

            int tempCount = pool.Count;
            if (tempCount > MAX_POOL_SIZE)
            {
                ArrayList tempToRemove = null;
                foreach (DictionaryEntry de in pool)
                {
                    string tempKey = (string)de.Key;
                    NTexture tempTexture = (NTexture)de.Value;
                    if (tempTexture.refCount == 0)
                    {
                        if (tempToRemove == null)
                        {
                            tempToRemove = new ArrayList();
                        }
                        tempToRemove.Add(tempKey);
                        tempTexture.Dispose();

                        tempCount--;
                        if (tempCount <= 8)
                        {
                            break;
                        }
                    }
                }
                if (tempToRemove != null)
                {
                    foreach (string key in tempToRemove)
                    {
                        pool.Remove(key);
                    }
                }
            }
        }
	}
}

class LoadItem
{
	public string url;
	public LoadCompleteCallback onSuccess;
	public LoadErrorCallback onFail;
}
