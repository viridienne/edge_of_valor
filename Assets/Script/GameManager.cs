using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private static GameManager _instance;
    private ControlPlayer _player;
    public ControlPlayer Player => _player;
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
    
    public void SetPlayer(ControlPlayer player)
    {
        _player = player;
    }
}
