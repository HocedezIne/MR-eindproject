using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneObserver : MonoBehaviour
{
    public AppManager app;
    public ARRaycastManager raycast_manager;

    private Vector2 screenpoint = new Vector2(Screen.width / 2, Screen.height / 2);
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // Update is called once per frame
    void Update()
    {
        if (app.RunInSimulator()) return;

        if (app.BuildMode == true)
        {
            if (raycast_manager.Raycast(screenpoint, hits))
            {
                app.EnableARCursor(hits[0].pose.position, hits[0].pose.rotation);
            }
            else
            {
                app.DisableARCursor();
            }
        }
    }
}
