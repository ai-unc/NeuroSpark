using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class RandomPatternGenerator : MonoBehaviour
{
    private Keyboard keyboard;

    [Header("Settings")]
    public int patternLength = 5;
    public bool autoGenerateOnStart = true;

    public PatternSystem patternSystem;
    private GridController gridController;

    void Start()
    {   
        StartCoroutine(DelayedStart());

    }

    IEnumerator DelayedStart()
    {
        while (gridController == null || gridController.Grid == null)
        {
            yield return null;
        }
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.patternGenerator = this;
        }
        keyboard = Keyboard.current;
        patternSystem = GetComponent<PatternSystem>();
        gridController = GetComponent<GridController>();
        
        if (autoGenerateOnStart)
        {
            GenerateNewPattern();
        }
    }

    public void GenerateNewPattern()
    {
        List<Vector2Int> newPattern = GenerateValidPattern();
        ApplyPatternToSystem(newPattern);
        patternSystem.StartPattern();
    }

    List<Vector2Int> GenerateValidPattern()
    {
        List<Vector2Int> pattern = new List<Vector2Int>();
        Vector2Int currentPos = GetRandomStartPosition();
        pattern.Add(currentPos);

        for (int i = 1; i < patternLength; i++)
        {
            List<Vector2Int> validNextPositions = GetValidAdjacentPositions(currentPos, pattern);
            
            if (validNextPositions.Count == 0) break; // Safety check
            
            currentPos = validNextPositions[Random.Range(0, validNextPositions.Count)];
            pattern.Add(currentPos);
        }

        return pattern;
    }

    List<Vector2Int> GetValidAdjacentPositions(Vector2Int pos, List<Vector2Int> existingPattern)
    {
        List<Vector2Int> adjacentPositions = new List<Vector2Int>();
        
        // Only check 4 directions (no diagonals)
        Vector2Int[] directions = {
            new Vector2Int(1, 0),  // Right
            new Vector2Int(-1, 0), // Left
            new Vector2Int(0, 1),  // Up
            new Vector2Int(0, -1)  // Down
        };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int newPos = pos + dir;
            
            if (IsPositionValid(newPos) && 
                !existingPattern.Contains(newPos))
            {
                adjacentPositions.Add(newPos);
            }
        }

        return adjacentPositions;
    }

    bool IsPositionValid(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < 3 && pos.y >= 0 && pos.y < 3;
    }

    Vector2Int GetRandomStartPosition()
    {
        return new Vector2Int(Random.Range(0, 3), Random.Range(0, 3));
    }

    void ApplyPatternToSystem(List<Vector2Int> pattern)
    {
        patternSystem.pattern.sequence = new Vector2Int[pattern.Count];
        for (int i = 0; i < pattern.Count; i++)
        {
            patternSystem.pattern.sequence[i] = pattern[i];
        }
    }
}