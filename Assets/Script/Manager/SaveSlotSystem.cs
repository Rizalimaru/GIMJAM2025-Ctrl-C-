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
    public string savePrefix = "SaveSlot";
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

    void Update()
    {
        if (progressSlider == null || !progressSlider.gameObject.activeInHierarchy)
        {
            return; // Jangan lakukan apa pun jika progressSlider null atau nonaktif
        }

        int lastSlot = PlayerPrefs.GetInt("SelectedSaveSlot", 0);

        if (progressSlider.value != progress[lastSlot]) 
        {
            progressSlider.value = progress[lastSlot];
        }
    }

    void Start()
    {
        int currentSlot = PlayerPrefs.GetInt("SelectedSaveSlot", 0);

        LoadNPCInteractions(currentSlot);
        LoadSaveSlots();
        //LoadAutoSaveSlot();

        // Atur tombol utama
        saveButton.onClick.AddListener(() => SetMode(true));  // Aktifkan mode Save
        loadButton.onClick.AddListener(() => SetMode(false)); // Aktifkan mode Load

        int lastSlot = PlayerPrefs.GetInt("SelectedSaveSlot", 0);

        // Mengambil nilai progress yang sudah disimpan, tanpa penambahan apapun
        int loadedProgress = PlayerPrefs.GetInt(savePrefix + lastSlot + "_progress", 0); // Pastikan memuat nilai progress yang benar
        progressSlider.value = loadedProgress; // Tetapkan progres yang benar tanpa modifikasi
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


    public void LoadSaveSlots()
    {
        for (int i = 0; i < slotButtons.Length; i++)
        {
            int slotIndex = i;
            int interactedNPCsCount = PlayerPrefs.GetInt(savePrefix + i + "_interactedNPCs", 0);
            int slotProgressValue = PlayerPrefs.GetInt(savePrefix + i + "_progress", 0);


    
        for (int j = 0; j < slotProgress.Length; j++)
        {
            if (PlayerPrefs.HasKey(savePrefix + i + "_progress"))
            {
                int savedProgress = PlayerPrefs.GetInt(savePrefix + i + "_progress", 0);
                slotProgress[i].text = savedProgress + "%"; // 🔹 Update UI dengan progress terbaru
            }
            else
            {
                slotProgress[i].text = "0%"; // Jika slot kosong, tampilkan 0%
            }
        }



            if (PlayerPrefs.HasKey(savePrefix + i + "_title"))
            {
                slotTitles[i].text = PlayerPrefs.GetString(savePrefix + i + "_title");
                slotDates[i].text = PlayerPrefs.GetString(savePrefix + i + "_date");
                slotTimes[i].text = PlayerPrefs.GetString(savePrefix + i + "_time");

                playerLastPosition[i] = PlayerPrefs.GetFloat(savePrefix + i + "_playerPosition");
                progress[i] = slotProgressValue;

                // Update UI progress
                slotProgress[i].text = progress[i] + "%";  // Tampilkan progress di UI

                slotButtons[i].interactable = true;
            }
            else
            {
                slotTitles[i].text = "Empty Slot";
                slotDates[i].text = "";
                slotTimes[i].text = "";
                slotProgress[i].text = "0%";

                slotButtons[i].interactable = false;
            }

            // Hapus listener sebelumnya biar tidak dobel
            slotButtons[i].onClick.RemoveAllListeners();
            slotButtons[i].onClick.AddListener(() => SlotAction(slotIndex));
        }
    }



    public void LoadAutoSaveSlot()
    {
        int currentSlot = PlayerPrefs.GetInt("SelectedSaveSlot", 0);

        if (PlayerPrefs.HasKey(savePrefix + currentSlot + "_title")) // Cek apakah ada auto-save di slot aktif
        {
            slotTitles[currentSlot].text = PlayerPrefs.GetString(savePrefix + currentSlot + "_title");
            slotDates[currentSlot].text = PlayerPrefs.GetString(savePrefix + currentSlot + "_date");
            slotTimes[currentSlot].text = PlayerPrefs.GetString(savePrefix + currentSlot + "_time");
            playerLastPosition[currentSlot] = PlayerPrefs.GetFloat(savePrefix + currentSlot + "_playerPosition");

            progress[currentSlot] = PlayerPrefs.GetInt(savePrefix + currentSlot + "_progress");

            // Hitung persentase interaksi NPC yang sudah dilakukan
            int npcProgress = (int)((float)interactedNPCs / totalNPCs * 100f);  // Menghitung persentase interaksi NPC dengan pembagian float

            // Update progress berdasarkan jumlah NPC yang sudah diinteraksi
            progress[currentSlot] += Mathf.Clamp(npcProgress, 0, 100);  // Pastikan nilai progress tidak lebih dari 100

            slotProgress[currentSlot].text = progress[currentSlot] + "%";  // Update progress UI

            slotButtons[currentSlot].interactable = true; // Bisa diklik
        }
        else
        {
            slotTitles[currentSlot].text = "No Auto Save";
            slotDates[currentSlot].text = "";
            slotTimes[currentSlot].text = "";
            slotProgress[currentSlot].text = "0%";
            slotButtons[currentSlot].interactable = false; // Tidak bisa di-load
        }
    }



    public void AutoSaveSlot()
    {
        int currentSlot = PlayerPrefs.GetInt("SelectedSaveSlot", 0); // Ambil slot aktif
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("⚠ Player tidak ditemukan saat auto-save.");
            return;
        }

        float playerX = player.transform.position.x;

        // 🔹 Simpan informasi dasar slot
        PlayerPrefs.SetString(savePrefix + currentSlot + "_title", "Auto Save Slot " + (currentSlot + 1));
        PlayerPrefs.SetString(savePrefix + currentSlot + "_date", System.DateTime.Now.ToString("dd/MM/yyyy"));
        PlayerPrefs.SetString(savePrefix + currentSlot + "_time", System.DateTime.Now.ToString("HH:mm"));
        PlayerPrefs.SetFloat(savePrefix + currentSlot + "_playerPosition", playerX);

    

        // 🔹 Simpan progress yang sudah dihitung sebelumnya
        PlayerPrefs.SetInt(savePrefix + currentSlot + "_progress", progress[currentSlot]);

        // 🔹 Simpan slot yang aktif
        PlayerPrefs.SetInt("SelectedSaveSlot", currentSlot);
        PlayerPrefs.Save();

        // 🔹 Update tampilan UI (progress slider)
        progressSlider.value = progress[currentSlot];

        Debug.Log("✅ Auto Save berhasil di slot " + currentSlot + " dengan posisi X: " + playerX + " dan Progress: " + progress[currentSlot] + "%");
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
    Debug.Log("🟢 Menyimpan ke Slot " + slot + " | Progress Sebelum Save: " + progress[slot] + "%");

    string currentDate = DateTime.Now.ToString("dd/MM/yyyy");
    string currentTime = DateTime.Now.ToString("HH:mm");
    GameObject player = GameObject.FindGameObjectWithTag("Player");

    if (player != null)
    {
        playerLastPosition[slot] = player.transform.position.x;
    }

    // 🔹 Hitung jumlah NPC yang sudah diinteraksi
    int interactedNPCsCount = 0;
    foreach (GameObject npc in NPCAmount)
    {
        if (npc != null)
        {
            string npcID = npc.name.ToLower();
            bool hasInteracted = PlayerPrefs.GetInt("NPC_" + slot + "_" + npcID, 0) == 1;

            if (hasInteracted)
            {
                interactedNPCsCount++;
            }
        }
    }

    // 🔹 Hitung progress baru (15% per NPC yang diinteraksi)
    int newProgress = Mathf.Clamp(interactedNPCsCount * 15, 0, 100);
    progress[slot] = newProgress;  // 🔹 Simpan nilai terbaru ke array progress

    Debug.Log("🟢 Progress Baru yang Disimpan di Slot " + slot + ": " + newProgress + "%");

    // 🔹 Simpan progress terbaru ke PlayerPrefs
    PlayerPrefs.SetInt(savePrefix + slot + "_progress", newProgress);
    PlayerPrefs.SetInt(savePrefix + slot + "_interactedNPCs", interactedNPCsCount);
    PlayerPrefs.SetFloat(savePrefix + slot + "_playerPosition", playerLastPosition[slot]);

    // 🔹 Simpan metadata slot
    PlayerPrefs.SetString(savePrefix + slot + "_title", "Save Slot " + (slot + 1));
    PlayerPrefs.SetString(savePrefix + slot + "_date", currentDate);
    PlayerPrefs.SetString(savePrefix + slot + "_time", currentTime);

    PlayerPrefs.Save();

    // 🔹 Perbarui UI slot agar mencerminkan nilai terbaru
    LoadSaveSlots();  

    Debug.Log("✅ Game Berhasil Disimpan di Slot " + slot + " | Progress: " + newProgress + "%");
}













    public void LoadGame(int slot)
    {
        if (!PlayerPrefs.HasKey(savePrefix + slot + "_title"))
        {
            Debug.LogWarning("⚠ Slot kosong, tidak bisa load!");
            return;
        }

        // 🔹 Pastikan hanya mengubah progress jika benar-benar load game
        PlayerPrefs.SetInt("SelectedSaveSlot", slot);
        PlayerPrefs.Save();

        // 🔹 Ambil progress dari PlayerPrefs
        int loadedProgress = PlayerPrefs.GetInt(savePrefix + slot + "_progress", 0);

        progress[slot] = loadedProgress;
        progressSlider.value = loadedProgress;

        LoadNPCInteractions(slot);
        LoadSaveSlots();

        Time.timeScale = 1f;
        SceneManager.LoadScene("GamePlay");
    }







    public void LoadNPCInteractions(int slot)
    {
        int interactedCount = 0; // 🔹 Buat penghitung NPC yang sudah diinteraksi

        foreach (GameObject npc in NPCAmount)
        {
            if (npc != null)
            {
                string npcID = npc.name.ToLower(); // Gunakan nama GameObject sebagai ID NPC
                bool hasInteracted = PlayerPrefs.GetInt("NPC_" + slot + "_" + npcID, 0) == 1;

                if (hasInteracted)
                {
                    npc.GetComponent<Collider2D>().enabled = false;
                    Debug.Log("Collider NPC " + npcID + " dinonaktifkan.");
                    interactedCount++; // 🔹 Tambahkan jumlah NPC yang sudah diinteraksi
                }
            }
        }

        interactedNPCs = interactedCount; // 🔹 Update jumlah NPC yang sudah diinteraksi
        PlayerPrefs.SetInt(savePrefix + slot + "_interactedNPCs", interactedNPCs); // 🔹 Simpan ke PlayerPrefs
        PlayerPrefs.Save();
    }

    public void AutoSaveSlotIbuIbu()
    {
        int currentSlot = PlayerPrefs.GetInt("SelectedSaveSlot", 0);
        
        // 🔹 Simpan informasi dasar slot
        PlayerPrefs.SetString(savePrefix + currentSlot + "_title", "Auto Save Slot " + (currentSlot + 1));
        PlayerPrefs.SetString(savePrefix + currentSlot + "_date", System.DateTime.Now.ToString("dd/MM/yyyy"));
        PlayerPrefs.SetString(savePrefix + currentSlot + "_time", System.DateTime.Now.ToString("HH:mm"));

        PlayerPrefs.SetInt(savePrefix + currentSlot + "_progress", progress[currentSlot]);

        // 🔹 Update tampilan UI (progress slider)
        progressSlider.value = progress[currentSlot];
        // 🔹 Simpan slot yang aktif
        PlayerPrefs.SetInt("SelectedSaveSlot", currentSlot);
        PlayerPrefs.Save();

        Debug.Log("✅ Auto Save berhasil di slot " + currentSlot + " dengan Progress: " + progress[currentSlot] + "%");
    }



    public void SaveNPCInteraction(int slot, string npcID)
    {
        string key = "NPC_" + slot + "_" + npcID;
        
        // 🔹 Cek apakah sudah tersimpan sebelumnya
        if (PlayerPrefs.GetInt(key, 0) == 1)
        {
            Debug.Log("NPC " + npcID + " sudah disimpan sebelumnya.");
            return;
        }

        // 🔹 Tambahkan atau kurangi progress berdasarkan NPC
        if (npcID == "npc_ibunoo")
        {
            Debug.Log("🔻 Progress berkurang karena interaksi dengan NPC Ibunoo.");
            ModifyProgress(slot, -15); // Kurangi progress jika bertemu Ibunoo
        }
        else
        {
            Debug.Log("🟢 Progress bertambah karena interaksi dengan NPC " + npcID);
            ModifyProgress(slot, 20); // Tambah progress untuk NPC lain
        }

        PlayerPrefs.SetInt(key, 1); // Simpan bahwa NPC ini sudah diinteraksi
        PlayerPrefs.Save();

        Debug.Log("✅ NPC " + npcID + " disimpan di slot " + slot);
    }




    public void ModifyProgress(int slot, int amount)
    {
        // Ambil progress saat ini dari PlayerPrefs
        int currentProgress = PlayerPrefs.GetInt(savePrefix + slot + "_progress", 0);

        // Tambahkan nilai amount ke currentProgress
        int newProgress = currentProgress + amount;

        // Batasi progress agar tidak melebihi 100
        newProgress = Mathf.Clamp(newProgress, 0, 100);

        // 🔹 Pastikan array progress[] diperbarui agar tidak hilang saat SaveGame()
        progress[slot] = newProgress;

        // Simpan progress yang baru ke PlayerPrefs
        PlayerPrefs.SetInt(savePrefix + slot + "_progress", newProgress);
        PlayerPrefs.Save();

        // Perbarui tampilan UI
        LoadSaveSlots();

        Debug.Log("Progress di slot " + slot + " berubah menjadi " + newProgress + "%");
    }



    public void NyimpanProgress()
    {
        int lastSlot = PlayerPrefs.GetInt("SelectedSaveSlot", 0);
        // Mengambil nilai progress yang sudah disimpan, tanpa penambahan apapun
        int loadedProgress = PlayerPrefs.GetInt(savePrefix + lastSlot + "_progress", 0); // Pastikan memuat nilai progress yang benar
        progressSlider.value = loadedProgress; // Tetapkan progres yang benar tanpa modifikas
    }



        







    


}
