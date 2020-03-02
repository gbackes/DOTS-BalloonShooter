
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Burst;
public class BalloonSpawnerSystem : JobComponentSystem
{
    BeginSimulationEntityCommandBufferSystem m_EntityCommandBufferSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
    }
    [BurstCompile]
    public struct SpawnerJob : IJobForEachWithEntity<BalloonSpawner, LocalToWorld>
    {
        private EntityCommandBuffer.Concurrent CommandBuffer;
        private Random Random;
        private readonly float DeltaTime;

        public SpawnerJob(EntityCommandBuffer.Concurrent commandBuffer, Random random, float deltaTime)
        {
            this.CommandBuffer = commandBuffer;
            this.Random = random;
            this.DeltaTime = deltaTime;
        }

        public void Execute(Entity entity, int index, ref BalloonSpawner balloonSpawner, [ReadOnly]ref LocalToWorld localToWorld)
        {
            balloonSpawner.DeltaTime += DeltaTime;

            while (balloonSpawner.DeltaTime > balloonSpawner.SpawnDelay)
            {
                balloonSpawner.DeltaTime -= balloonSpawner.SpawnDelay;

                var position = math.transform(localToWorld.Value, new float3(Random.NextFloat(-balloonSpawner.Extends.x, balloonSpawner.Extends.x),
                                                                                0,
                                                                                Random.NextFloat(-balloonSpawner.Extends.y, balloonSpawner.Extends.y)));
                var gravityFactor = Random.NextFloat(balloonSpawner.MinGravityFactor, balloonSpawner.MaxGravityFactor);

                Entity instance = CommandBuffer.Instantiate(index, balloonSpawner.Prefab);
                CommandBuffer.SetComponent(index, instance, new Translation { Value = position });
                CommandBuffer.AddComponent(index, instance, new LifeTime { Value = balloonSpawner.LifeTime });
                CommandBuffer.SetComponent(index, instance, new PhysicsGravityFactor { Value = gravityFactor });
                CommandBuffer.AddComponent(index, instance, new BalloonTag { PlayVFX = false });

            }
        }

    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var spawnerJob = new SpawnerJob(m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
                                        new Random((uint)UnityEngine.Random.Range(0, int.MaxValue)),
                                        Time.DeltaTime
                                        );
        JobHandle jobHandle = spawnerJob.Schedule(this, inputDeps);
        m_EntityCommandBufferSystem.AddJobHandleForProducer(jobHandle);
        return jobHandle;
    }
}
