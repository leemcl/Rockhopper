using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml.Serialization;

public class GameManager : MonoBehaviour {
    [SerializeField] PlayerController player;
    [SerializeField] PlanetGenerationManager planets;
    [SerializeField] CameraController cam;
    [SerializeField] OblivionController oblivion;

    int score, hiScore;
    [SerializeField] Text scoreText, hiScoreText;
    SaveObject saveObject;

    void Start() {
        saveObject = new SaveObject();
        Load();
    }

    void Update() {
        score = player.PlanetsHit * 5 + player.HighestPoint;

        scoreText.text = "SCORE: " + score;
        hiScoreText.text = "HI-SCORE: " + hiScore;

        if (score > hiScore) {
            hiScore = score;
        }
    }

    public void Reset() {
        player.Reset();
        planets.Reset();
        cam.Reset();
        oblivion.Reset();

        // save high score
        if (hiScore > saveObject.HiScore) {
            saveObject.HiScore = hiScore;
            Save();
        }
    }

    void Load() {
        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            hiScore = PlayerPrefs.GetInt("HiScore", 0);
        } else {
            if (File.Exists("Score.xml")) {
                Stream stream = File.Open("Score.xml", FileMode.Open);
                XmlSerializer serializer = new XmlSerializer(typeof(SaveObject));
                saveObject = serializer.Deserialize(stream) as SaveObject;
                stream.Close();

                hiScore = saveObject.HiScore;
            } else {
                hiScore = 0;
            }
        }
    }

    void Save() {
        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            PlayerPrefs.SetInt("HiScore", hiScore);
        } else {
            Stream stream = File.Open("Score.xml", FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(typeof(SaveObject));
            serializer.Serialize(stream, saveObject);
            stream.Close();
        }
    }

    public class SaveObject {
        public int HiScore { get; set; }
    }
}
