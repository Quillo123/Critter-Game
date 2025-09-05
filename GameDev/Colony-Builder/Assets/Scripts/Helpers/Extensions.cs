using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions_Vector2
{
    public static Vector3 ToVector3(this Vector2 vec)
    {
        return new Vector3(vec.x, vec.y, 0);
    }

    public static Quaternion LookDirection2D(this Vector2 dir)
    {
        return Quaternion.LookRotation(Vector3.forward, dir);
    }

}
public static class Extensions_Vector3
{
    public static Vector2 ToVector2(this Vector3 vec)
    {
        return new Vector2(vec.x, vec.y);
    }

    public static Vector3 SetZ(this Vector3 vec, float z)
    {
        return new Vector3(vec.x, vec.y, z);
    }
}

public static class Extensions_Quaternion
{
}
