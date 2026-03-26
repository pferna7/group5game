using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<CosmeticItem> ownedCosmetics = new List<CosmeticItem>();
    public int currency = 100; // starting currency
    public int playerLevel = 1;

    public bool CanAfford(CosmeticItem item)
    {
        return currency >= item.price;
    }

    public bool MeetsLevelRequirement(CosmeticItem item)
    {
        return playerLevel >= item.requiredLevel;
    }

    public bool OwnsItem(CosmeticItem item)
    {
        return ownedCosmetics.Contains(item);
    }

    public bool TryPurchase(CosmeticItem item)
    {
        if (OwnsItem(item)) return false;
        if (!CanAfford(item)) return false;
        if (!MeetsLevelRequirement(item)) return false;

        currency -= item.price;
        ownedCosmetics.Add(item);
        return true;
    }

    public void AddCurrency(int amount)
    {
        currency += amount;
    }

    public void AddLevel()
    {
        playerLevel++;
    }
}