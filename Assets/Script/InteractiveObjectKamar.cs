using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObjectKamar : MonoBehaviour
{   
    private SpriteRenderer originalSpriteRenderer;
    private GameObject outlineObject;
    private SpriteRenderer outlineRenderer;
    public float outlineScaleX;
    public float outlineScaleY;
    public bool objectReady = false;


    void Start()
    {   
        originalSpriteRenderer = GetComponent<SpriteRenderer>();
        if (originalSpriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer tidak ditemukan pada objek ini!");
            return;
        }

        // Membuat objek duplikat untuk outline
        outlineObject = new GameObject("Outline");
        outlineObject.transform.SetParent(transform);
        outlineObject.transform.localPosition = Vector3.zero;
        outlineObject.transform.localScale = new Vector3(outlineScaleX, outlineScaleY,1f );

        outlineRenderer = outlineObject.AddComponent<SpriteRenderer>();
        outlineRenderer.sprite = originalSpriteRenderer.sprite;
        outlineRenderer.color = new Color(1f, 1f, 0f); // Yellow color
        outlineRenderer.sortingLayerID = originalSpriteRenderer.sortingLayerID;
        outlineRenderer.sortingOrder = originalSpriteRenderer.sortingOrder - 1; // Agar berada di bawah sprite utama

        outlineObject.SetActive(false); // Mulai dalam keadaan nonaktif
    }

    void OnMouseEnter()
    {
        if (outlineObject != null)
            objectReady = true;
            outlineObject.SetActive(true);
    }

    void OnMouseExit()
    {
        if (outlineObject != null)
            objectReady = false;
            outlineObject.SetActive(false);
    }

}
