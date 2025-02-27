using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MachineShopElement : MonoBehaviour
{
    private MachineSO machine;
    private Button button;

    public void Initialize(MachineSO machine)
    {
        transform.Find("Top Beam/Name").GetComponent<TMP_Text>().text = machine.machineName;
        //transform.Find("Descriptors/Price/Price").GetComponent<TMP_Text>().text = machine.unlocked ? "Already owned" : $"${machine.price:f2}";
        transform.Find("Icon Panel/Icon").GetComponent<Image>().color = machine.iconColor;

        button = transform.Find("Bottom Beam (Button)").GetComponent<Button>();
        button.interactable = false;
        button.onClick.AddListener(OnButtonClicked);
        transform.Find("Bottom Beam (Button)/Text").GetComponent<TMP_Text>().text = machine.unlocked ? "Already owned" : $"${machine.price:f2}";


        this.machine = machine;
    }

    public void Update()
    {
        if (machine.unlocked)
            return;

        button.interactable = MoneyManager.Instance.Money >= machine.price;
    }

    private void OnButtonClicked()
    {
        if (machine.unlocked)
            return;

        if (MoneyManager.Instance.Money < machine.price)
            return;

        MoneyManager.Instance.Money -= machine.price;
        machine.unlocked = true;
        button.interactable = false;
    }
}
