using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] Transform playerTransform;
    Vector3 pos;

    void Update() {
        pos = transform.position;

        pos.y = playerTransform.position.y;

        if (pos.y > 5) {
            transform.position = pos;
        }      
    }

    public void Reset() {
        transform.position = new Vector3(0, 3.12f, -10);
    }
}
