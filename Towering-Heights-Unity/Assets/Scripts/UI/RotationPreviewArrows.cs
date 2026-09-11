using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPreviewArrows : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    
    private float rotation = 0;
    
    void Update() {
        rotation += speed * Time.deltaTime;
        transform.localEulerAngles = new Vector3(0, rotation, 0);
    }
}
