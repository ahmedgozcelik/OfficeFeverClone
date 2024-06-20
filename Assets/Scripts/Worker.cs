using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class Worker : MonoBehaviour
{
    private Player player;

    [SerializeField] private Animator workerAnimator;
    [SerializeField] private Transform paperPoint;
    [SerializeField] private GameObject moneyPrefab;
    [SerializeField] private List<Transform> moneyPoints = new List<Transform>();

    private List<GameObject> papersOnTable = new List<GameObject>();

    private int droppedPaperCount = 0; // b�rak�lan ka��t say�s�
    private int currentMoneyIndex = 0;
    private int moneyStackHeight = 0;
    private const float MoneyHeight = 0.2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<Player>();
            DropPapersFromPlayer();
            StartCoroutine(ProduceMoney());
        }
    }

    private void DropPapersFromPlayer()
    {
        player.isCarrying = false;
        int paperCount = player.collectedPapers.Count;

        for (int i = 0; i < paperCount; i++)
        {
            DropSinglePaper();
        }
        player.collectedPaperCount = 0;
    }

    private void DropSinglePaper()
    {
        int lastIndex = player.collectedPapers.Count - 1;
        var lastPaper = player.collectedPapers[lastIndex];

        Vector3 targetPosition = paperPoint.position + new Vector3(0, droppedPaperCount * 0.1f, 0);
        lastPaper.transform.DOJump(targetPosition, 5f, 1, 1f).SetEase(Ease.OutQuad);
        lastPaper.transform.rotation = Quaternion.identity;
        lastPaper.transform.SetParent(transform);

        player.collectedPapers.RemoveAt(lastIndex);
        droppedPaperCount++;
        papersOnTable.Add(lastPaper);
    }

    private IEnumerator ProduceMoney()
    {
        while (droppedPaperCount > 0)
        {
            yield return new WaitForSeconds(1f);

            ProduceSingleMoney();

            droppedPaperCount--;
            HideAndRemoveLastPaper();
        }
    }

    private void ProduceSingleMoney()
    {
        Transform targetPoint = moneyPoints[currentMoneyIndex];
        Vector3 targetPosition = targetPoint.position + new Vector3(0, moneyStackHeight * MoneyHeight, 0);

        GameObject producedMoney = Instantiate(moneyPrefab, targetPosition, Quaternion.identity);
        producedMoney.transform.rotation = targetPoint.localRotation;

        currentMoneyIndex++;
        if (currentMoneyIndex >= moneyPoints.Count)
        {
            moneyStackHeight++;
            currentMoneyIndex = 0;
        }
    }

    private void HideAndRemoveLastPaper()
    {
        var lastPaper = papersOnTable[papersOnTable.Count - 1];
        lastPaper.SetActive(false);
        papersOnTable.RemoveAt(papersOnTable.Count - 1);
    }
}
