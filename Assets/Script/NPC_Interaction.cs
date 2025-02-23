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
                dialogueManager.StartDialogue(dialogue);
                isTalking = true;
            }
            else
            {
                dialogueManager.DisplayNextSentence();
            }
        }

        if (dialogueManager.dialogEnd)
        {
            isTalking = false;
            dialogueManager.dialogEnd = false;
            loadPuzzle();
        }
    }

    private void loadPuzzle()
    {
        GameObject target = GameObject.Find("Player");
        SaveSlotSystem.instance.playerLastPosition[0] = target.transform.position.x;
        SaveSlotSystem.instance.AutoSaveSlot0();
        StartCoroutine(SceneController.instance.LoadScene(namaSceneLoad));
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
