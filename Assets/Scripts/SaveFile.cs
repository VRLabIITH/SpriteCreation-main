using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to draw the sketches that have been saved.
/// </summary>
public class SaveFile
{
    //serialize lines variable so json can store it
    [SerializeField]
    public List<Line> lines;

    //SaveFile class Initializer which takes in a LineRenderer List and stores it as a Line class list.
    public SaveFile(List<LineRenderer> lineRenderers)
    {
        lines = new List<Line>();

        foreach (LineRenderer lineRenderer in lineRenderers)
        {
            lines.Add(new Line(lineRenderer));

        }
    }

    public void Draw()
    {
        foreach (Line line in lines)
        {
            line.Draw();

        }
    }
}
