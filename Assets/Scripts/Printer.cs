using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class Printer : MonoBehaviour
{
    [SerializeField] private GameObject paperPrefab;
    [SerializeField] private Transform[] paperPoints;
    [SerializeField] private Transform printerExitPoint;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private float paperInterval = 0.5f; // Ka��t �retme h�z� -> saniye
    [SerializeField] private float paperHeight = 0.05f; // Ka��tlar aras� y�kseklik

    private List<GameObject> paperPool;
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
            // Gerekirse buraya ka��t hedef konumuna ula�t���nda yap�lacak i�lemleri ekleyin
        });

        paperCount++;
    }
}
