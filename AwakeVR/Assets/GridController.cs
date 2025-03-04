using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridController : MonoBehaviour
{
    public GameObject cubePrefab;
    public float spacing = 1.2f;
    private GameObject[,] grid = new GameObject[3,3];
    private Vector2Int currentPosition = new Vector2Int(0,0);
    private Color originalColor;
    private Keyboard keyboard;

    void Start()
    {
        originalColor = cubePrefab.GetComponent<Renderer>().sharedMaterial.color;
        CreateGrid();
        UpdateHighlight();
        keyboard = Keyboard.current;
    }

    void CreateGrid()
    {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                Vector3 position = new Vector3(x * spacing, y * spacing, 0);
                GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity, transform);
                cube.name = $"Cube_{x}_{y}";
                grid[x, y] = cube;
            }
        }
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        Vector2Int newPosition = currentPosition;

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

        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            ToggleSelection();
        }
    }

    void UpdateHighlight()
    {
        foreach (GameObject cube in grid)
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
        return grid[currentPosition.x, currentPosition.y].GetComponent<CubeController>();
    }
}
