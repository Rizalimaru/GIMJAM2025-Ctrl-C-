using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NPC_Interaction : MonoBehaviour
{
    private Collider2D colliderNPC;
    public GameObject question_mark;
    public GameObject LeftDialogImage;
    public GameObject RightDialogImage;
    public Sprite leftSprite;
    public Sprite rightSprite;
    public DialogueManager dialogueManager;
    public DialogueManager.Dialogue dialogue;

    public string namaSceneLoad;
    public string namaKiri;
    public string namaKanan;
    public int lineBeforeLoadScene;
    public bool useLoadPuzzle = true; // Opsi di Inspector
    public bool usePlayCutscene = false; // Opsi tambahan untuk Play Cutscene
    public List<string> cutsceneScenes; // List cutscene yang akan dimainkan
    public List<int> lineBeforePlayCutscene; // Line sebelum cutscene dimainkan

    private bool canTalk = false;
    private bool isTalking = false;
    private int currentLineIndex = 0; 
    public bool puzzleActive = false; // Cek apakah puzzle sedang berjalan
    public bool isPuzzleSolved = false; // Cek apakah puzzle sudah selesai
    private bool canReturnToRoom = false; // Tambahan untuk cek apakah bisa kembali ke kamar

    private string lastInteractedNPC; 
    void Start()
    {
        SetDialogImages();
    }

    private void Awake()
    {
        question_mark.SetActive(false);
        colliderNPC = GetComponent<Collider2D>();
        SetDialogImages();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canTalk = true;
            question_mark.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canTalk = false;
            question_mark.SetActive(false);
        }
    }

    private void Update()
    {
        if (canTalk && Input.GetKeyDown(KeyCode.Space))
        {
            if (puzzleActive) return; // Blokir dialog jika puzzle masih aktif

            if (!isTalking )
            {
                dialogueManager.StartDialogue(dialogue, namaKiri, namaKanan, leftSprite, rightSprite);
                isTalking = true;
                currentLineIndex = 0; 
            }
            else
            {
                dialogueManager.DisplayNextSentence();
                currentLineIndex++; 
            }
        }

        if (usePlayCutscene && lineBeforePlayCutscene.Contains(currentLineIndex))
        {
            PlayCutscene();
        }

        if (isTalking && currentLineIndex == lineBeforeLoadScene && isPuzzleSolved == false)
        {
            isTalking = false;
            dialogueManager.dialogEnd = false;
            
            if (useLoadPuzzle)
                LoadPuzzle();
            else
                canReturnToRoom = true; // Set flag untuk kembali ke kamar nanti
        }

        if (dialogueManager.dialogEnd)
        {
            MarkNPCAsInteracted();
            isTalking = false;
            dialogueManager.dialogEnd = false;
            
            if (canReturnToRoom) // Hanya kembali ke kamar setelah dialog selesai
            {
                balekKekamar();
                canReturnToRoom = false;
            }
        }
    }

    private void LoadPuzzle()
    {
        if (!puzzleActive ) 
        {
            AudioManager.Instance.StopBackgroundMusicWithTransition("Gameplay", 1f);

            AudioManager.Instance.PlayBackgroundMusicWithTransition("Bunga",0,1f);


            SceneManager.LoadScene(namaSceneLoad, LoadSceneMode.Additive);
            Scene scene = SceneManager.GetSceneByName("Gameplay");
            foreach (GameObject obj in scene.GetRootGameObjects())
            {
                obj.SetActive(false);
            }
            isPuzzleSolved = true;
            Invoke("tungguActive", 2f);
        }
    }

    private void PlayCutscene()
    {
        foreach (string sceneName in cutsceneScenes)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
    }

    private void balekKekamar()
    {
        StartCoroutine(SceneController.instance.LoadScene("Kamar"));
    }

    private void tungguActive()
    {
        puzzleActive = false;
    }

    private void MarkNPCAsInteracted()
    {
        int currentSlot = PlayerPrefs.GetInt("SelectedSaveSlot", 0);
        string npcname = gameObject.name.ToLower();
        SaveSlotSystem.instance.SaveNPCInteraction(currentSlot, npcname);

        
        
        SaveSlotSystem.instance.LoadNPCInteractions(currentSlot);
        SaveSlotSystem.instance.NyimpanProgress();
        
        
        Invoke(nameof(DelayedLoadNPC), 0.2f);
    }

    private void DelayedLoadNPC()
    {
        string npcname = gameObject.name.ToLower();
        int currentSlot = PlayerPrefs.GetInt("SelectedSaveSlot", 0);

        // 🔹 Auto Save terlebih dahulu sebelum mengubah posisi pemain
        SaveSlotSystem.instance.AutoSaveSlot();

        // 🔹 Cek apakah interaksi terakhir adalah dengan NPC "ibunoo"
        if (npcname == "ibunoo")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                // 🔹 Ubah posisi pemain ke -91.3 pada sumbu X
                player.transform.position = new Vector3(-91.3f, player.transform.position.y, player.transform.position.z);
                Debug.Log("📍 Pemain dipindahkan ke -91.3 karena interaksi dengan NPC Ibunoo.");
                
                // 🔹 Simpan posisi pemain yang sudah diperbarui
                PlayerPrefs.SetFloat("PlayerX_" + currentSlot, -91.3f);
                PlayerPrefs.SetFloat("PlayerY_" + currentSlot, player.transform.position.y);
                PlayerPrefs.SetFloat("PlayerZ_" + currentSlot, player.transform.position.z);
                PlayerPrefs.Save();
            }
            else
            {
                Debug.LogWarning("⚠ Pemain tidak ditemukan, tidak bisa dipindahkan.");
            }
        }

        // 🔹 Pastikan semua perubahan disimpan
        PlayerPrefs.Save();
    }

    private void SetDialogImages()
    {
        if (LeftDialogImage != null && leftSprite != null)
        {
            LeftDialogImage.GetComponent<UnityEngine.UI.Image>().sprite = leftSprite;
        }

        if (RightDialogImage != null && rightSprite != null)
        {
            RightDialogImage.GetComponent<UnityEngine.UI.Image>().sprite = rightSprite;
        }
    }
}
