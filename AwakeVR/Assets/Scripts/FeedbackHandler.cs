using UnityEngine;
using System.Collections;

public class FeedbackHandler : MonoBehaviour
{
    [Header("Feedback Settings")]
    [SerializeField] private Light feedbackLight;   // Assign the FeedbackLight child here
    [SerializeField] private float feedbackDuration = 1.0f;  // How long the light stays on

    // Call this method to show feedback. 'correct' true lights up green; false, red.
    public void ShowFeedback(bool correct)
    {
        if (feedbackLight == null)
        {
            Debug.LogWarning("FeedbackLight not assigned on " + gameObject.name);
            return;
        }
        
        // Set the light color based on result
        feedbackLight.color = correct ? Color.green : Color.red;
        feedbackLight.enabled = true;  // Turn the light on

        // Start a Coroutine to disable the light after the duration
       StartCoroutine(DisableFeedbackAfterDelay());
    }

    private IEnumerator DisableFeedbackAfterDelay()
    {
        yield return new WaitForSeconds(feedbackDuration);
        feedbackLight.enabled = false;
    }





}
