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
        // Música (como você já tem)
        float mVol = PlayerPrefs.GetFloat("musicVolume", 1f);
        bool  mOn  = PlayerPrefs.GetInt("musicOn", 1) == 1;
        musicSlider.value = mVol;
        musicToggle.isOn  = mOn;
        SoundTrack.Instance.SetVolume(mVol);
        SoundTrack.Instance.Enable(mOn);

        musicSlider.onValueChanged.AddListener(v => {
            PlayerPrefs.SetFloat("musicVolume", v);
            SoundTrack.Instance.SetVolume(v);
        });
        musicToggle.onValueChanged.AddListener(on => {
            PlayerPrefs.SetInt("musicOn", on?1:0);
            SoundTrack.Instance.Enable(on);
        });

        // EFEITOS
        // Inicializa o slider/toggle de SFX lendo do PlayerPrefs
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1f);
        sfxToggle.isOn  = PlayerPrefs.GetInt("sfxOn", 1) == 1;

        // Conecta ao SfxManager
        sfxSlider.onValueChanged.AddListener(v => {
            SfxManager.Instance.SetVolume(v);
        });
        sfxToggle.onValueChanged.AddListener(on => {
            SfxManager.Instance.SetEnabled(on);
        });
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
