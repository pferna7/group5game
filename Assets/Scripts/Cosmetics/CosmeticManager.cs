using System.Collections.Generic;
using UnityEngine;

public class CosmeticManager : MonoBehaviour
{
    [Header("Cosmetic Slots (assign child GameObjects)")]
    public SpriteRenderer hatRenderer;
    public SpriteRenderer shirtRenderer;
    public SpriteRenderer pantsRenderer;
    public SpriteRenderer shoesRenderer;
    public SpriteRenderer armorRenderer;

    [Header("Body (for skin color)")]
    public SpriteRenderer bodyRenderer;

    private Dictionary<CosmeticSlot, CosmeticItem> equippedItems = new Dictionary<CosmeticSlot, CosmeticItem>();

    public void EquipItem(CosmeticItem item)
    {
        equippedItems[item.slot] = item;
        ApplyCosmetic(item);
    }

    public void UnequipItem(CosmeticSlot slot)
    {
        if (equippedItems.ContainsKey(slot))
        {
            equippedItems.Remove(slot);
            ClearSlotVisual(slot);
        }
    }

    public CosmeticItem GetEquippedItem(CosmeticSlot slot)
    {
        equippedItems.TryGetValue(slot, out CosmeticItem item);
        return item;
    }

    public void SetSkinColor(Color color)
    {
        if (bodyRenderer != null)
        {
            bodyRenderer.color = color;
        }
    }

    private void ApplyCosmetic(CosmeticItem item)
    {
        if (item.slot == CosmeticSlot.Shirt)
        {
            if (bodyRenderer != null)
            {
                bodyRenderer.color = item.itemColor;
            }
        }
        else
        {
            SpriteRenderer renderer = GetRendererForSlot(item.slot);
            if (renderer != null)
            {
                renderer.sprite = item.worldSprite;
            }
        }
    }

    private void ClearSlotVisual(CosmeticSlot slot)
    {
        SpriteRenderer renderer = GetRendererForSlot(slot);
        if (renderer != null)
        {
            renderer.sprite = null;
        }
    }

    private SpriteRenderer GetRendererForSlot(CosmeticSlot slot)
    {
        switch (slot)
        {
            case CosmeticSlot.Hat: return hatRenderer;
            case CosmeticSlot.Shirt: return shirtRenderer;
            case CosmeticSlot.Pants: return pantsRenderer;
            case CosmeticSlot.Shoes: return shoesRenderer;
            case CosmeticSlot.Armor: return armorRenderer;
            default: return null;
        }
    }
}