using UnityEngine;

public class JumpingCrab : MonoBehaviour {
    float startY;
    float jumpHeight;
    float t;
    void Awake() {
        startY = transform.position.y;
        jumpHeight = Random.Range(1.5f, 3f);
        t = Random.Range(0f, 1f);
    }
    void Update() {
        t += Time.deltaTime;
        transform.position = new Vector3(
                transform.position.x,
                Util.Bounce(startY, startY + jumpHeight, t),
                transform.position.z);
    }
}
