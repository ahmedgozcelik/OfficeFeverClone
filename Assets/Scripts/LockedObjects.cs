using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LockedObjects : MonoBehaviour
{
    PlayerPrefsManager playerPrefsManager;

    [SerializeField] private GameObject lockedObject;
    [SerializeField] private Image progressImage;
    [SerializeField] private TextMeshProUGUI priceText;

    private bool isFilling;
    private float timer;
    private float fillTime = 3f;

    [SerializeField] private int price = 100;

    private void Start()
    {
        playerPrefsManager = PlayerPrefsManager.instance;

        UpdateVisual();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartFilling();
            FillingImage();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopFilling();
            FillingImage();
        }
    }

    private void FillingImage()
    {
        if (isFilling)
        {
            timer += Time.deltaTime;
            progressImage.fillAmount = timer / fillTime;

            if (progressImage.fillAmount >= 1f)
            {
                CheckMoney();
            }
        }
        else
        {
            timer = 0f;
            progressImage.fillAmount = 0f;
        }
    }
    private void StartFilling()
    {
        isFilling = true;
    }
    private void StopFilling()
    {
        isFilling = false;
    }

    private void CheckMoney()
    {
        if (playerPrefsManager.GetMoney() >= price)
        {
            gameObject.SetActive(false);
            lockedObject.SetActive(true);
            playerPrefsManager.DecreaseMoney(price);
        }
        else
        {
            Debug.Log("your balance is insufficient");
        }
    }

    private void UpdateVisual()
    {
        priceText.text = "$" + price.ToString();
    }
}
