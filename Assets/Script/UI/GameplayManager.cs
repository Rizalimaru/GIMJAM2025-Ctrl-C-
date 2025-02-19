using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private GameObject uiPause;
    [SerializeField] private GameObject saveUI;
    [SerializeField] private GameObject preferencesUI;
    [SerializeField] private GameObject loadUI;
    private bool isPaused = false;

    void Start()
    {
        if (uiPause != null)
            uiPause.SetActive(false); // Pastikan UI pause tidak aktif saat mulai
        if (saveUI != null)
            saveUI.SetActive(false);
        if (preferencesUI != null)
            preferencesUI.SetActive(false);
        if (loadUI != null)
            loadUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (saveUI.activeSelf || preferencesUI.activeSelf || loadUI.activeSelf)
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

    public void OpenPreferences()
    {

        preferencesUI.SetActive(true);
    }

    public void OpenLoad()
    {
        loadUI.SetActive(true);
    }

    public void CloseAllMenus()
    {
        saveUI.SetActive(false);
        preferencesUI.SetActive(false);
        loadUI.SetActive(false);
        uiPause.SetActive(true);
    }

    public void Mainmenu()
    {
        Time.timeScale = 1; // Pastikan waktu berjalan normal sebelum pindah scene
        SceneManager.LoadScene("Mainmenu");
        Debug.Log("Kembali ke Main Menu");
    }
}
