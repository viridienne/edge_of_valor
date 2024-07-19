using UnityEngine;
using UnityEngine.Events;

public class  FloatVariableEventListener : MonoBehaviour
{
    public FloatVariableEvent Event;
    public UnityEvent<float> Response;

    private void OnEnable()
    {
        Event.RegisterListener(OnEventRaised);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(OnEventRaised);
    }

    public void OnEventRaised(float value)
    {
        Response?.Invoke(value);
    }
}