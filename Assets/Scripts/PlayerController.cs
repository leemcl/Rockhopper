using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private enum PlayerState {
        Idle,
        Jumping
    }

    [SerializeField] PlayerState state;

    [SerializeField] float jumpSpeed;

    Rigidbody rb;
    
    void Start() {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        switch(state) {
            case PlayerState.Idle:
                break;
            case PlayerState.Jumping:
                rb.MovePosition(rb.position + transform.up * jumpSpeed * Time.deltaTime);
                break;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        //if other is a planet
            //become child of planet
            //rotate player so bottom is pointing toward planet.
            //state = playerstate.idle

        if (collision.transform.tag == "Planet") {
            Debug.Log("Player hit a planet.");

            transform.SetParent(collision.transform);
            transform.up = GetDirectionFromPoints(collision.transform.position, transform.position);
            state = PlayerState.Idle;
        }
    }

    Vector3 GetDirectionFromPoints(Vector3 start, Vector3 terminal) {
        // to get a direction from 2 points, subtract the start from the terminal point
        return terminal - start;
    }

    void OnJump() {
        Debug.Log("jumped");

        transform.parent = null;

        if (state == PlayerState.Idle) {
            state = PlayerState.Jumping;
        }
    }
}
