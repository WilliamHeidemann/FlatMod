using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UtilityToolkit.Runtime;

public class Builder : MonoBehaviour
{
    [SerializeField] private SquareComponent _squareComponent;
    private Camera _camera;
    private readonly Container _container = new();

    [SerializeField] private float _tileSize;

    [SerializeField] private SerializedWeightedCollection<SquareComponent> _serializedWeightedCollection;
    private WeightedCollection<SquareComponent> _pool;

    private Quaternion _targetRotation = Quaternion.identity;
    private const float RotationTime = 0.18f;
    private float _timeOfLastRotation;
    [SerializeField] private float _lastScrollValue;

    private void Awake()
    {
        _camera = Camera.main;
        _pool = _serializedWeightedCollection.Build();
        DrawFromPool();
    }

    private void DrawFromPool()
    {
        var abstractPosition = GetAbstractPosition(Mouse.current.position.ReadValue());
        var buildPosition = GetBuildPosition(abstractPosition);
        _targetRotation = Quaternion.identity;
        _squareComponent = Instantiate(_pool.GetRandomWeightedElement(), buildPosition, _targetRotation);
        Selection.activeGameObject = _squareComponent.gameObject;
    }

    void Update()
    {
        HandleRotation();
        HandleBuilding();

        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            if (_squareComponent != null)
            {
                Destroy(_squareComponent.gameObject);
            }

            DrawFromPool();
        }
    }

    private void HandleRotation()
    {
        if (RotationTime > Time.time - _timeOfLastRotation)
        {
            return;
        }

        if (IsScrolling(out var clockWise))
        {
            _targetRotation *= Quaternion.Euler(0, clockWise ? 90 : -90, 0);
            _squareComponent.transform.DORotateQuaternion(_targetRotation, RotationTime).SetEase(Ease.OutExpo);
            _squareComponent.Square.Rotate(clockWise);
            _timeOfLastRotation = Time.time;
        }
    }

    private bool IsScrolling(out bool clockWise)
    {
        clockWise = default;
        _lastScrollValue -= Time.deltaTime;
        var scroll = Mouse.current.scroll.y.ReadValue();
        var scrollAbs = Mathf.Abs(scroll);
        if (scrollAbs < _lastScrollValue || scrollAbs < 0.1f)
        {
            return false;
        }

        _lastScrollValue = scrollAbs;
        clockWise = scroll > 0;
        return true;
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