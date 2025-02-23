using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NPC_Interaction : MonoBehaviour
{
    private Collider2D coliider;
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
    private bool canTalk = false;
    private bool isTalking = false;
    private int currentLineIndex = 0; // Menyimpan baris dialog saat ini


    void Start()
    {
        dialogueManager.namaKiri = namaKiri;
        dialogueManager.namaKanan = namaKanan;
        dialogueManager.lineBeforeLoadScene = lineBeforeLoadScene;
        dialogueManager.nextSceneName = namaSceneLoad;

        SetDialogImages();
    }
    private void Awake()
    {
        question_mark.SetActive(false);
        coliider = GetComponent<Collider2D>();

        
        dialogueManager.namaKiri = namaKiri;
        dialogueManager.namaKanan = namaKanan;
        dialogueManager.lineBeforeLoadScene = lineBeforeLoadScene;
        dialogueManager.nextSceneName = namaSceneLoad;

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
            if (!isTalking)
            {
                dialogueManager.StartDialogue(dialogue, namaKiri, namaKanan, leftSprite, rightSprite);
                isTalking = true;
                currentLineIndex = 0; // Reset indeks saat mulai dialog
            }
            else
            {
                dialogueManager.DisplayNextSentence();
                currentLineIndex++; // Naikkan indeks setiap kali pemain lanjut dialog
            }
        }

        // Jika dialog mencapai lineBeforeLoadScene, ganti scene
        if (isTalking && currentLineIndex == lineBeforeLoadScene)
        {
            isTalking = false;
            dialogueManager.dialogEnd = false;
            LoadPuzzle();
        }

        // Jika dialog selesai tanpa pergantian scene, reset
        if (dialogueManager.dialogEnd)
        {
            isTalking = false;
            dialogueManager.dialogEnd = false;
        }
    }

private void LoadPuzzle()
{
    GameObject target = GameObject.Find("Player");

    // Pastikan Player ditemukan
    if (target == null)
    {
        Debug.LogWarning("Player tidak ditemukan saat LoadPuzzle.");
        return;
    }

    // Simpan posisi terakhir pemain
    int currentSlot = PlayerPrefs.GetInt("SelectedSaveSlot", 0);
    SaveSlotSystem.instance.playerLastPosition[currentSlot] = target.transform.position.x;

    // Simpan NPC ID berdasarkan puzzle yang dimuat
    if (namaSceneLoad == "TapTapBerhadiah")
    {
        SaveSlotSystem.instance.SaveNPCInteraction(currentSlot, "NPC_TapTapBerhadiah");
        Debug.Log("BOCIL CUYYYYYYYYYYYYYY");
    }
    else if (namaSceneLoad == "Bunga")
    {
        SaveSlotSystem.instance.SaveNPCInteraction(currentSlot, "NPC_Bunga");
        Debug.Log("BUNGAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA ");
    }
    else if (namaSceneLoad == "TongSampah")
    {
        SaveSlotSystem.instance.SaveNPCInteraction(currentSlot, "NPC_TongSampah");
        Debug.Log("TONGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG ");
    }
    SaveSlotSystem.instance.AutoSaveSlot();

    // Pindah ke scene yang dimaksud
    SceneManager.LoadScene(namaSceneLoad);
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
