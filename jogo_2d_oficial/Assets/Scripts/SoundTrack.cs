using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundTrack : MonoBehaviour
{
    public static SoundTrack Instance { get; private set; }
    public AudioSource Source { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Source = GetComponent<AudioSource>();
            Source.loop = true;
            Source.playOnAwake = true; // já pode estar marcado no Inspector
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetVolume(float v)
    {
        if (Source != null)
            Source.volume = Mathf.Clamp01(v);
    }


    public void Enable(bool on)
    {
        if (Source != null)
            Source.mute = !on;
    }
}
