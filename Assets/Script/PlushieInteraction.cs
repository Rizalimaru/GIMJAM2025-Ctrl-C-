using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlushieInteraction : MonoBehaviour
{   
    public GameObject bgObject;
    public GameObject plushieObject;
    private bool canInteract = false;
    private bool hasInteracted = false;
    private bool canDisable = false;
    private Vector2 plushiePosition;
    private Coroutine moveCoroutine;

    void Start()
    {   
        plushiePosition = plushieObject.transform.position;
        bgObject.SetActive(false);
        plushieObject.SetActive(false);
    }

    void OnMouseEnter()
    {
        if (!hasInteracted)
        {
            canInteract = true;
        }
    }

    void Update()
    {
        if (canInteract && Input.GetMouseButtonDown(0) && !hasInteracted)
        {
            hasInteracted = true;
            canInteract = false;
            moveCoroutine = StartCoroutine(MovePlushieToCenter());
        }

        if (canDisable && Input.GetMouseButtonDown(0))
        {
            bgObject.SetActive(false);
            plushieObject.SetActive(false);
            canDisable = false;
        }
    }

    private IEnumerator MovePlushieToCenter()
    {   
        bgObject.SetActive(true);
        plushieObject.SetActive(true);
        Vector2 startPosition = plushieObject.transform.position;
        plushieObject.transform.localScale *= 2;
        Vector2 targetPosition = new Vector2(0, 1);
        float duration = 0.7f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            plushieObject.transform.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        plushieObject.transform.position = targetPosition;
        yield return new WaitForSeconds(5f);
        canDisable = true;
        moveCoroutine = null;
    }
}
