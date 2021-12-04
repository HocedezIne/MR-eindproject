using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class Builder : MonoBehaviour
{

    private ARTrackedImageManager Manager;

    public GameObject CompleteBuild;

    private Dictionary<int, GameObject> InstanciatedObjects = new Dictionary<int, GameObject>();

    // Start is called before the first frame update
    void OnEnable()
    {
        Manager = GetComponent<ARTrackedImageManager>();
        Manager.trackedImagesChanged += OnImageEvent;
    }

    void OnDisable()
    {
        Manager.trackedImagesChanged -= OnImageEvent;

    }

    // Update is called once per frame
    void OnImageEvent(ARTrackedImagesChangedEventArgs Args)
    {
        Debug.Log("Changes");
        foreach (ARTrackedImage i in Args.added)
        {
            if (i.referenceImage.name == "One")
            {
                InstanciatedObjects.Add(i.GetInstanceID(), Instantiate(CompleteBuild, i.transform.position, i.transform.rotation));
            }
        }

        foreach (ARTrackedImage i in Args.updated)
        {
            int id = i.GetInstanceID();
            if (InstanciatedObjects.ContainsKey(id))
            {
                InstanciatedObjects[id].transform.position = i.transform.position;
                InstanciatedObjects[id].transform.rotation = i.transform.rotation;
            }
        }

        foreach (ARTrackedImage i in Args.removed)
        {
            int id = i.GetInstanceID();
            if (InstanciatedObjects.ContainsKey(id))
            {
                Object.Destroy(InstanciatedObjects[id]);
                InstanciatedObjects.Remove(id);
            }
        }
    }
}
