using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ColorTestCanvas : MonoBehaviour
{

    [SerializeField] Button m_button1;
    [SerializeField] Button m_button2;
    [SerializeField] Button m_button3;
    [SerializeField] Button m_button4;

    Color[] button_color = { Color.blue, Color.red, Color.green, Color.yellow, Color.gray, Color.cyan };
    HashSet<Color> usedColors = new HashSet<Color>();

    Button selectedButton = null;

    // Start is called before the first frame update
    void Start()
    {


        if (m_button1 != null && m_button2 != null && m_button3 != null && m_button4 != null)
        {
            Button[] buttons = { m_button1, m_button2, m_button3, m_button4 };

            /*            var color_button1 = m_button1.GetComponent<Button>().colors;
                        color_button1.normalColor = button_color[Random.Range(0, 6)];
                        m_button1.GetComponent<Button>().colors = color_button1;

                        var color_button2 = m_button2.GetComponent<Button>().colors;
                        color_button2.normalColor = button_color[Random.Range(0, 6)];
                        m_button2.GetComponent<Button>().colors = color_button2;

                        var color_button3 = m_button3.GetComponent<Button>().colors;
                        color_button3.normalColor = button_color[Random.Range(0, 6)];
                        m_button3.GetComponent<Button>().colors = color_button3;

                        var color_button4 = m_button4.GetComponent<Button>().colors;
                        color_button4.normalColor = button_color[Random.Range(0, 6)];
                        m_button4.GetComponent<Button>().colors = color_button4;*/

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
                buttons[i].GetComponent<Button>().colors = colors;

                //Adding listener for button click
                buttons[i].onClick.AddListener(() => toggleButtonSelection(buttons[i]));
                
            }
        }

    }

    //Method that handles button selection behavior
    void toggleButtonSelection(Button button)
    {
        // If the clicked button is already selected, deselect it
        if (selectedButton == button)
        {
            DeselectButton(button);
            selectedButton = null;
        }
        // If another button is already selected, deselect it and select the clicked button
        else if (selectedButton != null)
        {
            DeselectButton(selectedButton);
            SelectButton(button);
            selectedButton = button;
        }
        // If no button is selected, select the clicked button
        else
        {
            SelectButton(button);
            selectedButton = button;
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
    }

    // Update is called once per frame
    void Update()
    {
        /*        m_button1.onClick.AddListener(() =>
                {
                    Debug.Log("Button 1 clicked");
                });
                m_button2.onClick.AddListener(() =>
                {
                    Debug.Log("Button 2 clicked");
                });
                m_button3.onClick.AddListener(() =>
                {
                    Debug.Log("Button 3 clicked");
                });
                m_button4.onClick.AddListener(() =>
                {
                    Debug.Log("Button 4 clicked");
                });*/
    }

    void randomColor()
    {
        usedColors.Clear();
        Button[] buttons = { m_button1, m_button2, m_button3, m_button4 };

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
            buttons[i].GetComponent<Button>().colors = colors;

            // Reset button text if needed
            TextMeshProUGUI buttonText = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = " "; // Change this to reset the text to its original state
            }
        }
    }
}