using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapePreview : MonoBehaviour {

    [SerializeField] private List<Vector3> possibleRotations;
    
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material errorMaterial;
    [SerializeField] private Material errorerMaterial;

    [SerializeField] private TriggerZone innerCollider;
    [SerializeField] private List<Renderer> associatedRenderers;

    private int outerCollidingAmount;
    private int innerCollidingAmount;

    private void OnEnable() {
        innerCollider.onTriggerEnterEvent.AddListener(OnInnerTriggerEnter);
        innerCollider.onTriggerExitEvent.AddListener(OnInnerTriggerExit);
    }

    private void OnDisable() {
        innerCollider.onTriggerEnterEvent.RemoveListener(OnInnerTriggerEnter);
        innerCollider.onTriggerExitEvent.RemoveListener(OnInnerTriggerExit);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Block")) outerCollidingAmount++;
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Block")) outerCollidingAmount--;
    }

    private void OnInnerTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Block")) innerCollidingAmount++;
    }

    private void OnInnerTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Block")) innerCollidingAmount--;
    }

    private void Update() {
        if (innerCollidingAmount > 0) {
            foreach (Renderer associatedRenderer in associatedRenderers) {
                associatedRenderer.material = errorerMaterial;
            }
        } else if (outerCollidingAmount > 0) {
            foreach (Renderer associatedRenderer in associatedRenderers) {
                associatedRenderer.material = errorMaterial;
            }
        } else {
            foreach (Renderer associatedRenderer in associatedRenderers) {
                associatedRenderer.material = defaultMaterial;
            }
        }
    }

    public List<Vector3> GetPossibleRotations => possibleRotations;
}
