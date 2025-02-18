using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Referensi ke objek pemain
    public float smoothSpeed = 0.125f;  // Kecepatan smooth camera mengikuti pemain
    public Vector3 offset;  // Offset antara kamera dan pemain

    private void FixedUpdate()
    {
        // Posisi target untuk kamera mengikuti pemain
        Vector3 desiredPosition = player.position + offset;
        
        // Interpolasi posisi kamera untuk membuat pergerakan lebih smooth
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        // Menyusun posisi kamera, tetap mempertahankan posisi vertikal agar tidak ikut bergeser terlalu jauh
        transform.position = new Vector3(smoothedPosition.x, transform.position.y, transform.position.z);
    }
}
