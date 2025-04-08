using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternSystem : MonoBehaviour
{
    public bool IsPlaying { get; private set;}

    [System.Serializable]
    public class Pattern
    {
        [Tooltip("Predefined sequence of cube positions")]
        public Vector2Int[] sequence;
        public Color patternColor = Color.cyan;
        public float stepDuration = 0.5f;
        public float finalPause = 1f;
    }

    [Header("Settings")]
    public Pattern pattern;
    public bool autoStart = true;

    public GridController gridController;
    private Coroutine activePattern;

    void Start()
    {
        gridController = GetComponent<GridController>();
        if (autoStart) StartPattern();
    }

    public void StartPattern()
    {
        if (activePattern != null) StopCoroutine(activePattern);
        activePattern = StartCoroutine(PlayPattern());
    }

    IEnumerator PlayPattern()
    {
        IsPlaying = true;
        List<Vector2Int> validatedPath = new List<Vector2Int>();
        
        // Validate adjacency
        for (int i = 0; i < pattern.sequence.Length; i++)
        {
            if (i == 0 || IsAdjacent(pattern.sequence[i-1], pattern.sequence[i]))
            {
                validatedPath.Add(pattern.sequence[i]);
            }
            else
            {
                Debug.LogError($"Invalid pattern at step {i}! Non-adjacent cubes.");
                yield break;
            }
        }

        // Visualize pattern
        foreach (Vector2Int pos in validatedPath)
        {
            if (IsValidGridPosition(pos))
            {
                GetCube(pos).LightUpPattern(pattern.patternColor);
                yield return new WaitForSeconds(pattern.stepDuration);
            }
        }

        yield return new WaitForSeconds(pattern.finalPause);
        gridController.ResetAllColors();
        IsPlaying = false;
    }

    bool IsAdjacent(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) <= 1 && 
               Mathf.Abs(a.y - b.y) <= 1 &&
               !(a.x == b.x && a.y == b.y); // Exclude same position
    }

    bool IsValidGridPosition(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < 3 && pos.y >= 0 && pos.y < 3;
    }

    CubeController GetCube(Vector2Int pos)
    {
        if (gridController == null || gridController.Grid == null)
        {
            Debug.LogError("GridController reference missing");
            return null;
        }

        if (pos.x < 0 || pos.x >= 3 || pos.y < 0 || pos.y >= 3)
        {
            Debug.LogError($"Invalid grid position: {pos}");
            return null;
        }

        return gridController.Grid[pos.x, pos.y]?.GetComponent<CubeController>();
    }
}