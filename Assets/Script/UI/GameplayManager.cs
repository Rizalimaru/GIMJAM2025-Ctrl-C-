using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;
    public GameObject uiPause;
    [SerializeField] private GameObject saveUI;
    [SerializeField] private GameObject preferencesUI;
    private bool isPaused = false;

    public Toggle fullscreenCheck;
    public TextMeshProUGUI fullscreenStatusText; // UI Text untuk status

    public TextMeshProUGUI warningText;

    public GameObject uiWarning;

    void Start()
    {
        AudioManager.Instance.PlayBackgroundMusicWithTransition("Gameplay",0,3f);
        instance = this;
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        fullscreenCheck.isOn = isFullscreen;
        Screen.fullScreen = isFullscreen;

        // Update teks status awal
        UpdateFullscreenText(isFullscreen);

        // Tambahkan listener untuk Toggle
        fullscreenCheck.onValueChanged.AddListener(SetFullscreen);

        
        if (uiPause != null)
            uiPause.SetActive(false); // Pastikan UI pause tidak aktif saat mulai
        if (saveUI != null)
            saveUI.SetActive(false);
        if (preferencesUI != null)
            preferencesUI.SetActive(false);
        if (uiWarning != null)
            uiWarning.SetActive(false);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (uiWarning.activeSelf) // Jika UI Warning aktif, tutup dulu
            {
                uiWarning.SetActive(false);
            }
            else if (saveUI.activeSelf || preferencesUI.activeSelf)
            {
                CloseAllMenus();
                uiPause.SetActive(true);
            }
            else
            {
                TogglePause();
            }
        }
    }


    public void TogglePause()
    {
        isPaused = !isPaused;
        uiPause.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1; // Pause game saat UI aktif
    }

    public void ResumeGame()
    {
        isPaused = false;
        uiPause.SetActive(false);
        Time.timeScale = 1;
    }

    public void OpenSave()
    {
        saveUI.SetActive(true);
    }

    public void WarningSave(){
        uiWarning.SetActive(true);
    }

    public void CloseWarningSave(){
        uiWarning.SetActive(false);
    }


    public void OpenPreferences()
    {

        preferencesUI.SetActive(true);
    }


    public void CloseAllMenus()
    {
        saveUI.SetActive(false);
        preferencesUI.SetActive(false);
        uiPause.SetActive(true);
    }

    public void Mainmenu()
    {
        Time.timeScale = 1; // Pastikan waktu berjalan normal sebelum pindah scene
        SceneManager.LoadScene("Mainmenu");
        Debug.Log("Kembali ke Main Menu");
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();

        // Update teks status saat toggle berubah
        UpdateFullscreenText(isFullscreen);
    }

    void UpdateFullscreenText(bool isFullscreen)
    {
        fullscreenStatusText.text = isFullscreen ? "ON" : "OFF";
    }
}
