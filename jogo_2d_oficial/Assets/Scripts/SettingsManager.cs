using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Referências UI")]
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle musicToggle;
    public Toggle sfxToggle;

    void Start()
    {
        // Carrega valores
        float mVol = PlayerPrefs.GetFloat("musicVolume", 1f);
        bool  mOn  = PlayerPrefs.GetInt("musicOn", 1) == 1;
        // Atualiza UI
        musicSlider.value = mVol;
        musicToggle.isOn  = mOn;

        // Aplica ao SoundTrack singleton
        if (SoundTrack.Instance != null)
        {
            SoundTrack.Instance.SetVolume(mVol);
            SoundTrack.Instance.Enable(mOn);
        }

        // Conecta callbacks
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        musicToggle.onValueChanged.AddListener(SetMusicEnabled);

        // (deixe os SFX do jeito que estava ou, se não tiver SFXManager,
        // comente/remova essa parte até você criar um sistema de efeitos)
        float sVol = PlayerPrefs.GetFloat("sfxVolume", 1f);
        bool  sOn  = PlayerPrefs.GetInt( "sfxOn", 1) == 1;
        sfxSlider.value = sVol;
        sfxToggle.isOn  = sOn;
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);
        sfxToggle.onValueChanged.AddListener(SetSfxEnabled);
    }

    public void SetMusicVolume(float vol)
    {
        PlayerPrefs.SetFloat("musicVolume", vol);
        if (SoundTrack.Instance != null)
            SoundTrack.Instance.SetVolume(vol);
    }

    public void SetMusicEnabled(bool on)
    {
        PlayerPrefs.SetInt("musicOn", on ? 1 : 0);
        if (SoundTrack.Instance != null)
            SoundTrack.Instance.Enable(on);
    }

    // Se você ainda não tiver SFX, pode deixar vazio ou comentar:
    public void SetSfxVolume(float vol)
    {
        PlayerPrefs.SetFloat("sfxVolume", vol);
        // aqui, se tiver um SfxManager, chame algo como:
        // SfxManager.Instance.SetVolume(vol);
    }

    public void SetSfxEnabled(bool on)
    {
        PlayerPrefs.SetInt("sfxOn", on ? 1 : 0);
        // SfxManager.Instance.Enable(on);
    }

    void OnApplicationQuit() => PlayerPrefs.Save();
}
