using UnityEngine;

public class Crab : MonoBehaviour {
    [SerializeField] SphereCollider pickupCollider;
    GameObject shell;

    void Update() {
        Vector3 inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        transform.position += inputVector * 0.1f;

        Collider[] shells = Physics.OverlapSphere(
                    pickupCollider.transform.position,
                    pickupCollider.radius,
                    ~LayerMask.NameToLayer("Shell"));

        if (Input.GetButtonDown("Action")) {
            // Drop current shell
            if (shell != null) {
                shell.transform.parent = null;
                Vector3 pos = shell.transform.position;
                shell.transform.position = new Vector3(pos.x, 0, pos.z);
                shell.layer = LayerMask.NameToLayer("Shell");
                shell = null;
            }

            if (shells.Length > 0) {
                // For now, just pick a random shell in the returned array.
                // In the future, we might want to pick the closest shell, and also
                // display a highight around that closest shell when the player comes close.
                shell = shells[0].gameObject;
                shell.transform.parent = transform;
                shell.transform.localPosition = new Vector3(0, 0.5f, -1.0f);
                shell.transform.localRotation = Quaternion.identity;
                shell.layer = LayerMask.NameToLayer("Default");
            }
        }
    }
}
