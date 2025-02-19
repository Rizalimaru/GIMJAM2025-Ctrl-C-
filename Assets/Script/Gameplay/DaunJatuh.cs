using UnityEngine;

public class DaunJatuh : MonoBehaviour
{
    private Rigidbody2D rb;
    private float swayAmount = 0.5f;
    private float swaySpeed = 2f;
    private float startX;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Pastikan Rigidbody2D menggunakan pengaturan yang benar
        rb.gravityScale = 1f; // Biar jatuh alami
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        startX = transform.position.x;

        // Tambahkan sedikit gaya awal supaya jatuh dengan efek goyangan
        rb.velocity = new Vector2(Random.Range(-0.5f, 0.5f), -2f);
    }

    void FixedUpdate()
    {
        // Efek goyangan (sway) menggunakan gaya horizontal
        float sway = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        rb.velocity = new Vector2(sway, rb.velocity.y);
    }
}
