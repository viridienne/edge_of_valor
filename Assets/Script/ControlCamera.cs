using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class ControlCamera : MonoBehaviour
{
    public static ControlCamera Instance;
    private  static ControlCamera _instance;
    [SerializeField] private float distance = -10f;
    [ShowInInspector] private Camera _camera;
    private Transform _mainPlayer;
    private Transform _cameraTransform;
    
    private float _orthoSize => _camera.orthographicSize;
    private float _camHeight => _orthoSize * 2;
    private float _camWidth => _camHeight * _camera.aspect;
    private float _clampMinX => MapManager.Instance.MapController.MinX;
    private float _clampMaxX => MapManager.Instance.MapController.MaxX;

    private float _clampMinY => MapManager.Instance.MapController.MinY;
    
    private float _clampMaxY => MapManager.Instance.MapController.MaxY;
    
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            Instance = _instance;
        }
        else
        {
            Destroy(gameObject);
        }
        
        _camera = Camera.main;
        if (_camera != null) _cameraTransform = _camera.transform;
    }
    
    public void SetMainPlayer(Transform player)
    {
        _mainPlayer = player;
    }
    
    private void LateUpdate()
    {
        if(_mainPlayer == null) return;
        Vector3 playerPos = _mainPlayer.position;
        Vector3 cameraPos = _cameraTransform.position;
        cameraPos.x = playerPos.x;
        cameraPos.y = playerPos.y;
        cameraPos.z = distance;
        
        cameraPos.x = Mathf.Clamp(cameraPos.x, _clampMinX + _camWidth/2, _clampMaxX - _camWidth/2);
        cameraPos.y = Mathf.Clamp(cameraPos.y, _clampMinY + _camHeight/2, _clampMaxY - _camHeight/2);
        
        _cameraTransform.position = cameraPos;
    }
}
