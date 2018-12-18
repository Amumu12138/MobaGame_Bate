using UnityEngine;

public class MyLoaderExtension : MonoBehaviour
{
    void Start()
    {
        FairyGUI.UIObjectFactory.SetLoaderExtension(typeof(MyGLoader));
    }
}