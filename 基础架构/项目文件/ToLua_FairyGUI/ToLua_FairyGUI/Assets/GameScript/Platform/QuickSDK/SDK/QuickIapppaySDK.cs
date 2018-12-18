using UnityEngine;
using System.Collections;

public class QuickIapppaySDK : QuickBaseSDK 
{
	protected override string GetSource () { return "samsung"; }
    protected override string GetGoodsID(float _price) { return ((int)_price).ToString(); }
}