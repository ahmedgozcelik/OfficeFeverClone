using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class Worker : MonoBehaviour
{
    private Player player;
    private BotAI bot;

    [SerializeField] private Animator workerAnimator;
    [SerializeField] private Transform paperPoint;
    [SerializeField] private GameObject moneyPrefab;
    [SerializeField] private List<Transform> moneyPoints = new List<Transform>();

    private List<GameObject> papersOnTable = new List<GameObject>();

    public Transform aiDropOffPoint;

    public int droppedPaperCount = 0; // býrakýlan kaðýt sayýsý
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
        else if (other.CompareTag("AI"))
        {
            bot = other.GetComponent<BotAI>();
            DropPapersFromBot();
            StartCoroutine(ProduceMoney());
        }
    }

    private void DropPapersFromPlayer()
    {
        player.isCarrying = false;
        int paperCount = player.collectedPapers.Count;

        for (int i = 0; i < paperCount; i++)
        {
            DropSinglePaperFromPlayer();
        }
        player.collectedPaperCount = 0;
    }

    private void DropSinglePaperFromPlayer()
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

    private void DropPapersFromBot()
    {
        bot.isCarrying = false;
        int paperCount = bot.collectedPapers.Count;

        for (int i = 0; i < paperCount; i++)
        {
            DropSinglePaperFromBot();
        }
        bot.collectedPaperCount = 0;
    }

    private void DropSinglePaperFromBot()
    {
        int lastIndex = bot.collectedPapers.Count - 1;
        var lastPaper = bot.collectedPapers[lastIndex];

        Vector3 targetPosition = paperPoint.position + new Vector3(0, droppedPaperCount * 0.1f, 0);
        lastPaper.transform.DOJump(targetPosition, 5f, 1, 1f).SetEase(Ease.OutQuad);
        lastPaper.transform.rotation = Quaternion.identity;
        lastPaper.transform.SetParent(transform);

        bot.collectedPapers.RemoveAt(lastIndex);
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

    public void ResetMoneyStackHeight()
    {
        moneyStackHeight = 0;
        currentMoneyIndex = 0;
    }

    public int GetDroppedPaperCount()
    {
        return droppedPaperCount;
    }
}
