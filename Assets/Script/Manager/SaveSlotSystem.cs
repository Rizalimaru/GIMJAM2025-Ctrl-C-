using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SaveSlotSystem : MonoBehaviour
{   

    [Header("NPC INTERAKSI BUAT SAVE")]

    public GameObject[] NPCAmount; // Menyimpan daftar NPC dalam slot saat ini

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
        LoadAutoSaveSlot();

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

    void LoadAutoSaveSlot()
    {
        int currentSlot = PlayerPrefs.GetInt("SelectedSaveSlot", 0);

        if (PlayerPrefs.HasKey(savePrefix + currentSlot + "_title")) // Cek apakah ada auto-save di slot aktif
        {
            slotTitles[currentSlot].text = PlayerPrefs.GetString(savePrefix + currentSlot + "_title");
            slotDates[currentSlot].text = PlayerPrefs.GetString(savePrefix + currentSlot + "_date");
            slotTimes[currentSlot].text = PlayerPrefs.GetString(savePrefix + currentSlot + "_time");
            playerLastPosition[currentSlot] = PlayerPrefs.GetFloat(savePrefix + currentSlot + "_playerPosition");

            progress[currentSlot] = PlayerPrefs.GetInt(savePrefix + currentSlot + "_progress");

            slotProgress[currentSlot].text = progress[currentSlot] + "%";

            slotButtons[currentSlot].interactable = true; // Bisa diklik
        }
        else
        {
            slotTitles[currentSlot].text = "No Auto Save";
            slotDates[currentSlot].text = "";
            slotTimes[currentSlot].text = "";
            slotProgress[currentSlot].text =  "0";
            slotButtons[currentSlot].interactable = false; // Tidak bisa di-load
        }
    }




    public void AutoSaveSlot()
    {
        int currentSlot = PlayerPrefs.GetInt("SelectedSaveSlot", 0); // Ambil slot aktif
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("Player tidak ditemukan saat auto-save.");
            return;
        }

        float playerX = player.transform.position.x;

        // Simpan data ke slot yang sedang aktif
        PlayerPrefs.SetString(savePrefix + currentSlot + "_title", "Auto Save Slot " + (currentSlot + 1));
        PlayerPrefs.SetString(savePrefix + currentSlot + "_date", System.DateTime.Now.ToString("dd/MM/yyyy"));
        PlayerPrefs.SetString(savePrefix + currentSlot + "_time", System.DateTime.Now.ToString("HH:mm"));
        PlayerPrefs.SetFloat(savePrefix + currentSlot + "_playerPosition", playerX);
        PlayerPrefs.SetInt(savePrefix + currentSlot + "_progress", progress[currentSlot]);

        PlayerPrefs.SetInt("SelectedSaveSlot", currentSlot);
        PlayerPrefs.Save();

        Debug.Log("Auto Save berhasil di slot " + currentSlot + " dengan posisi X: " + playerX);
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
        PlayerPrefs.SetInt(savePrefix + slot + "_interactedNPCs", interactedNPCs);
        PlayerPrefs.SetFloat(savePrefix + slot + "_playerPosition", playerLastPosition[slot]);

        // Simpan semua NPC yang sudah diinteraksi
        foreach (GameObject npc in NPCAmount)
        {
            if (npc != null)
            {
                string npcID = npc.name; // Gunakan nama sebagai ID
                bool hasInteracted = !npc.activeSelf; // Jika NPC nonaktif, berarti sudah diinteraksi

                if (hasInteracted)
                {
                    SaveNPCInteraction(slot, npcID);
                }
            }
        }


        PlayerPrefs.Save();
        LoadSaveSlots();
    }


    public void LoadGame(int slot)
    {
        if (PlayerPrefs.HasKey(savePrefix + slot + "_title"))
        {
            Time.timeScale = 1f;
            Debug.Log("Loading Save Slot " + (slot + 1));

            PlayerPrefs.SetInt("SelectedSaveSlot", slot);
            PlayerPrefs.Save();

            LoadNPCInteractions(slot); // Muat jumlah NPC yang diinteraksi dari slot

            SceneManager.LoadScene("GamePlay");
        }
    }



    public void LoadNPCInteractions(int slot)
    {
        foreach (GameObject npc in NPCAmount)
        {
            if (npc != null)
            {
                string npcID = npc.name; // Gunakan nama GameObject sebagai ID NPC
                bool hasInteracted = PlayerPrefs.GetInt("NPC_" + slot + "_" + npcID, 0) == 1;

                if (hasInteracted)
                {
                    npc.SetActive(false); // Sembunyikan NPC jika sudah diinteraksi
                }
            }
        }
    }


    public void SaveNPCInteraction(int slot, string npcID)
    {
        PlayerPrefs.SetInt("NPC_" + slot + "_" + npcID, 1);
        PlayerPrefs.Save();
    }




    


}
