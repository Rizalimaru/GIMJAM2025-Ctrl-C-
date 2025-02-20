using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioSettingMainMenu : MonoBehaviour
{
    public Slider sliderMasterVolume;
    [SerializeField] private Slider sliderBackgroundMusic;
    [SerializeField] private Slider sliderSoundEffect;

    public Sprite[] spritemute;
    public Button buttonMute;

    private float previousMasterVolume;

    public TextMeshProUGUI muteStatusText;

    public static AudioSettingMainMenu Instance { get; private set; }

    private void Start()
    {
        
        if (PlayerPrefs.GetInt("FirstRun", 1) == 1) // Cek apakah ini pertama kali dijalankan
            {
                PlayerPrefs.DeleteAll(); // Hapus semua save
                PlayerPrefs.SetInt("FirstRun", 0); // Tandai bahwa game sudah dijalankan
                PlayerPrefs.Save();
            }
        

        Instance = this;
        
    }

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }

        if (AudioManager.Instance != null)
        {
            InitializeSliders();
            UpdateMuteButtonSprite();
            UpdateMuteStatusText();
            SetupSliderListeners();
            
            AudioManager.Instance.LoadVolumeSettings();

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
        buttonMute.image.sprite = AudioManager.Instance.IsMasterMuted() ? spritemute[1] : spritemute[0];
    }

    private void UpdateMuteStatusText()
    {
        muteStatusText.text = AudioManager.Instance.IsMasterMuted() ? "ON" : "OFF";
    }

    private void SetupSliderListeners()
    {
        sliderMasterVolume.onValueChanged.AddListener(SetMasterVolume);
        sliderBackgroundMusic.onValueChanged.AddListener(value => AudioManager.Instance.SetVolume("BackgroundMusic", value));
        sliderSoundEffect.onValueChanged.AddListener(value => AudioManager.Instance.SetVolume("SoundEffect", value));
    }

    public void SetMasterVolume(float sliderValue)
    {
        AudioManager.Instance.SetVolume("MasterVolume", sliderValue);
        buttonMute.image.sprite = (sliderValue <= 0.0001f) ? spritemute[1] : spritemute[0];

        UpdateMuteStatusText();
    }

    public void ButtonMute()
    {
        if (buttonMute.image != null)
        {
            AudioManager.Instance.ToggleMasterMute();
            if (AudioManager.Instance.IsMasterMuted())
            {
                previousMasterVolume = sliderMasterVolume.value;
                sliderMasterVolume.value = 0.0001f; // Atur slider ke nilai minimum jika mute
            }
            else
            {
                sliderMasterVolume.value = previousMasterVolume > 0.0001f ? previousMasterVolume : 1f;
            }

            UpdateMuteButtonSprite();
            UpdateMuteStatusText();
        }
    }

    public void PlaySFXSound(string soundName, int index)
    {
        AudioManager.Instance.PlaySFX(soundName, index);
    }

    public void PlayButton(){
        PlaySFXSound("Button",0);
        Debug.Log("Main");
    }

    public void StopBackgroundMusic()
    {
        AudioManager.Instance.StopBackgroundMusicWithTransition("Mainmenu", 1f);
    }
}