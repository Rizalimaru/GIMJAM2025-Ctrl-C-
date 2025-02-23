using UnityEngine;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour
{
    public GameObject objectSettings;
    public Button button;
    private ColorBlock originalColors;
    private bool isSelected = false;

    void Start()
    {
        // Simpan warna asli tombol
        originalColors = button.colors;
    }

    void Update()
    {
        if (objectSettings.activeSelf)
        {
            SelectButton();
        }
        else
        {
            DeselectButton();
        }
    }

    public void DeselectButton()
    {
        button.OnDeselect(null);
        button.transition = Selectable.Transition.ColorTint;
        isSelected = false;
    }

    public void SelectButton()
    {
        button.Select();
        button.transition = Selectable.Transition.None;
        isSelected = true;
    }

    public void SetButtonInteractable(bool state)
    {
        button.interactable = state;

        ColorBlock colors = button.colors;

        if (!state && isSelected)
        {
            // Paksa warna tombol tetap seperti "selected"
            colors.disabledColor = colors.selectedColor;
            button.colors = colors;
        }
        else
        {
            button.colors = originalColors; // Kembalikan ke warna asli saat aktif
        }
    }
}
