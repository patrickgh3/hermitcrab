using UnityEngine;

public class CameraMove : MonoBehaviour {
    [SerializeField] Crab crab;
    Vector3 crabOffset;
    public float offsetScalar = 1f;

    void Awake() {
        // Record the local position of the camera relative to the crab that we set
        // in the editor, and then unparent the camera from the crab.
        crabOffset = transform.localPosition;
        transform.parent = null;
    }

    void Update() {
        if (crab != null) {
            Vector3 targetPos = crab.transform.position + crabOffset * offsetScalar;
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.15f);
        }
    }
}
