using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    public static PlayerPrefsManager instance;

    private int defaultGold = 200;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    //GOLD
    public void IncreaseGold(int amount)
    {
        defaultGold = GetGold();
        defaultGold += amount;
        PlayerPrefs.SetInt("Gold", defaultGold);
    }

    public void DecreaseGold(int amount)
    {
        defaultGold = GetGold();
        defaultGold += amount;
        PlayerPrefs.SetInt("Gold", defaultGold);
    }

    public int GetGold()
    {
        return PlayerPrefs.GetInt("Gold", defaultGold);
    }
}
