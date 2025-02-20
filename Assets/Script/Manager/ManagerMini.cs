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

    private int totalTrash = 10; // Total jumlah sampah yang harus dikumpulkan
    private int collectedTrash = 0;

    void Start()
    {
        instance = this;
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
        
        SceneManager.LoadScene("GamePlay");
    }
}
