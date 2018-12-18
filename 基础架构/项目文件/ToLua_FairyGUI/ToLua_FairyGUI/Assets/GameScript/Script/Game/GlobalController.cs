using UnityEngine;

public class GlobalController : MonoBehaviour
{
    public bool doRookie = true;
    public bool isNewMatch = false;
    public bool loginDirect = false;

    public string yuliuziduan_1 = "";
    public string yuliuziduan_2 = "";
    public string yuliuziduan_3 = "";
    public string yuliuziduan_4 = "";
    public string yuliuziduan_5 = "";
    public string yuliuziduan_6 = "";
    public string yuliuziduan_7 = "";
    public string yuliuziduan_8 = "";
    public string yuliuziduan_9 = "";

    public static GlobalController mInst;

    void Awake()
    {
        mInst = this;
    }
}