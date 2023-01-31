using UnityEngine;

public static class TransformExtensions
{
    public static int GetSortingLayer(this Transform transform, float yOffset = 0)
    {
        return -(int)((transform.position.y + yOffset) * 100);
    }
}
