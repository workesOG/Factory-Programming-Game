using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    public Transform appWindow;
    public List<GameObject> applications;

    private int currentAppID = -1;
    private List<OpenedApp> openedApps = new();

    private static AppManager _instance;
    public static AppManager Instance
    {
        get
        {
            if (_instance == null) _instance = new AppManager();
            return _instance;
        }
    }

    private Dictionary<int, GameObject> apps = new Dictionary<int, GameObject>
    {
        { 0, null },
        { 1, null },
        { 2, null },
        { 3, null },
        { 4, null },
        { 5, null },
        { 6, null },
        { 7, null },
    };

    private AppManager()
    {
        //the constructor is private so that you can't instantiate it
    }

    void Start()
    {
        AssociateDict();
    }

    void Update()
    {

    }

    public void AssociateDict()
    {
        for (int i = 0; i < applications.Count; i++)
        {
            apps[i] = applications[i];
        }
    }

    public void ShowApp(int appID)
    {
        OpenedApp foundApp = OpenedApp.Find(appID, openedApps);
        if (foundApp == null)
        {
            GameObject window = Instantiate(apps[appID], appWindow, true);
            HandleAppInitialization(appID, window.transform);
            RectTransform rect = window.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, 0);
            rect.localScale = new Vector3(1, 1, 1);
            openedApps.Add(new OpenedApp(appID, window));
        }
        else
        {
            foundApp.Show();
        }
        HandleAppOnShow(appID);
        int oldAppID = currentAppID;
        currentAppID = appID;

        if (oldAppID != -1)
            MinimizeApp(oldAppID);
    }

    public void CloseApp(int appID)
    {
        OpenedApp foundApp = OpenedApp.Find(appID, openedApps);
        if (foundApp == null)
        {
            return;
        }

        Destroy(foundApp.mainObject);
        openedApps.Remove(foundApp);

        if (openedApps.Count != 0)
        {
            ShowApp(openedApps.FirstOrDefault().appID);
        }
    }

    public void MinimizeApp(int appID)
    {
        OpenedApp foundApp = OpenedApp.Find(appID, openedApps);
        if (foundApp == null)
        {
            Debug.LogWarning("App with id: " + appID + " cannot be minimized, because it was not found!");
            return;
        }

        foundApp.Hide();
    }

    private void HandleAppInitialization(int appID, Transform window)
    {
        switch (appID)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                MailManager.Instance.Initialize(window);
                break;
            case 7:
                break;
        }
    }

    private void HandleAppOnShow(int appID)
    {
        switch (appID)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
        }
    }
}

public class OpenedApp
{
    public int appID;
    public GameObject mainObject;

    public void Show()
    {
        mainObject.SetActive(true);
    }

    public void Hide()
    {
        mainObject.SetActive(false);
    }

    public OpenedApp(int id, GameObject mainObject)
    {
        this.appID = id;
        this.mainObject = mainObject;
    }

    public static OpenedApp Find(int id, List<OpenedApp> list)
    {
        foreach (OpenedApp openApp in list)
        {
            if (openApp.appID == id)
                return openApp;
        }
        return null;
    }
}
