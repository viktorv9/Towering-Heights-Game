using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    [SerializeField] private float delay;
    private float timeSpawn;

    private void Awake()
    {
        timeSpawn = Time.time;
    }

    void Update()
    {
        if (Time.time > timeSpawn + delay) {
            Destroy(gameObject);
        }
    }
}
