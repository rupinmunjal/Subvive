using System.Collections;
using UnityEngine;

public class MicDemoUi : MonoBehaviour
{
    public AudioSource micSource;
    public AudioSource staticSource;

    private string selectedMic;
    private bool isRecording = false;

    void Start()
    {
        if (Microphone.devices.Length > 0)
            selectedMic = Microphone.devices[0];
        else
            Debug.LogError("No microphone detected!");
    }

    public void StartRecording()
    {
        if (isRecording || selectedMic == null) return;

        micSource.clip = Microphone.Start(selectedMic, true, 5, 44100);
        while (Microphone.GetPosition(selectedMic) <= 0) { }
        micSource.loop = true;
        micSource.Play();

        staticSource.Play(); // start static here
        isRecording = true;

        Debug.Log("Mic recording started");
    }

    public void StopRecording()
    {
        if (!isRecording) return;

        Microphone.End(selectedMic);
        micSource.Stop();

        staticSource.Stop(); // stop static here
        isRecording = false;

        Debug.Log("Mic recording stopped");
    }

    IEnumerator StopStaticAfterPlayback()
    {
        yield return new WaitForSeconds(micSource.clip.length);
        staticSource.Stop();
    }

    public void PlayBack()
    {
        if (micSource.clip != null)
        {
            staticSource.Play();
            micSource.Play();
            StartCoroutine(StopStaticAfterPlayback());
        }
    }
}