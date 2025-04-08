using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridController : MonoBehaviour
{
    public GameObject[,] Grid {get; private set;}
    [Header("Positioning Options")]
    public bool useFixedPosition = true;
    public Vector3 fixedPosition = new Vector3(0, 1.5f, 2.5f);
    public bool maintainRotation = false;

    public GameObject cubePrefab;
    public float spacing = 1.2f;
    // public GameObject[,] grid = new GameObject[3,3];
    private Vector2Int currentPosition = new Vector2Int(0,0);
    private Color originalColor;
    private Keyboard keyboard;
    private RandomPatternGenerator randomPatternGenerator;

    void Awake()
    {
        if (useFixedPosition)
        {
            transform.position = fixedPosition;
        }
        
        Grid = new GameObject[3, 3];
        CreateGrid();

        
        if (!maintainRotation) transform.rotation = Quaternion.identity;
        
        randomPatternGenerator = GetComponent<RandomPatternGenerator>();
        originalColor = cubePrefab.GetComponent<Renderer>().sharedMaterial.color;
    }

    void Start()
    {   
        UpdateHighlight();
        keyboard = Keyboard.current;
    }

    void CreateGrid()
    {
        Grid = new GameObject[3, 3];

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                float xPos = (x-1) * spacing;
                float yPos = (y-1) * spacing;

                Vector3 position = new Vector3(xPos, yPos, 0);
                GameObject cube = Instantiate(cubePrefab, transform);
                cube.transform.localPosition = position;
                cube.name = $"Cube_{x}_{y}";
                Grid[x, y] = cube;
            }
        }
    }

    void Update()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Selection)
        {
            HandleInput();
        }
    }

    void HandleInput()
    {
        Vector2Int newPosition = currentPosition;
        if (keyboard.nKey.wasPressedThisFrame)        {
            // call GenerateNewPattern from RandomPatternGenerator.cs
            randomPatternGenerator.GenerateNewPattern();
        }
        if (keyboard.rKey.wasPressedThisFrame)        {
            // call RepeatPattern from GameManager.cs
            GameManager.Instance.RepeatPattern();
        }

        if (keyboard.leftArrowKey.wasPressedThisFrame) newPosition.x--;
        if (keyboard.rightArrowKey.wasPressedThisFrame) newPosition.x++;
        if (keyboard.upArrowKey.wasPressedThisFrame) newPosition.y++;
        if (keyboard.downArrowKey.wasPressedThisFrame) newPosition.y--;

        newPosition.x = Mathf.Clamp(newPosition.x, 0, 2);
        newPosition.y = Mathf.Clamp(newPosition.y, 0, 2);

        if (newPosition != currentPosition)
        {
            currentPosition = newPosition;
            UpdateHighlight();
        }
        bool pressed = false;
        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            ToggleSelection();
        }
    }

    void UpdateHighlight()
    {
        foreach (GameObject cube in Grid)
        {
            CubeController controller = cube.GetComponent<CubeController>();
            controller.ResetColor();
        }

        GetCurrentCubeController().Highlight();
    }

    void ToggleSelection()
    {
        CubeController controller = GetCurrentCubeController();
        controller.ToggleSelection();
    }

    CubeController GetCurrentCubeController()
    {
        return Grid[currentPosition.x, currentPosition.y].GetComponent<CubeController>();
    }

    public void ResetAllColors()
    {
        foreach (GameObject cube in Grid)
        {
            CubeController controller = cube.GetComponent<CubeController>();
            controller.ResetColor();
        }
    }

    public void SetInputEnabled(bool enabled)
    {
        this.enabled = enabled;
    }

    public void ResetToDefaultPosition()
    {
        currentPosition = new Vector2Int(1, 1);
        UpdateHighlight();
    }
}
