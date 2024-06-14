using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    public static PlayerPrefsManager instance;

    private int defaultMoney = 200;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    //Money
    public void IncreaseMoney(int amount)
    {
        defaultMoney = GetMoney();
        defaultMoney += amount;
        PlayerPrefs.SetInt("Money", defaultMoney);
    }

    public void DecreaseMoney(int amount)
    {
        defaultMoney = GetMoney();
        defaultMoney += amount;
        PlayerPrefs.SetInt("Money", defaultMoney);
    }

    public int GetMoney()
    {
        return PlayerPrefs.GetInt("Money", defaultMoney);
    }
}
