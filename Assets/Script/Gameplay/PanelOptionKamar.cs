using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelOptionKamar : MonoBehaviour
{
    public GameObject panelOptionKamar;
    private bool canInteract = false;
    public Button yesButton;
    public Button noButton;
    public TextMeshProUGUI questionText;
    public string textQuestion;
    public GameObject darkImage;
    public TextMeshProUGUI TextEnd;
    public string textEnd;

    void Start()
    {   
        panelOptionKamar.SetActive(false);
        darkImage.SetActive(false);
        TextEnd.gameObject.SetActive(false);
        // Menghubungkan tombol dengan metode pembungkus
        yesButton.onClick.AddListener(ReplySceneWrapper);
        noButton.onClick.AddListener(HidePanel);
    }

    void OnMouseEnter()
    {
        canInteract = true;
    }

    void OnMouseExit()
    {
        canInteract = false;
    }

    void Update()
    {
        if (canInteract && Input.GetMouseButtonDown(0))
        {   
            questionText.text = textQuestion;
            TextEnd.text = textEnd;
            panelOptionKamar.SetActive(true);
        }
    }

    // Fungsi pembungkus untuk memulai Coroutine
    void ReplySceneWrapper()
    {
        StartCoroutine(ReplyScene());
    }

    public IEnumerator ReplyScene()
    {   
        darkImage.SetActive(true);
        panelOptionKamar.SetActive(false);
        yield return new WaitForSeconds(1f);
        TextEnd.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        TextEnd.gameObject.SetActive(true);
        StartCoroutine(SceneController.instance.LoadScene("Kamar"));
    }

    public void HidePanel()
    {
        panelOptionKamar.SetActive(false);
    }
}
