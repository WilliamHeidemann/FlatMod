using System;

[Serializable]
public class Square
{
    public bool PointsForward;
    public bool PointsRight;
    public bool PointsBackward;
    public bool PointsLeft;
    
    public void Rotate(bool clockWise)
    {
        bool originalForward = PointsForward;
        bool originalRight = PointsRight;
        bool originalBackward = PointsBackward;
        bool originalLeft = PointsLeft;

        PointsRight = clockWise ? originalForward : originalBackward;
        PointsBackward = clockWise ? originalRight : originalLeft;
        PointsLeft = clockWise ? originalBackward : originalForward;
        PointsForward = clockWise ? originalLeft : originalRight;
    }
}