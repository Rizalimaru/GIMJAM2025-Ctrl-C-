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

    public void NewGame()
    {
        // Hapus semua save slot
        for (int i = 0; i < slotButtons.Length; i++)
        {
            PlayerPrefs.DeleteKey(savePrefix + i + "_title");
            PlayerPrefs.DeleteKey(savePrefix + i + "_date");
            PlayerPrefs.DeleteKey(savePrefix + i + "_time");
            PlayerPrefs.DeleteKey(savePrefix + i + "_progress");
            PlayerPrefs.DeleteKey(savePrefix + i + "_playerPosition");
            PlayerPrefs.DeleteKey(savePrefix + i + "_image");
        }

        // Hapus auto save
        PlayerPrefs.DeleteKey(savePrefix + "0_title");
        PlayerPrefs.DeleteKey(savePrefix + "0_date");
        PlayerPrefs.DeleteKey(savePrefix + "0_time");
        PlayerPrefs.DeleteKey(savePrefix + "0_progress");
        PlayerPrefs.DeleteKey(savePrefix + "0_playerPosition");
        PlayerPrefs.DeleteKey(savePrefix + "0_image");

        PlayerPrefs.Save(); // Simpan perubahan

        Debug.Log("Semua data telah direset. Memulai game baru...");
        
        progress = 0;

        // Load scene pertama (misalnya, "GameScene" atau "Level1")
        SceneManager.LoadScene("GamePlay");
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
        // Pastikan player memiliki posisi yang valid
        float playerX = GameObject.FindGameObjectWithTag("Player").transform.position.x; 

        // Simpan data auto save di slot 0
        PlayerPrefs.SetString(savePrefix + "0_title", "Auto Save");
        PlayerPrefs.SetString(savePrefix + "0_date", System.DateTime.Now.ToString("dd/MM/yyyy"));
        PlayerPrefs.SetString(savePrefix + "0_time", System.DateTime.Now.ToString("HH:mm"));
        PlayerPrefs.SetFloat(savePrefix + "0_playerPosition", playerX);
        PlayerPrefs.SetInt(savePrefix + "0_progress", progress);

        // Simpan gambar jika perlu
        if (slotImages.Length > 0 && slotImages[0] != null)
        {
            SaveImageToPrefs(savePrefix + "0_image", slotImages[0].sprite);
        }

        // Tetapkan slot yang digunakan
        PlayerPrefs.SetInt("SelectedSaveSlot", 0);

        // Simpan perubahan
        PlayerPrefs.Save();

        Debug.Log("Auto Save berhasil di slot 0 dengan posisi X: " + playerX);
    }





    

    void SetMode(bool saveMode)
    {
        isSaving = saveMode;
        titleText.text = isSaving ? "Pilih slot untuk menyimpan" : "Pilih slot untuk memuat game"; 

        for (int i = 0; i < slotButtons.Length; i++)
        {
            if (i == 0) 
            {
                slotButtons[i].interactable = false; // Slot 0 dinonaktifkan
            }
            else
            {
                slotButtons[i].interactable = !isSaving || PlayerPrefs.HasKey(savePrefix + i + "_title");
            }
        }
    }


    void SlotAction(int slot)
    {
        if (slot == 0)
        {
            Debug.Log("Slot 0 digunakan untuk Auto Save dan tidak bisa dipilih.");
            return; // Mencegah eksekusi lebih lanjut
        }

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
        GameplayManager.instance.CloseAllMenus();
        GameplayManager.instance.uiPause.SetActive(true);
        
        string currentDate = DateTime.Now.ToString("dd/MM/yyyy");
        string currentTime = DateTime.Now.ToString("HH:mm");
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerLastPosition[slot] = player.transform.position.x;
        }

        PlayerPrefs.SetInt("SelectedSaveSlot", slot);

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
            Time.timeScale = 1f;
            Debug.Log("Loading Save Slot " + (slot + 1));
            // Simpan informasi slot yang dipilih sebelum memuat scene
            PlayerPrefs.SetInt("SelectedSaveSlot", slot);
            PlayerPrefs.Save();

            SceneManager.LoadScene("GamePlay"); // Pindah ke scene utama
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
