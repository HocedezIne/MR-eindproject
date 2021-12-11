using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum AppMode { ONBOARDING, PLACING, BUILDING }

public class AppManager : MonoBehaviour
{
    //variables
    public AppMode appMode;
    public GameObject ARCursorPrefab;

    public Canvas loadingScreen;
    public Canvas overviewScreen;
    public Button setButtonPrefab;
    public Canvas detailScreen;
    public Canvas Screen;
    private Canvas previousScreen;

    public List<Build> allSets;
    public List<SetGeometry> setGeometries;
    private SetGeometry setGeometry;
    private GameObject shownObject;
    public List<SetImage> setImages;

    private GameObject ARCursor;
    private TouchPhase last_phase = TouchPhase.Began;


    public bool RunInSimulator()
    {
# if UNITY_EDITOR
        return true;
#else
        return false;
#endif
    }

    private void OnEnable()
    {
        appMode = AppMode.ONBOARDING;

        loadingScreen.gameObject.SetActive(false);
        overviewScreen.gameObject.SetActive(false);
        detailScreen.gameObject.SetActive(false);
        Screen.gameObject.SetActive(false);

        ARCursor = Instantiate(ARCursorPrefab, transform);
        ARCursor.SetActive(false);
    }

    // call loading screen on start
    private void Start()
    {
        loadingScreen.gameObject.SetActive(true); // show loading screen

        SaveLoad.Load(); // load save data

        // get the content panel
        Transform panel = overviewScreen.transform.Find("Panel");
        Transform content = panel.transform.Find("Content");

        foreach (Build b in allSets)
        {
            // add button prefab to overviewscreen for each set
            Button button = Instantiate(setButtonPrefab, content);
            button.GetComponent<Image>().sprite = setImages.Find((x) => x.buildIid == b._id).image;
            button.GetComponentInChildren<Text>().text = b.name;
            button.name = b._id.ToString();
            button.onClick.AddListener(OnSetSelect);
        }

        overviewScreen.gameObject.SetActive(true);
        loadingScreen.gameObject.SetActive(false);
        previousScreen = loadingScreen;
    }

    // event on overview buttons
    private void OnSetSelect()
    {
        // set current build, check if there is save data for build 
        if (SaveLoad.savedBuilds.Contains(allSets[Int32.Parse(EventSystem.current.currentSelectedGameObject.name)]))
        {
            Build.current = SaveLoad.savedBuilds[SaveLoad.savedBuilds.IndexOf(allSets[Int32.Parse(EventSystem.current.currentSelectedGameObject.name)])];
        }
        else
        {
            Build.current = allSets[Int32.Parse(EventSystem.current.currentSelectedGameObject.name)];
        }
        // set setGeometry
        setGeometry = setGeometries.Find((x) => x.buildId == Build.current._id);

        // put data on detail screen
        Transform mainPanel = detailScreen.transform.Find("Panel");
        Image setImage = mainPanel.transform.Find("image").GetComponent<Image>();
        setImage.sprite = setImages.Find((x) => x.buildIid == Build.current._id).image;
        Transform smallPanel = mainPanel.transform.Find("Panel");
        smallPanel.GetComponentInChildren<Text>().text = Build.current.name;

        detailScreen.gameObject.SetActive(true);
        overviewScreen.gameObject.SetActive(false);
        previousScreen = overviewScreen;
    }

    // called when set is selected to be build
    public void StartPlacing()
    {
        appMode = AppMode.PLACING;

        // load data onto screen
        Transform panel = Screen.transform.Find("Panel");
        panel.GetComponentInChildren<Text>().text = "Tap the cursor to place";
        foreach (Transform child in panel.transform)
        {
            if (child.GetComponent<Button>()) child.gameObject.SetActive(false);
        }

        Screen.gameObject.SetActive(true);
        detailScreen.gameObject.SetActive(false);
        previousScreen = detailScreen;
    }

    // called when set is placed
    public void StartBuilding()
    {
        appMode = AppMode.BUILDING;
        DisableARCursor();

        /*// make right blocks visible
        for (int i = 0; i < setGeometry.totalSteps; i++)
        {
            if (i <= Build.current.stepNumber-1) shownObject.transform.GetChild(i).gameObject.SetActive(true);
            else shownObject.transform.GetChild(i).gameObject.SetActive(false);
        }*/

        // change data on screen
        Transform panel = Screen.transform.Find("Panel");
        panel.GetComponentInChildren<Text>().text = Build.current.stepNumber.ToString() + " out of "  + setGeometry.totalSteps.ToString();
        foreach (Transform child in panel.transform)
        {
            if (child.GetComponent<Button>()) child.gameObject.SetActive(true);
        }
    }

    public void Next()
    {
        // check if not last step
        if (Build.current.stepNumber < setGeometry.totalSteps) Build.current.stepNumber += 1;
        else return;

        // update visible blocks
        // shownObject.transform.GetChild(Build.current.stepNumber-1).gameObject.SetActive(true);

        // update ui
        Transform panel = Screen.transform.Find("Panel");
        panel.GetComponentInChildren<Text>().text = Build.current.stepNumber.ToString() + " out of " + setGeometry.totalSteps.ToString();
    }

    public void Previous()
    {
        // check if not first step
        if (Build.current.stepNumber > 1) Build.current.stepNumber -= 1;
        else return;

        // update visible blocks
        // shownObject.transform.GetChild(Build.current.stepNumber).gameObject.SetActive(false);

        // update ui
        Transform panel = Screen.transform.Find("Panel");
        panel.GetComponentInChildren<Text>().text = Build.current.stepNumber.ToString() + " out of " + setGeometry.totalSteps.ToString();
    }

    // called when go back button is pressed
    public void GoToPreviousScreen()
    {
        if (previousScreen == overviewScreen)
        {
            detailScreen.gameObject.SetActive(false);
            overviewScreen.gameObject.SetActive(true);

            previousScreen = null;
        } else if (previousScreen == detailScreen)
        {
            appMode = AppMode.ONBOARDING;
            SaveLoad.Save();

            Screen.gameObject.SetActive(false);
            detailScreen.gameObject.SetActive(true);

            previousScreen = overviewScreen;
        }
    }

    public void EnableARCursor(Vector3 position, Quaternion rotation)
    {
        if (!shownObject)
        {
            ARCursor.SetActive(true);
            ARCursor.transform.position = position;
            ARCursor.transform.rotation = rotation;
        }
        else ARCursor.SetActive(false);
    }

    public void DisableARCursor()
    {
        ARCursor.SetActive(false);
    }

    private void Update()
    {
        if (appMode != AppMode.PLACING) return;

        if (shownObject) return;

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
                // place geometry for chosen lego set
                shownObject = Instantiate(setGeometry.parentObject, ARCursor.transform);
                Debug.Log(shownObject + " " + shownObject.transform);

                StartBuilding();
            }
        }

        last_phase = touch.phase;
    }

    public void DeleteObject()
    {
        UnityEngine.Object.Destroy(shownObject);
        shownObject = null;
    }

    private void OnApplicationPause(bool pause)
    {
        if(Build.current != null) SaveLoad.Save();
    }

    private void OnApplicationQuit()
    {
        if (Build.current != null) SaveLoad.Save();

        UnityEngine.Object.Destroy(ARCursor);
        if (shownObject) UnityEngine.Object.Destroy(shownObject);
        shownObject = null;
    }

    private void OnDisable()
    {
        if (Build.current != null) SaveLoad.Save();

        UnityEngine.Object.Destroy(ARCursor);
        if (shownObject) UnityEngine.Object.Destroy(shownObject);
        shownObject = null;
    }
}
