using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSystem : MonoBehaviour {

    [SerializeField] private GameObject targetHolder;
    [SerializeField] private float spawnInterval;
    
    private float timeLastSpawn = 0;
    LineRenderer lr;
    
    private struct TargetData
    {
        public Vector3 OriginPoint;
        public Vector3 TargetPoint;
    }
    
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.textureScale = new Vector2(1, 1);
        lr.widthMultiplier = 0.1f;
    }

    void Update()
    {
        if (timeLastSpawn + spawnInterval < Time.time) {
            SpawnTargetingLaser();
            timeLastSpawn = Time.time;
        }
    }

    void SpawnTargetingLaser() {
        TargetData? targetData = RerollTarget();
        if (targetData == null) return;

        lr.positionCount = 2;
        lr.SetPosition(0, targetData.Value.OriginPoint);
        lr.SetPosition(1, targetData.Value.TargetPoint);
    }

    TargetData? RerollTarget() {
        RaycastHit hit = default;
        Collider[] possibleTargets = targetHolder.GetComponentsInChildren<Collider>();
        if (possibleTargets.Length == 0) return null;
        
        bool foundHit = false;
        Vector3 originPoint = default;

        while (!foundHit) {
            Collider target = possibleTargets[Random.Range(0, possibleTargets.Length)];

            Bounds b = target.bounds;

            Vector3 targetPoint = new Vector3(
                Random.Range(b.min.x, b.max.x),
                Random.Range(b.min.y, b.max.y),
                Random.Range(b.min.z, b.max.z)
            );

            originPoint = Random.onUnitSphere * 100f;
            
            Vector3 randomDirection = (targetPoint - originPoint).normalized;

            foundHit = Physics.Raycast(originPoint, randomDirection, out hit, 150f);
            if (foundHit) {
                // quick dirty way of filtering out platform and camera blockers
                if (hit.transform.gameObject.name == "rock_platform" ||
                    hit.transform.gameObject.name == "CameraBlocker") foundHit = false;
            }
        }

        return new TargetData {
            OriginPoint = originPoint,
            TargetPoint = hit.point
        };
    }
}
