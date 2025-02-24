using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class FlowerManager : MonoBehaviour
{
    public static FlowerManager instance;
    private Flower[] allFlowers;

    public GameObject retryPanel; // Panel retry UI
    public TextMeshProUGUI timerText; // UI teks untuk menampilkan waktu
    
    private float timeRemaining = 30f; // Waktu dalam detik
    private bool levelCompleted = false;

    public List<string> exceptionObjects;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        allFlowers = FindObjectsOfType<Flower>();
        retryPanel.SetActive(false); // Sembunyikan UI retry di awal
    }

    void Update()
    {
        if (!levelCompleted && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            ;
            
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                TimeOut();
            }
            else
            {
                UpdateTimerUI();
            }
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void CheckAllFlowersPlaced()
    {
        if (levelCompleted) return;

        foreach (Flower flower in allFlowers)
        {
            if (!flower.isPlaced)
                return;
        }

        levelCompleted = true;
        Debug.Log("Semua bunga sudah terpasang! Pindah ke level berikutnya.");
        Invoke("NextLevel", 1.5f);
    }

    void TimeOut()
    {
        if (!levelCompleted)
        {
            Time.timeScale = 0f;
            Debug.Log("Waktu habis! Silakan retry.");
            retryPanel.SetActive(true); // Munculkan UI retry
        }
    }

    void NextLevel()
    {
        int currentSlot = PlayerPrefs.GetInt("SelectedSaveSlot", 0);

        AudioManager.Instance.StopBackgroundMusicWithTransition("Bunga", 1f);
        AudioManager.Instance.PlayBackgroundMusicWithTransition("Gameplay",0,1f);
    
        SaveSlotSystem.instance.ModifyProgress(currentSlot, 5);
        SceneManager.UnloadSceneAsync("Bunga");
        Scene scene = SceneManager.GetSceneByName("Gameplay");

        foreach (GameObject obj in scene.GetRootGameObjects())
        {
            if(!exceptionObjects.Contains(obj.name))
            {
                obj.SetActive(true);
            }
        }
        
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        Debug.Log("Mengulang level...");

        // Reset timer
        timeRemaining = 30f;

        // Reset semua bunga
        foreach (Flower flower in allFlowers)
        {
            flower.ResetFlower(); // Pastikan Anda memiliki fungsi ResetFlower di script Flower untuk mereset bunga ke status awalnya.
        }

        // Reset status level
        levelCompleted = false;

        // Menyembunyikan panel retry
        retryPanel.SetActive(false);

        // Update timer UI
        UpdateTimerUI();
    }

    

}
