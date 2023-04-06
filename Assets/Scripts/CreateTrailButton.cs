using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTrailButton : MonoBehaviour
{
    public GameObject startTrailButton;
    public GameObject endTrailButton;
    public GameObject Trail;
    public GameObject indicator;

    public void OnTrailButtonStart()
    { 
        startTrailButton.SetActive(false);
        endTrailButton.SetActive(true);
        Camera.main.GetComponent<Draw>().enabled = false;
        Trail.GetComponent<CreateTrail>().enabled = true;
    }
    public void OnTrailButtonEnd()
    {
        endTrailButton.SetActive(false);
        startTrailButton.SetActive(true);
        Camera.main.GetComponent<Draw>().enabled = true;
        Trail.GetComponent<CreateTrail>().enabled = false;
        indicator.transform.position = new Vector3(100f, 0f, 0f);

    }
}
