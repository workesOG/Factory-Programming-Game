using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonHandlerTemplate : MonoBehaviour
{
    private static SingletonHandlerTemplate _instance;

    public static SingletonHandlerTemplate Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SingletonHandlerTemplate>();
            }

            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
