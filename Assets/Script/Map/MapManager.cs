using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    private static MapManager _instance;

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
    }
    private MapController _mapController;
    public MapController MapController => _mapController;

    private void Start()
    {
        FindMapController();
    }
    
    public void FindMapController()
    {
        _mapController = FindObjectOfType<MapController>();
    }

}
