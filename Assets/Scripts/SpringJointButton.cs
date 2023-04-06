using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringJointButton : MonoBehaviour
{
    public GameObject springJointButton;
    public GameObject endSpringJointButton;
    public GameObject springObjectManager;
    public GameObject indicator;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void startSpringJoint()
    {
        springJointButton.SetActive(false);
        endSpringJointButton.SetActive(true);
        springObjectManager.SetActive(true);
        Camera.main.GetComponent<Draw>().enabled = false;
        Camera.main.GetComponent<Draw>().ChangeColour();

    }
    public void endSpringJoint()
    {
        springJointButton.SetActive(true);
        endSpringJointButton.SetActive(false);
        springObjectManager.SetActive(false);
        Camera.main.GetComponent<Draw>().enabled = true;
        indicator.transform.position = new Vector3(100f, 0f, 0f);
    }
}
