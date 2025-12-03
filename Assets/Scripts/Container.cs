using System.Collections.Generic;
using UnityEngine;

public class Container
{
    private readonly Dictionary<Vector2Int, Square> _positions = new();
    
    public void Add(Vector2Int position, Square square)
    {
        if (Contains(position)) return;
        
        _positions.Add(position, square);
    }
    
    public bool Contains(Vector2Int position) => 
        _positions.ContainsKey(position);
    
    public bool Fits(Vector2Int position, Square square)
    {
        if (Contains(position))
        {
            Debug.Log("Position already occupied: " + position);
            return false;
        }
        
        if (!ConnectsToNeighbors(position, square))
        {
            Debug.Log("Does not connect to neighbors at position: " + position);
            return false;
        }
        
        if (BlocksPath(position, square))
        {
            Debug.Log("Blocks path at position: " + position);
            return false;
        }
        
        return true;
    }

    private bool ConnectsToNeighbors(Vector2Int position, Square square)
    {
        // Check forward
        if (square.PointsForward)
        {
            if (_positions.TryGetValue(position + Vector2Int.up, out var neighbor))
            {
                if (!neighbor.PointsBackward) 
                {
                    Debug.Log("Failed forward check with neighbor at " + (position + Vector2Int.up));
                    return false;
                }
            }
        }
        
        // Check right
        if (square.PointsRight)
        {
            if (_positions.TryGetValue(position + Vector2Int.right, out var neighbor))
            {
                if (!neighbor.PointsLeft) 
                {
                    Debug.Log("Failed right check with neighbor at " + (position + Vector2Int.right));
                    return false;
                }
            }
        }
        
        // Check backward
        if (square.PointsBackward)
        {
            if (_positions.TryGetValue(position + Vector2Int.down, out var neighbor))
            {
                if (!neighbor.PointsForward) 
                {
                    Debug.Log("Failed backward check with neighbor at " + (position + Vector2Int.down));
                    return false;
                }
            }
        }
        
        // Check left
        if (square.PointsLeft)
        {
            if (_positions.TryGetValue(position + Vector2Int.left, out var neighbor))
            {
                if (!neighbor.PointsRight) 
                {
                    Debug.Log("Failed left check with neighbor at " + (position + Vector2Int.left));
                    return false;
                }
            }
        }

        return true;
    }

    private bool BlocksPath(Vector2Int position, Square square)
    {
        // Check forward
        if (!square.PointsForward)
        {
            if (_positions.TryGetValue(position + Vector2Int.up, out var neighbor))
            {
                if (neighbor.PointsBackward)
                {
                    Debug.Log("Blocks path forward with neighbor at " + (position + Vector2Int.up));
                    return true;
                }
            }
        }
        
        // Check right
        if (!square.PointsRight)
        {
            if (_positions.TryGetValue(position + Vector2Int.right, out var neighbor))
            {
                if (neighbor.PointsLeft)
                {
                    Debug.Log("Blocks path right with neighbor at " + (position + Vector2Int.right));
                    return true;
                }
            }
        }
        
        // Check backward
        if (!square.PointsBackward)
        {
            if (_positions.TryGetValue(position + Vector2Int.down, out var neighbor))
            {
                if (neighbor.PointsForward)
                {
                    Debug.Log("Blocks path backward with neighbor at " + (position + Vector2Int.down));
                    return true;
                }
            }
        }
        
        // Check left
        if (!square.PointsLeft)
        {
            if (_positions.TryGetValue(position + Vector2Int.left, out var neighbor))
            {
                if (neighbor.PointsRight)
                {
                    Debug.Log("Blocks path left with neighbor at " + (position + Vector2Int.left));
                    return true;
                }
            }
        }
        
        return false;
    }
}