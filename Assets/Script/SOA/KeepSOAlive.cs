using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// KeepSOAlive is a MonoBehaviour that ensures certain ScriptableObjects persist across scene loads.
/// Scriptable Objects WILL BE CLEANED by Garbage Collector if there are no references to it.
/// </summary>
public class KeepSOAlive : MonoBehaviour
{
    [SerializeField] private List<ScriptableObject> _ScriptableObjects;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
