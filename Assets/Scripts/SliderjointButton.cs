using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderjointButton : MonoBehaviour
{
    public GameObject sliderJointButton;
    public GameObject endSliderJointButton;
    public GameObject sliderObjectManager;
    public GameObject indicator;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void startSliderJoint()
    {
        sliderJointButton.SetActive(false);
        endSliderJointButton.SetActive(true);
        sliderObjectManager.SetActive(true);
        Camera.main.GetComponent<Draw>().enabled = false;
        Camera.main.GetComponent<Draw>().ChangeColour();

    }
    public void endSliderJoint()
    {
        sliderJointButton.SetActive(true);
        endSliderJointButton.SetActive(false);
        sliderObjectManager.SetActive(false);
        Camera.main.GetComponent<Draw>().enabled = true;
        indicator.transform.position = new Vector3(100f, 0f, 0f);
    }
}
