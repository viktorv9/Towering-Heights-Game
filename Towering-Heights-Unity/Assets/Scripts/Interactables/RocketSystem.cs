using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSystem : MonoBehaviour {

    [SerializeField] private GameObject targetHolder;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float spawnInterval;
    
    [SerializeField] private Material lineWarningMaterial;
    [SerializeField] private Material lineDangerMaterial;
    
    private float timeLastSpawn = 0;
    LineRenderer lr;
    GameObject projectileObj;
    TargetData? targetData;
    
    private ShapeDropper shapeDropper;
    
    private struct TargetData
    {
        public Vector3 OriginPoint;
        public Vector3 TargetPoint;
    }

    void Start()
    {
        shapeDropper = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<ShapeDropper>();
        
        lr = GetComponent<LineRenderer>();
        lr.textureScale = new Vector2(1, 1);
        lr.widthMultiplier = 0.1f;
    }

    void Update()
    {
        if (timeLastSpawn + spawnInterval < Time.time) {
            if (shapeDropper.GetHasWon()) return;
            
            SpawnTargetingLaser();
            timeLastSpawn = Time.time;
            timeLastSpawn += Random.Range(-spawnInterval/5, spawnInterval/5); // random variation
        }
        
        if (projectileObj != null && targetData.HasValue) {
            float projectileDistance = Vector3.Distance(projectileObj.transform.position, targetData.Value.OriginPoint);
            float maxDistance = 200f;
            if (projectileDistance / maxDistance < 0.6f) {
                lr.material = lineWarningMaterial;
            } else {
                lr.material = lineDangerMaterial;
            }
        }
        
        if (projectileObj == null && lr.positionCount > 0) {
            lr.positionCount = 0;
        }
    }

    void SpawnTargetingLaser() {
        targetData = RerollTarget();
        if (targetData == null) return;

        projectileObj = Instantiate(projectilePrefab, targetData.Value.OriginPoint, Quaternion.identity);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        projectile.Initialize(targetData.Value.TargetPoint - targetData.Value.OriginPoint);

        lr.positionCount = 2;
        lr.SetPosition(0, targetData.Value.OriginPoint);
        lr.SetPosition(1, targetData.Value.TargetPoint);
    }

    TargetData? RerollTarget() {
        RaycastHit hit = default;
        targetHolder = shapeDropper.GetTowerState();
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

            originPoint = Random.onUnitSphere * 200f;
            
            Vector3 randomDirection = (targetPoint - originPoint).normalized;

            foundHit = Physics.Raycast(originPoint, randomDirection, out hit);
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
