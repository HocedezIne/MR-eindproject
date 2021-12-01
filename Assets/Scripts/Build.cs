using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildState {LOADED, BUILDING, DONE}

[System.Serializable]
public class Build: IEquatible<Build>
{
    public static Build current;
    public int _id;
    public string name;
    public BuildState state;
    public int stepNumber;
    public int totalSteps;

    public Build()
    {
        _id = 0;
        name = "Test";
        state = BuildState.BUILDING;
        stepNumber = 0;
        totalSteps = 10;
    }

    public bool Equals(Build other)
    {
        return null != other && _id == other._id;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Build);
    }

    public override int GetHashCode()
    {
        return _id;
    }
}
