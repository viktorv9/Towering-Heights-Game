using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeDropper : MonoBehaviour {

    [SerializeField] private Transform heightFollowTarget;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float horizontalSpeed;
    [SerializeField] private List<GameObject> shapes;

    void Update() {
        UpdatePosition();
        if (Input.GetKeyDown("space") || Input.GetMouseButtonDown(0)) HandleDrop();
    }

    void HandleDrop() {
        GameObject dropShape = shapes[Random.Range(0,(shapes.Count))];
        Instantiate(dropShape, transform.position, Quaternion.identity);
    }
    
    void UpdatePosition() {
        Vector3 newPosition = transform.position;

        newPosition.y = heightFollowTarget.position.y + 3f;

        if (Input.GetKey(KeyCode.W)) newPosition += cameraTransform.forward * (horizontalSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.A)) newPosition -= cameraTransform.right * (horizontalSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S)) newPosition -= cameraTransform.forward * (horizontalSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D)) newPosition += cameraTransform.right * (horizontalSpeed * Time.deltaTime);
        
        newPosition = Vector3.MoveTowards(transform.position, newPosition, 0.01f);

        transform.position = newPosition;
    }
}
