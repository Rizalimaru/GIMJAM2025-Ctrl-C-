using UnityEngine;
using UnityEngine.UI;

public class PThrow : MonoBehaviour
{
    public GameObject sandalPrefab;
    public Transform throwPoint;
    public GameObject crosshair; // Titik aim
    public Slider powerBar; // Power bar UI

    private bool isAiming = true;
    private bool isPowering = false;
    private float power = 0f;
    private bool increasing = true;
    private Vector3 lockedTarget; // Simpan posisi aim yang terkunci

    void Update()
    {
        if (isAiming)
        {
            // Aim crosshair ke arah mouse, tetapi tetap di Z = -5
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z - throwPoint.position.z)));
            mousePos.z = -5f; 
            crosshair.transform.position = mousePos;

            // Tekan klik kiri untuk mengunci aim dan mulai power
            if (Input.GetMouseButtonDown(0))
            {
                isAiming = false;
                isPowering = true;
                powerBar.gameObject.SetActive(true);

                // Simpan posisi locked aim dengan koordinat dunia yang benar
                lockedTarget = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z - throwPoint.position.z)));
                lockedTarget.z = -5f;
            }
        }
        else if (isPowering)
        {
            // Power bar naik turun otomatis
            if (increasing)
                power += Time.deltaTime * 3;
            else
                power -= Time.deltaTime * 3;

            if (power >= 1f) increasing = false;
            if (power <= 0f) increasing = true;

            powerBar.value = power;

            // Klik kiri untuk lempar sandal
            if (Input.GetMouseButtonDown(0))
            {
                isPowering = false;
                powerBar.gameObject.SetActive(false);
                ThrowSandal(power);

                // Reset aim untuk lemparan berikutnya
                isAiming = true;
                power = 0f;
                increasing = true;
            }
        }
    }

    void ThrowSandal(float power)
    {
        Vector3 spawnPos = throwPoint.position;

        // Hitung arah berdasarkan locked target
        Vector3 direction = (lockedTarget - spawnPos).normalized;

        // Spawn sandal
        GameObject sandal = Instantiate(sandalPrefab, spawnPos, Quaternion.identity);
        sandal.GetComponent<Sandal>().SetDirection(direction, power); // Kirim power juga
}

}
