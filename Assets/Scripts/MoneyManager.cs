using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyHandler : MonoBehaviour
{
    public TMP_Text moneyDisplay;

    private double _money;
    public double Money
    {
        get { return Math.Floor(_money); }
        set
        {
            _money = value;
            moneyDisplay.text = $"${_money.ToString():f0}";
        }
    }

    private static MoneyHandler _instance;

    public static MoneyHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<MoneyHandler>();
            }

            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Money = 0;
    }
}
