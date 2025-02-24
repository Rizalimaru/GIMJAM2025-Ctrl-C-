using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ManagerMini : MonoBehaviour
{
    public static ManagerMini instance;
    public Slider progressBar;
    public TextMeshProUGUI hasil;
    public List<string> exceptionObjects;

    private int totalTrash = 10; // Total jumlah sampah yang harus dikumpulkan
    private int collectedTrash = 0;

    void Start()
    {
        instance = this;
        AudioManager.Instance.PlayBackgroundMusicWithTransition2("Sampah", 0,1f,0.5f);
    }

    public void AddProgress()
    {
        collectedTrash++;
        progressBar.value = (float)collectedTrash / totalTrash;

        if (collectedTrash >= totalTrash)
        {
            hasil.gameObject.SetActive(true);
            Invoke("ReturnToGame", 2f);
            // Tambahkan efek kemenangan atau lanjutkan ke level berikutnya
        }
    }

    public void ReturnToGame()
    {   
        int currentSlot = PlayerPrefs.GetInt("SelectedSaveSlot", 0);

        AudioManager.Instance.StopBackgroundMusicWithTransition("Sampah", 1f);

        AudioManager.Instance.PlayBackgroundMusicWithTransition("Gameplay",0,1f);
    
        SaveSlotSystem.instance.ModifyProgress(currentSlot, 2);
        SceneManager.UnloadSceneAsync("TongSampah");
        Scene scene = SceneManager.GetSceneByName("Gameplay");

        foreach (GameObject obj in scene.GetRootGameObjects())
        {
            if(!exceptionObjects.Contains(obj.name))
            {
                obj.SetActive(true);
            }
        }

    }
}
