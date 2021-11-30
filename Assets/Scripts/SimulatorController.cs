using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulatorController : MonoBehaviour
{

#if UNITY_EDITOR

    public AppManager app;

    private Camera cam;
    private Vector2 mouse_position = new Vector2();

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    public void MoveForward(float speed)
    {
        transform.localPosition += transform.TransformDirection(Vector3.forward * speed * Time.deltaTime);
    }

    public void MoveBackward(float speed)
    {
        transform.localPosition += transform.TransformDirection(Vector3.back * speed * Time.deltaTime);
    }

    public void MoveLeft(float speed)
    {
        transform.localPosition += transform.TransformDirection(Vector3.left * speed * Time.deltaTime);
    }

    public void MoveRight(float speed)
    {
        transform.localPosition += transform.TransformDirection(Vector3.right * speed * Time.deltaTime);
    }

    public void SetRotation(float xrot, float yrot)
    {
        transform.localRotation = Quaternion.Euler(xrot, yrot, 0.0f);
    }

    void OnGUI()
    {
        Event e = Event.current;
        if (e.type == EventType.Repaint)
        {
            mouse_position.x = e.mousePosition.x;
            mouse_position.y = Screen.height - e.mousePosition.y;
        }
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(mouse_position);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.scene.name == "Simulator")
            {
                app.EnableARCursor(hit.point, Quaternion.FromToRotation(hit.transform.up, hit.normal));
            }
            else
            {
                app.DisableARCursor();
            }
        }
        else
        {
            app.DisableARCursor();
        }
    }

#endif

}