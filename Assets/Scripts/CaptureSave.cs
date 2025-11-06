using UnityEngine;
using System.IO;

public class CaptureSave : MonoBehaviour
{
    public WebcamController webcam;

    public void SavePNG()
    {
        Texture2D tex = webcam.CaptureFrame();
        if (tex == null) { Debug.LogWarning("No frame to capture."); return; }
        byte[] png = tex.EncodeToPNG();
        string path = Path.Combine(Application.persistentDataPath, $"capture_{System.DateTime.Now:yyyyMMdd_HHmmss}.png");
        File.WriteAllBytes(path, png);
        Debug.Log("Saved: " + path);
    }
}
