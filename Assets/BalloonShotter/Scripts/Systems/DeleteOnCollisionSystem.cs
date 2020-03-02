using Unity.Jobs;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Collections;

public struct BalloonTag : IComponentData
{
    public bool PlayVFX;
}
public struct ProjectileTag : IComponentData {}

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class DeleteOnCollisionSystem : JobComponentSystem
{
    EndSimulationEntityCommandBufferSystem m_EntityCommandBufferSystem;

    BuildPhysicsWorld m_BuildPhysicsWorldSystem;
    StepPhysicsWorld m_StepPhysicsWorldSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        m_BuildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
        m_StepPhysicsWorldSystem = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    public struct ProcessCollisionEventsJob : ICollisionEventsJob
    {
        public ComponentDataFromEntity<BalloonTag> Balloons;
        [ReadOnly] public ComponentDataFromEntity<ProjectileTag> Projectiles;

        public EntityCommandBuffer commandBuffer;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.Entities.EntityA;
            Entity entityB = collisionEvent.Entities.EntityB;

            if(Projectiles.Exists(entityA) && Balloons.Exists(entityB))
            {
                commandBuffer.AddComponent<DestroyTag>(entityA);
                commandBuffer.AddComponent<DestroyTag>(entityB);
                commandBuffer.SetComponent(entityB, new BalloonTag { PlayVFX = true });
            }
            else if (Projectiles.Exists(entityB) && Balloons.Exists(entityA))
            {
                commandBuffer.AddComponent<DestroyTag>(entityB);
                commandBuffer.AddComponent<DestroyTag>(entityA);
                commandBuffer.SetComponent(entityA, new BalloonTag { PlayVFX = true });
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new ProcessCollisionEventsJob
        {
            Projectiles = GetComponentDataFromEntity<ProjectileTag>(true),
            Balloons = GetComponentDataFromEntity<BalloonTag>(false),
            commandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer()
        }.Schedule(m_StepPhysicsWorldSystem.Simulation, ref m_BuildPhysicsWorldSystem.PhysicsWorld, inputDeps);

        m_EntityCommandBufferSystem.AddJobHandleForProducer(job);
        return job;
    }
}
