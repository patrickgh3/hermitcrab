using UnityEngine;

public class Util {
    public static Vector2 RadianToVector2(float radian) {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree) {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    public static float Vector2ToDegrees(Vector2 vec) {
        return Vector2.SignedAngle(Vector2.right, vec);
    }
}
