using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaterialShopElement : MonoBehaviour
{
    private MaterialSO material;
    private Button button;

    public void Initialize(MaterialSO material)
    {
        transform.Find("Top Beam/Name").GetComponent<TMP_Text>().text = material.materialName;
        transform.Find("Descriptors/Buying Price/Price").GetComponent<TMP_Text>().text = material.buyPrice <= 0 ? "N/A" : $"${material.buyPrice:f2}";
        transform.Find("Descriptors/Selling Price/Price").GetComponent<TMP_Text>().text = material.sellPrice <= 0 ? "N/A" : $"${material.sellPrice:f2}";
        Image icon = transform.Find("Icon Panel/Icon").GetComponent<Image>();
        icon.sprite = material.icon;
        icon.color = material.iconColor;

        button = transform.Find("Bottom Beam (Button)").GetComponent<Button>();
        button.interactable = false;
        button.onClick.AddListener(OnButtonClicked);
        transform.Find("Bottom Beam (Button)/Text").GetComponent<TMP_Text>().text = material.materialType == MaterialType.Raw ? material.unlocked ? "Unlocked" : $"${material.licensePrice:f0}" : "Can't be bought";


        this.material = material;
    }

    public void Update()
    {
        if (material.unlocked)
            return;

        button.interactable = MoneyManager.Instance.Money >= material.licensePrice;
    }

    private void OnButtonClicked()
    {
        if (material.unlocked)
            return;

        if (MoneyManager.Instance.Money < material.licensePrice)
            return;

        MoneyManager.Instance.Money -= material.licensePrice;
        material.unlocked = true;
        button.interactable = false;
    }
}
