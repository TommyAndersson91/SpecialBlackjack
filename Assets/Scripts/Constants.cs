using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{

    public static GameObject FindObjectInChilds(this GameObject gameObject, string gameObjectName)
    {
        Transform[] children = gameObject.GetComponentsInChildren<Transform>(true);
        foreach (Transform item in children)
        {
            if (item.name == gameObjectName)
            {
                return item.gameObject;
            }
        }
        return null;
    }
}