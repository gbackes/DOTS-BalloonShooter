using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitateAnim : MonoBehaviour
{
    public Vector3 DisplacementAmount;
    public float Speed = 1;

    Vector3 startPosition;
    private void Awake()
    {
        startPosition = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = startPosition + DisplacementAmount * Mathf.Sin(Time.time * Speed);
    }
}
