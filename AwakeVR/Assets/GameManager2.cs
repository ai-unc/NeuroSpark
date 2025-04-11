using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public enum GameState { PatternDisplay, Selection }
    public GameState CurrentState { get; private set; }

    [Header("References")]
    public GridController gridController;
    public PatternSystem patternSystem;
    public RandomPatternGenerator patternGenerator;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        StartNewRound();
    }

    public void StartNewRound()
    {
        StartCoroutine(NewRoundRoutine());
    }

    IEnumerator NewRoundRoutine()
    {
        // Pattern Display Phase
        CurrentState = GameState.PatternDisplay;
        gridController.SetInputEnabled(false);
        patternGenerator.GenerateNewPattern();
        
        // Wait for pattern to complete
        yield return new WaitWhile(() => patternSystem.IsPlaying);
        
        // Selection Phase
        CurrentState = GameState.Selection;
        gridController.SetInputEnabled(true);
        gridController.ResetToDefaultPosition();
    }

    public void ResetSelections()
    {
        foreach (GameObject cube in gridController.Grid)
        {
            if (cube.GetComponent<CubeController>().isSelected)
            {
                cube.GetComponent<CubeController>().ToggleSelection();
            }
        }
    }

    public void RepeatPattern()
    {
        StartCoroutine(RepeatPatternRoutine());
    }

    IEnumerator RepeatPatternRoutine()
    {
        CurrentState = GameState.PatternDisplay;
        gridController.SetInputEnabled(false);
        patternSystem.StartPattern();
        
        yield return new WaitWhile(() => patternSystem.IsPlaying);
        
        CurrentState = GameState.Selection;
        gridController.SetInputEnabled(true);
        gridController.ResetToDefaultPosition();
    }

}