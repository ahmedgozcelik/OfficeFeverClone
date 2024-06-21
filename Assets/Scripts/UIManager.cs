using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    PlayerPrefsManager playerPrefsManager;

    public TextMeshProUGUI moneyText;
    private int moneyCount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        playerPrefsManager = PlayerPrefsManager.instance;
    }

    private void Update()
    {
        UpdateMoney();
    }

    private void UpdateMoney()
    {
        moneyCount = playerPrefsManager.GetMoney();
        moneyText.text = "$" + moneyCount.ToString();
    }
}
