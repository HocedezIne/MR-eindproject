using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SimulatorWindow : EditorWindow
{

    [MenuItem("Tools/Simulator")]
    public static void Init()
    {
        EditorWindow.GetWindow<SimulatorWindow>().Show();
    }

    private float speed = 2.0f;
    private float xrot = 0.0f;
    private float yrot = 0.0f;

    public SimulatorWindow()
    {
        EditorApplication.playModeStateChanged += this.PlayModeChanged;
    }

    void PlayModeChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            EditorSceneManager.LoadSceneInPlayMode("Assets/Editor/Scenes/Simulator.unity", new LoadSceneParameters(LoadSceneMode.Additive));

        }
    }

    private void OnGUI()
    {
        if (Application.isPlaying)
        {
            EditorGUILayout.BeginHorizontal();
            speed = EditorGUILayout.FloatField("Speed", speed);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.RepeatButton("Forward"))
            {
                MoveForward(speed);
                Repaint();
            }
            if (GUILayout.RepeatButton("Backward"))
            {
                MoveBackward(speed);
                Repaint();
            }
            if (GUILayout.RepeatButton("Left"))
            {
                MoveLeft(speed);
                Repaint();
            }
            if (GUILayout.RepeatButton("Right"))
            {
                MoveRight(speed);
                Repaint();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginVertical();
            xrot = EditorGUILayout.Slider("Rotation Up and Down", xrot, -90.0f, 90.0f);
            yrot = EditorGUILayout.Slider("Rotation Left and Right", yrot, -90.0f, 90.0f);
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("Reset"))
            {
                xrot = 0.0f;
                yrot = 0.0f;
            }

            SetRotation(xrot, yrot);
        }
        else
        {
            GUILayout.Label("Unity is not playing");
        }
    }

    void MoveForward(float speed)
    {
        SimulatorController[] controllers = Object.FindObjectsOfType<SimulatorController>();
        foreach (var controller in controllers)
        {
            controller.MoveForward(speed);
        }
    }

    void MoveBackward(float speed)
    {
        SimulatorController[] controllers = Object.FindObjectsOfType<SimulatorController>();
        foreach (var controller in controllers)
        {
            controller.MoveBackward(speed);
        }
    }

    void MoveLeft(float speed)
    {
        SimulatorController[] controllers = Object.FindObjectsOfType<SimulatorController>();
        foreach (var controller in controllers)
        {
            controller.MoveLeft(speed);
        }
    }

    void MoveRight(float speed)
    {
        SimulatorController[] controllers = Object.FindObjectsOfType<SimulatorController>();
        foreach (var controller in controllers)
        {
            controller.MoveRight(speed);
        }
    }

    void SetRotation(float xrot, float yrot)
    {
        SimulatorController[] controllers = Object.FindObjectsOfType<SimulatorController>();
        foreach (var controller in controllers)
        {
            controller.SetRotation(xrot, yrot);
        }
    }

}