using UnityEngine;

public class CubeController : MonoBehaviour
{
    private Renderer rend;
    private Color originalColor;
    public bool isSelected = false;
    public Vector2Int gridPosition;
    void Awake()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    public void Highlight()
    {
        if (GameManager.Instance != null && 
            GameManager.Instance.CurrentState == GameManager.GameState.Selection)
        {
            rend.material.color = Color.yellow;
        }
    }

    public void ResetColor()
    {
        if (GameManager.Instance != null && 
            GameManager.Instance.CurrentState == GameManager.GameState.Selection)
        {
            rend.material.color = isSelected ? Color.red : originalColor;
        }
    }

    public void ResetToOriginal()
    {
        rend.material.color = originalColor;
    }

    public void ToggleSelection()
    {
        isSelected = !isSelected;
        rend.material.color = isSelected ? Color.red : originalColor;
    }

    public void LightUpPattern(Color color)
    {
        rend.material.color = color;
    }
}