using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPC_Interaction : MonoBehaviour
{

    private Collider2D coliider;
    public GameObject question_mark;
    public DialogueManager dialogueManager;
    public DialogueManager.Dialogue dialogue;
    public string namaSceneLoad;
    private bool canTalk = false;
    private bool isTalking = false;

    private void Awake()
    {
        question_mark.SetActive(false);
        coliider = GetComponent<Collider2D>();
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
            if (!isTalking) // Jika belum berbicara, mulai dialog
            {
                dialogueManager.StartDialogue(dialogue);
                isTalking = true; 
            }
            else // Jika dialog sudah berjalan, lanjut ke kalimat berikutnya
            {
                dialogueManager.DisplayNextSentence();
            }
        }

        if(dialogueManager.dialogEnd)
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


}
