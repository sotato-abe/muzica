using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PointDatabase : MonoBehaviour
{
    List<Point> pointList = new List<Point>();

    private void Awake()
    {
        pointList.Clear();
    }

    public void AddPoint(Point point)
    {
        if (pointList.Any(p => p.Base == point.Base))
        {
            Debug.LogWarning("Point already exists in the database.");
            return;
        }
        pointList.Add(point);
    }

    public Point GetPoint(PointBase baseData)
    {
        Point point = pointList.FirstOrDefault(point => point.Base == baseData);
        if (point == null)
        {
            point = Point.CreateFrom(baseData);
            AddPoint(point);
        }
        return point;
    }

    public void ResetMerchandise()
    {
        foreach (var point in pointList)
        {
            point.ResetMerchandise();
        }
    }
}
