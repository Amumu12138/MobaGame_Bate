using UnityEngine;
using System.Collections;

public class QuickAiyouxiSDK : QuickBaseSDK
{
	protected override string GetSource () { return "aiyouxi"; }
    protected override string GetCallbackUrl() { return "http://callback.sdk.quicksdk.net/callback/21/10918317025310946243337522333215"; }
}