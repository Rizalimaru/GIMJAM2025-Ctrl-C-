using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class changeSceneInteraction : MonoBehaviour
{   
    public string nextSceneName;
    private bool canInteract = false;
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
            StartCoroutine(SceneController.instance.LoadScene(nextSceneName));
        }
    }
}
