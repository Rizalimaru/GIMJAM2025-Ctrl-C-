using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuSaveLoad : MonoBehaviour
{
    public Button[] slotButtons;  // Tombol slot penyimpanan
    public TextMeshProUGUI[] slotTitles;  // Menampilkan "Save Slot X"
    public TextMeshProUGUI[] slotDates;   // Menampilkan tanggal
    public TextMeshProUGUI[] slotTimes;   // Menampilkan waktu
    public Image[] slotImages;   // Gambar thumbnail setiap slot
    public TextMeshProUGUI titleText; // Judul di atas slot
    public Sprite defaultImage;   // Gambar default jika belum ada save
    private string savePrefix = "SaveSlot";

    void Start()
    {
        titleText.text = "Pilih slot untuk memuat game"; // Set judul untuk Load Mode
        LoadSaveSlots();
    }


    public void NewGame()
    {
        int slot = 0;
     

        // Simpan data baru ke slot yang dipilih
        PlayerPrefs.SetString(savePrefix + slot + "_title", "Auto Save");
        PlayerPrefs.SetString(savePrefix + slot + "_date", DateTime.Now.ToString("dd/MM/yyyy"));
        PlayerPrefs.SetString(savePrefix + slot + "_time", DateTime.Now.ToString("HH:mm"));
        PlayerPrefs.SetString(savePrefix + slot + "_image", ""); // Jika ada gambar, simpan di sini
        PlayerPrefs.SetInt(savePrefix + slot + "_progress", 0); // Reset progress
        PlayerPrefs.SetFloat(savePrefix + slot + "_playerPosition", 0f); // Reset posisi karakter

        PlayerPrefs.SetInt("SelectedSaveSlot", slot); // Tambahkan ini
        PlayerPrefs.Save();

        Debug.Log("New Game dimulai di slot " + slot);
        
        // Pindah ke scene game
        StartCoroutine(SceneController.instance.LoadScene("GamePlay"));
    }



    void LoadSaveSlots()
    {
        for (int i = 0; i < slotButtons.Length; i++)
        {
            int slotIndex = i;

            if (PlayerPrefs.HasKey(savePrefix + i + "_title")) // Jika slot ada isinya
            {
                slotTitles[i].text = PlayerPrefs.GetString(savePrefix + i + "_title");
                slotDates[i].text = PlayerPrefs.GetString(savePrefix + i + "_date");
                slotTimes[i].text = PlayerPrefs.GetString(savePrefix + i + "_time");
                slotImages[i].sprite = LoadImageFromPrefs(savePrefix + i + "_image");
                slotButtons[i].interactable = true; // Bisa diklik
            }
            else
            {
                slotTitles[i].text = "Empty Slot";
                slotDates[i].text = "";
                slotTimes[i].text = "";
                slotImages[i].sprite = defaultImage;
                slotButtons[i].interactable = false; // Tidak bisa diklik
            }


            slotButtons[i].onClick.RemoveAllListeners();
            slotButtons[i].onClick.AddListener(() => LoadGame(slotIndex));
        }
    }



    

    public void LoadGame(int slot)
    {
        if (PlayerPrefs.HasKey(savePrefix + slot + "_title"))
        {
            Debug.Log("Loading Save Slot " + (slot + 1));



            // Simpan informasi slot yang dipilih sebelum memuat scene buat nanti set position karakter
            PlayerPrefs.SetInt("SelectedSaveSlot", slot);
            PlayerPrefs.Save();

            StartCoroutine(SceneController.instance.LoadScene("GamePlay"));




        }
    }

    private Sprite LoadImageFromPrefs(string key)
    {
        // Implementasi pengambilan gambar jika diperlukan
        return defaultImage;
    }
}
