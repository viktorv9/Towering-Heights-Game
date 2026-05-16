using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveScanner : MonoBehaviour
{
    
    [SerializeField] private ShapeDropper shapeDropper;
    [SerializeField] private float riseSpeed;
    
    private List<GameObject> blocksInTrigger = new ();
    
    private void OnTriggerEnter(Collider other) {
        if (!other.gameObject.CompareTag("Block")) return;
        blocksInTrigger.Add(other.gameObject);
    }
    
    private void OnTriggerExit(Collider other) {
        if (!other.gameObject.CompareTag("Block")) return;
        blocksInTrigger.Remove(other.gameObject);
        
        if (blocksInTrigger.Count == 0) {
            shapeDropper.SetGameOver(true);
        }
    }

    void Update()
    {
        float newY = transform.position.y + riseSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
