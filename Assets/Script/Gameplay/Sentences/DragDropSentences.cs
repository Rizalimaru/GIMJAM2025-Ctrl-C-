using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropSentences : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 originalPosition;
    private Transform originalParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        // Simpan posisi awal setelah di-setup oleh JumbledSentencesGame
        originalPosition = transform.position;
        originalParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f; // Transparan saat diseret
        canvasGroup.blocksRaycasts = false; // Supaya bisa didrop ke slot
        transform.SetParent(transform.root); // Pindah ke root agar tidak terkunci layout
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition; // Gerakkan sesuai mouse
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Jika tidak di-drop di slot, kembalikan ke posisi awal
        if (transform.parent == transform.root)
        {
            transform.SetParent(originalParent); // Kembalikan ke parent semula
            transform.position = originalPosition; // Kembalikan ke posisi awal
        }
    }
}
