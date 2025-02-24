using UnityEngine;

public class Sandal : MonoBehaviour
{
    public float baseSpeed = 5f; // Kecepatan dasar
    public float maxSpeed = 30f; // Kecepatan maksimum
    public float rotationSpeed = 500f;
    public float detectRadius = 0.5f; // Radius deteksi untuk musuh
    public LayerMask enemyLayer; // Layer untuk mendeteksi musuh
    private Vector3 moveDirection;
    private float speed; // Kecepatan aktual berdasarkan power

    public Animator animator; // Tambahkan variabel Animator

    public void SetDirection(Vector3 direction, float power)
    {
        moveDirection = direction;
        speed = baseSpeed + (maxSpeed - baseSpeed) * Mathf.Pow(power, 2f); // Scaling power

        // Sesuaikan kecepatan animasi berdasarkan power
    if (animator != null)
    {
        animator.speed = 1f + (power * 10f); // Skala animasi secara natural
    }

    }



    void Update()
    {
        // Gerakan sandal dengan kecepatan sesuai power
        transform.position += moveDirection * speed * Time.deltaTime;

        // Putar sandal
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // Skalakan berdasarkan Z (semakin dekat ke 0, semakin kecil)
        float scaleFactor = Mathf.Clamp(1 - Mathf.Abs(transform.position.z) * 0.5f, 0.2f, 1f);
        transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);

        // Cek musuh di sekitar sandal
        DetectEnemy();
    }

    void DetectEnemy()
    {
        Collider2D enemy = Physics2D.OverlapCircle(transform.position, detectRadius, enemyLayer);
        
        if (enemy != null)
        {
            // Cek apakah posisi sandal berada dalam rentang Z yang valid
            if (transform.position.z >= -1f && transform.position.z <= 1f)
            {
                AudioManager.Instance.PlaySFX("Maling1",1);
                Destroy(enemy.gameObject); // Hapus musuh
                Destroy(gameObject); // Hapus sandal juga
            }
        }
    }

    void OnDrawGizmos()
    {
        // Gambar radius deteksi di editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}
