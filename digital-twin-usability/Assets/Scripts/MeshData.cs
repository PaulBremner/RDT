using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public List<Color> ColorRGB { get; set; }
    public List<Color> ColorRad { get; set; }
    public List<Vector3> Location { get; set; }
    public int? parent_ID { get; set; }

    public MeshData(Vector3 loc, Color? rgb = null, Color? rad = null, int? parent_ID = null)//? after the type makes the variable type nullable
    {
        Location = new List<Vector3> { loc };
        ColorRGB = new List<Color>{rgb ?? Color.white};//if no color is passed the value is null, so init with default colour, else init with the passed color
        ColorRad = new List<Color>{rad ?? Color.blue};
    }
    
    public MeshData()
    {
        Location = new List<Vector3>();
        ColorRGB = new List<Color>();//if no color is passed the value is null, so init with default colour, else init with the passed color
        ColorRad = new List<Color>();
    }

    public void AddPoint(Vector3 loc, Color rgb, Color rad, int? p_ID = null)
    {
        Location.Add(loc);
        ColorRGB.Add(rgb);
        ColorRad.Add(rad);
        parent_ID = p_ID;
    }

    public string GetName()
    {
        string name = "_" + Location[0][0] + '_' + Location[0][1] + '_' + Location[0][2];
        return name;
    }

    public string GetNamePoint()
    {
        string name = "" + Location[0][0] + ' ' + Location[0][1] + ' ' + Location[0][2];
        return name;
    }

    public void RemovePoint(int idx)
    {
        Location.RemoveAt(idx);
        ColorRGB.RemoveAt(idx);
        ColorRad.RemoveAt(idx);        
    }
    

}
