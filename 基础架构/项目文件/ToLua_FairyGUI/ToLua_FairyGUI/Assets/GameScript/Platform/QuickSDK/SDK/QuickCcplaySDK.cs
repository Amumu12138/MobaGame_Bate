using UnityEngine;
using System.Collections;

public class QuickCcplaySDK : QuickBaseSDK
{
	protected override string GetSource () { return "ccplay"; }
    protected override string GetGoodsID(float _price) { return ((int)_price).ToString(); }
}