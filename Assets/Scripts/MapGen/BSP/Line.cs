using UnityEngine;

// All code has been produced following the tutorial by Sunny Valley Studio, 2019 (https://www.youtube.com/watch?v=VnqN0v95jtU&list=PLcRSafycjWFfEPbSSjGMNY-goOZTuBPMW&index=1)
public class Line
{
    Orientation orientation;
    Vector2Int coordinates;

    public Line(Orientation orientation, Vector2Int coordinates)
    {
        this.Orientation = orientation;
        this.Coordinates = coordinates;
    }

    public Orientation Orientation { get => orientation; set => orientation = value; }
    public Vector2Int Coordinates { get => coordinates; set => coordinates = value; }
}
public enum Orientation
{ 
    Horizontal = 0,
    Vertical = 1

}