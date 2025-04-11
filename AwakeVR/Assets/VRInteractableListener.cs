using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRInteractableListener : MonoBehaviour
{
    [Tooltip("Check this if the object is a shape; uncheck if it is a bin.")]
    public bool isShape = true;

    // Called when the ray selects this object (trigger pressed)
    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (isShape)
            GameManager.Instance.OnShapeSelected(gameObject);
        else
            GameManager.Instance.OnBinSelected(gameObject);
    }

    // Called when the ray hovers over this object.
    public void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (isShape)
            GameManager.Instance.OnShapeHovered(gameObject);
        else
            GameManager.Instance.OnBinHovered(gameObject);
    }
}
