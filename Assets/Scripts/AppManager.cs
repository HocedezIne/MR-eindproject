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

    public List<Build> Sets;

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
            Debug.Log("you toucha da screen");
            if (ARCursor.activeSelf)
            {
                Debug.Log("special screen touch");
                World = Instantiate(Sets[0].parentObject.gameObject, ARCursor.transform.position, ARCursor.transform.rotation);
                StartBuilding();
                Debug.Log("i don't get called");
            }
        }

        last_phase = touch.phase;
        Debug.Log("e");
    }




    public void DeleteWorld()
    {
        Object.Destroy(World);
        World = null;
    }

    // called when selecting a build
    public void StartPlacing()
    {
        Fullscreen.gameObject.SetActive(false);
        BuildMenu.gameObject.SetActive(true);
        appMode = AppMode.PLACING;

        prev.gameObject.SetActive(false);
        next.gameObject.SetActive(false);
        CurrentStepNumber.text = "Point at a flat surface and tap";
    }

    // called when model has been placed
    public void StartBuilding()
    {
        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        appMode = AppMode.BUILDING;
        prev.gameObject.SetActive(true);
        next.gameObject.SetActive(true);
        CurrentStepNumber.text = "Step number " + Sets[0].stepNumber.ToString() + " out of " + Sets[0].totalSteps.ToString();

        DisableARCursor();

        List<GameObject> Blocks = new List<GameObject>();
        foreach (Transform transform in World.transform)
        {
            Blocks.Add(transform.gameObject);
        }

        foreach (GameObject child in Blocks)
        {
            Debug.Log(child);
            child.SetActive(false);
            // MeshRenderer render = child.gameObject.GetComponent<MeshRenderer>();
            // render.enabled = false;
        }

        for (int i = 0; i < Sets[0].stepNumber; i++)
        {
            Debug.Log(Blocks[i]);
            Blocks[i].SetActive(true);
        }
    }

    public void nextStep()
    {
        List<GameObject> Blocks = new List<GameObject>();
        foreach (Transform transform in World.transform)
        {
            Blocks.Add(transform.gameObject);
        }

        if (Sets[0].stepNumber < Sets[0].totalSteps) Sets[0].stepNumber += 1;
        CurrentStepNumber.text = "Step number " + Sets[0].stepNumber.ToString() + " out of " + Sets[0].totalSteps.ToString();

        for (int i = 0; i < Sets[0].stepNumber; i++)
        {
            Blocks[i].SetActive(true);
        }
    }

    public void previousStep()
    {
        List<GameObject> Blocks = new List<GameObject>();
        foreach (Transform transform in World.transform)
        {
            Blocks.Add(transform.gameObject);
        }

        if (Sets[0].stepNumber > 1) Sets[0].stepNumber -= 1;
        CurrentStepNumber.text = "Step number " + Sets[0].stepNumber.ToString() + " out of " + Sets[0].totalSteps.ToString();

        for (int i = Sets[0].totalSteps - 1; i >= Sets[0].stepNumber; i--)
        {
            Debug.Log(Sets[0].Blocks[i]);
            Blocks[i].SetActive(false);
        }
    }
}
