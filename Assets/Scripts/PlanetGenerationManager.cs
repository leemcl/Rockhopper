using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerationManager : MonoBehaviour {
    [SerializeField] float fieldWidth, fieldHeight; //in unity ~units~
    [SerializeField] float firstFieldYPos; //screens after this one will be stacked on top
    [SerializeField] float distanceBetweenFields;
    [SerializeField] int minPlanets, maxPlanets; //min and max planets per screen
    List<ObstacleField> fields = new List<ObstacleField>();

    GameObject planet;

    [SerializeField] Transform playerTransform;

    void Start() {
        planet = Resources.Load<GameObject>("Planet");
        Reset();
    }

    void Update() {
        //when player position > 2nd field, destroy first field and create a new one at end of list (top of fields)
        if (playerTransform.position.y > fields[1].position) {
            fields.Add(new ObstacleField(fields[3].position + fieldHeight, Random.Range(minPlanets, maxPlanets + 1), this));

            fields[0].ClearPlanets();
            fields.Remove(fields[0]);
        }
    }

    public void Reset() {
        foreach (ObstacleField field in fields) {
            field.ClearPlanets();
        }

        fields.Clear();

        //create the first 4 fields
        for (int i = 0; i < 4; i++) {
            if (i == 0) {
                fields.Add(new ObstacleField(firstFieldYPos, Random.Range(minPlanets, maxPlanets + 1), this));
            } else {
                fields.Add(new ObstacleField(fields[i - 1].position + fieldHeight, Random.Range(minPlanets, maxPlanets + 1), this));
            }
        }
    }

    public class ObstacleField {
        public List<float> yPositions = new List<float>();
        List<GameObject> planets = new List<GameObject>();
        PlanetGenerationManager parent;

        public float position;

        float bottomOfField;
        float distanceBetweenLines;
        float leftSideOfField, rightSideOfField;

        public ObstacleField(float position, int numOfPlanets, PlanetGenerationManager parent) {
            this.parent = parent;

            this.position = position;
            bottomOfField = position - (parent.fieldHeight / 2);
            leftSideOfField = parent.fieldWidth / 2 * -1;
            rightSideOfField = parent.fieldWidth / 2;

            Debug.Log(position);

            distanceBetweenLines = parent.fieldHeight / (numOfPlanets + 1);

            for (int i = 1; i <= numOfPlanets; i++) {
                yPositions.Add(bottomOfField + (distanceBetweenLines * i));
            }

            for (int i = 0; i < numOfPlanets; i++) {
                //instantiate a new planet
                GameObject planet = Instantiate<GameObject>(parent.planet);
                planets.Add(planet);

                //generate a random size for this planet (this will be the diameter of the planet)
                planet.GetComponent<PlanetContoller>().Size = (Random.Range(1f, 4f));

                //set speed
                planet.GetComponent<PlanetContoller>().Speed = (Random.Range(80, 180));

                //set direction
                if (Random.Range(0,2) == 0) {
                    planet.GetComponent<PlanetContoller>().Direction = -1;
                } else {
                    planet.GetComponent<PlanetContoller>().Direction = 1;
                }

                float xPos;

                //if this isnt the first line point
                if (i > 0) {
                    //the minimum amount of space between the previous planet and this one
                    float minDistanceBetweenPlanets = (planets[i].GetComponent<PlanetContoller>().Size / 2) + 1.5f + (planets[i - 1].GetComponent<PlanetContoller>().Size / 2);

                    //check if there's enough room for this planet to be generated anywhere on this line
                    if (minDistanceBetweenPlanets <= distanceBetweenLines) {
                        //planet can be placed anywhere
                        xPos = Random.Range(leftSideOfField, rightSideOfField);
                    } else {
                        //if not, calculate the amount of room to be ruled out from this line
                        float prevXPos = planets[i - 1].transform.position.x;
                        float xDistanceFromPrevPlanet = Mathf.Sqrt(Mathf.Pow(minDistanceBetweenPlanets, 2) - Mathf.Pow(distanceBetweenLines, 2));

                        float lowerExcludedRange = prevXPos - xDistanceFromPrevPlanet;
                        float higherExcludedRange = prevXPos + xDistanceFromPrevPlanet;

                        xPos = Random.Range(leftSideOfField, rightSideOfField);

                        //if this would be too close to the previous planet
                        if (xPos >= lowerExcludedRange && xPos <= higherExcludedRange) {
                            Debug.Log("out of range: " + xPos);
                            //check if excluded range is closer to the left or right side of the screen and adjust accordingly
                            if (prevXPos <= 0) {
                                //adjust right
                                xPos = higherExcludedRange;
                            } else {
                                //adjust left
                                xPos = lowerExcludedRange;
                            }
                        }
                    }
                } else {
                    xPos = Random.Range(leftSideOfField, rightSideOfField);
                }

                planet.transform.position = new Vector3(xPos, yPositions[i]);
            }
        }

        public void ClearPlanets() {
            foreach (var planet in planets) {
                Destroy(planet);
            }
        }
    }
}