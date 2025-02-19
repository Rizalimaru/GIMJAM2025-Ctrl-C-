using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Referensi ke objek pemain
    public float smoothSpeed = 0.125f;  // Kecepatan smooth camera mengikuti pemain
    public Vector3 offset;  // Offset antara kamera dan pemain

    public Transform leftWall;  // Objek Invisible Wall kiri
    public Transform rightWall; // Objek Invisible Wall kanan

    private float leftLimit;
    private float rightLimit;
    private float fixCoordinate = 10f;

    private void Start()
    {
        // Ambil posisi X dari Invisible Wall saat game dimulai
        if (leftWall != null) leftLimit = leftWall.position.x + fixCoordinate;
        if (rightWall != null) rightLimit = rightWall.position.x - fixCoordinate;
    }

    private void FixedUpdate()
    {
        // Posisi target kamera mengikuti pemain
        Vector3 desiredPosition = player.position + offset;

        // Interpolasi posisi kamera untuk membuat pergerakan lebih smooth
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Pastikan kamera tidak melewati batas kiri dan kanan
        float clampedX = Mathf.Clamp(smoothedPosition.x, leftLimit, rightLimit);

        // Terapkan posisi kamera yang sudah dibatasi
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}
