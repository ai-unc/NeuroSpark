using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ColorTestCanvas : MonoBehaviour
{

    [SerializeField] private Button m_button1;
    [SerializeField] private Button m_button2;
    [SerializeField] private Button m_button3;
    [SerializeField] private Button m_button4;
    [SerializeField] private TextMeshProUGUI m_button1_text;
    [SerializeField] private TextMeshProUGUI m_button2_text;
    [SerializeField] private TextMeshProUGUI m_button3_text;
    [SerializeField] private TextMeshProUGUI m_button4_text;
    private bool button_selected = false;
    private IEnumerator coroutine;
    private Coroutine coroutineInstance;

    Color[] button_color = { Color.blue, Color.red, Color.green, Color.yellow, Color.gray, Color.cyan };
    HashSet<Color> usedColors = new HashSet<Color>();

    Button selectedButton = null;

    // Start is called before the first frame update
    void Start()
    {


        if (m_button1 != null && m_button2 != null && m_button3 != null && m_button4 != null) {
            Button[] buttons = { m_button1, m_button2, m_button3, m_button4 };

            for (int i = 0; i < buttons.Length; i++) {
                Color newColor;
                do {
                    newColor = button_color[Random.Range(0, button_color.Length)];
                } while (usedColors.Contains(newColor));

                usedColors.Add(newColor);

                var colors = buttons[i].GetComponent<Button>().colors;
                colors.normalColor = newColor;
                buttons[i].GetComponent<Button>().colors = colors;

                // Reset button text if needed
                TextMeshProUGUI buttonText = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = ""; // Change this to reset the text to its original state
                }

            }
        }
    }

    //Method that handles button selection behavior
    void toggleButtonSelection(Button button)
    {
        coroutine = WaitForUser(10.0f);
        Debug.Log("Button " + button + " Clicked!");
        // If the clicked button is already selected, deselect it
        if (selectedButton == button)
        {
            StopCoroutine(coroutineInstance);
            DeselectButton(button);
            selectedButton = null;
            
        }
        // If another button is already selected, deselect it and select the clicked button
        else if (selectedButton != null)
        {
            DeselectButton(selectedButton);
            StopCoroutine(coroutineInstance);
            SelectButton(button);
            selectedButton = button;
            coroutineInstance = StartCoroutine(coroutine);
        }
        // If no button is selected, select the clicked button
        else
        {
            SelectButton(button);
            selectedButton = button;
            coroutineInstance = StartCoroutine(coroutine);

        }
    }

    //Method to handle button selection, provide a text saying that the button was selected
    void SelectButton(Button button)
    {
        // Change the button's text to indicate selection

        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = "Selected"; 
        }
        button_selected = true;
    }

    //Method to handle button deselection, provide original null string text 
    void DeselectButton(Button button)
    {
        // Reset the button's appearance to indicate deselection
        // For example, you can reset the color or change the text back to its original state
        // Here, we just change the text back to its original state
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = ""; // Change this to reset the text to its original state
        }
        button_selected = false;
    }

    private IEnumerator WaitForUser(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        randomColor();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void randomColor()
    {
        usedColors.Clear();
        Button[] buttons = { m_button1, m_button2, m_button3, m_button4 };
        selectedButton = null;

        for (int i = 0; i < buttons.Length; i++)
        {
            Color newColor;
            do
            {
                newColor = button_color[Random.Range(0, button_color.Length)];
            } while (usedColors.Contains(newColor));

            usedColors.Add(newColor);

            var colors = buttons[i].GetComponent<Button>().colors;
            colors.normalColor = newColor;
            colors.pressedColor = newColor;
            colors.highlightedColor = newColor;
            colors.selectedColor = newColor;

            buttons[i].GetComponent<Button>().colors = colors;

            // Reset button text if needed
            TextMeshProUGUI buttonText = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = ""; // Change this to reset the text to its original state
            }
        }
    }

    public void ClickedButton1() {
        Debug.Log("Button 1 Clicked!");
        toggleButtonSelection(m_button1);
    }

    public void ClickedButton2() {
        Debug.Log("Button 2 Clicked!");
        toggleButtonSelection(m_button2);

    }

    public void ClickedButton3() {
        Debug.Log("Button 3 Clicked!");
        toggleButtonSelection(m_button3);
    }

    public void ClickedButton4() {
        Debug.Log("Button 4 Clicked!");
        toggleButtonSelection(m_button4);
    }

}