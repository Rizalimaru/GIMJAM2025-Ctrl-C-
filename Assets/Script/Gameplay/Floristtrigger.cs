using System.Collections;
using UnityEngine;

public class FloristTrigger : MonoBehaviour
{
    public MonologueManager monologueManager;
    public MonologueManager.Monologue monologue;
    public GameObject floristBubble;

    private bool objectReady = false;
    private bool isMonologueActive = false;
    private bool eventStarted = false;
    private bool hasTriggered = false; // Menandai apakah event sudah terjadi

    private void Awake()
    {
        floristBubble.SetActive(false);
    }

    private void Update()
    {
        if (objectReady && !eventStarted && !isMonologueActive && !hasTriggered)
        {
            eventStarted = true;
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
        isMonologueActive = true;
        hasTriggered = true; // Menandai bahwa event sudah terjadi

        monologueManager.StartMonologue(monologue);

        // Tunggu sampai monolog selesai (panel monolog ditutup)
        while (monologueManager.dialoguePanel.activeSelf)
        {
            yield return null;
        }

        // Aktifkan bubble setelah monolog selesai
        floristBubble.SetActive(true);
        yield return new WaitForSeconds(2);
        floristBubble.SetActive(false);

        // Reset status event
        isMonologueActive = false;
        eventStarted = false;
    }
}
