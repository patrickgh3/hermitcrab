using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour {
    [SerializeField] float length = 4f;
    float t;

    void Update() {
        t += Time.deltaTime;
        if (t > length) {
            SceneManager.LoadScene("GenScene");
        }
    }
}
