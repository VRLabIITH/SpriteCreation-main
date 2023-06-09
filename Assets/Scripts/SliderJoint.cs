using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderJoint : MonoBehaviour
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

    public List<Vector3> pointsOfSliderJoints;
    public List<HingeJoint2D> joints;
    public Dictionary<Vector3, SliderJoint2D> sliderJointDict;

    // Start is called before the first frame update
    void Start()
    {
        pointsOfSliderJoints = new List<Vector3>();
        listOfSelectedObjects = new List<Collider2D>();
        sliderJointDict = new Dictionary<Vector3, SliderJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Line = GetComponent<LineRenderer>();         //LineRenderer component is added to LineObject
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
            CheckCollisionForSlider();
        }
        
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
        //Line.GetComponent<LineRenderer>().material.color = Color.red;

        // set width of the renderer
        //Line.startWidth = lineWidth;
        //Line.endWidth = lineWidth;
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


    // point for connected anchor
    Vector3 sliderConnectedAnchor;

    Vector3 IntersectionPoint(LineRenderer linkLine)
    {
        Vector2 p1 = new Vector2(Line.GetPosition(0).x, Line.GetPosition(0).y);
        Vector2 p2 = new Vector2(Line.GetPosition(Line.positionCount - 1).x, Line.GetPosition(Line.positionCount - 1).y);

        // points in line 2
        Vector2 p3 = new Vector2(linkLine.GetPosition(0).x, linkLine.GetPosition(0).y);
        Vector2 p4 = new Vector2(linkLine.GetPosition(linkLine.positionCount - 1).x, linkLine.GetPosition(linkLine.positionCount - 1).y);

        // line 1
        float A1 = p2.y - p1.y;
        float B1 = p1.x - p2.x;
        float C1 = A1 * p1.x + B1 * p1.y;

        // line 2
        float A2 = p4.y - p3.y;
        float B2 = p3.x - p4.x;
        float C2 = A2 * p3.x + B2 * p3.y;


        float denominator = A1 * B2 - A2 * B1;

        //Make sure the denominator is > 0, if so the lines are parallel
        if (denominator != 0)
        {
            
            float x_intersect = (B2 * C1 - B1 * C2) / denominator;
            float y_intersect = (A1 * C2 - A2 * C1) / denominator;
            sliderConnectedAnchor = new Vector3(x_intersect, y_intersect, -1f);
            
        }
        return sliderConnectedAnchor;
    }
    void CheckCollisionForSlider()
    {

        foreach (Collider2D lineCol in Camera.main.GetComponent<Draw>().listOfLines)
        {
            if (edgeCollider.bounds.Intersects(lineCol.bounds))
            {
                //Debug.Log("Intersects : " + lineCol.gameObject.name);
                LineRenderer lineLink = lineCol.GetComponent<LineRenderer>();
                listOfSelectedObjects.Add(lineCol);
                //points in line 1
                
                
            }
        }

        if (listOfSelectedObjects.Count == 1)
        {
            //Debug.Log("1 Object");
            SliderJoint2D groundjoint = ground.AddComponent<SliderJoint2D>();
            groundjoint.anchor = ground.transform.InverseTransformPoint(Line.GetPosition(Line.positionCount - 1));
            groundjoint.autoConfigureConnectedAnchor = false;
            groundjoint.connectedAnchor = IntersectionPoint(listOfSelectedObjects[0].GetComponent<LineRenderer>());
            Rigidbody2D link0 = listOfSelectedObjects[0].gameObject.GetComponent<Rigidbody2D>();
            link0.gravityScale = 0;
            groundjoint.connectedBody = link0;
            //joints.Add(groundjoint);
            pointsOfSliderJoints.Add(Line.GetPosition(Line.positionCount - 1));
            sliderJointDict.Add(Line.GetPosition(Line.positionCount -1), groundjoint);
        }
        if(listOfSelectedObjects.Count == 2)
        {
            //Debug.Log("2 Object");
            SliderJoint2D sj1 = listOfSelectedObjects[0].gameObject.AddComponent<SliderJoint2D>();
            sj1.anchor = IntersectionPoint(listOfSelectedObjects[0].GetComponent<LineRenderer>());
            sj1.autoConfigureConnectedAnchor = false;
            sj1.connectedAnchor = IntersectionPoint(listOfSelectedObjects[1].GetComponent<LineRenderer>());
            Rigidbody2D link1 = listOfSelectedObjects[1].GetComponent<Rigidbody2D>();
            sj1.connectedBody = link1;
            sliderJointDict.Add(IntersectionPoint(listOfSelectedObjects[0].GetComponent<LineRenderer>()), sj1);
            pointsOfSliderJoints.Add(IntersectionPoint(listOfSelectedObjects[0].GetComponent<LineRenderer>()));
        }
        
        else
        {
            Debug.Log("Choose lines");
        }
        listOfSelectedObjects.Clear();
        //pointsOfJoints.Clear();
    }
}
