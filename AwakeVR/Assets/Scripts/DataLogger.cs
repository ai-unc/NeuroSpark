using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class DataLogger : MonoBehaviour
{
    // The list to store all trial data for JSON output
    private List<StroopData> trialDataList = new List<StroopData>();

    // Paths for CSV and JSON files
    private string csvFilePath;
    private string jsonFilePath;

    private void Awake()
    {
        // Choose how/where to save
        // Using Application.persistentDataPath so it works in builds
        // You might want to add timestamps or unique IDs to filenames if multiple sessions
        string timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        csvFilePath = Path.Combine(Application.persistentDataPath, $"StroopData_{timeStamp}.csv");
        jsonFilePath = Path.Combine(Application.persistentDataPath, $"StroopData_{timeStamp}.json");


        Debug.Log("CSV File Path: " + Application.persistentDataPath);



        // If CSV doesn't exist, write a header row
        if (!File.Exists(csvFilePath))
        {
            string header = "TrialIndex,IsCongruent,WordText,FontColorWord,Responded,ReactionTime,IsCorrect\n";
            File.WriteAllText(csvFilePath, header);
        }
    }

    /// <summary>
    /// LogTrial is called each time we finish a trial.
    /// Writes a CSV line immediately, and stores in our list for JSON export at the end.
    /// </summary>
    public void LogTrial(StroopData data)
    {
        // 1) Append to CSV


        string csvLine = string.Format(
            "{0},{1},{2},{3},{4},{5},{6}\n",
            data.trialIndex,
            data.isCongruent,
            data.wordText,
            data.fontColorWord,
            data.responded,
            data.reactionTime,
            data.isCorrect
        );
        File.AppendAllText(csvFilePath, csvLine);

        // 2) Store in the in-memory list for JSON
        trialDataList.Add(data);

        Debug.Log($"[DataLogger] Trial {data.trialIndex} logged to CSV and queued for JSON.");
    }

    /// <summary>
    /// When the application or scene ends, we write out a single JSON array of all trials.
    /// Typically runs when the object is destroyed or the scene closes.
    /// </summary>
    private void OnDestroy()
    {
        WriteJsonFile();
    }

    // Alternatively, you could use OnDisable or call a "FinishSession()" method explicitly.
    // private void OnDisable() { WriteJsonFile(); }

    /// <summary>
    /// Writes the entire trialDataList to a JSON file as an array.
    /// </summary>
    private void WriteJsonFile()
    {
        if (trialDataList.Count == 0)
        {
            Debug.LogWarning("[DataLogger] No trials to write to JSON.");
            return;
        }

        // Convert the entire list to JSON array
        // Example: [{"trialIndex":0, "wordText":"RED", ...}, {"trialIndex":1, ...}]
        string json = JSONHelper.ToJson(trialDataList.ToArray(), true);

        // Write it out to file
        File.WriteAllText(jsonFilePath, json);

        Debug.Log($"[DataLogger] Wrote {trialDataList.Count} trials to JSON at: {jsonFilePath}");
    }
}






