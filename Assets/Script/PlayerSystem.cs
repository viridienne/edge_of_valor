using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Player System", fileName = "New Player System")]
public class PlayerSystem : ScriptableObject, ISODispose
{
    private Transform _playerTransform;
    private ISODispose _isoDisposeImplementation;
    public Transform PlayerTransform => _playerTransform;
    public void RegisterPlayer(Transform player)
    {
        _playerTransform = player;
    }

    public void UnregisterPlayer()
    {
        _playerTransform = null;
    }

    public void OnDisable()
    {
        Dispose();
    }
    
    public DisposeAction OnDisableAction
    {
        get => _isoDisposeImplementation.OnDisableAction;
        set => _isoDisposeImplementation.OnDisableAction = value;
    }

    public void Dispose()
    {
        if(_isoDisposeImplementation.OnDisableAction == DisposeAction.ClearOnDisable)
        {
            _playerTransform = null;
        }
    }
}
