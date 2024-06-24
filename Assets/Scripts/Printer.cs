using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class Printer : MonoBehaviour
{
    [SerializeField] private GameObject paperPrefab;
    [SerializeField] private List<Transform> paperPoints;
    [SerializeField] private Transform printerExitPoint;

    [SerializeField] private int poolSize = 10;
    [SerializeField] private float paperInterval = 0.5f;
    [SerializeField] private float paperHeight = 0.02f;


    private Player player;
    private bool isPlayerIn;

    private BotAI botAI;
    public Transform aiPaperPoint;
    private bool isBotIn;

    private List<GameObject> paperPool;
    private List<GameObject> producedPapers;
    public int producedPaperCount = 0; // �retilen ka��t say�s�
    private int instantiatedObj = 1;
    private int currentIndex = 0;

    private void Awake()
    {
        InitializePool();
    }

    private void Start()
    {
        producedPapers = new List<GameObject>();
        StartCoroutine(ProducePapers());
    }

    private void InitializePool()
    {
        paperPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject paper = Instantiate(paperPrefab);
            paper.SetActive(false);
            paperPool.Add(paper);
        }
    }

    private GameObject GetPaperFromPool()
    {
        foreach (var paper in paperPool)
        {
            if (!paper.activeInHierarchy)
            {
                paper.SetActive(true);
                return paper;
            }
        }

        GameObject newPaper = Instantiate(paperPrefab);
        paperPool.Add(newPaper);
        return newPaper;
    }

    private IEnumerator ProducePapers()
    {
        while (true)
        {
            yield return new WaitForSeconds(paperInterval);

            if (isPlayerIn && player.collectedPaperCount < player.maxPapers)
            {
                CollectPaperForPlayer();
            }
            else if (isBotIn && botAI != null && !botAI.isCarrying)
            {
                CollectPaperForBot();
            }
            else
            {
                ProducePaper();
            }
        }
    }

    private void ProducePaper()
    {
        GameObject newPaper = GetPaperFromPool();
        producedPapers.Add(newPaper);
        newPaper.transform.position = printerExitPoint.position;
        newPaper.transform.localScale = paperPrefab.transform.localScale * 3;

        Transform targetPoint = paperPoints[currentIndex];
        Vector3 targetPosition = targetPoint.position + new Vector3(0, (instantiatedObj / paperPoints.Count) * paperHeight, 0);

        newPaper.transform.DOMove(targetPosition, 0.25f);
        instantiatedObj++;
        currentIndex = (currentIndex + 1) % paperPoints.Count;

        producedPaperCount++; // �retilen ka��t say�s�n� art�r
    }

    private void CollectPaperForPlayer()
    {
        var paperPoint = player.paperPoint.localPosition;
        GameObject newPaper;

        if (producedPapers.Count == 0)
        {
            newPaper = GetPaperFromPool();
            newPaper.transform.position = printerExitPoint.position;
            newPaper.transform.localScale = paperPrefab.transform.localScale * 3;
        }
        else
        {
            newPaper = producedPapers[producedPapers.Count - 1];
            producedPapers.RemoveAt(producedPapers.Count - 1);
        }

        newPaper.transform.SetParent(player.transform);
        newPaper.transform.DOLocalJump(paperPoint + new Vector3(0, player.collectedPaperCount * 0.1f, 0), 5f, 1, 1f).SetEase(Ease.OutQuad);
        player.collectedPapers.Add(newPaper);
        player.collectedPaperCount++;
        //// Remove the paper from other lists
        //paperPool.Remove(newPaper);
    }

    private void CollectPaperForBot()
    {
        var paperPoint = botAI.paperPoint.localPosition;
        GameObject newPaper;

        if (producedPapers.Count == 0)
        {
            newPaper = GetPaperFromPool();
            newPaper.transform.position = printerExitPoint.position;
            newPaper.transform.localScale = paperPrefab.transform.localScale * 3;
        }
        else
        {
            newPaper = producedPapers[producedPapers.Count - 1];
            producedPapers.RemoveAt(producedPapers.Count - 1);
        }

        newPaper.transform.SetParent(botAI.transform);
        newPaper.transform.DOLocalJump(paperPoint + new Vector3(0, botAI.collectedPaperCount * 0.1f, 0), 5f, 1, 1f).SetEase(Ease.OutQuad);
        botAI.collectedPapers.Add(newPaper);
        botAI.collectedPaperCount++;
        botAI.hasPaper = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerIn = true;
            player = other.GetComponent<Player>();
            player.isCarrying = true;

            var paperPoint = player.paperPoint.localPosition;
            for (int i = producedPapers.Count - 1; i >= 0; i--)
            {
                if (player.collectedPaperCount < player.maxPapers)
                {
                    var paper = producedPapers[i];
                    paper.transform.SetParent(player.transform);
                    paper.transform.DOLocalJump(paperPoint + new Vector3(0, player.collectedPaperCount * 0.1f, 0), 5f, 1, 1f).SetEase(Ease.OutQuad);
                    player.collectedPapers.Add(paper);
                    player.collectedPaperCount++;

                    producedPapers.RemoveAt(i);
                    paperPool.Remove(paper);
                }
            }
        }
        else if (other.CompareTag("AI"))
        {
            isBotIn = true;
            botAI = other.GetComponent<BotAI>();
            botAI.isCarrying = true;

            var paperPoint = botAI.paperPoint.localPosition;
            for (int i = producedPapers.Count - 1; i >= 0; i--)
            {
                if (botAI.collectedPaperCount < botAI.botMaxPapers)
                {
                    var paper = producedPapers[i];
                    paper.transform.SetParent(botAI.transform);
                    paper.transform.DOLocalJump(paperPoint + new Vector3(0, botAI.collectedPaperCount * 0.1f, 0), 5f, 1, 1f).SetEase(Ease.OutQuad);
                    botAI.collectedPapers.Add(paper);
                    botAI.collectedPaperCount++;

                    producedPapers.RemoveAt(i);
                    paperPool.Remove(paper);
                    botAI.hasPaper = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerIn = false;
        }
        else if (other.CompareTag("AI"))
        {
            isBotIn = false;
            botAI.hasPaper = false;
        }
    }

    public int GetProducedPaperCount()
    {
        return producedPaperCount;
    }
}
