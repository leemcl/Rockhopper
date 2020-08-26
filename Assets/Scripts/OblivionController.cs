using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OblivionController : MonoBehaviour {
    [SerializeField] PlanetGenerationManager planetGenerationManager;
    [SerializeField] PlayerController player;
    [SerializeField] float speed;
    Rigidbody rb;
    Vector3 startPos;

    void Start() {
        rb = gameObject.GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    void Update() {
        if (!player.FirstJump) {
            rb.MovePosition(rb.position + transform.up * speed * Time.deltaTime);
        }        
    }

    public void Reset() {
        transform.position = startPos;
    }
}
