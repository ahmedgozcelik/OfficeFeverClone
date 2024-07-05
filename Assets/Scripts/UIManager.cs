using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private PlayerPrefsManager playerPrefsManager;
    private Player player;

    public TextMeshProUGUI moneyText;
    public GameObject managementPanel;

    public List<GameObject> helperList = new List<GameObject>();
    public TextMeshProUGUI helperPriceText;
    public TextMeshProUGUI increaseCapacityText;

    private int moneyCount;
    private int helperNumber = 0;
    private int increaseCapacityNumber = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerPrefsManager = PlayerPrefsManager.instance;
        UpdateMoney();
    }

    private void Update()
    {
        UpdateMoney();
    }

    private void UpdateMoney()
    {
        moneyCount = playerPrefsManager.GetMoney();
        moneyText.text = $"${moneyCount}";
    }

    public void OnManagementPanel()
    {
        managementPanel.SetActive(true);
    }

    public void OffManagementPanel()
    {
        managementPanel.SetActive(false);
    }

    public void AddHelper()
    {
        int helperPrice = CalculateHelperPrice();
        if (CanAfford(helperPrice))
        {
            playerPrefsManager.DecreaseMoney(helperPrice);
            ActivateHelper();
            UpdateHelperPriceText(helperPrice);
        }
    }

    public void IncreasePlayerCapacity()
    {
        int increaseCapacityPrice = CalculateIncreaseCapacityPrice();
        if (CanAfford(increaseCapacityPrice))
        {
            playerPrefsManager.DecreaseMoney(increaseCapacityPrice);
            player.maxPapers += 5;
            UpdateIncreaseCapacityText(increaseCapacityPrice);
        }
    }

    private int CalculateHelperPrice()
    {
        return helperNumber * 1000 + 1000;
    }

    private int CalculateIncreaseCapacityPrice()
    {
        return increaseCapacityNumber * 1000 + 1000;
    }

    private bool CanAfford(int price)
    {
        return moneyCount >= price;
    }

    private void ActivateHelper()
    {
        helperList[helperNumber].SetActive(true);
        helperNumber++;
    }

    private void UpdateHelperPriceText(int price)
    {
        helperPriceText.text = $"${price}";
    }

    private void UpdateIncreaseCapacityText(int price)
    {
        increaseCapacityText.text = $"${price}";
        increaseCapacityNumber++;
    }
}
