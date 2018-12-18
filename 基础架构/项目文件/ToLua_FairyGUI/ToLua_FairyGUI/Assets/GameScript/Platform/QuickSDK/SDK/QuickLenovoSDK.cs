using UnityEngine;
using System.Collections;

public class QuickLenovoSDK : QuickBaseSDK
{
	protected override string GetSource () { return "lenovo"; }
    protected override string GetGoodsID(float _price) { return ((int)_price).ToString(); }
}