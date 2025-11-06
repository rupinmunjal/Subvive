using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WebcamController : MonoBehaviour
{
    [Header("UI")]
    public RawImage rawImage;        // assign WebcamDisplay RawImage
    public AspectRatioFitter ratioFitter; // optional: AspectRatioFitter on RawImage
    public Text statusText;         // optional, for status messages

    [Header("Settings")]
    public string requestedDeviceName = "";
    public int requestedWidth = 1280;
    public int requestedHeight = 720;
    public int requestedFPS = 30;
    public bool mirrorHorizontal = false;
    public bool mirrorVertical = false;

    private WebCamTexture webCamTexture;
    private WebCamDevice[] devices;
    private int currentDeviceIndex = 0;

    IEnumerator Start()
    {
        // Enumerate devices
        devices = WebCamTexture.devices;
        if (devices == null || devices.Length == 0)
        {
            Debug.LogWarning("No webcam devices found.");
            if (statusText) statusText.text = "No webcams found";
            yield break;
        }

        // If a specific name requested, find it
        if (!string.IsNullOrEmpty(requestedDeviceName))
        {
            for (int i = 0; i < devices.Length; i++)
                if (devices[i].name.Contains(requestedDeviceName))
                {
                    currentDeviceIndex = i;
                    break;
                }
        }

        StartWebcam();
        yield return null;
    }

    public void StartWebcam()
    {
        StopWebcam();

        string deviceName = devices.Length > 0 ? devices[currentDeviceIndex].name : null;
        if (statusText) statusText.text = $"Starting: {deviceName}";

        webCamTexture = new WebCamTexture(deviceName, requestedWidth, requestedHeight, requestedFPS);
        if (rawImage) rawImage.texture = webCamTexture;

        webCamTexture.Play();
        StartCoroutine(UpdateUI());
    }

    public void StopWebcam()
    {
        if (webCamTexture != null)
        {
            if (webCamTexture.isPlaying) webCamTexture.Stop();
            Destroy(webCamTexture);
            webCamTexture = null;
        }
    }

    IEnumerator UpdateUI()
    {
        while (webCamTexture != null)
        {
            if (webCamTexture.width > 16) // means initialized
            {
                // adjust aspect ratio
                if (ratioFitter)
                {
                    float videoRatio = (float)webCamTexture.width / webCamTexture.height;
                    ratioFitter.aspectRatio = videoRatio;
                }

                // apply mirroring if needed
                if (rawImage)
                {
                    rawImage.rectTransform.localScale = new Vector3(
                        mirrorHorizontal ? -1 : 1,
                        mirrorVertical ? -1 : 1,
                        1);
                }

                if (statusText) statusText.text = $"Running: {webCamTexture.width}x{webCamTexture.height} @{webCamTexture.requestedFPS}fps";
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void FlipCamera()
    {
        if (devices == null || devices.Length <= 1) return;
        currentDeviceIndex = (currentDeviceIndex + 1) % devices.Length;
        StartWebcam();
    }

    // Capture current frame to Texture2D (useful to save an image)
    public Texture2D CaptureFrame()
    {
        if (webCamTexture == null || !webCamTexture.isPlaying) return null;

        Texture2D snap = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.RGB24, false);
        snap.SetPixels(webCamTexture.GetPixels());
        snap.Apply();
        return snap;
    }

    private void OnDisable()
    {
        StopWebcam();
    }
}
