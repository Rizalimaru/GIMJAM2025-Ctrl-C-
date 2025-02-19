using UnityEngine;

public class TongSampahMiniGame : MonoBehaviour
{
    private Vector2 offset;
    private bool isDragging = false;
    private bool isOverTrashBin = false;
    private Rigidbody2D rb; // Rigidbody2D untuk mengatur fisika

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;  // Smooth motion
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;  // Prevent tunneling
    }

    void Update()
    {
        if (Time.timeScale == 0f)
        {
            isDragging = false;
        }
    }

    void OnMouseDown()
    {
        if (Time.timeScale == 0f) return; // Nonaktifkan drag saat game di-pause
        
        offset = (Vector2)transform.position - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (isDragging && Time.timeScale != 0f)
        {
            Vector2 newPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            rb.MovePosition(newPos);
        }
    }

    void OnMouseUp()
    {
        if (Time.timeScale == 0f) return; // Nonaktifkan release saat game di-pause
        
        isDragging = false;
        
        if (isOverTrashBin)
        {
            CollectTrash();
        }

        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("TrashBin"))
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("TrashBin"))
        {
        }
    }

    void CollectTrash()
    {
        // Animasi daun jatuh

        // Hapus objek sampah
    }
}