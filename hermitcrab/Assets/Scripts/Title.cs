using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour {
    void Update() {
        if (Input.GetButtonDown("Action")) {
            SceneManager.LoadScene("TestScene");
        }
    }
}
