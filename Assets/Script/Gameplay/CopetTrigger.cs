using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopetTrigger : MonoBehaviour
{
    public MonologueManager monologueManager;
    public MonologueManager.Monologue monologue;
    public GameObject dialogIbu;
    public GameObject canvasOption;
    private bool objectReady = false;
    private bool hasTriggered = false;
    public GameObject ibuDefault;
    public GameObject ibuYes;
    public GameObject ibuNo;

    void Awake()
    {
        dialogIbu.SetActive(false);
        canvasOption.SetActive(false);
    }

    void Update()
    {
        if (objectReady && !hasTriggered)
        {
            StartCoroutine(StartEvent());
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasTriggered)
        {
            objectReady = true;
        }
    }

    IEnumerator StartEvent()
    {   
        PlayerController player = FindObjectOfType<PlayerController>();
        player.canMove = false;
        hasTriggered = true;
        dialogIbu.SetActive(true);
        yield return new WaitForSeconds(2);
        dialogIbu.SetActive(false);
        while (dialogIbu.activeSelf)
        {
            yield return null;
        }
        monologueManager.StartMonologue(monologue);
        while (monologueManager.dialoguePanel.activeSelf)
        {
            yield return null;
        }
        canvasOption.SetActive(true);
        player.canMove = true;
       
    }

    public void ibuYesClick()
    {
        ibuDefault.SetActive(false);
        ibuYes.SetActive(true);
        ibuNo.SetActive(false);
        canvasOption.SetActive(false);
    }

    public void ibuNoClick()
    {
        ibuDefault.SetActive(false);
        ibuYes.SetActive(false);
        ibuNo.SetActive(true);
        canvasOption.SetActive(false);
    }



}
