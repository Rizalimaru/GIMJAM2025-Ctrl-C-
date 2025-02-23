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
    public TextMeshProUGUI[] slotProgress;   // Menampilkan progress dalam persen

    public TextMeshProUGUI titleText; // Judul di atas slot
    private string savePrefix = "SaveSlot";
    public string triggerKey = "HasTriggered";

    void Start()
    {
        titleText.text = "Pilih slot untuk memuat game"; // Set judul untuk Load Mode
        LoadSaveSlots();
    }

    public void NewGame()
    {
        int slot = 0;

        AudioManager.Instance.PlaySFX("Button", 0);
        AudioManager.Instance.StopBackgroundMusicWithTransition("Mainmenu", 2f);

        PlayerPrefs.SetInt(triggerKey, 0);

        // Simpan data baru ke slot yang dipilih
        PlayerPrefs.SetString(savePrefix + slot + "_title", "Auto Save");
        PlayerPrefs.SetString(savePrefix + slot + "_date", DateTime.Now.ToString("dd/MM/yyyy"));
        PlayerPrefs.SetString(savePrefix + slot + "_time", DateTime.Now.ToString("HH:mm"));
        PlayerPrefs.SetString(savePrefix + slot + "_image", ""); // Jika ada gambar, simpan di sini
        PlayerPrefs.SetInt(savePrefix + slot + "_progress", 0); // Reset progress ke 0%
        PlayerPrefs.SetFloat(savePrefix + slot + "_playerPosition", -91.3f); // Reset posisi karakter

        PlayerPrefs.SetInt("SelectedSaveSlot", slot);
        PlayerPrefs.Save();

        Debug.Log("New Game dimulai di slot " + slot);

        // Set progress di UI
        if (slotProgress.Length > slot)
        {
            slotProgress[slot].text = "0%";
        }

        // Pindah ke scene game
        StartCoroutine(SceneController.instance.LoadScene("CutSceneIntro"));
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

                // Ambil progress yang tersimpan dan tampilkan dalam format "X%"
                int progress = PlayerPrefs.GetInt(savePrefix + i + "_progress", 0);
                slotProgress[i].text = progress + "%";

                slotButtons[i].interactable = true; // Bisa diklik
            }
            else
            {
                slotTitles[i].text = "Empty Slot";
                slotDates[i].text = "";
                slotTimes[i].text = "";
                slotProgress[i].text = ""; // Kosongkan jika tidak ada save

                slotButtons[i].interactable = false; // Tidak bisa diklik
            }

            slotButtons[i].onClick.RemoveAllListeners();
            slotButtons[i].onClick.AddListener(() => LoadGame(slotIndex));
        }
    }

    public void LoadGame(int slot)
    {
        AudioManager.Instance.PlaySFX("Button", 0);
        AudioManager.Instance.StopBackgroundMusicWithTransition("Mainmenu", 2f);

        if (PlayerPrefs.HasKey(savePrefix + slot + "_title"))
        {
            Debug.Log("Loading Save Slot " + (slot + 1));

            // Simpan informasi slot yang dipilih sebelum memuat scene untuk nanti set posisi karakter
            PlayerPrefs.SetInt("SelectedSaveSlot", slot);
            PlayerPrefs.Save();

            StartCoroutine(SceneController.instance.LoadScene("GamePlay"));
        }
    }
}
