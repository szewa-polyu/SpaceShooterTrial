using System;

public enum RestrictPositionMode
{
    Clamp,
    PeriodicBC
}

[Serializable]
public struct Boundary
{
    public float XMin, XMax, ZMin, ZMax;
}