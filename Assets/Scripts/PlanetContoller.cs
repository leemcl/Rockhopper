using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetContoller : MonoBehaviour {
    [SerializeField] float rotationSpeed;
    float size;
    int direction = 1; //1 for cw -1 for ccw
    [SerializeField] GameObject cw, ccw;
    
    void Update() {
        transform.Rotate(new Vector3(0, rotationSpeed * direction * Time.deltaTime, 0));
    }

    public float Size {
        get { return size; }
        set {
            gameObject.transform.localScale = new Vector3(value, 1, value);
            size = value;
        }
    }

    public float Speed {
        get { return rotationSpeed; }
        set {
            rotationSpeed = Mathf.Abs(value);
        }
    }

    public int Direction {
        get { return direction; }
        set {
            if (value == 1 || value == -1) {
                direction = value;
            } else {
                direction = 1;
            }

            if (direction == -1) {
                cw.SetActive(false);
                ccw.SetActive(true);
            } else {
                cw.SetActive(true);
                ccw.SetActive(false);
            }
        }
    }
}
