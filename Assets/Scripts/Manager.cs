using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Animator gateAnimator;

    bool isPlayerIn = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CheckGateAnimation();
        }
    }

    private void CheckGateAnimation()
    {
        if(!isPlayerIn)
        {
            isPlayerIn = true;
            gateAnimator.Play("GateRotationIn");
        }
        else
        {
            isPlayerIn = false;
            gateAnimator.Play("GateRotationOut");
        }
    }
}
