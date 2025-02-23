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

    public TextMeshProUGUI[] slotProgress;   // Menampilkan waktu

    public TextMeshProUGUI titleText; // Judul di atas slot

    public Slider progressSlider;


    private int selectedSlot; // Menyimpan slot yang dipilih

    public Sprite defaultImage;   // Gambar default jika belum ada save
    private string savePrefix = "SaveSlot";
    public int[] progress;
    public float[] playerLastPosition;


    public int totalNPCs = 7;  // Total NPC yang bisa diinteraksi
    public int interactedNPCs;   // NPC yang sudah diinteraksi

    
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

        int lastSlot = PlayerPrefs.GetInt("SelectedSaveSlot", 0);
        progressSlider.value = progress[lastSlot];
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
            PlayerPrefs.DeleteKey(savePrefix + i + "_interactedNPCs"); // Reset NPC interaction
        }

        // Hapus auto save
        PlayerPrefs.DeleteKey(savePrefix + "0_title");
        PlayerPrefs.DeleteKey(savePrefix + "0_date");
        PlayerPrefs.DeleteKey(savePrefix + "0_time");
        PlayerPrefs.DeleteKey(savePrefix + "0_progress");
        PlayerPrefs.DeleteKey(savePrefix + "0_playerPosition");
        PlayerPrefs.DeleteKey(savePrefix + "0_image");
        PlayerPrefs.DeleteKey(savePrefix + "0_interactedNPCs"); // Reset NPC interaction untuk auto-save

        PlayerPrefs.Save(); // Simpan perubahan

        Debug.Log("Semua data telah direset. Memulai game baru...");
        
        progress = new int[slotButtons.Length];
        interactedNPCs = 0; // Reset jumlah NPC yang diinteraksi

        // Load scene pertama (misalnya, "GameScene" atau "Level1")
        SceneManager.LoadScene("GamePlay");
    }


    void LoadSaveSlots()
    {
        for (int i = 0; i < slotButtons.Length; i++)
        {
            if (PlayerPrefs.HasKey(savePrefix + i + "_interactedNPCs"))
{
                interactedNPCs = PlayerPrefs.GetInt(savePrefix + i + "_interactedNPCs");
                progress[i] = Mathf.Clamp((interactedNPCs * 100) / totalNPCs, 0, 100);
            }
            int slotIndex = i;
            if (PlayerPrefs.HasKey(savePrefix + i + "_title")) // Cek apakah slot ada isinya
            {
                slotTitles[i].text = PlayerPrefs.GetString(savePrefix + i + "_title");
                slotDates[i].text = PlayerPrefs.GetString(savePrefix + i + "_date");
                slotTimes[i].text = PlayerPrefs.GetString(savePrefix + i + "_time");

                playerLastPosition[i] = PlayerPrefs.GetFloat(savePrefix + i + "_playerPosition");
                progress[i] = PlayerPrefs.GetInt(savePrefix + i + "_progress");


                slotProgress[i].text = progress[i] + "%";

                slotButtons[i].interactable = true; // Bisa diklik
            }
            else
            {
                slotTitles[i].text = "Empty Slot";
                slotDates[i].text = "";
                slotTimes[i].text = "";
                slotProgress[i].text = progress[i] + "0%";

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
            playerLastPosition[0] = PlayerPrefs.GetFloat(savePrefix + "0_playerPosition");

            progress[0] = PlayerPrefs.GetInt(savePrefix + "0_progress");

            slotProgress[0].text = progress[0] + "%";


            slotButtons[0].interactable = true; // Bisa diklik
        }
        else
        {
            slotTitles[0].text = "No Auto Save";
            slotDates[0].text = "";
            slotTimes[0].text = "";
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
        PlayerPrefs.SetInt(savePrefix + "0_progress", progress[0]); // Perbaikan di sini!



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
            if (isSaving)
            {
                // Semua slot bisa dipilih kecuali slot 0 (karena auto-save)
                slotButtons[i].interactable = (i != 0);
            }
            else
            {
                // Saat load mode, hanya slot yang sudah ada datanya yang bisa dipilih
                slotButtons[i].interactable = PlayerPrefs.HasKey(savePrefix + i + "_title");
            }
        }
    }




    void SlotAction(int slot)
    {

        if (isSaving)
        {
            selectedSlot = slot; // Simpan slot yang dipilih
            GameplayManager.instance.WarningSave();
            Debug.Log("Saving brpid");
        }
        else
        {
            LoadGame(slot);
        }
    }
    public void ConfirmSave()
    {
        SaveGame(selectedSlot); // Simpan game di slot yang dipilih
        GameplayManager.instance.CloseWarningSave();
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

        // Hitung progress berdasarkan NPC yang sudah diinteraksi
        progress[slot] = Mathf.Clamp((interactedNPCs * 100) / totalNPCs, 0, 100);

        PlayerPrefs.SetInt("SelectedSaveSlot", slot);

        PlayerPrefs.SetString(savePrefix + slot + "_title", "Save Slot " + (slot + 1));
        PlayerPrefs.SetString(savePrefix + slot + "_date", currentDate);
        PlayerPrefs.SetString(savePrefix + slot + "_time", currentTime);
        PlayerPrefs.SetInt(savePrefix + slot + "_progress", progress[slot]);
        PlayerPrefs.SetInt(savePrefix + slot + "_interactedNPCs", interactedNPCs); // Simpan NPC yang sudah diinteraksi
        PlayerPrefs.SetFloat(savePrefix + slot + "_playerPosition", playerLastPosition[slot]);

        PlayerPrefs.Save();
        LoadSaveSlots();

        progressSlider.value = progress[slot];
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

            progressSlider.value = progress[slot];

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
