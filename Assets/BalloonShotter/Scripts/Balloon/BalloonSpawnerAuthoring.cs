using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class BalloonSpawnerAuthoring : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
    public GameObject m_Prefab;
    public Vector2 m_Size;
    public float m_SpawnRate;
    public float m_MinGravityFactor;
    public float m_MaxGravityFactor;
    public float m_LifeTime;
    

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var spawnerData = new BalloonSpawner
        {
            Prefab = conversionSystem.GetPrimaryEntity(m_Prefab),
            Extends = m_Size/2,
            SpawnDelay = 1f / m_SpawnRate,
            MinGravityFactor = m_MinGravityFactor,
            MaxGravityFactor = m_MaxGravityFactor,
            LifeTime = m_LifeTime,
            DeltaTime = 0
        };
        dstManager.AddComponentData(entity, spawnerData);
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(m_Prefab);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(m_Size.x, 1, m_Size.y));
    }
}
