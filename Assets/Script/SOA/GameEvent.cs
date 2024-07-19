using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum DisposeAction
{
    None,
    ClearOnDisable,
    SceneClear
}
public interface ISODispose
{
    public DisposeAction OnDisableAction { get; set; }
    public void Dispose();
}
/// <summary>
/// GameEvent is a ScriptableObject that represents a game event.
/// It maintains a list of listeners that are notified when the event is raised.
/// </summary>
[CreateAssetMenu(menuName = "Scriptable Object/Game Event", fileName = "New Game Event")]
public class GameEvent : ScriptableObject, ISODispose
{
    [SerializeField] private bool _logEvent;
    [SerializeField] private DisposeAction _onDisableAction;
    
    /// <summary>
    /// A list of listeners that are notified when the event is raised.
    /// </summary>
    private readonly List<GameEventListener> _listeners = new();

    
    /// <summary>
    /// Raises the event, notifying all registered listeners.
    /// </summary>
    [Button]
    public void Raise()
    {
        // Loop through all listeners and raise the event, starting from the last one to avoid any issues with removing listeners during the loop
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            if (_logEvent)
            {
                LogUtility.Log(LogTags.GAME_EVENT, $"Event raised by {name}");
            }
            _listeners[i].OnEventRaised();
        }
    }

    /// <summary>
    /// Registers a listener to be notified when the event is raised.
    /// </summary>
    /// <param name="listener">The listener to register.</param>
    public void RegisterListener(GameEventListener listener)
    {
        _listeners.Add(listener);
    }

    /// <summary>
    /// Unregisters a listener, so it will no longer be notified when the event is raised.
    /// </summary>
    /// <param name="listener">The listener to unregister.</param>
    public void UnregisterListener(GameEventListener listener)
    {
        _listeners.Remove(listener);
    }

    private void OnDisable()
    {
        Dispose();
    }

    public void Dispose()
    {
        if (_onDisableAction == DisposeAction.ClearOnDisable)
        {
            _listeners.Clear();
        }
    }

    DisposeAction ISODispose.OnDisableAction
    {
        get => _onDisableAction;
        set => _onDisableAction = value;
    }
}

public static class LogUtility
{
    public static void Log(string tag,string message)
    {
        Debug.Log($"[{tag}] {message}");
    }
    public static void LogWarning(string tag,string message)
    {
        Debug.LogWarning($"[{tag}] {message}");
    }
    public static void LogError(string tag,string message)
    {
        Debug.LogError($"[{tag}] {message}");
    }
    public static void LogColor(string tag,string message,Color color)
    {
        Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>[{tag}] {message}</color>");
    }
}

public static class LogTags
{
    public static string GAME_EVENT = "GameEvent";
}