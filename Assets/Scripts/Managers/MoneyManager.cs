using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
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

    private static MoneyManager _instance;

    public static MoneyManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<MoneyManager>();
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
