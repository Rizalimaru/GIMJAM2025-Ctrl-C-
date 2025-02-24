using UnityEngine;

public class Flower : MonoBehaviour
{
    private Vector2 startPosition;
    private float startPositionZ; // Simpan posisi awal Z
    private bool isDragging = false;
    public bool isPlaced = false;
    public int flowerID; // ID unik untuk bunga ini

    private Vector3 originalScale = new Vector3(0.4f, 0.4f, 1f); // Skala awal 0.4
    private Vector3 asliScale = new Vector3(1f, 1f, 1f);
    public float maxScale = 1f;
    public float scaleSpeed = 3f;
    private float snapThreshold = 10f; // Jarak minimal agar bisa dipasang

    private bool hasPlayedSound = false; // Tambahkan flag untuk suara

    void Start()
    {
        startPosition = transform.position;
        startPositionZ = transform.position.z; // Simpan Z awal
        transform.localScale = originalScale;
    }

    void OnMouseDown()
    {
        if (!isPlaced)
        {
            isDragging = true;
        }
    }

    void OnMouseDrag()
    {
        if (isDragging && Time.timeScale != 0f)
        {
            if (!hasPlayedSound) // Cek agar suara hanya diputar sekali
            {
                AudioManager.Instance.PlaySFX("Bunga",0);
                hasPlayedSound = true;
            }

            Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(newPos.x, newPos.y, startPositionZ); // Pastikan Z tetap
            AdjustScale();
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        hasPlayedSound = false; // Reset flag agar suara bisa dimainkan lagi nanti

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (Collider2D col in colliders)
        {
            FlowerSlot slot = col.GetComponent<FlowerSlot>();
            if (slot != null && slot.slotID == flowerID)
            {
                float distance = Vector2.Distance(transform.position, slot.transform.position);
                if (distance < snapThreshold)
                {
                    SnapToSlot(slot);
                    return;
                }
            }
        }

        ResetPosition();
    }

    void AdjustScale()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2f);
        foreach (Collider2D col in colliders)
        {
            FlowerSlot slot = col.GetComponent<FlowerSlot>();
            if (slot != null && slot.slotID == flowerID)
            {
                float distance = Vector2.Distance(transform.position, slot.transform.position);
                float t = Mathf.Clamp01(1 - (distance / 2f));
                float newScale = Mathf.Lerp(originalScale.x, maxScale, t);
                transform.localScale = new Vector3(newScale, newScale, 1);
                return;
            }
        }

        transform.localScale = originalScale;
    }

    void SnapToSlot(FlowerSlot slot)
    {
        transform.position = new Vector3(slot.transform.position.x, slot.transform.position.y, startPositionZ); // Pastikan Z tetap
        transform.localScale = asliScale;
        isPlaced = true;
        GetComponent<Collider2D>().enabled = false;
        AudioManager.Instance.PlaySFX("Bunga",1);
        FlowerManager.instance.CheckAllFlowersPlaced();
    }

    void ResetPosition()
    {
        transform.position = new Vector3(startPosition.x, startPosition.y, startPositionZ); // Pastikan Z tetap
        transform.localScale = originalScale;
    }

    public void ResetFlower()
    {
        transform.position = new Vector3(startPosition.x, startPosition.y, startPositionZ);
        transform.localScale = originalScale;
        isPlaced = false;
        GetComponent<Collider2D>().enabled = true;
    }
}
