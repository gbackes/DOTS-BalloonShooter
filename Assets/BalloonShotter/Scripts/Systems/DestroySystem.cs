
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public struct DestroyTag : IComponentData { }
public class DestroySystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAll<ProjectileTag, DestroyTag, Transform>().ForEach((Entity entity, Transform transform) =>
        {
            PostUpdateCommands.DestroyEntity(entity);
            UnityEngine.Object.Destroy(transform.gameObject);
        });
        Entities.WithAll<BalloonTag, DestroyTag>().ForEach((Entity entity, ref BalloonTag balloonTag, ref Translation translation) =>
        {
            if (balloonTag.PlayVFX == true)
                VFXManager.Instance.Play(translation.Value);

            PostUpdateCommands.DestroyEntity(entity);
        });
    }
}
