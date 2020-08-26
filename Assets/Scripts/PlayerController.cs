using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    private enum PlayerState {
        Idle,
        Jumping
    }

    PlayerState state;

    [SerializeField] GameManager gameManager;
    [SerializeField] float jumpSpeed;
    [SerializeField] float timeLimit;
    float timer, timeStamp;
    bool firstJump = true;
    [SerializeField] Slider timeSlider;

    Rigidbody rb;
    Vector3 startPos, startRotation;

    int planetsHit;
    int highestPoint;

    void Start() {
        rb = gameObject.GetComponent<Rigidbody>();
        startPos = transform.position;
        startRotation = transform.rotation.eulerAngles;

        planetsHit = 0;
        highestPoint = 0;
    }

    void Update() {
        if (transform.position.y > highestPoint) {
            highestPoint = (int)transform.position.y;
        }

        switch(state) {
            case PlayerState.Idle:
                timeSlider.value = timeSlider.maxValue;
                break;
            case PlayerState.Jumping:
                rb.MovePosition(rb.position + transform.up * jumpSpeed * Time.deltaTime);

                if (!firstJump) {
                    timer = Time.time - timeStamp;
                    timeSlider.value = timeSlider.maxValue - (timeSlider.maxValue * (timer / timeLimit));

                    if (timer >= timeLimit) {
                        gameManager.Reset();
                    }
                }
                break;
        }
    }

    void OnCollisionEnter(Collision collision) {
         if (collision.transform.tag == "Planet") {
            planetsHit++;
            transform.SetParent(collision.transform);
            transform.up = GetDirectionFromPoints(collision.transform.position, transform.position);
            state = PlayerState.Idle;
        }

        if (firstJump) {
            firstJump = false;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.name == "Oblivion") {
            gameManager.Reset();
        }
    }

    Vector3 GetDirectionFromPoints(Vector3 start, Vector3 terminal) {
        Vector2 terminal2d = new Vector2(terminal.x, terminal.y);
        Vector2 start2d = new Vector2(start.x, start.y);
        Vector2 result2d = terminal - start;

        // to get a direction from 2 points, subtract the start from the terminal point
        /*return terminal - start*/
        return new Vector3(result2d.x, result2d.y);
    }

    void OnJump() {
        transform.parent = null;

        transform.localScale = Vector3.one;

        timeStamp = Time.time;

        if (state == PlayerState.Idle) {
            state = PlayerState.Jumping;
        }
    }

    public void Reset() {
        state = PlayerState.Idle;
        transform.parent = null;
        transform.position = startPos;
        transform.rotation = Quaternion.Euler(startRotation);
        firstJump = true;
        planetsHit = 0;
        highestPoint = 0;
    }

    public int PlanetsHit {
        get { return planetsHit; }
    }

    public int HighestPoint {
        get { return highestPoint; }
    }

    public bool FirstJump {
        get { return firstJump; }
    }
}
