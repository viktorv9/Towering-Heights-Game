using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArea : MonoBehaviour {
    [SerializeField] private float windStrength;

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Block")) {
            // Debug.DrawRay(other.gameObject.transform.position, transform.forward * -4, Color.blue);
            if (!Physics.Raycast(other.gameObject.transform.position, -transform.forward, out RaycastHit hit, 4))
            {
                other.gameObject.transform.parent.GetComponent<Rigidbody>().AddForce(transform.forward * windStrength);
            }
        }
    }
}
