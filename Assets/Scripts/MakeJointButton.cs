using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeJointButton : MonoBehaviour
{
    public GameObject makeJointButton;
    public GameObject endJointButton;
    public GameObject jointcreationObject;
    public GameObject createLineObject;
    public GameObject indicator;
    
    public void EndJointCreation()
    {
        makeJointButton.SetActive(true);
        endJointButton.SetActive(false);
        jointcreationObject.GetComponent<JointCreation>().enabled = false;
        indicator.transform.position = new Vector3(100f, 0f, 0f);
        createLineObject.SetActive(false);
        jointcreationObject.GetComponent<JointCreation>().jointSelectionPanel.SetActive(false);
        Camera.main.GetComponent<Draw>().enabled = true;
        

    }
    public void StartJointCreation()
    {
        makeJointButton.SetActive(false);
        endJointButton.SetActive(true);
        jointcreationObject.GetComponent<JointCreation>().enabled = true;
        createLineObject.SetActive(true);
        Camera.main.GetComponent<Draw>().ChangeColour();
        Camera.main.GetComponent<Draw>().enabled = false;
    }
}
