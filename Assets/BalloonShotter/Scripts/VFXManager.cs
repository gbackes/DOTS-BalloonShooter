using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance;

    public VisualEffect VFXPrefab;
    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void Play(Vector3 position)
    {
        StartCoroutine(PlayCoroutine(position));
    }

    IEnumerator PlayCoroutine(Vector3 position)
    {
        VisualEffect instance = Instantiate(VFXPrefab, position, Quaternion.identity);
        yield return new WaitForSeconds(10);
        Destroy(instance.gameObject);
    }


}
