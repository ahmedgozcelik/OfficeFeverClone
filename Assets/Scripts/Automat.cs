using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Automat : MonoBehaviour
{
    [SerializeField] private GameObject moneyPrefab;
    [SerializeField] private Transform moneyPoint;

    private List<GameObject> producedMoneys = new List<GameObject>();

    private float height = 0.130f;
    private int producedMoneyCount = 0;

    private void Start()
    {
        InvokeRepeating(nameof(ProduceMoney), 0f, 2f);
    }

    private void ProduceMoney()
    {
        Vector3 targetPosition = moneyPoint.position + new Vector3(0, height * producedMoneyCount, 0);
        GameObject produceMoney = Instantiate(moneyPrefab, targetPosition, moneyPoint.rotation);

        producedMoneys.Add(produceMoney);
        producedMoneyCount++;
    }

    public void RemoveMoney(GameObject money)
    {
        if (producedMoneys.Contains(money))
        {
            producedMoneys.Remove(money);
            UpdateMoneyPositions();
            producedMoneyCount--;
        }
    }

    private void UpdateMoneyPositions()
    {
        for (int i = 0; i < producedMoneys.Count; i++)
        {
            Vector3 newPosition = moneyPoint.position + new Vector3(0, height * i, 0);
            producedMoneys[i].transform.position = newPosition;
        }
    }
}
