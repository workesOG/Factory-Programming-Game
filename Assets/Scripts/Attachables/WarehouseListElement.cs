using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarehouseListElement : MonoBehaviour
{
    private MaterialSO material;
    private TMP_Text amount;

    public void Initialize(MaterialSO material)
    {
        transform.Find("Name").GetComponent<TMP_Text>().text = material.name;
        amount = transform.Find("Amount").GetComponent<TMP_Text>();
        Image icon = transform.Find("Icon Background/Icon").GetComponent<Image>();
        icon.sprite = material.icon;
        icon.color = material.iconColor;

        this.material = material;
    }

    public void Update()
    {
        amount.text = material.amount.ToString();
    }
}
