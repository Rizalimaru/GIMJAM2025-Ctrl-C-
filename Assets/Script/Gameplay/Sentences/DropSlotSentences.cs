using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlotSentences : MonoBehaviour, IDropHandler
{
    public string correctWord; // Kata yang benar untuk slot ini

    public void OnDrop(PointerEventData eventData)
    {
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
                }
                else
                {
                    Debug.Log("Salah!");
                }
            }
        }
    }
}
