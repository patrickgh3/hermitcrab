using UnityEngine;

public class GullShadow : MonoBehaviour {
    public float direction = 0; // 0 = right
    public float curve = 2f;
    public float speed = 5f;
    float bounds = 175f;

    void Update() {
        direction += Time.deltaTime * curve;

        // Move
        Vector2 velocityXZ = Util.DegreeToVector2(direction);
        Vector3 deltaPos = new Vector3(velocityXZ.x, 0, velocityXZ.y) * speed * Time.deltaTime;
        transform.position += deltaPos;

        // Set rotation
        transform.localEulerAngles = new Vector3(90, 90 - direction, 0);

        // Destroy if far away
        if (transform.position.x < -bounds || transform.position.x > bounds
                || transform.position.z < -bounds || transform.position.z > bounds) {
            Destroy(gameObject);
        }
    }
}
