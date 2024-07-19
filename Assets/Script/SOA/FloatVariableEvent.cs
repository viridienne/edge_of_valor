using UnityEngine;
using UnityEngine.Events;

public class FloatVariableEvent : ScriptableObject
{
    public event UnityAction<float> OnFloatVariableChanged;
    public void Raise(float value)
    {
        OnFloatVariableChanged?.Invoke(value);
    }
    public void RegisterListener(UnityAction<float> listener)
    {
        OnFloatVariableChanged += listener;
    }
    public void UnregisterListener(UnityAction<float> listener)
    {
        OnFloatVariableChanged -= listener;
    }

}