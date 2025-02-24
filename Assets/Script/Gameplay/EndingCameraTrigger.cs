using System.Collections;
using UnityEngine;

public class EndingCameraTrigger : MonoBehaviour
{   
    
    public MonologueManager monologueManager;
    public MonologueManager.Monologue monologue;
    public GameObject mainCamera;
    public GameObject cutSceneCamera;
    public Transform player;
    public Transform target;
    public bool isCutSceneTriggered = false;
    public float transitionSpeed = 2f;

    private Coroutine transitionCoroutine;

    void Start()
    {
        cutSceneCamera.SetActive(false);
    }

    void Update()
    {   
        if (isCutSceneTriggered)
        {   
            // Smoothly move cutscene camera to target position
            cutSceneCamera.transform.position = Vector3.Lerp(
                cutSceneCamera.transform.position,
                new Vector3(target.position.x, cutSceneCamera.transform.position.y, cutSceneCamera.transform.position.z),
                Time.deltaTime * transitionSpeed
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCutSceneTriggered)
        {
            isCutSceneTriggered = true;
            if (transitionCoroutine != null)
                StopCoroutine(transitionCoroutine);

            transitionCoroutine = StartCoroutine(SwitchToCutscene());
        }
    }

    public void buttonTesting()
    {
            isCutSceneTriggered = true;
            if (transitionCoroutine != null)
                StopCoroutine(transitionCoroutine);

            transitionCoroutine = StartCoroutine(SwitchToCutscene());
    }

    public void EndCutscene()
    {
        isCutSceneTriggered = false;
        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);

        transitionCoroutine = StartCoroutine(SwitchToMainCamera());
    }

    IEnumerator SwitchToCutscene()
    {
        float elapsedTime = 0f;
        Vector3 startPos = mainCamera.transform.position;
        Vector3 targetPos = cutSceneCamera.transform.position;

        cutSceneCamera.SetActive(true);

        while (elapsedTime < 1f)
        {
            mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime);
            elapsedTime += Time.deltaTime * transitionSpeed;
            yield return null;
        }
        mainCamera.SetActive(false);
        yield return new WaitForSeconds(2);
        monologueManager.StartMonologue(monologue);
        while (monologueManager.dialoguePanel.activeSelf)
        {
            yield return null;
        }
        EndCutscene();
    }

    IEnumerator SwitchToMainCamera()
    {
        float elapsedTime = 0f;
        Vector3 startPos = cutSceneCamera.transform.position;
        Vector3 targetPos = mainCamera.transform.position;

        mainCamera.SetActive(true);

        while (elapsedTime < 1f)
        {
            cutSceneCamera.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime);
            elapsedTime += Time.deltaTime * transitionSpeed;
            yield return null;
        }

        cutSceneCamera.SetActive(false);
    }
}
