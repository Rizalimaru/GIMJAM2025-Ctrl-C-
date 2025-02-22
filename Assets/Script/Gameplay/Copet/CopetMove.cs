using UnityEngine;

public class CopetMove : MonoBehaviour
{
    public float moveDistance = 3f;
    public float moveSpeed = 2f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (!enabled) return; // Jika disabled, tidak bergerak

        float offset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        transform.position = new Vector3(startPos.x + offset, startPos.y, startPos.z);
    }

    public void Disable()
    {
        enabled = false; // Menonaktifkan script CopetMove
    }

    public void Enable()
    {
        enabled = true; // Menonaktifkan script CopetMove
    }
}
