using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public Vector2 shadowOffset = new Vector2(0.1f, -0.1f); // Offset bayangan
    public float shadowScale = 1.1f; // Skala bayangan
    public Color shadowColor = Color.white; // Warna bayangan

    private GameObject shadowObject;
    private SpriteRenderer shadowRenderer;

    void Start()
    {
        // Membuat GameObject baru untuk bayangan
        shadowObject = new GameObject(gameObject.name + "_Shadow");
        shadowObject.transform.parent = transform; // Menjadikan anak dari object utama
        shadowObject.transform.localPosition = shadowOffset; // Mengatur posisi relatif
        shadowObject.transform.localScale = new Vector3(shadowScale, shadowScale, 1);
        
        // Menambahkan SpriteRenderer ke bayangan
        shadowRenderer = shadowObject.AddComponent<SpriteRenderer>();
        SpriteRenderer originalRenderer = GetComponent<SpriteRenderer>();
        
        if (originalRenderer != null)
        {
            shadowRenderer.sprite = originalRenderer.sprite;
            shadowRenderer.color = shadowColor;
            shadowRenderer.sortingLayerID = originalRenderer.sortingLayerID;
            shadowRenderer.sortingOrder = originalRenderer.sortingOrder - 1; // Agar berada di bawah objek asli
        }
    }
}
