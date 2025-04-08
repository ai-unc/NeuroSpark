using UnityEngine;

public class CubeController : MonoBehaviour
{
    private Renderer rend;
    private Color originalColor;
    private bool isSelected = false;

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
        if (!isSelected)
        {
            rend.material.color = originalColor;
        } else {
            rend.material.color = Color.red;
        }
    }

    public void ToggleSelection()
    {
        isSelected = !isSelected;
        rend.material.color = isSelected ? Color.red : Color.yellow;
    }

    public void LightUpPattern(Color color)
    {
        if (!isSelected)
        {
            rend.material.color = color;
        }
    }
}