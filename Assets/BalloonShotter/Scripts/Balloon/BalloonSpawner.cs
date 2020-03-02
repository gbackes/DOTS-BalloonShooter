using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct BalloonSpawner : IComponentData
{
    public Entity Prefab;
    public float2 Extends;
    public float SpawnDelay;
    public float MinGravityFactor;
    public float MaxGravityFactor;
    public float LifeTime;
    public float DeltaTime;
}
