using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GOAPAgent : MonoBehaviour
{
    [Title("Sensors")]
    [SerializeField] private Sensor _chaseSensor;
    [SerializeField] private Sensor _attackSensor;
} 
