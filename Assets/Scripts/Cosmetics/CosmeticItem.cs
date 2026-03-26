using UnityEngine;

[CreateAssetMenu(fileName = "NewCosmetic", menuName = "Cosmetics/CosmeticItem")]
public class CosmeticItem : ScriptableObject
{
    public string itemName;
    public CosmeticSlot slot;
    public Sprite icon;
    public Sprite worldSprite;
    public int price;
    public int requiredLevel;
    [TextArea]
    public string description;
    public Color itemColor = Color.white;
}