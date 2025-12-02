using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UtilityToolkit.Runtime;

public class Builder : MonoBehaviour
{
    [SerializeField] private SquareComponent _squareComponent;
    private Camera _camera;
    private readonly Container _container = new();

    [SerializeField] private float _tileSize;

    [SerializeField] private List<SquareComponent> _pool;
    
    
    private void Awake()
    {
        _camera = Camera.main;
        DrawFromPool();
    }

    private void DrawFromPool()
    {
        var abstractPosition = GetAbstractPosition(Mouse.current.position.ReadValue());
        var buildPosition = GetBuildPosition(abstractPosition);
        _squareComponent = Instantiate(_pool.RandomElement(), buildPosition, Quaternion.identity);
    }

    void Update()
    {
        HandleRotation();
        HandleBuilding();
    }

    private void HandleRotation()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            _squareComponent.transform.Rotate(0, 90, 0);
            _squareComponent.Square.Rotate(true);
        }
    }

    private void HandleBuilding()
    {
        var abstractPosition = GetAbstractPosition(Mouse.current.position.ReadValue());
        if (_container.Contains(abstractPosition))
        {
            _squareComponent.gameObject.SetActive(false);
        }
        else
        {
            _squareComponent.gameObject.SetActive(true);
            var buildPosition = GetBuildPosition(abstractPosition);
            _squareComponent.transform.position = buildPosition;

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                var square = _squareComponent.Square;
                if (!_container.Fits(abstractPosition, square)) return;

                _container.Add(abstractPosition, square);
                DrawFromPool();
            }
        }
    }


    private Vector2Int GetAbstractPosition(Vector3 mousePosition)
    {
        Ray ray = _camera.ScreenPointToRay(mousePosition);

        Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Plane"));

        Vector3 hoverPosition = hit.point + hit.normal;

        hoverPosition /= _tileSize;

        var abstractPosition = new Vector2Int(
            Mathf.RoundToInt(hoverPosition.x),
            Mathf.RoundToInt(hoverPosition.z)
        );

        return abstractPosition;
    }

    private Vector3 GetBuildPosition(Vector2Int gridPosition)
    {
        return new Vector3(
            gridPosition.x * _tileSize,
            0,
            gridPosition.y * _tileSize
        );
    }
}