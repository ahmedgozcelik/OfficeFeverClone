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

    private int droppedPaperCount = 0; //býrakýlan kaðýt sayýsý
    private int currentIndex = 0;
    private int producedMoneyCount = 0;
    private int a = 0;
    private float moneyHeight = 0.2f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<Player>();
            DropThePapers();
            StartCoroutine(ProduceMoney());
        }
    }

    private void DropThePapers()
    {
        player.isCarrying = false;
        int x = player.collectedPapers.Count;

        for (int i = 0; i < x; i++)
        {
            int indexCount = player.collectedPapers.Count;
            var lastPaper = player.collectedPapers[indexCount - 1];

            lastPaper.transform.DOJump(paperPoint.transform.position + new Vector3(0, droppedPaperCount * 0.1f, 0), 5f, 1, 1f).SetEase(Ease.OutQuad);
            lastPaper.transform.rotation = Quaternion.identity;
            lastPaper.transform.SetParent(gameObject.transform);

            player.collectedPapers.RemoveAt(indexCount - 1);
            droppedPaperCount++;
        }
        player.collectedPaperCount = 0;
    }

    private IEnumerator ProduceMoney()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            Transform targetPoint = moneyPoints[currentIndex];
            Vector3 targetPosition = targetPoint.position + new Vector3(0, a * moneyHeight, 0);

            GameObject producedMoney = Instantiate(moneyPrefab, targetPosition, Quaternion.identity);
            producedMoney.transform.rotation = targetPoint.localRotation;

            currentIndex++;
            producedMoneyCount++;
            droppedPaperCount--;

            if(currentIndex >= moneyPoints.Count)
            {
                a++;
                currentIndex = 0;
            }

            if(droppedPaperCount == 0)
            {
                break;
            }
        }
    }
}
