using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
    public int _lineID { get; private set; }
    public Vector3 _coords { get; private set; }
    public Vector3 _normal { get; private set; }
    public Vector2 _positionneur { get; private set; }
    public int _level { get; private set; }
    public bool _arc { get; private set; }
    public float _speed { get; private set; }

    public Point()
    {
        _coords = new Vector3();
        _normal = new Vector3();
        _positionneur = new Vector2();
        _level = 0;
        _arc = false;
        _lineID = 0;
    }
    public Point(int lineID, int level, bool arc, Vector3 coords, Vector3 normal, Vector2 positionneur, float speed)
    {
        _lineID = lineID;
        _level = level;
        _arc = arc;
        _coords = coords;
        _normal = normal;
        _positionneur = positionneur;
        _speed = speed;

    }

    public void printPoint()
    {
        Debug.Log("Point:" +
        "Point:" +
        "\n\tN: " + _lineID +
        "\n\tLevel: " + _level +
        "\n\tArc: " + _arc +
        "\n\tCoords: " + _coords +
        "\n\tNormal: " + _normal +
        "\n\tPositionneur: " + _positionneur +
        "\n\tSpeed: " + _speed);
    }
}