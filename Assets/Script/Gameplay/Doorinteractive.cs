using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Doorinteractive : MonoBehaviour
{
    public GameObject OutText;
    private bool canGoOut;

    void Awake()
    {
        OutText.SetActive(false);
    }

    void Update()
    {
        if (canGoOut && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(SceneController.instance.LoadScene("GamePlay"));
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {   
            canGoOut = true;
            OutText.SetActive(true);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canGoOut = false;
            OutText.SetActive(false);
        }
    }
}
