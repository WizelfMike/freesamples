using UnityEngine;

public static class VectorTranslator
{
    public static Vector2 ToVector2Z(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }

    public static Vector3 ToVector3Z(this Vector2 vector)
    {
        return new Vector3(vector.x, 0, vector.y);
    }
}