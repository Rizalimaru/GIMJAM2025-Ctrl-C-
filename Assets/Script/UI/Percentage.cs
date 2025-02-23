using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Percentage : MonoBehaviour
{
    public Slider slider;  // Referensi ke Slider
    public TextMeshProUGUI percentageText; // Referensi ke teks UI untuk menampilkan persen

    void Start()
    {
        // Pastikan slider memiliki nilai default yang diperbarui saat awal
        UpdatePercentage(slider.value);
        
        // Tambahkan listener untuk memperbarui teks saat nilai slider berubah
        slider.onValueChanged.AddListener(UpdatePercentage);
    }

    void UpdatePercentage(float value)
    {
        int percentage = Mathf.RoundToInt(value * 1); // Konversi ke persen
        percentageText.text = percentage + "%"; // Tampilkan di UI
    }
}
