using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.EventSystems;
public class Draw : NetworkBehaviour
{
    public GameObject LinePrefab;
    public GameObject LineObject;
    public GameObject LineParent;
    public GameObject ground;
    public LineRenderer Line;
    public float lineWidth = 1f;
    public float minimumVertexDistance = 0.1f;
    private bool isLineStarted;
    
    public float jt_x, jt_y;
    public GameObject indicatorCircle;
    

    EdgeCollider2D edgeCollider;
    int layer;
    public GameObject indicator;
    List<Vector2> points;

    public List<LineRenderer> listOfColouredLines;
    public List<Collider2D> listOfLines;

    int linesCount;
    public static Stack<GameObject> listOfLinesStack;
    public static Stack<GameObject> undoLinesStack;
    void Start()
    {
        //matrix = new Matrix4x4(new Vector4(Mathf.Cos(0.5236f), Mathf.Sin(0.5236f), 0f, 0f), new Vector4(-Mathf.Sin(0.5236f), Mathf.Cos(0.5236f), 0f, 0f),
        //    new Vector4(0f, 0f, 1f, 0f), new Vector4(0f, 0f, 0f, 1f));
        //listOfPoints = new List<Vector3>();
        listOfLines = new List<Collider2D>();
        listOfColouredLines = new List<LineRenderer>();
        listOfLinesStack = new Stack<GameObject>();
        undoLinesStack = new Stack<GameObject>();
    }

    public override void OnNetworkSpawn()
    {
        
    }
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            LineObject = Instantiate(LinePrefab);                         //creates a game object for every line object
            LineObject.transform.parent = LineParent.transform;
            Line = LineObject.GetComponent<LineRenderer>();         //LineRenderer component is added to LineObject
            //LineObject.AddComponent<NetworkObject>();
            //LineObject.AddComponent<PlayerNetwork>();
            listOfColouredLines.Add(Line);
            Rigidbody2D rigidbody = LineObject.GetComponent<Rigidbody2D>();
            rigidbody.gravityScale = 0;
            edgeCollider = LineObject.GetComponent<EdgeCollider2D>();
            edgeCollider.isTrigger = true;
            listOfLines.Add(edgeCollider);
            Line.gameObject.layer = LayerMask.NameToLayer("Lines");
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //edgeCollider = LineObject.AddComponent<EdgeCollider2D>();
            points = new List<Vector2>();
            listOfLinesStack.Push(LineObject);
            Debug.Log("Stack of lines: " + listOfLinesStack.Count);
            linesCount++;
            DrawLink();
            
        }

        if (Input.GetMouseButton(0) && isLineStarted)
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

        if (Input.GetMouseButtonUp(0))
        {
            isLineStarted = false;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            SetEdgeCollider(Line);
            points.Clear();
        }

        
        
    }

    
    
    private void UpdateLine()
    {
        Line.positionCount++;
        Line.SetPosition(Line.positionCount - 1, GetWorldCoordinate(Input.mousePosition));
    }

    private Vector3 GetWorldCoordinate(Vector3 mousePosition)
    {
        Vector3 mousePos = new Vector3(mousePosition.x, mousePosition.y, 1);
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
    
    private void DrawLink()
    {
        Vector3 mousePos = GetWorldCoordinate(Input.mousePosition);     //gets world coordinates from mousePosition
        //Debug.Log("In renderLine function");
       
        Line.positionCount = 0;
        Line.positionCount = 2;
        Line.SetPosition(0, mousePos);
        Line.SetPosition(1, mousePos);
        isLineStarted = true;
        Line.GetComponent<LineRenderer>().material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        Line.startColor = Color.black;
        Line.endColor = Color.black;
        // set width of the renderer
        Line.startWidth = lineWidth;
        Line.endWidth = lineWidth;
        Line.useWorldSpace = false;
        Line.positionCount = 0;
    }

    public void SelectLine()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        layer = LayerMask.NameToLayer("Lines");
        if (hit.collider.gameObject.layer == layer)
        {
           // Line.GetComponent<LineRenderer>().material.color = Color.yellow;
            Debug.Log("Hit");
            
        }
    }

    
    void SetEdgeCollider(LineRenderer lineRend)

    {
        List<Vector2> edges = new List<Vector2>();

        for(int point = 0; point<lineRend.positionCount; point++)
        {
            Vector3 lineRendererPoint = lineRend.GetPosition(point);

            edges.Add(new Vector2(lineRendererPoint.x, lineRendererPoint.y));
        }

        edgeCollider.SetPoints(edges);
    }
    public void ChangeColour()
    {
        foreach(LineRenderer lineRenderer in listOfColouredLines)
        {
            if (lineRenderer.startColor == Color.black)
            {
                if(lineRenderer.gameObject.transform.parent == LineParent.transform)
                {
                    Color color = Random.ColorHSV();
                    lineRenderer.startColor = color;
                    lineRenderer.endColor = color;
                }
                else
                {
                    lineRenderer.startColor = lineRenderer.gameObject.transform.parent.GetComponent<LineRenderer>().startColor;
                    lineRenderer.endColor = lineRenderer.gameObject.transform.parent.GetComponent<LineRenderer>().endColor;
                }
            }
        }
    }
    public void FinishChangeColour()
    {
        foreach (LineRenderer lineRenderer in listOfColouredLines)
        {
            lineRenderer.startColor = Color.black;
            lineRenderer.endColor = Color.black;
        }
    }
    public TMPro.TMP_Text sketchEnabled;
    public void StartDraw()
    {
        Camera.main.GetComponent<Draw>().enabled = true;
        sketchEnabled.enabled = true;
        sketchEnabled.text = "Sketching Mode is enabled.";
    }
    public void StopDraw()
    {
        sketchEnabled.enabled = false;
        Camera.main.GetComponent<Draw>().enabled = false;
    }
    public void UndoLine()
    {
        if (listOfLines != null)
        {
            Debug.Log("Started Undo");
            GameObject go = listOfLinesStack.Pop();
            if (go != null){
                Debug.Log("Object there");
            }
            undoLinesStack.Push(go);
            go.SetActive(false);
            linesCount--;
        }
    }
    public void RedoLine()
    {
        if (undoLinesStack != null)
        {
            GameObject go = undoLinesStack.Pop();
            listOfLinesStack.Push(go);
            go.SetActive(true);
            linesCount++;

        }
    }

    public GameObject StartAttachmentButton;
    public GameObject FinishAttachmentButton;
    public GameObject attachManager;
    public void AttachObject()
    {
        attachManager.GetComponent<AttachObjects>().enabled = true;
        StartAttachmentButton.SetActive(false);
        FinishAttachmentButton.SetActive(true);
    }
    public void EndAttachObject()
    {
        attachManager.GetComponent<AttachObjects>().enabled = false;
        StartAttachmentButton.SetActive(true);
        FinishAttachmentButton.SetActive(false);
    }
}
