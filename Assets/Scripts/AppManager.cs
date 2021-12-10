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
    }

    // call loading screen on start
    private void Start()
    {
        loadingScreen.gameObject.SetActive(true); // show loading screen

        Transform panel = overviewScreen.transform.Find("Panel"); // get the content panel
        Debug.Log(panel);
        Transform content = panel.transform.Find("Content"); // get the content panel
        Debug.Log(content);

        foreach (Build b in allSets)
        {
            // add button prefab to overviewscreen for each set
            Button button = Instantiate(setButtonPrefab, content);
            button.GetComponent<Image>().sprite = b.image;
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
        // set current build
        Debug.Log(EventSystem.current.currentSelectedGameObject.name);
        //Build.current = SaveLoad.savedBuilds[Int32.Parse(EventSystem.current.currentSelectedGameObject.name)];

        // load details onto detail page


        detailScreen.gameObject.SetActive(true);
        overviewScreen.gameObject.SetActive(false);
        previousScreen = overviewScreen;
    }

    // called when set is selected to be build
    public void StartPlacing()
    {
        appMode = AppMode.PLACING;

        // load data onto screen


        Screen.gameObject.SetActive(true);
        detailScreen.gameObject.SetActive(false);
        previousScreen = detailScreen;
    }

    // called when set is placed
    public void StartBuilding()
    {
        appMode = AppMode.BUILDING;
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
            Screen.gameObject.SetActive(false);
            detailScreen.gameObject.SetActive(true);
            previousScreen = overviewScreen;
        }
    }

    public void EnableARCursor(Vector3 position, Quaternion rotation)
    {
        ARCursor = Instantiate(ARCursorPrefab, transform);
        ARCursor.SetActive(true);
        ARCursor.transform.position = position;
        ARCursor.transform.rotation = rotation;
    }

    public void DisableARCursor()
    {
        // ARCursor.SetActive(false);
    }

    private void Update()
    {
        if (appMode != AppMode.PLACING) return;
        Debug.Log("appmode not placing");

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
                StartBuilding();
            }
        }

        last_phase = touch.phase;
    }
}
