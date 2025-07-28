using UnityEngine;

public class AspectRatioEnforcer: MonoBehaviour
{
    public float targetAspectX = 16f;
    public float targetAspectY = 9f;
    private float targetAspectRatio; // Set your desired aspect ratio (e.g., 16:9)
    private int lastWidth;
    private int lastHeight;

    void Start()
    {
        targetAspectRatio = targetAspectX / targetAspectY;
        lastWidth = Screen.width;
        lastHeight = Screen.height;
        ApplyAspectRatio();
    }

    void Update()
    {
        if(Screen.width != lastWidth || Screen.height != lastHeight)
        {
            ApplyAspectRatio();
            lastWidth = Screen.width;
            lastHeight = Screen.height;
        }
    }

    void ApplyAspectRatio()
    {
        float currentAspectRatio = (float)Screen.width / Screen.height;

        if(currentAspectRatio > targetAspectRatio)
        {
            // Window is wider than target aspect ratio, adjust width
            int newWidth = Mathf.RoundToInt( Screen.height * targetAspectRatio );
            Screen.SetResolution( newWidth, Screen.height, false );
        }
        else if(currentAspectRatio < targetAspectRatio)
        {
            // Window is taller than target aspect ratio, adjust height
            int newHeight = Mathf.RoundToInt( Screen.width / targetAspectRatio );
            Screen.SetResolution( Screen.width, newHeight, false );
        }
    }
}