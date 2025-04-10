// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.XR.Interaction.Toolkit;

// public class ColorChoiceHandler : MonoBehaviour
// {

//     public String colorLabel;
//     public TrialManager trialManager;

//     // Start is called before the first frame update
//     void Start()
//     {
//         XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
//         if (interactable != null){
//             interactable.selectEntered.AddListener(OnSelectEntered);
//         Invoke("TestFeedback", 2f);

//         }

        
//     }

//     private void OnSelectEntered(SelectEnterEventArgs args)
//     {
//         Debug.Log($"{gameObject.name} OnSelectEntered triggered!");
//         if (trialManager != null)
//         {
//             trialManager.UserChoseColor(colorLabel,gameObject);
//         }
//         else
//         {
//             Debug.LogWarning("TrialManager reference is null on " + gameObject.name);
//         }
//     }


//     private void TestFeedback() {
//         GetComponent<FeedbackHandler>()?.ShowFeedback(true);
//     }

//     // Update is called once per frame
//     // void Update()
//     // {
        
//     // }
// }


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ColorChoiceHandler : MonoBehaviour
{
    public string colorLabel;
    public TrialManager trialManager;

    private void Start()
    {
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnSelectEntered);
        }
        
        // Add a test feedback invocation after 2 seconds:
        Invoke("TestFeedback", 2.0f);
    }

    private void TestFeedback()
    {
        Debug.Log("TestFeedback invoked on " + gameObject.name);
        // Attempt to get the FeedbackHandler and show positive feedback.
        FeedbackHandler fb = GetComponent<FeedbackHandler>();
        if (fb != null)
        {
            fb.ShowFeedback(true); // Should light up green
        }
        else
        {
            Debug.LogWarning("No FeedbackHandler found on " + gameObject.name);
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log($"{gameObject.name} OnSelectEntered triggered!");
        if (trialManager != null)
        {
            trialManager.UserChoseColor(colorLabel, gameObject);
        }
        else
        {
            Debug.LogWarning("TrialManager reference is null on " + gameObject.name);
        }
    }
}
