using UnityEngine;

public class GullShadow : MonoBehaviour {
    float direction = 0; // 0 = right
    float speed = 5f;

    void Update() {
        direction += Time.deltaTime * 10;

        // Move
        Vector2 velocityXZ = Util.DegreeToVector2(direction);
        Vector3 deltaPos = new Vector3(velocityXZ.x, 0, velocityXZ.y) * speed * Time.deltaTime;
        transform.position += deltaPos;

        // Set rotation
        transform.localEulerAngles = new Vector3(90, 90 - direction, 0);
    }
}
