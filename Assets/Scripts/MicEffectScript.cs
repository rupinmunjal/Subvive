using UnityEngine;

public class MicEffectScript : MonoBehaviour
{
    AudioSource micSource;

    void Start()
    {
        micSource = GetComponent<AudioSource>();
        micSource.outputAudioMixerGroup = Resources.Load<UnityEngine.Audio.AudioMixer>("RadioMixer")
            .FindMatchingGroups("Master")[0];

        string mic = Microphone.devices.Length > 0 ? Microphone.devices[0] : null;
        if (mic != null)
        {
            micSource.clip = Microphone.Start(mic, true, 5, 44100);
            while (Microphone.GetPosition(mic) <= 0) { } // wait for mic
            micSource.loop = true;
            micSource.Play();
        }
    }
}
