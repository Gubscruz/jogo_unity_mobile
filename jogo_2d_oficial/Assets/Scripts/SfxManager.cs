using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public static SfxManager Instance { get; private set; }

    [Header("AudioSource para efeitos")]
    public AudioSource source;

    // Valores lidos via PlayerPrefs
    public float Volume { get; private set; } = 1f;
    public bool  Enabled { get; private set; } = true;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Inicializa do PlayerPrefs
        Volume  = PlayerPrefs.GetFloat("sfxVolume", 1f);
        Enabled = PlayerPrefs.GetInt("sfxOn", 1) == 1;
        ApplySettings();
    }

    void ApplySettings()
    {
        // Ajusta mute direto na fonte
        source.mute   = !Enabled;
        source.volume = Mathf.Clamp01(Volume);
    }

    public void SetVolume(float v)
    {
        Volume = Mathf.Clamp01(v);
        PlayerPrefs.SetFloat("sfxVolume", Volume);
        ApplySettings();
    }

    public void SetEnabled(bool on)
    {
        Enabled = on;
        PlayerPrefs.SetInt("sfxOn", on ? 1 : 0);
        ApplySettings();
    }
    
    /// <summary>
    /// Use em vez de audioSource.PlayOneShot()
    /// </summary>
    public void Play(AudioClip clip)
    {
        if (Enabled && clip != null)
            source.PlayOneShot(clip, Volume);
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
