using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotCamera : Singleton<ScreenshotCamera> {

    private RenderTexture renderTexture;
    private Texture2D screenShot;
    private Rect rect;

    private void Start()
    {
        rect = new Rect(0, 0, 800, 600);
    }

    public Texture2D TakePic()
    {
		renderTexture = new RenderTexture(800, 600, 24);
		screenShot = new Texture2D(800, 600, TextureFormat.RGB24, false);
        Camera tempCamera = GetComponent<Camera>();
        tempCamera.enabled = true;
        tempCamera.targetTexture = renderTexture;
        tempCamera.Render();
		RenderTexture.active = renderTexture;

		screenShot.ReadPixels(rect, 0, 0);
        
		tempCamera.targetTexture = null;
		RenderTexture.active = null; // added to avoid errors 
		DestroyImmediate(renderTexture);
        tempCamera.enabled = false;
		screenShot.Apply ();
        return screenShot;
    }
}
