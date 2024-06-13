using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Printer : MonoBehaviour
{
    [SerializeField] GameObject paperPrefab;
    [SerializeField] Transform[] paperPoints;
    [SerializeField] Transform printerExitPoint;

    public float paperInterval = 0.5f; //Kağıt üretme hızı -> saniye
    public float paperHeight = 0.05f; //Kağıtlar arası yükseklik

    private int paperCount = 0;

    private void Start()
    {
        StartCoroutine(ProducePapers());
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
        GameObject newPaper = Instantiate(paperPrefab, printerExitPoint.position, Quaternion.identity);
        newPaper.transform.localScale = (paperPrefab.transform.localScale) * 3;

        Transform targetPoint = paperPoints[paperCount % paperPoints.Length];
        Vector3 targetPosition = targetPoint.position + new Vector3(0, (paperCount / paperPoints.Length) * paperHeight, 0);

        newPaper.transform.DOMove(targetPosition, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
        {

        });
        paperCount++;
    }
}
