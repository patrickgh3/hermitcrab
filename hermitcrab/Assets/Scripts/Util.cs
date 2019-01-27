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

    // Easing function from:
    // https://gist.github.com/cjddmut/d789b9eb78216998e95c

    public static float EaseOutElastic(float start, float end, float value) {
        end -= start;

        float d = 1f;
        float p = d * .3f;
        float s;
        float a = 0;

        if (value == 0) return start;

        if ((value /= d) == 1) return start + end;

        if (a == 0f || a < Mathf.Abs(end)) {
            a = end;
            s = p * 0.25f;
        }
        else {
            s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
        }

        return (a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) + end + start);
    }

    public static float Bounce(float start, float end, float value) {
        float lerpT = value % 1f;
        float airTime = 0.8f;

        if (lerpT < airTime) {
            lerpT /= airTime;
            lerpT = -Mathf.Pow(2f * lerpT - 1f, 2f) + 1f;
            return Mathf.Lerp(start, end, lerpT);
        }
        else {
            return start;
        }
    }
}
