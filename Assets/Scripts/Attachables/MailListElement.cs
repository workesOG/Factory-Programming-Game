using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MailListElement : MonoBehaviour
{
    public string associatedMailTitle;

    private Button buttonComponent;

    // Start is called before the first frame update
    public void Initialize()
    {
        TMP_Text titleText = transform.Find("Title").GetComponent<TMP_Text>();
        buttonComponent = GetComponent<Button>();

        titleText.text = associatedMailTitle;
        buttonComponent.onClick.AddListener(ShowMail);

    }

    public void SetButtonInteractableState(bool interactable)
    {
        buttonComponent.interactable = interactable;
    }

    private void ShowMail()
    {
        MailManager.Instance.OpenMail(associatedMailTitle);
    }
}
