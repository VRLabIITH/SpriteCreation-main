using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The AttachObject class is used to attach a sketch to an existing link. 
/// </summary>
/// <summary>
/// When this script is activated, we can choose a sketch and link and attach them to each other. First, we choose the sketch and then we choose the link to which we want it attached. 
/// If we want to attach multiple sketches to a link, we attach them one at a time. Choose first sketch and then link to attach it to. Then, we choose the second sketch and the link we want to attach it to. 
/// We can select the sketch and link using the second button on the stylus. 
/// </summary>
public class AttachObjects : MonoBehaviour
{
    HapticPlugin HapticDevice;
    HapticGrabber grabber;

    bool buttonHoldDown = false;
    public List<GameObject> selectedObjects;

    // Start is called before the first frame update
    void Start()
    {
        selectedObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
        foreach (LineRenderer line in Camera.main.GetComponent<Draw>().listOfColouredLines)
        {
            //Debug.Log(" list of lines : " + Camera.main.GetComponent<Draw>().listOfColouredLines.Count);
            //Debug.Log(" list of lines : " + Camera.main.GetComponent<Draw>().listOfColouredLines.Count);
            float minimumDistance = Mathf.Infinity;
            for (int i = 0; i < line.positionCount - 1; i++)
            {
                float dist = Vector3.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), line.GetPosition(i));
                //Debug.Log("Point Position id: "+ i + "position: "+ line.GetPosition(i) + "Distance : " + dist);
                if (dist < minimumDistance)
                {
                    minimumDistance = dist;
                }
                
                if (minimumDistance < 1.3f)
                {
                    //Debug.Log("Dist " + dist);
                    if (Input.GetMouseButtonDown(1))
                    {
                        if (selectedObjects.Count == 0)
                        {

                            selectedObjects.Add(line.gameObject);

                        }
                        else if (selectedObjects.Count == 1)
                        {
                            if (selectedObjects[0] != line.gameObject)
                            {
                                Debug.Log("Second Line selected");
                                selectedObjects.Add(line.gameObject);
                                selectedObjects[0].transform.parent = selectedObjects[1].transform;
                                selectedObjects[0].GetComponent<EdgeCollider2D>().enabled = false;
                                selectedObjects[0].GetComponent<LineRenderer>().useWorldSpace = false;
                                selectedObjects[0].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                                selectedObjects[0].GetComponent<LineRenderer>().startColor = selectedObjects[1].GetComponent<LineRenderer>().startColor;
                                selectedObjects[0].GetComponent<LineRenderer>().endColor = selectedObjects[1].GetComponent<LineRenderer>().startColor;
                            }
                            selectedObjects.Clear();

                        }

                        //else if(selectedObjects.Count == 2)
                        //{
                        //    Debug.Log(" Erased ");
                        //    selectedObjects.Clear();
                        //    selectedObjects.RemoveAt(0);
                        //}

                    }
                }
            }
        }
    }
}