using UnityEngine;
using UnityEngine.SceneManagement;

public class World : MonoBehaviour {
    public enum Sound {
        CantGrab,
        GameOverSwoop,
        SeagullScreech,
        ShellGrab,
    }

    public static void PlaySound(Sound sound) {
        // TODO
    }

    //static World instance;

    private void Awake() {
        //instance = this;
    }

    void Update() {
        if (Input.GetButtonDown("Restart")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
