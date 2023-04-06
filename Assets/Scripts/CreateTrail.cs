using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTrail : MonoBehaviour
{
    public GameObject linePrefab;
    GameObject lineObject;
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

    public List<Vector3> pointsOfJoints;
    public List<HingeJoint2D> joints;
    GameObject jointIndicatorObject;

    // Start is called before the first frame update
    void Start()
    {
        listOfSelectedObjects = new List<Collider2D>();
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
            
            points.Clear();
            CheckCollision();

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
    

    void CheckCollision()
    {

        foreach (Collider2D lineCol in Camera.main.GetComponent<Draw>().listOfLines)
        {
            if (edgeCollider.bounds.Intersects(lineCol.bounds))
            {
                listOfSelectedObjects.Add(lineCol);
            }
        }
        if (listOfSelectedObjects.Count == 1)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 placement = new Vector3((float)pos.x, (float)pos.y, -1f);

            Debug.Log("CLI");
            lineObject = Instantiate(linePrefab, placement, Quaternion.identity);
            lineObject.transform.parent = listOfSelectedObjects[0].gameObject.transform;
            TrailRenderer trail = lineObject.AddComponent<TrailRenderer>();
            trail.startWidth = 0.1f;
            trail.endWidth = 0.1f;
            Color color = Random.ColorHSV();
            trail.material.color = color;
            trail.time = 20;


        }
        
        listOfSelectedObjects.Clear();
        //pointsOfJoints.Clear();
    }
}
