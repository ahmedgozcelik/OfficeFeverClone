using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Management : MonoBehaviour
{
    UIManager uiManager;

    public Animator gateAnimator;

    bool isPlayerIn = false;

    private void Start()
    {
        uiManager = UIManager.instance;
    }
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
            Invoke("OnManagementPanel", 1.5f);
        }
        else
        {
            isPlayerIn = false;
            gateAnimator.Play("GateRotationOut");
        }
    }

    private void OnManagementPanel()
    {
        uiManager.OnManagementPanel();
    }
}
