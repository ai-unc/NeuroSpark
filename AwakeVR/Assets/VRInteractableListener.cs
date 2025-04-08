using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRInteractableListener : MonoBehaviour
{
    [Tooltip("Check this if the object is a shape; uncheck if it is a bin.")]
    public bool isShape = true;

    // This function will be called when the interactable is selected (trigger pressed)
    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (isShape)
            GameManager.Instance.OnShapeSelected(gameObject);
        else
            GameManager.Instance.OnBinSelected(gameObject);
    }
}
