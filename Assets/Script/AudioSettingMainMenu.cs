using UnityEngine;
using UnityEngine.UI;

public class AudioSettingMainMenu : MonoBehaviour
{
    public Slider sliderMasterVolume;
    [SerializeField] private Slider sliderBackgroundMusic;
    [SerializeField] private Slider sliderSoundEffect;

    public Sprite[] spritemute;
    public Button buttonMute;

    private AudioManager audioManagerInstance;
    private float previousMasterVolume;

    public static AudioSettingMainMenu Instance { get; private set; }

    private void Start()
    {
        audioManagerInstance.LoadVolumeSettings();
    }

    private void Awake()
    {
        audioManagerInstance = AudioManager.Instance;

        if (Instance == null)
        {
            Instance = this;
        }

        if (audioManagerInstance != null)
        {
            InitializeSliders();
            UpdateMuteButtonSprite();
            SetupSliderListeners();
            


            audioManagerInstance.PlayBackgroundMusicWithTransition("Mainmenu", 0, 1f);
        }
        else
        {
            Debug.LogWarning("AudioManager instance not found.");
        }
    }

    private void InitializeSliders()
    {
        sliderMasterVolume.value = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        sliderBackgroundMusic.value = PlayerPrefs.GetFloat("BackgroundMusic", 1.0f);
        sliderSoundEffect.value = PlayerPrefs.GetFloat("SoundEffect", 1.0f);
    }

    private void UpdateMuteButtonSprite()
    {
        buttonMute.image.sprite = audioManagerInstance.IsMasterMuted() ? spritemute[1] : spritemute[0];
    }

    private void SetupSliderListeners()
    {
        sliderMasterVolume.onValueChanged.AddListener(SetMasterVolume);
        sliderBackgroundMusic.onValueChanged.AddListener(value => audioManagerInstance.SetVolume("BackgroundMusic", value));
        sliderSoundEffect.onValueChanged.AddListener(value => audioManagerInstance.SetVolume("SoundEffect", value));
    }

    public void SetMasterVolume(float sliderValue)
    {
        audioManagerInstance.SetVolume("MasterVolume", sliderValue);
        buttonMute.image.sprite = (sliderValue <= 0.0001f) ? spritemute[1] : spritemute[0];
    }

    public void ButtonMute()
    {
        if (buttonMute.image != null)
        {
            audioManagerInstance.ToggleMasterMute();
            if (audioManagerInstance.IsMasterMuted())
            {
                previousMasterVolume = sliderMasterVolume.value;
                sliderMasterVolume.value = 0.0001f; // Atur slider ke nilai minimum jika mute
            }
            else
            {
                sliderMasterVolume.value = previousMasterVolume > 0.0001f ? previousMasterVolume : 1f;
            }
            UpdateMuteButtonSprite();
        }
    }

    public void PlaySFXSound(string soundName, int index)
    {
        audioManagerInstance.PlaySFX(soundName, index);
    }

    public void PlayButton(){
        PlaySFXSound("Button",0);
        Debug.Log("Main");
    }

    public void StopBackgroundMusic()
    {
        audioManagerInstance.StopBackgroundMusicWithTransition("Mainmenu", 1f);
    }
}