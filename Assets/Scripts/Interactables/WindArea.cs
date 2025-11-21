using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArea : MonoBehaviour {
    [SerializeField] private float windStrength;

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Block")) {
            other.gameObject.transform.parent.GetComponent<Rigidbody>().AddForce(transform.forward * windStrength);
        }
    }
}
