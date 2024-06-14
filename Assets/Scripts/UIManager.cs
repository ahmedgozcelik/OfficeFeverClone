using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    PlayerPrefsManager playerPrefsManager;

    [SerializeField] private TextMeshProUGUI moneyText;

    private int moneyCount;

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
