using UnityEngine;

public class ClampPosition : MonoBehaviour
{
    [SerializeField] private MapControl _mapControl;

    private Transform _tf;
    private void Awake()
    {
        _tf = transform;
    }

    private void LateUpdate()
    {
        var mapController = _mapControl.MapController;
        var minX = mapController.MinX;
        var maxX = mapController.MaxX;
        var minY = mapController.MinY;
        var maxY = mapController.MaxY;
        if (_tf.position.x > minX && _tf.position.x < maxX && _tf.position.y > minY && _tf.position.y < maxY) return;
        
        var _pos = _tf.position;
        _pos.x = Mathf.Clamp(_pos.x, minX, maxX);
        _pos.y = Mathf.Clamp(_pos.y, minY, maxY);
        _tf.position = _pos;
    }
}