using UnityEngine;
using UnityEngine.UI;

public class ShopItemEntry : MonoBehaviour
{
    public Image iconImage;
    public Text nameText;
    public Text priceText;
    public Button buyButton;

    private CosmeticItem item;
    private CosmeticShopUI shop;

    public void Setup(CosmeticItem item, CosmeticShopUI shop)
    {
        this.item = item;
        this.shop = shop;

        nameText.text = item.itemName;
        iconImage.sprite = item.icon;

        if (shop.playerInventory.OwnsItem(item))
        {
            priceText.text = "Owned";
        }
        else if (!shop.playerInventory.MeetsLevelRequirement(item))
        {
            priceText.text = "Lvl " + item.requiredLevel;
        }
        else
        {
            priceText.text = item.price + " coins";
        }

        buyButton.onClick.AddListener(() => shop.OnItemBuyClicked(item));
    }
}