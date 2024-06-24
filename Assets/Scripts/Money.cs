using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Money : MonoBehaviour
{
    private PlayerPrefsManager playerPrefsManager;
    private Player player;
    private Worker worker;

    [SerializeField] private int moneyValue = 10;

    private void Start()
    {
        playerPrefsManager = PlayerPrefsManager.instance;
        worker = FindObjectOfType<Worker>(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            this.player = player;
            CollectMoney();
        }
    }

    private void CollectMoney()
    {
        Vector3 targetPoint = player.paperPoint.transform.position;

        gameObject.transform.DOMove(targetPoint, 0.2f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            playerPrefsManager.IncreaseMoney(moneyValue);
            Destroy(gameObject);
            worker.ResetMoneyStackHeight(); // yýðýn yüksekliðini sýfýrla, üstten üretmeye baþlamasýn.
        });
    }
}
