using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMap
{
   public float MinX { get; }
   public float MaxX { get; }

   public float MinY { get; }
   public float MaxY { get; }
}
