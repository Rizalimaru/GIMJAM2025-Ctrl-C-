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
    public bool useLoadPuzzle = true;
    public bool usePlayCutscene = false;
    public List<string> cutsceneScenes;
    public List<int> lineBeforePlayCutscene;

    private bool canTalk = false;
    private bool isTalking = false;
    private static int currentLineIndex = -1; // Menggunakan variabel static
    public bool puzzleActive = false;
    public bool isPuzzleSolved = false;
    private bool canReturnToRoom = false;
    public static int savedLineIndex = -1; // Simpan index terakhir sebelum puzzle


    private string lastInteractedNPC;

    void Start()
    {
        SetDialogImages();
        if(lineBeforePlayCutscene != null)
        {
            usePlayCutscene = true;
        }
        
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
        Debug.Log("Current Line Index: " + currentLineIndex);
        currentLineIndex = DialogueManager.currentLineIndex;
        if (canTalk && Input.GetKeyDown(KeyCode.Space))
        {
            if (puzzleActive) return;
            
            if (!isTalking)
            {
                dialogueManager.StartDialogue(dialogue, namaKiri, namaKanan, leftSprite, rightSprite);
                isTalking = true;
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

    if (isTalking && currentLineIndex == lineBeforeLoadScene && !isPuzzleSolved)
    {
        savedLineIndex = currentLineIndex; // Simpan index terakhir
        isTalking = false;
        dialogueManager.dialogEnd = false;
        
        if (useLoadPuzzle)
            LoadPuzzle();
        else
            canReturnToRoom = true; 
    }


        if (dialogueManager.dialogEnd)
        {
            MarkNPCAsInteracted();
            isTalking = false;
            dialogueManager.dialogEnd = false;
            
            if (canReturnToRoom)
            {
                balekKekamar();
                canReturnToRoom = false;
            }
        }
    }

    private void LoadPuzzle()
    {
        if (!puzzleActive)
        {
            AudioManager.Instance.StopBackgroundMusicWithTransition("Gameplay", 1f);
            AudioManager.Instance.PlayBackgroundMusicWithTransition("Bunga", 0, 1f);

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
        int index = lineBeforePlayCutscene.IndexOf(currentLineIndex);
        if (index != -1 && index < cutsceneScenes.Count)
        {
            string sceneName = cutsceneScenes[index];
            if (!SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            }
        }
        usePlayCutscene = false;
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
        SaveSlotSystem.instance.AutoSaveSlot();

        if (npcname == "ibunoo")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                player.transform.position = new Vector3(-91.3f, player.transform.position.y, player.transform.position.z);
                PlayerPrefs.SetFloat("PlayerX_" + currentSlot, -91.3f);
                PlayerPrefs.SetFloat("PlayerY_" + currentSlot, player.transform.position.y);
                PlayerPrefs.SetFloat("PlayerZ_" + currentSlot, player.transform.position.z);
                PlayerPrefs.Save();
            }
        }

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
