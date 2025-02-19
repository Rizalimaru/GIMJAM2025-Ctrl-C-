using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SaveSlotSystem : MonoBehaviour
{
    public Button saveButton;   // Tombol utama untuk Save
    public Button loadButton;   // Tombol utama untuk Load
    public Button[] slotButtons;  // Tombol untuk slot penyimpanan
    public TextMeshProUGUI[] slotTitles;  // Menampilkan "Save Slot X"
    public TextMeshProUGUI[] slotDates;   // Menampilkan tanggal
    public TextMeshProUGUI[] slotTimes;   // Menampilkan waktu
    public Image[] slotImages;   // Gambar thumbnail setiap slot
    public TextMeshProUGUI titleText; // Judul di atas slot
    public Sprite defaultImage;   // Gambar default jika belum ada save
    private string savePrefix = "SaveSlot";
    
    private bool isSaving = false;  // Mode Save atau Load

    void Start()
    {
        LoadSaveSlots();

        // Atur tombol utama
        saveButton.onClick.AddListener(() => SetMode(true));  // Aktifkan mode Save
        loadButton.onClick.AddListener(() => SetMode(false)); // Aktifkan mode Load
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
            slotButtons[i].onClick.AddListener(() => SlotAction(slotIndex));
        }
    }

    void SetMode(bool saveMode)
    {
        isSaving = saveMode;
        titleText.text = isSaving ? "Pilih slot untuk menyimpan" : "Pilih slot untuk memuat game"; // Ubah teks judul

        foreach (var button in slotButtons)
        {
            button.interactable = !isSaving || button.interactable; 
            // Saat save, semua bisa diklik, saat load hanya yang ada isinya
        }
    }

    void SlotAction(int slot)
    {
        if (isSaving)
        {
            SaveGame(slot);
        }
        else
        {
            LoadGame(slot);
        }
    }

    public void SaveGame(int slot)
    {
        string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
        string currentTime = DateTime.Now.ToString("HH:mm");

        PlayerPrefs.SetString(savePrefix + slot + "_title", "Save Slot " + (slot + 1));
        PlayerPrefs.SetString(savePrefix + slot + "_date", currentDate);
        PlayerPrefs.SetString(savePrefix + slot + "_time", currentTime);
        SaveImageToPrefs(savePrefix + slot + "_image", slotImages[slot].sprite);

        PlayerPrefs.Save();
        LoadSaveSlots();
    }

    public void LoadGame(int slot)
    {
        if (PlayerPrefs.HasKey(savePrefix + slot + "_title"))
        {
            Debug.Log("Loading Save Slot " + (slot + 1));
            SceneManager.LoadScene("GameScene"); // Ganti dengan nama scene utama
        }
    }

    private void SaveImageToPrefs(string key, Sprite sprite)
    {
        // Implementasi penyimpanan gambar jika diperlukan
    }

    private Sprite LoadImageFromPrefs(string key)
    {
        // Implementasi pengambilan gambar jika diperlukan
        return defaultImage;
    }
}
