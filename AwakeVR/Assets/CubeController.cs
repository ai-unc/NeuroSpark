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
        if (!isSelected)
        {
            rend.material.color = Color.yellow;
        }
    }

    public void ResetColor()
    {
        if (!isSelected)
        {
            rend.material.color = originalColor;
        }
    }

    public void ToggleSelection()
    {
        isSelected = !isSelected;
        rend.material.color = isSelected ? Color.red : originalColor;
    }
}