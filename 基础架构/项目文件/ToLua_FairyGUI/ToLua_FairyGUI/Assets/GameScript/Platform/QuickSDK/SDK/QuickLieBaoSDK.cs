using UnityEngine;
using System.Collections;

public class QuickLieBaoSDK : QuickBaseSDK
{
	protected override string GetSource () { return "liebao"; }
    protected override string GetProductKey() { return "20613784"; }
    protected override string GetOrderID(string _orderID) { return _orderID + "_672"; }

    // Quick打包工具 NBA范特西打包参数
    //protected override string GetProductKey (){ return "16956686"; }
    //protected override string GetOrderID (string _orderID) { return _orderID + "_673"; }
}