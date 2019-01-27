using UnityEngine;
using UnityEngine.SceneManagement;

public class GullEat : MonoBehaviour {
    [SerializeField] GameObject[] positions;
    [SerializeField] GameObject crab;
    [SerializeField] GameObject upperBeak;
    [SerializeField] GameObject lowerBeak;
    float t = 0;
    float duration1 = 0.75f;
    float duration2 = 1.5f;
    bool gotCrab = false;

    void Awake() {
        foreach (GameObject pos in positions) {
            pos.SetActive(false);
        }
    }

    void Update() {
        t += Time.deltaTime;

        if (t > duration1 * 0.65f) {
            lowerBeak.transform.localRotation = Quaternion.Slerp(lowerBeak.transform.localRotation, Quaternion.identity, 0.1f);
            upperBeak.transform.localRotation = Quaternion.Slerp(upperBeak.transform.localRotation, Quaternion.identity, 0.1f);
        }

        if (t < duration1) {
            float lerpT = t / duration1;
            transform.position = Vector3.Lerp(positions[0].transform.position, positions[1].transform.position, lerpT);
        }
        else {
            if (!gotCrab) {
                gotCrab = true;
                crab.transform.parent = transform;
            }
            float lerpT = (t - duration1) / duration2;
            transform.position = Vector3.Lerp(positions[1].transform.position, positions[2].transform.position, lerpT);
        }

        if (t >= duration1 + duration2) {
            SceneManager.LoadScene("TestScene");
        }
    }
}
