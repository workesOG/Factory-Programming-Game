using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppButton : MonoBehaviour
{
    public int associatedApp;

    public void Start()
    {
        if (name == "Cross Background")
            GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.CloseApp(associatedApp));
        else
            GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.MinimizeApp(associatedApp));
    }
}
