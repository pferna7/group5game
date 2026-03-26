using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CosmeticShopUI : MonoBehaviour
{
    [Header("References")]
    public PlayerInventory playerInventory;
    public CosmeticManager cosmeticManager;

    [Header("Shop Items")]
    public List<CosmeticItem> shopItems = new List<CosmeticItem>();

    [Header("UI Elements")]
    public Transform itemListParent;
    public GameObject shopItemPrefab;
    public Text currencyText;
    public Text feedbackText;
    public GameObject shopPanel;

    private void Start()
    {
        shopPanel.SetActive(true);
        RefreshShop();
    }

    public void ToggleShop()
    {
        shopPanel.SetActive(!shopPanel.activeSelf);
        if (shopPanel.activeSelf)
        {
            RefreshShop();
        }
    }

    public void RefreshShop()
    {
        // clear old buttons
        foreach (Transform child in itemListParent)
        {
            Destroy(child.gameObject);
        }

        currencyText.text = "Coins: " + playerInventory.currency;

        foreach (CosmeticItem item in shopItems)
        {
            GameObject entry = Instantiate(shopItemPrefab, itemListParent);
            ShopItemEntry entryScript = entry.GetComponent<ShopItemEntry>();
            entryScript.Setup(item, this);
        }
    }

    public void OnItemBuyClicked(CosmeticItem item)
    {
        if (playerInventory.OwnsItem(item))
        {
            // already owned, just equip it
            cosmeticManager.EquipItem(item);
            feedbackText.text = "Equipped " + item.itemName;
        }
        else if (!playerInventory.MeetsLevelRequirement(item))
        {
            feedbackText.text = "Need level " + item.requiredLevel;
        }
        else if (!playerInventory.CanAfford(item))
        {
            feedbackText.text = "Not enough coins!";
        }
        else
        {
            playerInventory.TryPurchase(item);
            cosmeticManager.EquipItem(item);
            feedbackText.text = "Bought & equipped " + item.itemName;
        }

        RefreshShop();
    }
}