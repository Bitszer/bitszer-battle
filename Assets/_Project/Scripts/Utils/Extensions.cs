using UnityEngine;

public static class Extensions
{
    public static Transform Clear(this Transform transform)
    {
        foreach (Transform child in transform)
            GameObject.Destroy(child.gameObject);

        return transform;
    }
}