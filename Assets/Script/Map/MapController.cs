using UnityEngine;

public class MapController : MonoBehaviour, IMap
{
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;
    [SerializeField] private float _minY;
    [SerializeField] private float _maxY;
    [SerializeField] private MapControl _mapControl;
    
    public float MinX => _minX;
    public float MaxX => _maxX;
    public float MinY => _minY;
    public float MaxY => _maxY;
    
    private void OnEnable()
    {
        _mapControl.RegisterMapController(this);
    }

    private void OnDisable()
    {
        _mapControl.UnregisterMapController(this);
    }
}
