using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapePreview : MonoBehaviour {

    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material errorMaterial;
    [SerializeField] private List<Renderer> associatedRenderers;

    private int collidingAmount;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Block")) collidingAmount++;
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Block")) collidingAmount--;
    }

    private void Update() {
        if (collidingAmount > 0) {
            foreach (Renderer associatedRenderer in associatedRenderers) {
                associatedRenderer.material = errorMaterial;
            }
        } else {
            foreach (Renderer associatedRenderer in associatedRenderers) {
                associatedRenderer.material = defaultMaterial;
            }
        }
    }
}
