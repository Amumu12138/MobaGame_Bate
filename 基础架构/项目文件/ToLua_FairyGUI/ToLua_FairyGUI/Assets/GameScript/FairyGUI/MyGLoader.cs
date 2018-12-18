using FairyGUI;
using UnityEngine;

public class MyGLoader : GLoader
{
	protected override void LoadExternal()
	{
		IconManager.mInst.LoadIcon(this.url, OnLoadSuccess, OnLoadFail);
	}

	protected override void FreeExternal(NTexture _texture)
	{
        _texture.refCount--;
	}

	void OnLoadSuccess(NTexture _texture)
	{
        if (string.IsNullOrEmpty(this.url))
        {
            return;
        }

        try
        {
            this.onExternalLoadSuccess(_texture);
        }
        catch{}
    }

	void OnLoadFail(string _error)
	{
		this.onExternalLoadFailed();
	}
}