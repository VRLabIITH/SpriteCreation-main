using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// The Line class contains the function Draw which is used in the SaveFile class to draw. This class saves the data related to the line.
/// </summary>
//use system namespace to make this class serializable
[Serializable]
public class Line
{
    public Vector3[] positions;
    public Color color;
    public AnimationCurve width;
    //GameObject grabber = HapticPlugin.GetComponent<HapticPlugin>().hapticManipulator.GetComponent<HapticGrabber>();

    //Line class Initializer. Takes in a LineRenderer and stores its data in the respective places
    public Line(LineRenderer lineRenderer)
    {
        positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);
        color = lineRenderer.startColor;
        width = lineRenderer.widthCurve;
    }

    public void Draw()
    {
        GameObject newLineRenderer = new GameObject();
        newLineRenderer.AddComponent<LineRenderer>();
        LineRenderer lineRenderer = newLineRenderer.GetComponent<LineRenderer>();
        EdgeCollider2D edgeCollider = newLineRenderer.AddComponent<EdgeCollider2D>();
        Rigidbody2D rgbody = newLineRenderer.AddComponent<Rigidbody2D>();
        rgbody.gravityScale = 0;
        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
        lineRenderer.generateLightingData = true;
        lineRenderer.receiveShadows = true;
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
        lineRenderer.widthCurve = width;
        lineRenderer.useWorldSpace = false;
        Camera.main.GetComponent<Draw>().listOfLines.Add(edgeCollider);


        //for setting the edge collider
        List<Vector2> edges = new List<Vector2>();

        for (int point = 0; point < lineRenderer.positionCount; point++)
        {
            Vector3 lineRendererPoint = lineRenderer.GetPosition(point);

            edges.Add(new Vector2(lineRendererPoint.x, lineRendererPoint.y));
        }

        edgeCollider.SetPoints(edges);
        Camera.main.GetComponent<Draw>().listOfColouredLines.Add(lineRenderer);
    }

}