using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Money : MonoBehaviour
{
    PlayerPrefsManager playerPrefsManager;
    Player player;

    [SerializeField] private int moneyValue = 10;
    private void Start()
    {
        playerPrefsManager = PlayerPrefsManager.instance;
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
        Vector3 targetPoint = player.paperPoint.transform.localPosition;

        gameObject.transform.SetParent(player.paperPoint);
        gameObject.transform.DOLocalMove(targetPoint, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            playerPrefsManager.IncreaseMoney(moneyValue);
            Destroy(gameObject);
        });
    }
}
