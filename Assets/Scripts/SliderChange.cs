using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// SliderChange class is used to configure the change in values of force and velocity of the slider.
/// </summary>
public class SliderChange : MonoBehaviour
{
    public float snapInterval = 5; //any interval you want to round to
    public Slider sliderUI;
    public TMP_Text textUI;
    void Start()
    {
        sliderUI.onValueChanged.AddListener(delegate { ShowSliderValue(); });
        ShowSliderValue();
    }

    public void ShowSliderValue()
    {
        float value = sliderUI.value;
        value = Mathf.Round(value / snapInterval) * snapInterval;
        sliderUI.value = value;
        if (sliderUI.name == "VelocitySlider")
        {
            textUI.text = "Velocity : " + sliderUI.value.ToString();
        }
        else
        {
            textUI.text = "Force : " + sliderUI.value.ToString();

        }
    }
}
 