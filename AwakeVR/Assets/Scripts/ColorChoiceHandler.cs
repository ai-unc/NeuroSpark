using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ColorChoiceHandler : MonoBehaviour
{

    public String colorLabel;
    public TrialManager trialManager;

    // Start is called before the first frame update
    void Start()
    {
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        if (interactable != null){
            interactable.selectEntered.AddListener(OnSelectEntered);

        }

        
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log($"{gameObject.name} OnSelectEntered triggered!");
        if (trialManager != null)
        {
            trialManager.UserChoseColor(colorLabel);
        }
        else
        {
            Debug.LogWarning("TrialManager reference is null on " + gameObject.name);
        }
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
