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

    void LoadSaveSlots()
    {
        for (int i = 0; i < slotButtons.Length; i++)
        {
            int slotIndex = i;
            if (PlayerPrefs.HasKey(savePrefix + i + "_title")) // Cek apakah slot ada isinya
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
                slotButtons[i].interactable = false; // Tidak bisa di-load
            }

            // Hapus listener sebelumnya biar tidak dobel
            slotButtons[i].onClick.RemoveAllListeners();
            slotButtons[i].onClick.AddListener(() => LoadGame(slotIndex));
        }
    }

    public void LoadGame(int slot)
    {
        if (PlayerPrefs.HasKey(savePrefix + slot + "_title"))
        {
            Debug.Log("Loading Save Slot " + (slot + 1));
            SceneManager.LoadScene("GameScene"); // Ganti dengan nama scene utama
        }
    }

    private Sprite LoadImageFromPrefs(string key)
    {
        // Implementasi pengambilan gambar jika diperlukan
        return defaultImage;
    }
}
