using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
    private static readonly int fillAmount = Shader.PropertyToID("_fillAmount");

    [SerializeField] private PickupType pickupType;
    [SerializeField] private float pickupCollisionDuration;
    [SerializeField] private GameObject pickupEffect;

    private MeshRenderer meshRenderer;
    
    private List<GameObject> collidersInTrigger = new ();
    private float timeCollisionStart = 0;
    
    public enum PickupType {
        Default,
        Platform,
        RotationUpgrade,
        HoldBlockUpgrade,
        UndoUpgrade,
    }

    public delegate void PickupAction();
    public event PickupAction OnPickup;
    
    private void Start() {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    
    private void OnEnable()
    {
        UpgradeManager.OnUpgradesLoaded += SetPickupBehavior;
    }
    
    private void OnDisable()
    {
        UpgradeManager.OnUpgradesLoaded -= SetPickupBehavior;
    }

    private void SetPickupBehavior() {
        switch (pickupType) {
            case PickupType.RotationUpgrade: {
                if (UpgradeManager.UnlockedUpgrades.Contains(UpgradeManager.UpgradeType.RotationUpgrade)) {
                    Destroy(gameObject);
                    return;
                }
                OnPickup += () => UpgradeManager.UnlockUpgrade(UpgradeManager.UpgradeType.RotationUpgrade);
                break;
            }
            case PickupType.HoldBlockUpgrade: {
                if (UpgradeManager.UnlockedUpgrades.Contains(UpgradeManager.UpgradeType.HoldUpgrade)) {
                    Destroy(gameObject);
                    return;
                }
                OnPickup += () => UpgradeManager.UnlockUpgrade(UpgradeManager.UpgradeType.HoldUpgrade);
                break;
            }
            case PickupType.UndoUpgrade: {
                if (UpgradeManager.UnlockedUpgrades.Contains(UpgradeManager.UpgradeType.UndoUpgrade)) {
                    Destroy(gameObject);
                    return;
                }
                OnPickup += () => UpgradeManager.UnlockUpgrade(UpgradeManager.UpgradeType.UndoUpgrade);
                break;
            }
        }
    }

    private void Update() {
        if (timeCollisionStart != 0) {
            var collisionDuration = Time.time - timeCollisionStart;
            meshRenderer.material.SetFloat(fillAmount, collisionDuration / pickupCollisionDuration);
            if (collisionDuration > pickupCollisionDuration) {
                PickUp();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.gameObject.CompareTag("Block")) return;
        collidersInTrigger.Add(other.gameObject);
        
        if (collidersInTrigger.Count > 0) {
            if (timeCollisionStart == 0) timeCollisionStart = Time.time;
        }
    }
    
    private void OnTriggerExit(Collider other) {
        if (!other.gameObject.CompareTag("Block")) return;
        collidersInTrigger.Remove(other.gameObject);
        
        if (collidersInTrigger.Count == 0) {
            timeCollisionStart = 0;
            meshRenderer.material.SetFloat(fillAmount, 0);
        }
    }
    
    private void PickUp() {
        OnPickup?.Invoke();
        Instantiate(pickupEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
    public PickupType GetPickupType() {
        return pickupType;
    }
}
