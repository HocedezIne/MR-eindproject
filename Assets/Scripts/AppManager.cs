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

    public Canvas Fullscreen;
    public Canvas BuildMenu;
    public Text CurrentStepNumber;
    public bool BuildMode = false;

    private GameObject ARCursor;
    private GameObject World;
    private TouchPhase last_phase = TouchPhase.Began;

    public void OnEnable()
    {
        ARCursor = Instantiate(ARCursorPrefab, transform);
        ARCursor.SetActive(false);
        Fullscreen.gameObject.SetActive(true);
    }

    private void OnApplicationPause(bool pause)
    {
        
    }

    private void OnApplicationQuit()
    {
        Object.Destroy(ARCursor);
        if (World) Object.Destroy(World);
        World = null;
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

    public bool RunInSimulator()
    {
# if UNITY_EDITOR
        return true;
#else
        return false;
#endif
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

    private int stepNumber =0;
    private int totalSteps = 10;

    public void StartBuild()
    {
        Fullscreen.gameObject.SetActive(false);
        BuildMenu.gameObject.SetActive(true);
        BuildMode = true;

        CurrentStepNumber.text = "Step number " + stepNumber.ToString() + "out of " + totalSteps.ToString();
    }

    public void nextStep()
    {
        stepNumber += 1;
        CurrentStepNumber.text = "Step number " + stepNumber.ToString() + "out of " + totalSteps.ToString();
    }

    public void previousStep()
    {
        stepNumber -= 1;
        CurrentStepNumber.text = "Step number " + stepNumber.ToString() + "out of " + totalSteps.ToString();
    }
}
