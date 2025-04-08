using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Shape and Bin Objects")]
    public GameObject[] shapes;
    public GameObject[] bins;

    [Header("Vertical Offsets")]
    public float topYOffset = 3.0f;
    public float placedYOffset = 1.0f;

    private List<GameObject> correctOrder;
    private List<GameObject> shuffledOrder;
    private List<GameObject> unplacedShapes;

    private Dictionary<GameObject, int> shapeToBinIndex = new Dictionary<GameObject, int>();
    private Dictionary<GameObject, Vector3> initialPositions = new Dictionary<GameObject, Vector3>();

    private Keyboard keyboard;
    private int currentShapeIndex = 0;
    private int currentBinIndex = 0;

    private GameObject currentSelectedShape = null;

    private enum SelectionMode { Memorization, SelectionShape, SelectionBin, Completed }
    private SelectionMode currentMode = SelectionMode.Memorization;

    private bool useVRInput = false;
    private bool vrAxisReset = true;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        useVRInput = XRSettings.isDeviceActive;
        keyboard = Keyboard.current;

        correctOrder = new List<GameObject>(shapes);
        correctOrder = correctOrder.OrderBy(s => Random.value).ToList();

        for (int i = 0; i < correctOrder.Count; i++)
            shapeToBinIndex[correctOrder[i]] = i;

        for (int i = 0; i < correctOrder.Count && i < bins.Length; i++)
        {
            Vector3 binPos = bins[i].transform.position;
            Vector3 shapePos = new Vector3(binPos.x, binPos.y + topYOffset, binPos.z);
            correctOrder[i].transform.position = shapePos;
            SetShapeColor(correctOrder[i], Color.green);
        }
        StartCoroutine(WaitAndShuffle(5f));
    }

    private IEnumerator WaitAndShuffle(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        shuffledOrder = new List<GameObject>(correctOrder);
        Shuffle(shuffledOrder);

        for (int i = 0; i < shuffledOrder.Count && i < bins.Length; i++)
        {
            Vector3 binPos = bins[i].transform.position;
            Vector3 shapePos = new Vector3(binPos.x, binPos.y + topYOffset, binPos.z);
            shuffledOrder[i].transform.position = shapePos;
            SetShapeColor(shuffledOrder[i], Color.white);
            initialPositions[shuffledOrder[i]] = shapePos;
        }

        unplacedShapes = new List<GameObject>(shuffledOrder);
        currentMode = SelectionMode.SelectionShape;
        currentShapeIndex = 0;
        currentBinIndex = 0;
        if (!useVRInput)
        {
            UpdateShapeHighlight(currentShapeIndex);
            UpdateBinHighlight(currentBinIndex);
        }
    }

    void Update()
    {
        if (useVRInput)
        {
            if (Gamepad.current != null)
            {
                Vector2 stickValue = Gamepad.current.leftStick.ReadValue();
                if (currentMode == SelectionMode.SelectionShape && unplacedShapes.Count > 0)
                {
                    if (vrAxisReset && Mathf.Abs(stickValue.x) > 0.5f)
                    {
                        vrAxisReset = false;
                        if (stickValue.x > 0)
                            currentShapeIndex = (currentShapeIndex + 1) % unplacedShapes.Count;
                        else if (stickValue.x < 0)
                            currentShapeIndex = (currentShapeIndex - 1 + unplacedShapes.Count) % unplacedShapes.Count;
                        UpdateShapeHighlight(currentShapeIndex);
                    }
                    else if (Mathf.Abs(stickValue.x) < 0.1f)
                    {
                        vrAxisReset = true;
                    }
                }
                else if (currentMode == SelectionMode.SelectionBin)
                {
                    if (vrAxisReset && Mathf.Abs(stickValue.x) > 0.5f)
                    {
                        vrAxisReset = false;
                        if (stickValue.x > 0)
                            currentBinIndex = (currentBinIndex + 1) % bins.Length;
                        else if (stickValue.x < 0)
                            currentBinIndex = (currentBinIndex - 1 + bins.Length) % bins.Length;
                        UpdateBinHighlight(currentBinIndex);
                    }
                    else if (Mathf.Abs(stickValue.x) < 0.1f)
                    {
                        vrAxisReset = true;
                    }
                }
            }
        }
        else
        {
            if (currentMode == SelectionMode.SelectionShape)
            {
                if (keyboard.leftArrowKey.wasPressedThisFrame)
                {
                    currentShapeIndex = (currentShapeIndex - 1 + unplacedShapes.Count) % unplacedShapes.Count;
                    UpdateShapeHighlight(currentShapeIndex);
                }
                else if (keyboard.rightArrowKey.wasPressedThisFrame)
                {
                    currentShapeIndex = (currentShapeIndex + 1) % unplacedShapes.Count;
                    UpdateShapeHighlight(currentShapeIndex);
                }
                else if (keyboard.spaceKey.wasPressedThisFrame)
                {
                    currentSelectedShape = unplacedShapes[currentShapeIndex];
                    SetShapeColor(currentSelectedShape, Color.green);
                    currentMode = SelectionMode.SelectionBin;
                    currentBinIndex = 0;
                    UpdateBinHighlight(currentBinIndex);
                }
            }
            else if (currentMode == SelectionMode.SelectionBin)
            {
                if (keyboard.leftArrowKey.wasPressedThisFrame)
                {
                    currentBinIndex = (currentBinIndex - 1 + bins.Length) % bins.Length;
                    UpdateBinHighlight(currentBinIndex);
                }
                else if (keyboard.rightArrowKey.wasPressedThisFrame)
                {
                    currentBinIndex = (currentBinIndex + 1) % bins.Length;
                    UpdateBinHighlight(currentBinIndex);
                }
                else if (keyboard.spaceKey.wasPressedThisFrame)
                {
                    PlaceShape(currentSelectedShape, bins[currentBinIndex]);
                }
            }
        }
    }

    public void OnShapeSelected(GameObject shape)
    {
        if (currentMode != SelectionMode.SelectionShape) return;
        if (!unplacedShapes.Contains(shape)) return;
        currentSelectedShape = shape;
        SetShapeColor(currentSelectedShape, Color.green);
        currentMode = SelectionMode.SelectionBin;
    }

    public void OnBinSelected(GameObject bin)
    {
        if (currentMode != SelectionMode.SelectionBin || currentSelectedShape == null) return;
        PlaceShape(currentSelectedShape, bin);
    }

    // Called by VR helper when a shape is hovered.
    public void OnShapeHovered(GameObject shape)
    {
        if (currentMode != SelectionMode.SelectionShape)
            return;
        if (!unplacedShapes.Contains(shape))
            return;
        currentSelectedShape = shape;
        // Update the highlight on all shapes so that the hovered shape turns green.
        for (int i = 0; i < unplacedShapes.Count; i++)
        {
            if(unplacedShapes[i] == shape)
                SetShapeColor(unplacedShapes[i], Color.green);
            else
                SetShapeColor(unplacedShapes[i], Color.white);
        }
        // Optionally, update the selection index.
        currentShapeIndex = unplacedShapes.IndexOf(shape);
    }

    // Called by VR helper when a bin is hovered.
    public void OnBinHovered(GameObject bin)
    {
        if (currentMode != SelectionMode.SelectionBin)
            return;
        // Update the bin highlight so that the hovered bin turns yellow.
        for (int i = 0; i < bins.Length; i++)
        {
            Renderer rend = bins[i].GetComponent<Renderer>();
            if (rend != null)
                rend.material.color = (bins[i] == bin) ? Color.yellow : Color.gray;
        }
        // Optionally, update the bin selection index.
        currentBinIndex = System.Array.IndexOf(bins, bin);
    }


    private void PlaceShape(GameObject shape, GameObject bin)
    {
        int selectedBinIndex = System.Array.IndexOf(bins, bin);
        if (selectedBinIndex == -1)
            return;
        int intendedBinIndex = shapeToBinIndex[shape];
        Vector3 binPos = bin.transform.position;

        if (selectedBinIndex == intendedBinIndex)
        {
            Vector3 targetPos = new Vector3(binPos.x, binPos.y + placedYOffset, binPos.z);
            shape.transform.position = targetPos;
            SetShapeColor(shape, Color.green);
            unplacedShapes.Remove(shape);
            currentSelectedShape = null;
            if (unplacedShapes.Count == 0)
            {
                FinalCelebration();
                currentMode = SelectionMode.Completed;
            }
            else
            {
                currentMode = SelectionMode.SelectionShape;
                currentShapeIndex = 0;
                UpdateShapeHighlight(currentShapeIndex);
            }
        }
        else
        {
            SetShapeColor(shape, Color.red);
            StartCoroutine(ResetShape(shape));
            currentSelectedShape = null;
            currentMode = SelectionMode.SelectionShape;
        }
    }

    private IEnumerator ResetShape(GameObject shape)
    {
        yield return new WaitForSeconds(0.5f);
        if (initialPositions.ContainsKey(shape))
            shape.transform.position = initialPositions[shape];
        SetShapeColor(shape, Color.white);
    }

    private void UpdateShapeHighlight(int selectedIndex)
    {
        for (int i = 0; i < unplacedShapes.Count; i++)
            SetShapeColor(unplacedShapes[i], i == selectedIndex ? Color.green : Color.white);
    }

    private void UpdateBinHighlight(int selectedIndex)
    {
        for (int i = 0; i < bins.Length; i++)
        {
            Renderer rend = bins[i].GetComponent<Renderer>();
            if (rend != null)
                rend.material.color = (i == selectedIndex) ? Color.yellow : Color.gray;
        }
    }

    private void SetShapeColor(GameObject shape, Color color)
    {
        Renderer rend = shape.GetComponent<Renderer>();
        if (rend != null)
            rend.material.color = color;
    }

    private void Shuffle(List<GameObject> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randIndex = Random.Range(0, i + 1);
            GameObject temp = list[i];
            list[i] = list[randIndex];
            list[randIndex] = temp;
        }
    }

    private void FinalCelebration()
    {
        Debug.Log("All shapes placed correctly!");
        StartCoroutine(RainbowCelebration());
    }

    private IEnumerator RainbowCelebration()
    {
        while (true)
        {
            for (int i = 0; i < correctOrder.Count; i++)
            {
                float hue = (Time.time + i / (float)correctOrder.Count) % 1f;
                Color rainbowColor = Color.HSVToRGB(hue, 1f, 1f);
                SetShapeColor(correctOrder[i], rainbowColor);
            }
            yield return null;
        }
    }
}
