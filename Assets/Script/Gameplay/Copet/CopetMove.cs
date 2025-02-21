using UnityEngine;

public class CopetMove : MonoBehaviour
{
    public float rotateAngle = 20f; // Seberapa jauh kepala berputar
    public float rotateSpeed = 5f; // Kecepatan geleng-geleng kepala

    public Transform head; // Referensi ke kepala copet

    void Start()
    {


        // Jika tidak ada, pakai objek utama
        if (head == null) head = transform;
    }

    void Update()
    {
        // Geleng-geleng kepala
        float headRotation = Mathf.Sin(Time.time * rotateSpeed) * rotateAngle;
        head.localRotation = Quaternion.Euler(0, 0, headRotation);
    }
}
