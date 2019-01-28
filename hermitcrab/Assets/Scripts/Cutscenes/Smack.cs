using UnityEngine;
using UnityEngine.SceneManagement;

public class Smack : MonoBehaviour {
    [SerializeField] GameObject crab;
    [SerializeField] GameObject gulls;
    float t;
    float length = 5f;
    float crabT = 2.5f;
    float crabSpeed = 50f;
    float gullsYTarget;

    void Awake() {
        gullsYTarget = gulls.transform.position.y;
        gulls.transform.position += Vector3.down * 15f;
    }

    void Update() {
        t += Time.deltaTime;

        if (t > 1f) {
            float y = Mathf.Lerp(gulls.transform.position.y, gullsYTarget, 0.05f);
            gulls.transform.position = new Vector3(gulls.transform.position.x, y, gulls.transform.position.z);
        }

        if (t > crabT) {
            crab.transform.position += Vector3.right * crabSpeed * Time.deltaTime;
        }

        if (t > length) {
            SceneManager.LoadScene("Win");
        }
    }
}
