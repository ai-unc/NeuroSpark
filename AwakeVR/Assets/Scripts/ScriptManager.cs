using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// Uncomment if you have TextMeshPro installed
using TMPro;









public class StimulusManager : MonoBehaviour
{
    [Header("Prefab for the color word")]
    [SerializeField] private GameObject colorWordPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnDistance = 2f; // how far in front of the camera (or manager) to spawn
    [SerializeField] private Vector3 spawnOffset = new Vector3(0, 0, 0);

    // The set of color words we'll use
    private List<string> colorWords = new List<string> { "RED", "BLUE", "GREEN", "YELLOW" };

    // Map each color word to an actual Unity color
    private Dictionary<string, Color> colorMap = new Dictionary<string, Color>()
    {
        { "RED", Color.red },
        { "BLUE", Color.blue },
        { "GREEN", Color.green },
        { "YELLOW", Color.yellow }
    };

    // Reference to the currently spawned text object (if any)
    private GameObject currentStimulus;

    /// <summary>
    /// Spawns a new color word prefab, sets its text/color, and returns a StroopData object
    /// with info about the chosen text, color, and whether they match.
    /// </summary>
    public StroopData SetupTrial(int trialIndex)
    {
        // 1. Pick a random word from the list
        string chosenWord = colorWords[Random.Range(0, colorWords.Count)];

        // 2. Pick a random color word for the text color
        string chosenColorWord = colorWords[Random.Range(0, colorWords.Count)];

        // 3. Determine congruency
        bool isCongruent = (chosenWord == chosenColorWord);

        // 4. Instantiate the prefab in front of this manager or camera
        Transform camTransform = Camera.main.transform;
        Vector3 spawnPos = camTransform.position + camTransform.forward * spawnDistance + spawnOffset;
        currentStimulus = Instantiate(colorWordPrefab, spawnPos, Quaternion.identity);

        TextMeshPro tmp = currentStimulus.GetComponent<TextMeshPro>();
        if (tmp != null)
        {
            tmp.text = chosenWord;
            tmp.color = colorMap[chosenColorWord];
        }

        // 6. Build a StroopData object with the trial info
        StroopData data = new()
        {
            trialIndex = trialIndex,
            wordText = chosenWord,
            fontColorWord = chosenColorWord,
            isCongruent = isCongruent
        };

        // We'll fill out responded, reactionTime, isCorrect later in the TrialManager
        return data;
    }

    /// <summary>
    /// Destroys the currently spawned stimulus, if any.
    /// Call this after the trial ends, or before the next trial begins.
    /// </summary>
    public void Cleanup()
    {
        if (currentStimulus != null)
        {
            Destroy(currentStimulus);
            currentStimulus = null;
        }
    }
}
