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

    private bool canTalk = false;
    private bool isTalking = false;
    private int currentLineIndex = 0; 
    public bool puzzleActive = false; // Cek apakah puzzle sedang berjalan
    public bool isPuzzleSolved = false; // Cek apakah puzzle sudah selesai

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

        if (isTalking && currentLineIndex == lineBeforeLoadScene && isPuzzleSolved == false)
        {
            isTalking = false;
            dialogueManager.dialogEnd = false;
            LoadPuzzle();
        }

        if (dialogueManager.dialogEnd)
        {
            isTalking = false;
            dialogueManager.dialogEnd = false;
        }
    }

    private void LoadPuzzle()
    {
        if (!puzzleActive ) 
        {
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

    private void tungguActive()
    {
        puzzleActive = false;

    }


    private void MarkNPCAsInteracted()
    {
        int currentSlot = PlayerPrefs.GetInt("SelectedSaveSlot", 0);
        string npcID = gameObject.name; // Gunakan nama GameObject sebagai ID

        SaveSlotSystem.instance.SaveNPCInteraction(currentSlot, npcID);

        // Sembunyikan NPC setelah interaksi (jika ingin NPC menghilang)
        gameObject.SetActive(false);
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
