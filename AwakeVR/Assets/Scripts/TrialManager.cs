



// using UnityEngine;
// using System.Collections;

// public class TrialManager : MonoBehaviour
// {
//     [Header("References")]
//     [SerializeField] private GameObject fixationCross;      // Optional fixation cross
//     [SerializeField] private StimulusManager stimulusManager; 
//     [SerializeField] private DataLogger dataLogger;         // DataLogger to record trial data

//     [Header("Task Settings")]
//     [SerializeField] private int totalTrials = 10;
//     [SerializeField] private float fixationDuration = 2.0f;
//     [SerializeField] private float stimulusDuration = 20.0f;
//     [SerializeField] private float feedbackDuration = 1.0f;

//     // For testing with keyboard (placeholder; VR input will call UserChoseColor)
//     private KeyCode testResponseKey = KeyCode.Space;

//     // Variables to track the current trial's state
//     private bool responded = false;
//     private float trialStartTime = 0f;
//     private StroopData currentTrialData;

//     private void Start()
//     {
//         StartCoroutine(RunTrials());
//     }

//     private IEnumerator RunTrials()
//     {
//         for (int trialIndex = 0; trialIndex < totalTrials; trialIndex++)
//         {
//             // 1. Fixation Phase
//             if (fixationCross != null)
//                 fixationCross.SetActive(true);

//             yield return new WaitForSeconds(fixationDuration);

//             if (fixationCross != null)
//                 fixationCross.SetActive(false);

//             // 2. Request a new stimulus from the StimulusManager
//             currentTrialData = stimulusManager.SetupTrial(trialIndex);

//             // 3. Wait for user input (via VR interaction) or until stimulusDuration expires
//             responded = false;
//             trialStartTime = Time.time;
//             float reactionTime = 0f;

//             // Wait until the user responds (via UserChoseColor) or timeout
//             while (!responded && (Time.time - trialStartTime < stimulusDuration))
//             {
//                 yield return null;
//             }

//             // If time expires without a response, record a timeout.
//             if (!responded)
//             {
//                 reactionTime = stimulusDuration;
//                 currentTrialData.responded = false;
//                 currentTrialData.reactionTime = reactionTime;
//                 currentTrialData.isCorrect = false;
//                 currentTrialData.timedOut = true;
//                 currentTrialData.userChosenColor = ""; // or "No Response"
//                 Debug.Log($"Trial {trialIndex} timed out.");
//             }

//             // 4. Feedback (simple console message)
//             if (currentTrialData.isCorrect)
//                 Debug.Log($"Trial {trialIndex} Feedback: Correct!");
//             else
//                 Debug.Log($"Trial {trialIndex} Feedback: Incorrect!");

//             yield return new WaitForSeconds(feedbackDuration);

//             // 5. Clean up the stimulus
//             stimulusManager.Cleanup();

//             // 6. Log data (if using DataLogger)
//             if (dataLogger != null)
//             {
//                 dataLogger.LogTrial(currentTrialData);
//             }
//         }

//         Debug.Log("All trials finished!");
//     }

//     /// <summary>
//     /// This method is called by the VR color choice objects (via their ColorChoiceHandler) when the user selects a color.
//     /// It records the reaction time and determines if the choice was correct.
//     /// </summary>
//     /// <param name="chosenColor">The color chosen by the user (e.g., "RED")</param>
//     public void UserChoseColor(string chosenColor)
//     {
//         if (!responded)
//         {
//             responded = true;
//             float reactionTime = Time.time - trialStartTime;

//             // The correct answer is the font color defined in the current trial data.
//             bool correct = (chosenColor.ToUpper() == currentTrialData.fontColorWord.ToUpper());

//             currentTrialData.responded = true;
//             currentTrialData.reactionTime = reactionTime;
//             currentTrialData.isCorrect = correct;
//             currentTrialData.timedOut = false;
//             currentTrialData.userChosenColor = chosenColor;

//             Debug.Log($"User chose {chosenColor}. Correct answer: {currentTrialData.fontColorWord}. Reaction time: {reactionTime:F2} sec. " + (correct ? "Correct!" : "Incorrect!"));
//         }
//     }
// }




using UnityEngine;
using System.Collections;

public class TrialManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject fixationCross;      // Optional fixation cross
    [SerializeField] private StimulusManager stimulusManager; 
    [SerializeField] private DataLogger dataLogger;         // DataLogger to record trial data

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

    // New: store reference to the last selected cube for feedback
    private GameObject lastSelectedCube;

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

            // Reset response tracking and record the start time
            responded = false;
            trialStartTime = Time.time;
            lastSelectedCube = null;  // Reset the reference

            // 3. Wait for user input (via VR interaction) or until stimulusDuration expires
            while (!responded && (Time.time - trialStartTime < stimulusDuration))
            {
                yield return null;
            }

            // If time expires without a response, record a timeout.
            if (!responded)
            {
                currentTrialData.responded = false;
                currentTrialData.reactionTime = stimulusDuration;
                currentTrialData.isCorrect = false;
                currentTrialData.timedOut = true;
                currentTrialData.userChosenColor = ""; // or "No Response"
                Debug.Log($"Trial {trialIndex} timed out.");
            }

            // 4. Provide feedback by lighting up the selected cube
            // Here we use the lastSelectedCube reference set by UserChoseColor().
            if (lastSelectedCube != null)
            {
                FeedbackHandler feedbackHandler = lastSelectedCube.GetComponent<FeedbackHandler>();
                if (feedbackHandler != null)
                {
                    feedbackHandler.ShowFeedback(currentTrialData.isCorrect);
                }
                else
                {
                    Debug.LogWarning("FeedbackHandler not found on " + lastSelectedCube.name);
                }
            }
            else
            {
                Debug.LogWarning("No cube was selected during Trial " + trialIndex);
            }

            // Optionally, wait for feedbackDuration to let the visual feedback remain visible
            yield return new WaitForSeconds(feedbackDuration);

            // 5. Clean up the stimulus
            stimulusManager.Cleanup();

            // 6. Log data (if using DataLogger)
            if (dataLogger != null)
            {
                dataLogger.LogTrial(currentTrialData);
            }
        }

        Debug.Log("All trials finished!");
    }

    /// <summary>
    /// This method is called by the VR color choice objects (via their ColorChoiceHandler) when the user selects a color.
    /// It records the reaction time, the selected cube (for feedback), and determines if the choice was correct.
    /// </summary>
    /// <param name="chosenColor">The color chosen by the user (e.g., "RED")</param>
    /// <param name="selectedCube">The GameObject (the cube) that was selected.</param>
    public void UserChoseColor(string chosenColor, GameObject selectedCube)
    {
        if (!responded)
        {
            responded = true;
            float reactionTime = Time.time - trialStartTime;

            bool correct = (chosenColor.ToUpper() == currentTrialData.fontColorWord.ToUpper());

            currentTrialData.responded = true;
            currentTrialData.reactionTime = reactionTime;
            currentTrialData.isCorrect = correct;
            currentTrialData.timedOut = false;
            currentTrialData.userChosenColor = chosenColor;

            lastSelectedCube = selectedCube;
            Debug.Log($"User chose {chosenColor}. Correct answer: {currentTrialData.fontColorWord}. Reaction time: {reactionTime:F2} sec. " +
                    (correct ? "Correct!" : "Incorrect!"));
            Debug.Log("Last selected cube is: " + (lastSelectedCube != null ? lastSelectedCube.name : "null"));
        }
    }

}
