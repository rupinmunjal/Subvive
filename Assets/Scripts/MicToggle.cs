using Photon.Voice.Unity;
using UnityEngine;

public class MicToggle : MonoBehaviour
{
    public Recorder recorder;

    void Update()
    {
        if (recorder == null) return;

        if (Input.GetKeyDown(KeyCode.M))
        {
            recorder.TransmitEnabled = !recorder.TransmitEnabled;
            Debug.Log("Mic toggled. Mic now: " + recorder.TransmitEnabled);
        }
    }
}
