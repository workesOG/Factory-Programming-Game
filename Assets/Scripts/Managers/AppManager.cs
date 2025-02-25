using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class AppManager : MonoBehaviour
{
    public Transform appWindow;
    public List<GameObject> applications;
    public List<Indicator> indicators;

    public int currentAppID = -1;
    private List<OpenedApp> openedApps = new();
    [SerializeField]
    private CursorManager cursorManager;

    private static AppManager _instance;
    public static AppManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<AppManager>();
            }

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
        { 8, null },
    };

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
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
            openedApps.Last().Show();
        }
        else
        {
            foundApp.Show();
        }
        HandleAppOnShow(appID);
        int oldAppID = currentAppID;
        currentAppID = appID;

        if (oldAppID != -1 && oldAppID != appID)
            MinimizeApp(oldAppID);

        indicators.Find(x => x.ID == appID).SetState(Indicator.State.Opened);
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

        indicators.Find(x => x.ID == appID).SetState(Indicator.State.Unopened);
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
        indicators.Find(x => x.ID == appID).SetState(Indicator.State.Minimized);
    }

    private void HandleAppInitialization(int appID, Transform window)
    {
        switch (appID)
        {
            case 0:
                ConsoleManager.Instance.Initialize(window);
                break;
            case 1:
                break;
            case 2:
                MaterialsManager.Instance.Initialize(window);
                break;
            case 3:
                break;
            case 4:
                WarehouseManager.Instance.Initialize(window);
                break;
            case 5:
                break;
            case 6:
                MailManager.Instance.Initialize(window);
                break;
            case 7:
                break;
            case 8:
                ScriptManager.Instance.Initialize(window);
                break;
        }
        AssignCursorFunctions(window);
    }

    private void HandleAppOnShow(int appID)
    {
        switch (appID)
        {
            case 0:
                ConsoleManager.Instance.UpdateLog();
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
            case 8:
                break;
        }
    }

    private void AssignCursorFunctions(Transform window)
    {
        EventTrigger et1 = window.Find("Top Beam/Minimize Background").GetComponent<EventTrigger>();
        EventTrigger et2 = window.Find("Top Beam/Cross Background").GetComponent<EventTrigger>();

        EventTrigger.Entry enterEntry = new EventTrigger.Entry();
        enterEntry.eventID = EventTriggerType.PointerEnter;
        enterEntry.callback.AddListener((data) => { cursorManager.OnButtonCursorEnter(); });

        // Create Entry for Pointer Exit
        EventTrigger.Entry exitEntry = new EventTrigger.Entry();
        exitEntry.eventID = EventTriggerType.PointerExit;
        exitEntry.callback.AddListener((data) => { cursorManager.OnButtonCursorExit(); });

        // Add Entries to Event Trigger
        et1.triggers.Add(enterEntry);
        et1.triggers.Add(exitEntry);
        et2.triggers.Add(enterEntry);
        et2.triggers.Add(exitEntry);
    }
}

[Serializable]
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
