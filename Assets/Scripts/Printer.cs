using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class Printer : MonoBehaviour
{
    [SerializeField] private GameObject paperPrefab;
    [SerializeField] private Transform[] paperPoints;
    [SerializeField] private Transform printerExitPoint;
    [SerializeField] private Transform carryingPoint;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private float paperInterval = 0.5f; // Kaðýt üretme hýzý -> saniye
    [SerializeField] private float paperHeight = 0.05f; // Kaðýtlar arasý yükseklik

    public List<GameObject> paperPool;
    private int paperCount = 0;

    private void Awake()
    {
        InitializePool();
    }

    private void Start()
    {
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

    private void ReturnPaperToPool(GameObject paper)
    {
        paper.SetActive(false);
    }

    private IEnumerator ProducePapers()
    {
        while (true)
        {
            yield return new WaitForSeconds(paperInterval);
            ProducePaper();
        }
    }

    private void ProducePaper()
    {
        GameObject newPaper = GetPaperFromPool();
        newPaper.transform.position = printerExitPoint.position;
        newPaper.transform.localScale = paperPrefab.transform.localScale * 3;

        Transform targetPoint = paperPoints[paperCount % paperPoints.Length];
        Vector3 targetPosition = targetPoint.position + new Vector3(0, (paperCount / paperPoints.Length) * paperHeight, 0);

        newPaper.transform.DOMove(targetPosition, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            // Gerekirse buraya kaðýt hedef konumuna ulaþtýðýnda yapýlacak iþlemleri ekleyin
        });

        paperCount++;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerMovement>();
            player.isCarrying = true;
            StartCoroutine(CollectPapers());
        }
    }

    private IEnumerator CollectPapers()
    {
        List<GameObject> papersToCollect = new List<GameObject>();

        foreach (var paper in paperPool)
        {
            if (paper.activeInHierarchy)
            {
                papersToCollect.Add(paper);
            }
        }

        int collectedPaperCount = 0;

        foreach (var paper in papersToCollect)
        {
            Vector3 targetPosition = carryingPoint.position + new Vector3(0, collectedPaperCount * paperHeight, 0);
            paper.transform.DOJump(targetPosition, 2f, 1, 0.5f).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                // Kaðýdý taþýma noktasýna (carryingPoint) sabitle ve düzenli bir þekil almasýný saðla
                paper.transform.SetParent(carryingPoint);
                paper.transform.localPosition = new Vector3(0, collectedPaperCount * paperHeight, 0);
            });
            collectedPaperCount++;

            yield return new WaitForSeconds(0.1f); // Kaðýtlar arasýnda küçük bir gecikme ekleyerek sýralý toplama efekti
        }
    }





}
