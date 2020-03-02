using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using UnityEngine;

public struct LifeTime : IComponentData
{
    public float Value;
}
public class LifeTimeSystem : JobComponentSystem
{
    EndSimulationEntityCommandBufferSystem m_Barrier;

    protected override void OnCreate()
    {
        base.OnCreate();
        m_Barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    [BurstCompile]
    public struct LifeTimeJob : IJobForEachWithEntity<LifeTime>
    {
        [WriteOnly]
        public EntityCommandBuffer.Concurrent CommandBuffer;

        public float DeltaTime;

        public void Execute(Entity entity, int index, ref LifeTime lifeTime)
        {
            lifeTime.Value -= DeltaTime;
            if (lifeTime.Value < 0)
                CommandBuffer.AddComponent<DestroyTag>(index, entity);

        }
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new LifeTimeJob
        {
            CommandBuffer = m_Barrier.CreateCommandBuffer().ToConcurrent(),
            DeltaTime = Time.DeltaTime
        }.Schedule(this, inputDeps);

        m_Barrier.AddJobHandleForProducer(job);
        return job;
    }
}
