using UnityEngine;

public class TongSampahMiniGame : MonoBehaviour
{
    private Vector2 offset;
    private bool isDragging = false;
    private bool isOverTrashBin = false;
    private Rigidbody2D rb; // Rigidbody2D untuk mengatur fisika
    private bool hasPlayedSound = false; // Tambahkan flag

    

    [SerializeField] private GameObject audioPrefab; // Prefab yang berisi AudioSource
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
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
        if (!hasPlayedSound) // Cek apakah suara sudah dimainkan
        {
            AudioManager.Instance.PlaySFX("Sampah", 0);
            hasPlayedSound = true; // Set flag agar tidak dimainkan berulang
        }

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
        hasPlayedSound = false; // Reset flag agar bisa dimainkan lagi nanti

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
            ManagerMini.instance.AddProgress();

            // Instantiate objek audio
            if (audioPrefab != null)
            {
                GameObject audioObject = Instantiate(audioPrefab, transform.position, Quaternion.identity);
                Destroy(audioObject, 2f); // Hancurkan setelah 2 detik agar tidak menumpuk
            }

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