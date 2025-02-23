using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class DropSlotSentences : MonoBehaviour, IDropHandler
{
    public string correctWord; // Kata yang benar untuk slot ini
    private RawImage slotImage;
    private bool isCorrect = false;

    private void Awake()
    {
        slotImage = GetComponent<RawImage>(); // Ambil RawImage dari slot
        if (slotImage == null)
        {
            Debug.LogError("RawImage tidak ditemukan pada slot!");
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (isCorrect) return; // Jika sudah benar, tidak bisa diisi ulang

        GameObject droppedObject = eventData.pointerDrag;

        if (droppedObject != null)
        {
            DragDropSentences dragDrop = droppedObject.GetComponent<DragDropSentences>();
            if (dragDrop != null)
            {
                droppedObject.transform.SetParent(transform); // Tempatkan di slot
                droppedObject.transform.position = transform.position;

                // Cek apakah kata yang ditempatkan benar
                if (droppedObject.name == correctWord)
                {
                    Debug.Log("Benar!");
                    slotImage.color = Color.yellow; // Ubah warna menjadi kuning
                    isCorrect = true; // Tandai sebagai benar
                    dragDrop.enabled = false; // Matikan drag-drop
                }
                else
                {
                    Debug.Log("Salah!");
                    StartCoroutine(FlashRed()); // Ubah merah lalu kembali putih
                }
            }
        }
    }

    private IEnumerator FlashRed()
    {
        slotImage.color = Color.red; // Ubah warna merah
        yield return new WaitForSeconds(0.5f); // Tunggu 0.5 detik
        slotImage.color = new Color(1, 1, 1, 0.2f); // Warna putih dengan transparansi 50%
    }
}
