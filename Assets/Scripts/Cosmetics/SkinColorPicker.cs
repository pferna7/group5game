using UnityEngine;
using UnityEngine.UI;

public class SkinColorPicker : MonoBehaviour
{
    public CosmeticManager cosmeticManager;
    public Button[] colorButtons;
    public Color[] availableColors;

    private void Start()
    {
        for (int i = 0; i < colorButtons.Length; i++)
        {
            int index = i;
            colorButtons[i].GetComponent<Image>().color = availableColors[index];
            colorButtons[i].onClick.AddListener(() => cosmeticManager.SetSkinColor(availableColors[index]));
        }
    }
}