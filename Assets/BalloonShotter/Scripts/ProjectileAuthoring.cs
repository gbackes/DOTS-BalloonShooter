using UnityEngine;
using Unity.Entities;
using System.Collections.Generic;
using Unity.Transforms;
using Unity.Physics;

public class ProjectileAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public Vector3 Velocity;
    public float LifeTime;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.SetComponentData(entity, new Translation { Value = transform.position });
        dstManager.SetComponentData(entity, new Rotation { Value = transform.rotation });
        dstManager.AddComponentData(entity, new PhysicsVelocity { Linear = Velocity });
        dstManager.AddComponentData(entity, new LifeTime { Value = LifeTime });
        dstManager.AddComponentData(entity, new ProjectileTag { });
        dstManager.AddComponentData(entity, new CopyTransformToGameObject { });
       
    }

}
