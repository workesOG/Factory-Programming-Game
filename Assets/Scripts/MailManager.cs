using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MailManager : MonoBehaviour
{
    private Transform mailWindow;
    public List<Mail> mails = new List<Mail>();
    private GameObject currentlyLoadedMailGO = null;
    private string currentlyLoadedMail;

    private Transform listWindow;
    private List<MailListElement> mailListElements = new List<MailListElement>();
    private List<GameObject> dividers = new();

    public Button loadUnread;
    public Button loadRead;

    private static MailManager _instance;

    public static MailManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<MailManager>();
            }

            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //RecieveMail("");
    }

    public void Initialize(Transform windowTransform)
    {
        mailWindow = windowTransform.Find("Reader Window/Main Scroll/Viewport/Content");
        listWindow = windowTransform.Find("List/Mails Scroll View/Viewport/Content");
        loadUnread = windowTransform.Find("List/Show Unread Button").GetComponent<Button>();
        loadRead = windowTransform.Find("List/Show Read Button").GetComponent<Button>();
    }

    public void ClearReferenceLists()
    {
        foreach (MailListElement element in mailListElements)
            Destroy(element.gameObject);
        mailListElements.Clear();

        foreach (GameObject go in dividers)
            Destroy(go);
        dividers.Clear();
    }

    public void LoadMails(bool loadOnlyRead)
    {
        ClearReferenceLists();
        List<Mail> mailsToShow = mails.Where(x => x.recieved == true && x.read == loadOnlyRead).ToList();
        foreach (Mail mail in mailsToShow)
        {
            GameObject listElement = Instantiate(Resources.Load<GameObject>("Prefabs/Mail List Element"), listWindow);
            MailListElement element = listElement.AddComponent<MailListElement>();
            element.associatedMailTitle = mail.title;
            element.Initialize();
            dividers.Add(Instantiate(Resources.Load<GameObject>("Prefabs/Mail List Element Divider"), listWindow));
        }
        loadRead.interactable = !loadOnlyRead;
        loadUnread.interactable = loadOnlyRead;
    }

    public void OpenMail(string title)
    {
        Mail foundMail = FindMail(title);
        if (foundMail == null)
        {
            Debug.LogWarning($"Mail {title} was not found!");
            return;
        }

        MailListElement mailListElement = mailListElements.Find(x => x.associatedMailTitle == foundMail.title);

        if (currentlyLoadedMailGO != null)
        {
            Destroy(currentlyLoadedMailGO);
            mailListElements.Find(x => x.associatedMailTitle == currentlyLoadedMail).SetButtonInteractableState(true);
        }

        currentlyLoadedMailGO = Instantiate(foundMail.mailPrefab, mailWindow, false);
        currentlyLoadedMail = foundMail.title;
        mailListElements.Find(x => x.associatedMailTitle == foundMail.title).SetButtonInteractableState(false);

        if (foundMail.read == false)
            foundMail.read = true;
    }

    public void RecieveMail(string title)
    {
        Mail foundMail = FindMail(title);
        if (foundMail == null)
        {
            Debug.LogWarning($"Mail {title} was not found!");
            return;
        }
        foundMail.recieved = true;
    }

    public Mail FindMail(string title)
    {
        return mails.Find(x => x.title == title);
    }
}

[Serializable]
public class Mail
{
    public string title;
    public bool recieved;
    public bool read;

    public GameObject mailPrefab;
}