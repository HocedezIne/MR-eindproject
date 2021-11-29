using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AppManager : MonoBehaviour
{
    //[System.Serializable]

    public GameObject ARCursorPrefab;
    public GameObject WorldPrefab;

    private GameObject ARCursor;
    private GameObject World;
    private TouchPhase last_phase = TouchPhase.Began;

    public void OnEnable()
    {
        ARCursor = Instantiate(ARCursorPrefab, transform);
        ARCursor.SetActive(false);
    }

    public void OnDisable()
    {
        Object.Destroy(ARCursor);
        if (World) Object.Destroy(World);
        World = null;
    }



    public void EnableARCursor(Vector3 position, Quaternion rotation)
    {
        if (!World)
        {
            ARCursor.SetActive(true);
            ARCursor.transform.position = position;
            ARCursor.transform.rotation = rotation;
        }
        else
        {
            ARCursor.SetActive(false);
        }
    }

    public void DisableARCursor()
    {
        ARCursor.SetActive(false);
    }



    private void Update()
    {
        if (World) return;

        if (Input.touchCount != 1) return;

        Touch touch = Input.GetTouch(0);

        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
        {
            return;
        }

        if ((touch.phase == TouchPhase.Ended) && (last_phase != TouchPhase.Ended))
        {
            if (ARCursor.activeSelf)
            {
                World = Instantiate(WorldPrefab, ARCursor.transform.position, ARCursor.transform.rotation);
            }
        }

        last_phase = touch.phase;
    }




    public void DeleteWorld()
    {
        Object.Destroy(World);
        World = null;
    }
}
