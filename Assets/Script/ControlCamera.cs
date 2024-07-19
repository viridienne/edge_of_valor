using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class ControlCamera : MonoBehaviour
{
    [SerializeField] private float distance = -10f;
    [ShowInInspector] private Camera _camera;
    [SerializeField] private PlayerSystem _playerSystem;
    [SerializeField] private MapControl _mapControl;
    private Transform _mainPlayer => _playerSystem.PlayerTransform;
    private Transform _cameraTransform;
    
    private float _orthoSize => _camera.orthographicSize;
    private float _camHeight => _orthoSize * 2;
    private float _camWidth => _camHeight * _camera.aspect;
    private float _clampMinX => _mapControl.MinX;
    private float _clampMaxX => _mapControl.MaxX;

    private float _clampMinY => _mapControl.MinY;

    private float _clampMaxY => _mapControl.MaxY;
    
    
    private void Awake()
    {
        _camera = Camera.main;
        if (_camera != null) _cameraTransform = _camera.transform;
    }
    
    private void LateUpdate()
    {
        if(_mainPlayer == null) return;
        var playerPos = _mainPlayer.position;
        var cameraPos = _cameraTransform.position;
        
        cameraPos.x = playerPos.x;
        cameraPos.y = playerPos.y;
        cameraPos.z = distance;
        
        cameraPos.x = Mathf.Clamp(cameraPos.x, _clampMinX + _camWidth/2, _clampMaxX - _camWidth/2);
        cameraPos.y = Mathf.Clamp(cameraPos.y, _clampMinY + _camHeight/2, _clampMaxY - _camHeight/2);
        
        _cameraTransform.position = cameraPos;
    }
}
