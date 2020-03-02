using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Physics;

public class GunController : MonoBehaviour
{
    public static GunController Instance;
    public ProjectileAuthoring projectilePrefab;
    public float ShotDelay;
    public float ProjectileSpeed;
    public float ProjectileLifeTime;
    public Transform gunTransform;
    public Animator animator;
    private float timer;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    void Update()
    {
        animator.SetBool("Shoot", false);
        if (Input.GetMouseButtonDown(0))
            timer = ShotDelay;
        if(Input.GetMouseButton(0))
        {
            timer += Time.deltaTime;
            if(timer > ShotDelay)
            {
                timer -= ShotDelay;
                ProjectileAuthoring instance = Instantiate(projectilePrefab);

                instance.transform.position = gunTransform.transform.position;
                instance.transform.rotation = gunTransform.transform.rotation;
                instance.Velocity = gunTransform.transform.forward * ProjectileSpeed;
                instance.LifeTime = ProjectileLifeTime;
                animator.speed = 1f / ShotDelay;
                animator.SetBool("Shoot", true);
            }
        }
    }
}
