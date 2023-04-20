using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class is used to highlight a link, when the stylus is close to the link. 
/// </summary>
/// <summary>
/// This script contains a function called HighlightLine which takes in a parameter of line renderer.
/// The position of the stylus is tracked and the distance between the stylus and the line is measured. If the distance is less than a certain number, then the line is highlighted i.e. the line width is increased.
/// The highlightLine function is run through a loop of links in the scene. 
/// </summary>
public class LineHighlight : MonoBehaviour
{

    LineRenderer lineRenderer;
    float lineWidth = 0.05f;
    public float verticalAxis;
    public float horizontalAxis;

    private void Start()
    {
        

    }
    // Update is called once per frame
    void Update()
    {
        foreach (LineRenderer line in Camera.main.GetComponent<Draw>().listOfColouredLines)
        {
            HighlightLine(line);
        }
        verticalAxis = Input.GetAxis("Vertical");
        horizontalAxis = Input.GetAxis("Horizontal");

        //Debug.Log(verticalAxis + "hor : " + horizontalAxis);
        ////lineRenderer.startWidth = lineWidth;
        //lineRenderer.endWidth = lineWidth;
        //var localVelocity = transform.InverseTransformDirection(grabber.gameObject.GetComponent<Rigidbody>().velocity);
    }
    void HighlightLine(LineRenderer line)
    {
        float minimumDistance = Mathf.Infinity;
        for (int i = 0; i < line.positionCount - 1; i++)
        {
            float dist = Vector3.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), line.GetPosition(i));
            //Debug.Log("Point Position id: "+ i + "position: "+ line.GetPosition(i) + "Distance : " + dist);
            if (dist < minimumDistance)
            {
                minimumDistance = dist;
            }
            //Debug.Log("Dist " + dist);
            if (minimumDistance < 1.3f)
            {
                //Debug.Log("close Enough");
                line.startWidth = lineWidth * 1.75f;
                line.endWidth = lineWidth * 1.75f;
            }
            else
            {
                line.startWidth = lineWidth;
                line.endWidth = lineWidth;
            }

        }
    }
}