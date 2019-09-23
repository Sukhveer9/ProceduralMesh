using UnityEngine;
using System.Collections;

public class ControlScript 
{

    public static bool shapeChanging = false;

    public delegate void ChangeShape();
    public static  ChangeShape cShape;

    public static void AddShapeChange(ChangeShape delegats)
    {
        cShape = delegats;
    }

    public static void ChangeTrigger()
    {
        if (!shapeChanging)
        {
            cShape();
           // shapeChanging = true;
        }
    }
}
