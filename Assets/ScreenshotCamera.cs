using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScreenshotCamera : Singleton<ScreenshotCamera> {

    private RenderTexture renderTexture;
    private Texture2D screenShot;
    private Rect rect;
    private int counter;

    private void Start()
    {
        rect = new Rect(0, 0, Screen.width, Screen.height);
        renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    }

    public Texture2D TakePic()
    {
        Camera tempCamera = GetComponent<Camera>();
        tempCamera.enabled = true;

        tempCamera.targetTexture = renderTexture;
        tempCamera.Render();

        // read pixels will read from the currently active render texture so make our offscreen 
        // render texture active and then read the pixels
        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(rect, 0, 0);

        // reset active camera texture and render texture
        tempCamera.targetTexture = null;
        RenderTexture.active = null;
        tempCamera.enabled = false;

        return screenShot;
    }
}
