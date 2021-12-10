using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SetGeometry
{
    public int buildId;
    public GameObject parentObject;

    public List<GameObject> Blocks
    {
        get
        {
            List<GameObject> found = new List<GameObject>();

            foreach (Transform child in parentObject.transform)
                found.Add(child.gameObject);

            return found;
        }
    }

    public int totalSteps { get { return parentObject.transform.childCount; } }
}
