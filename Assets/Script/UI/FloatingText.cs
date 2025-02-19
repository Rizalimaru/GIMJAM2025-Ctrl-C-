using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 50f;
    public float duration = 1f;
    private TextMeshProUGUI textMesh;
    private RectTransform rectTransform;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();

        // Jalankan animasi naik
        StartCoroutine(MoveUp());
    }

    private System.Collections.IEnumerator MoveUp()
    {
        float elapsed = 0f;
        Vector3 startPos = rectTransform.anchoredPosition;
        Vector3 targetPos = startPos + new Vector3(0, 100, 0); // Naik 100px

        while (elapsed < duration)
        {
            rectTransform.anchoredPosition = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject); // Hapus teks setelah selesai animasi
    }
}
