using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SaveSlotSystem : MonoBehaviour
{   
    public static SaveSlotSystem instance;
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
    public int progress;
    public float[] playerLastPosition;
    
    private bool isSaving = false;  // Mode Save atau Load\

    void Awake()
        {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("SaveSlotSystem sudah ada di scene ini.");
        }

        // Inisialisasi array playerLastPosition jika belum dibuat
        if (playerLastPosition == null || playerLastPosition.Length == 0)
        {
            playerLastPosition = new float[4]; 
        }
    

    }

    void Start()
    {
        LoadSaveSlots();
        LoadAutoSaveSlot0();

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
                playerLastPosition[i] = PlayerPrefs.GetFloat(savePrefix + i + "_playerPosition");
                progress = PlayerPrefs.GetInt(savePrefix + i + "_progress");

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

    void LoadAutoSaveSlot0()
    {
        if (PlayerPrefs.HasKey(savePrefix + "0_title")) // Cek apakah ada data auto-save
        {
            slotTitles[0].text = PlayerPrefs.GetString(savePrefix + "0_title");
            slotDates[0].text = PlayerPrefs.GetString(savePrefix + "0_date");
            slotTimes[0].text = PlayerPrefs.GetString(savePrefix + "0_time");
            slotImages[0].sprite = LoadImageFromPrefs(savePrefix + "0_image");
            playerLastPosition[0] = PlayerPrefs.GetFloat(savePrefix + "0_playerPosition");
            progress = PlayerPrefs.GetInt(savePrefix + "0_progress");

            slotButtons[0].interactable = true; // Bisa diklik
        }
        else
        {
            slotTitles[0].text = "No Auto Save";
            slotDates[0].text = "";
            slotTimes[0].text = "";
            slotImages[0].sprite = defaultImage;
            slotButtons[0].interactable = false; // Tidak bisa di-load
        }
    }



    public void AutoSaveSlot0()
    {
        PlayerPrefs.SetString(savePrefix + "0_title", "Auto Save");
        PlayerPrefs.SetString(savePrefix + "0_date", System.DateTime.Now.ToString("yyyy-MM-dd"));
        PlayerPrefs.SetString(savePrefix + "0_time", System.DateTime.Now.ToString("HH:mm:ss"));
        PlayerPrefs.SetFloat(savePrefix + "0_playerPosition", playerLastPosition[0]);
        PlayerPrefs.SetInt(savePrefix + "0_progress", progress);
        SaveImageToPrefs(savePrefix + "0_image", slotImages[0].sprite);

        PlayerPrefs.Save(); // Simpan perubahan ke PlayerPrefs
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
        PlayerPrefs.SetInt(savePrefix + slot + "_progress", progress);
        PlayerPrefs.SetFloat(savePrefix + slot + "_playerPosition", playerLastPosition[slot]);
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
