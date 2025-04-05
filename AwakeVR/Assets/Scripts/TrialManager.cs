using UnityEngine;
using System.Collections;

public class TrialManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject fixationCross;      // Optional fixation cross
    [SerializeField] private StimulusManager stimulusManager; 
    // [SerializeField] private DataLogger dataLogger;       // Uncomment if you have a DataLogger script

    [Header("Task Settings")]
    [SerializeField] private int totalTrials = 10;
    [SerializeField] private float fixationDuration = 2.0f;
    [SerializeField] private float stimulusDuration = 20.0f;
    [SerializeField] private float feedbackDuration = 1.0f;

    // For testing with keyboard (placeholder; VR input will call UserChoseColor)
    private KeyCode testResponseKey = KeyCode.Space;

    // Variables to track the current trial's state
    private bool responded = false;
    private float trialStartTime = 0f;
    private StroopData currentTrialData;

    private void Start()
    {
        StartCoroutine(RunTrials());
    }

 private IEnumerator RunTrials()
{
    for (int trialIndex = 0; trialIndex < totalTrials; trialIndex++)
    {
        // 1. Fixation Phase
        if (fixationCross != null)
            fixationCross.SetActive(true);

        yield return new WaitForSeconds(fixationDuration);

        if (fixationCross != null)
            fixationCross.SetActive(false);

        // 2. Request a new stimulus from the StimulusManager
        currentTrialData = stimulusManager.SetupTrial(trialIndex);

        // 3. Wait for user input (via VR interaction) or until stimulusDuration expires
        responded = false;
        trialStartTime = Time.time;
        float reactionTime = 0f;

        // Wait until the user responds (which should set 'responded' via UserChoseColor) or timeout
        while (!responded && (Time.time - trialStartTime < stimulusDuration))
        {
            yield return null;
        }

        // If time expires without a response, record a timeout.
        if (!responded)
        {
            reactionTime = stimulusDuration;
            currentTrialData.responded = false;
            currentTrialData.reactionTime = reactionTime;
            currentTrialData.isCorrect = false;
            Debug.Log($"Trial {trialIndex} timed out.");
        }

        // 4. Feedback (simple console message)
        if (currentTrialData.isCorrect)
            Debug.Log($"Trial {trialIndex} Feedback: Correct!");
        else
            Debug.Log($"Trial {trialIndex} Feedback: Incorrect!");

        yield return new WaitForSeconds(feedbackDuration);

        // 5. Clean up the stimulus
        stimulusManager.Cleanup();

        // 6. Log data (if using DataLogger)
        // if (dataLogger != null)
        // {
        //     dataLogger.LogTrial(currentTrialData);
        // }
    }

    Debug.Log("All trials finished!");
}


    /// <summary>
    /// This method is called by the VR color choice objects (via their ColorChoiceHandler) when the user selects a color.
    /// It records the reaction time and determines if the choice was correct.
    /// </summary>
    /// <param name="chosenColor">The color chosen by the user (e.g., "RED")</param>
    public void UserChoseColor(string chosenColor)
    {
        if (!responded)
        {
            responded = true;
            float reactionTime = Time.time - trialStartTime;

            // The correct answer is the font color as defined in the current trial data.
            bool correct = (chosenColor == currentTrialData.fontColorWord);

            currentTrialData.responded = true;
            currentTrialData.reactionTime = reactionTime;
            currentTrialData.isCorrect = correct;

            Debug.Log($"User chose {chosenColor}. Correct answer: {currentTrialData.fontColorWord}. Reaction time: {reactionTime:F2} sec. " + (correct ? "Correct!" : "Incorrect!"));
        }
    }
}
