using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Map Control", fileName = "New Map Control")]
public class MapControl : ScriptableObject, ISODispose, IMap
{
    private MapController _mapController;
    public MapController MapController => _mapController;

    public float MinX
    {
        get
        {
            if (_mapController == null)
            {
                return 0;
            }
            return _mapController.MinX;
        }
    }

    public float MaxX
    {
        get
        {
            if (_mapController == null)
            {
                return 0;
            }
            return _mapController.MaxX;
        }
    }
    public float MinY
    {
        get
        {
            if (_mapController == null)
            {
                return 0;
            }
            return _mapController.MinY;
        }
    }
    public float MaxY
    {
        get
        {
            if (_mapController == null)
            {
                return 0;
            }
            return _mapController.MaxY;
        }
    }
    
    public void RegisterMapController(MapController mapController)
    {
        _mapController = mapController;
    }

    public void UnregisterMapController(MapController mapController)
    {
        _mapController = null;
    }

    public void ClearMapControllers()
    {
        _mapController = null;
    }

    private void OnDisable()
    {
        Dispose();
    }

    public DisposeAction OnDisableAction { get; set; }

    public void Dispose()
    {
        if (OnDisableAction == DisposeAction.ClearOnDisable || OnDisableAction == DisposeAction.SceneClear)
        {
            ClearMapControllers();
        }
    }
}
