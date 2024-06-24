using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class BotAI : MonoBehaviour
{
    private NavMeshAgent agent;

    public Transform paperLocation;
    public Transform dropOffLocation;

    private bool hasPaper = false;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!hasPaper && agent.remainingDistance < 0.5f)
        {
            PickUpPaper();
        }
        else if (hasPaper && agent.remainingDistance < 0.5f)
        {
            DropOffPaper();
        }
    }

    private void GoToPaper()
    {
        agent.SetDestination(paperLocation.position);
    }

    private void PickUpPaper()
    {

        hasPaper = true;
        GoToDropOff();
    }

    private void GoToDropOff()
    {
        agent.SetDestination(dropOffLocation.position);
    }

    private void DropOffPaper()
    {
        hasPaper = false;
        GoToPaper();
    }
}
