using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Build: IEquatable<Build>
{
    // variables
    public static Build current;
    public int _id;
    public string name;
    public int stepNumber = 1;

    // needed to check if two builds are the same
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
