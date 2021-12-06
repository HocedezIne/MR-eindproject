using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum AppMode { ONBOARDING, PLACING, BUILDING }

public class AppManager : MonoBehaviour
{
    //variables
    public GameObject ARCursorPrefab;
    public GameObject WorldPrefab;

    public Canvas Fullscreen;
    public Canvas BuildMenu;
    public Text CurrentStepNumber;
    public Button prev;
    public Button next;
    public AppMode appMode = AppMode.ONBOARDING;

    private GameObject ARCursor;
    private GameObject World;
    private TouchPhase last_phase = TouchPhase.Began;

    public void OnEnable()
    {
        ARCursor = Instantiate(ARCursorPrefab, transform);
        ARCursor.SetActive(false);
        Fullscreen.gameObject.SetActive(true);
        appMode = AppMode.ONBOARDING;
    }

    // called when app is no longer focused without closing it
    private void OnApplicationPause(bool pause)
    {
        
    }

    //on android onapplicationquit is called instead of ondisable
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
                StartBuilding();
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

    // called when selecting a build
    public void StartPlacing()
    {
        Fullscreen.gameObject.SetActive(false);
        BuildMenu.gameObject.SetActive(true);
        appMode = AppMode.PLACING;

        prev.gameObject.SetActive(false);
        next.gameObject.SetActive(false);
        CurrentStepNumber.text = "Point the camera at a flat surface and tap the cursor";
    }

    // called when model has been placed
    public void StartBuilding()
    {
        appMode = AppMode.BUILDING;
        prev.gameObject.SetActive(true);
        next.gameObject.SetActive(true);
        CurrentStepNumber.text = "Step number " + stepNumber.ToString() + "out of " + totalSteps.ToString();

        DisableARCursor();
    }

    public void nextStep()
    {
        if (stepNumber<totalSteps) stepNumber += 1;
        CurrentStepNumber.text = "Step number " + stepNumber.ToString() + "out of " + totalSteps.ToString();
    }

    public void previousStep()
    {
        if (stepNumber>0) stepNumber -= 1;
        CurrentStepNumber.text = "Step number " + stepNumber.ToString() + "out of " + totalSteps.ToString();
    }
}
