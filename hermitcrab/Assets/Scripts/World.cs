using UnityEngine;
using UnityEngine.SceneManagement;

public class World : MonoBehaviour {
    void Update() {
        if (Input.GetButtonDown("Restart")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
