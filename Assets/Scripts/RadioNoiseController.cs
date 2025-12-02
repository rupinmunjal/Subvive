using UnityEngine;

public class RadioNoiseController : MonoBehaviour
{
    public AudioSource voiceSource;   // #1
    public AudioSource staticSource;  // #2
    public AudioSource morseSource;   // #3

    public float maxDistance = 200f;  
    public Transform player;
    public Transform otherPlayer;

    void Start()
    {

        var all = Object.FindObjectsByType<RadioNoiseController>(FindObjectsSortMode.None);

        foreach (RadioNoiseController r in all)
        {
            if (r != this)
            {
                otherPlayer = r.player;
            }
        }
    }

    void Update()
    {
        if (otherPlayer == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject p in players)
            {
                if (p.transform != player)
                {
                    otherPlayer = p.transform;
                    break;
                }
            }

            return;
        }

        if (player == null || otherPlayer == null) return;

        float dist = Vector3.Distance(player.position, otherPlayer.position);
        float t = Mathf.Clamp01(dist / maxDistance);

        // Person volume: 100% → 60%
        float voiceVolume = Mathf.Lerp(1f, 0.6f, t);
        voiceSource.volume = voiceVolume;

        // Noise volume: 1% → 50%
        float noiseVolume = Mathf.Lerp(0.01f, 0.5f, t);
        staticSource.volume = noiseVolume;
        morseSource.volume = noiseVolume;


        if (voiceSource.isPlaying)
        {
            if (!staticSource.isPlaying) staticSource.Play();
            if (!morseSource.isPlaying) morseSource.Play();
        }
        else
        {
            staticSource.Stop();
            morseSource.Stop();
        }
    }
}
