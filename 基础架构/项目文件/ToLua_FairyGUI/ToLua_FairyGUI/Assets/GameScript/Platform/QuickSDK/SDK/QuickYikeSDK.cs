
public class QuickYikeSDK : QuickBaseSDK
{
    protected override string GetSource() { return "yike"; }
    protected override string GetGoodsID(float _price) { return ((int)_price).ToString(); }
}