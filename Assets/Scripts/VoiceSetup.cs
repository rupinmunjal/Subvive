using Photon.Pun;
using Photon.Voice.Unity;
using UnityEngine;

public class VoiceSetup : MonoBehaviour
{
    private PhotonView pv;
    private Recorder recorder;
    private Speaker speaker;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        recorder = GetComponent<Recorder>();
        speaker = GetComponentInChildren<Speaker>();
    }

    private void Start()
    {
        // Local player = talk but don't hear self
        if (pv.IsMine)
        {
            if (speaker != null)
                speaker.enabled = false;   // mute local playback

            if (recorder != null)
                recorder.TransmitEnabled = true; // send voice normally
        }
        else
        {
            // Remote players = hear them
            if (speaker != null)
                speaker.enabled = true;
        }
    }
}
