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
            Debug.Log("Position already occupied.");
            return false;
        }
        
        if (!ConnectsToNeighbors(position, square))
        {
            Debug.Log("Does not connect to neighbors.");
            return false;
        }
        
        if (BlocksPath(position, square))
        {
            Debug.Log("Blocks path.");
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
                if (!neighbor.PointsBackward) return false;
            }
        }
        
        // Check right
        if (square.PointsRight)
        {
            if (_positions.TryGetValue(position + Vector2Int.right, out var neighbor))
            {
                if (!neighbor.PointsLeft) return false;
            }
        }
        
        // Check backward
        if (square.PointsBackward)
        {
            if (_positions.TryGetValue(position + Vector2Int.down, out var neighbor))
            {
                if (!neighbor.PointsForward) return false;
            }
        }
        
        // Check left
        if (square.PointsLeft)
        {
            if (_positions.TryGetValue(position + Vector2Int.left, out var neighbor))
            {
                if (!neighbor.PointsRight) return false;
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
                if (neighbor.PointsBackward) return true;
            }
        }
        
        // Check right
        if (!square.PointsRight)
        {
            if (_positions.TryGetValue(position + Vector2Int.right, out var neighbor))
            {
                if (neighbor.PointsLeft) return true;
            }
        }
        
        // Check backward
        if (!square.PointsBackward)
        {
            if (_positions.TryGetValue(position + Vector2Int.down, out var neighbor))
            {
                if (neighbor.PointsForward) return true;
            }
        }
        
        // Check left
        if (!square.PointsLeft)
        {
            if (_positions.TryGetValue(position + Vector2Int.left, out var neighbor))
            {
                if (neighbor.PointsRight) return true;
            }
        }
        
        return false;
    }
}