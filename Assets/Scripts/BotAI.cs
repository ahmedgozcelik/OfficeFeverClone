using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class BotAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator botAnimator;

    public Transform paperPoint; // kaðýtlarý eli ile tuttuðu nokta
    public int collectedPaperCount = 0;
    public int botMaxPapers = 5;
    public List<GameObject> collectedPapers;
    public bool hasPaper = false;
    public bool isCarrying = false;

    private Worker[] workers;
    private Printer[] printers;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        botAnimator = GetComponent<Animator>();

        workers = FindObjectsOfType<Worker>(); 
        printers = FindObjectsOfType<Printer>(); 

        GoToBestPrinter();
    }

    private void Update()
    {
        UpdateAnimations();
        if (!hasPaper && agent.remainingDistance < 0.5f)
        {
            PickUpPaper();
        }
        else if (hasPaper && agent.remainingDistance < 0.5f)
        {
            DropOffPaper();
        }
    }

    private void GoToBestPrinter()
    {
        Printer bestPrinter = GetPrinterWithMostPapers();
        if (bestPrinter != null)
        {
            agent.SetDestination(bestPrinter.aiPaperPoint.position);
        }
    }

    private Printer GetPrinterWithMostPapers()
    {
        Printer bestPrinter = null;
        int maxPapers = 0;

        foreach (var printer in printers)
        {
            int paperCount = printer.GetProducedPaperCount();
            if (paperCount > maxPapers)
            {
                maxPapers = paperCount;
                bestPrinter = printer;
            }
        }

        return bestPrinter;
    }

    private void GoToBestWorker()
    {
        Worker bestWorker = GetWorkerWithLeastPapers();
        if (bestWorker != null)
        {
            agent.SetDestination(bestWorker.aiDropOffPoint.position);
        }
    }

    private Worker GetWorkerWithLeastPapers()
    {
        Worker bestWorker = null;
        int minPapers = int.MaxValue;

        foreach (var worker in workers)
        {
            int paperCount = worker.GetDroppedPaperCount();
            if (paperCount < minPapers)
            {
                minPapers = paperCount;
                bestWorker = worker;
            }
        }

        return bestWorker;
    }

    private void PickUpPaper()
    {
        hasPaper = true;
        GoToBestPrinter();
    }

    private void DropOffPaper()
    {
        hasPaper = false;
        GoToBestWorker();
    }


    private void UpdateAnimations()
    {
        if (isCarrying == true)
        {
            botAnimator.SetBool("IsRunning", false);
            botAnimator.SetBool("IsCarrying", true);
        }
        else
        {
            botAnimator.SetBool("IsCarrying", false);
            botAnimator.SetBool("IsRunning", true);
        }
    }
}
