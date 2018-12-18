// Attach this to a GUIText to make a frames/second indicator.
//
// It calculates frames/second over each updateInterval,
// so the display does not keep changing wildly.
//
// It is also fairly accurate at very low FPS counts (<10).
// We do this not by simply counting frames per interval, but
// by accumulating FPS for each frame. This way we end up with
// correct overall FPS even if the interval renders something like
// 5.5 frames.

// A FPS counter.
// It calculates frames/second over each updateInterval,
// so the display does not keep changing wildly.
using UnityEngine;

public class FPS : MonoBehaviour
{
    float fps;              // Current FPS
    int frames = 0;         // Frames over current interval
    double lastInterval;    // Last interval end time
    float updateInterval = 0.5f;

    void Start()
    {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
    }

    void Update()
    {
        ++frames;
        double timeNow = Time.realtimeSinceStartup;
        if (timeNow > lastInterval + updateInterval)
        {
            fps = (float)(frames / (timeNow - lastInterval));
            frames = 0;
            lastInterval = timeNow;
        }
    }

    void OnGUI()
    {
        GUILayout.Label("FPS : " + fps.ToString("f2"));
    }
}