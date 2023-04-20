using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JointCreation : MonoBehaviour
{
    LineRenderer Line;
    float lineWidth = 0.1f;
    public float minimumVertexDistance = 0.1f;

    bool isLineStarted;
    float x_total = 0;
    float y_total = 0;
    int layer;
    public GameObject indicator;
    List<Vector2> points;
    Vector3 anchorPoint;

    private JointMotor2D hjm;

    List<Collider2D> listOfSelectedObjects;

    public GameObject ground;
    EdgeCollider2D edgeCollider;

    public List<Vector3> pointsOfHingeJoints;
    public List<HingeJoint2D> joints;
    GameObject jointIndicatorObject;

    public Dictionary<Vector3, HingeJoint2D> hingeJointDict;

    public GameObject jointSelectionPanel;
    public TMPro.TMP_Dropdown jointselectionDropDown;
    // Start is called before the first frame update
    void Start()
    {
        pointsOfHingeJoints = new List<Vector3>();
        listOfSelectedObjects = new List<Collider2D>();
        jointList = new List<GameObject>();
        hingeJointDict = new Dictionary<Vector3, HingeJoint2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Line = GetComponent<LineRenderer>();         //LineRenderer component is added to LineObject
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            points = new List<Vector2>();
            edgeCollider = this.gameObject.GetComponent<EdgeCollider2D>();
            CreateJoint();

        }
        if (Input.GetMouseButton(1) && isLineStarted)
        {
            Vector3 currentPos = GetWorldCoordinate(Input.mousePosition);
            float distance = Vector3.Distance(currentPos, Line.GetPosition(Line.positionCount - 1));

            if (distance > minimumVertexDistance)
            {
                points.Add(currentPos);
                //Debug.Log(distance);
                UpdateLine();
                

            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            isLineStarted = false;
            
            FindJoint();
            SetEdgeCollider(Line);
            CheckCollision();
            points.Clear();
            //CheckCollision();
            Debug.Log("hinge joint dictionary count "+hingeJointDict.Count);
        }
        //if (Input.GetMouseButton(0))
        //{
        //   // clickMotor();
        //}
    }

    private void CreateJoint()
    {
        Vector3 mousePos = GetWorldCoordinate(Input.mousePosition);     //gets world coordinates from mousePosition
                                                                        //Debug.Log("In renderLine function");

        Line.positionCount = 0;
        Line.positionCount = 2;
        Line.SetPosition(0, mousePos);
        Line.SetPosition(1, mousePos);
        isLineStarted = true;
        Line.GetComponent<LineRenderer>().material.color = Color.red;

        // set width of the renderer
        Line.startWidth = lineWidth;
        Line.endWidth = lineWidth;
        Line.useWorldSpace = false;
        Line.positionCount = 0;
    }
    private Vector3 GetWorldCoordinate(Vector3 mousePosition)
    {
        Vector3 mousePos = new Vector3(mousePosition.x, mousePosition.y, 1);
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
    public void FindJoint()
    {
        x_total = 0;
        y_total = 0;
        foreach (Vector3 point in points)
        {
            x_total += point.x;
            y_total += point.y;
        }

        float x_avg = x_total / points.Count;
        float y_avg = y_total / points.Count;

        anchorPoint = new Vector3(x_avg, y_avg, -1f);
        indicator.transform.position = anchorPoint;
    }
    private void UpdateLine()
    {
        Line.positionCount++;
        Line.SetPosition(Line.positionCount - 1, GetWorldCoordinate(Input.mousePosition));
    }
    void SetEdgeCollider(LineRenderer lineRend)
    {
        List<Vector2> edges = new List<Vector2>();

        for (int point = 0; point < lineRend.positionCount; point++)
        {
            Vector3 lineRendererPoint = lineRend.GetPosition(point);

            edges.Add(new Vector2(lineRendererPoint.x, lineRendererPoint.y));
        }

        edgeCollider.SetPoints(edges);
    }
   

    public void CheckCollision()
    {
        
        foreach (Collider2D lineCol in Camera.main.GetComponent<Draw>().listOfLines)
        {
            if (edgeCollider.bounds.Intersects(lineCol.bounds))
            {
                //Debug.Log("Intersects : " + lineCol.gameObject.name);
                listOfSelectedObjects.Add(lineCol);
            }
        }
        if (listOfSelectedObjects.Count == 1)
        {

            HingeJoint2D groundjoint = ground.AddComponent<HingeJoint2D>();
            groundjoint.anchor = ground.transform.InverseTransformPoint(new Vector3(anchorPoint.x, anchorPoint.y, -1f));
            Rigidbody2D link0 = listOfSelectedObjects[0].gameObject.GetComponent<Rigidbody2D>();
            link0.gravityScale = 0;
            hingeJointDict.Add(new Vector3(anchorPoint.x, anchorPoint.y, -1f), groundjoint);
            groundjoint.connectedBody = link0;
            //joints.Add(groundjoint);
            pointsOfHingeJoints.Add(new Vector3(anchorPoint.x, anchorPoint.y, -1f));
        } 
        if(listOfSelectedObjects.Count == 2)
        {
            //jointSelectionPanel.transform.position = new Vector3(anchorPoint.x, anchorPoint.y - 1.2f, -2f);
            HingeJoint2D hj1 = listOfSelectedObjects[0].gameObject.AddComponent<HingeJoint2D>();
            hj1.anchor = new Vector3(anchorPoint.x, anchorPoint.y, -1f);
            hj1.attachedRigidbody.gravityScale = 0;
            Rigidbody2D link1 = listOfSelectedObjects[1].gameObject.GetComponent<Rigidbody2D>();
            link1.gravityScale = 0;
            hj1.connectedBody = link1;
            //joints.Add(hj1);
            pointsOfHingeJoints.Add(new Vector3(anchorPoint.x, anchorPoint.y, -1f));
            hingeJointDict.Add(new Vector3(anchorPoint.x, anchorPoint.y, -1f), hj1);
            
        }
        else
        {
            Debug.Log("Choose lines");
        }
        listOfSelectedObjects.Clear();
        //pointsOfJoints.Clear();
    }

    public GameObject jointIndicator;
    
    List<GameObject> jointList;
    public GameObject chooseMotorButton;
    public GameObject endMotorButton;
    public Collider2D circleCollider;
    public GameObject velocitySlider;
    public GameObject forceSlider;
    public void ChooseMotorFunction()
    {
        
        for(int i = 0; i<pointsOfHingeJoints.Count; i ++)
        {
            GameObject go = Instantiate(jointIndicator, pointsOfHingeJoints[i], Quaternion.identity);
            go.tag = "Indicator";
            jointList.Add(go);  
        }
        for (int j = 0; j < sliderJointManager.GetComponent<SliderJoint>().pointsOfSliderJoints.Count; j++)
        {
            GameObject go = Instantiate(sliderJointIndicator, sliderJointManager.GetComponent<SliderJoint>().pointsOfSliderJoints[j], Quaternion.identity);
            go.tag = "Indicator";
            jointList.Add(go);

        }

        Debug.Log("Joint List" + jointList.Count);

        endMotorButton.SetActive(true);
        chooseMotorButton.SetActive(false);
        Camera.main.GetComponent<Draw>().enabled = false;
        Camera.main.GetComponent<ChooseMotor>().enabled = true;
        velocitySlider.SetActive(true);
        forceSlider.SetActive(true);
        
    }
    public void EndMotor()
    {
        foreach(GameObject go in jointList)
        {
            Destroy(go);
        }
        endMotorButton.SetActive(false);
        chooseMotorButton.SetActive(true);
        Camera.main.GetComponent<Draw>().enabled = true;
        Camera.main.GetComponent<ChooseMotor>().enabled = false;
        velocitySlider.SetActive(false);
        forceSlider.SetActive(false);
    }
    public GameObject JointManipulationStartButton;
    public GameObject JointManipulationEndButton;
    public GameObject springJointIndicator;
    public GameObject sliderJointIndicator;
    public GameObject springJointManager;
    public GameObject sliderJointManager;

    public void StartManipulation()
    {

        for (int i = 0; i < pointsOfHingeJoints.Count; i++)
        {
            GameObject go = Instantiate(jointIndicator, pointsOfHingeJoints[i], Quaternion.identity);
            go.tag = "Indicator";
            jointList.Add(go);
        }
        for (int j = 0; j < springJointManager.GetComponent<SpringJointCreation>().pointsOfSpringJoints.Count; j++)
        {
            GameObject go = Instantiate(springJointIndicator, springJointManager.GetComponent<SpringJointCreation>().pointsOfSpringJoints[j], Quaternion.identity);
            go.tag = "Indicator";
            jointList.Add(go);
        }
        for (int j = 0; j < sliderJointManager.GetComponent<SliderJoint>().pointsOfSliderJoints.Count; j++)
        {
            GameObject go = Instantiate(sliderJointIndicator, sliderJointManager.GetComponent<SliderJoint>().pointsOfSliderJoints[j], Quaternion.identity);
            go.tag = "Indicator";
            jointList.Add(go);

        }
        JointManipulationStartButton.SetActive(false);
        JointManipulationEndButton.SetActive(true);
        //Camera.main.GetComponent<ChooseMotor>().enabled = true;
        Camera.main.GetComponent<Draw>().enabled = false;
        Camera.main.GetComponent<JointManipulation>().enabled = true;

    }
    public void EndManipulation()
    {
        foreach (GameObject go in jointList)
        {
            Destroy(go);
        }
        JointManipulationStartButton.SetActive(true);
        JointManipulationEndButton.SetActive(false);
        Camera.main.GetComponent<JointManipulation>().enabled = false;
        //Camera.main.GetComponent<ChooseMotor>().enabled = false;
        Camera.main.GetComponent<Draw>().enabled = true;
    }

    public GameObject interactionButton;
    public GameObject stopInteractionButton;
    public GameObject InteractionManager;
    public void StartInteraction()
    {
        for (int i = 0; i < pointsOfHingeJoints.Count; i++)
        {
            GameObject go = Instantiate(jointIndicator, pointsOfHingeJoints[i], Quaternion.identity);
            go.tag = "Indicator";
            jointList.Add(go);
        }
        for (int j = 0; j < springJointManager.GetComponent<SpringJointCreation>().pointsOfSpringJoints.Count; j++)
        {
            GameObject go = Instantiate(springJointIndicator, springJointManager.GetComponent<SpringJointCreation>().pointsOfSpringJoints[j], Quaternion.identity);
            go.tag = "Indicator";
            jointList.Add(go);
        }
        for (int j = 0; j < sliderJointManager.GetComponent<SliderJoint>().pointsOfSliderJoints.Count; j++)
        {
            GameObject go = Instantiate(sliderJointIndicator, sliderJointManager.GetComponent<SliderJoint>().pointsOfSliderJoints[j], Quaternion.identity);
            go.tag = "Indicator";
            jointList.Add(go);

        }
        interactionButton.SetActive(false);
        stopInteractionButton.SetActive(true);
        Camera.main.GetComponent<Draw>().enabled = false;
        InteractionManager.SetActive(true);
        forceSlider.SetActive(true);
    }
    public void EndInteraction()
    {
        foreach (GameObject go in jointList)
        {
            Destroy(go);
        }
        interactionButton.SetActive(true);
        stopInteractionButton.SetActive(false);
        Camera.main.GetComponent<Draw>().enabled = true;
        InteractionManager.SetActive(false);
        forceSlider.SetActive(false);
    }
}
