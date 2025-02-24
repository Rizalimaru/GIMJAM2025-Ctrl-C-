using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch : MonoBehaviour
{
    public static Touch instance;
    public bool touching;
    public GameObject player;
    public GameObject grandma;

    void Awake()
    {
        instance = this;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Cek jika yang bersentuhan adalah objek dengan nama "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            touching = true;  // Menandakan player sedang bersentuhan
            StartCoroutine(perubahanPosisi());  // Mulai coroutine untuk mengubah posisi
        }
    }

    IEnumerator perubahanPosisi()
    {
        yield return new WaitForSeconds(8f);  // Tunggu selama 6 detik

        // Ubah posisi player dan grandma
        player.transform.position = new Vector2(-13.9177f, player.transform.position.y);
        grandma.transform.position = new Vector2(-12.68f, grandma.transform.position.y);
    }
}
