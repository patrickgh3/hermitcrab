using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour {
    [SerializeField] SphereCollider pickupCollider;
    [SerializeField] Transform bottleCapPos;
    [SerializeField] Transform canPos;
    GameObject shell;

    void Update() {
        Vector3 inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        transform.position += inputVector * 0.1f;

        if (inputVector.magnitude != 0) {
            transform.forward = inputVector;
        }

        Collider[] shells = Physics.OverlapSphere(
                    pickupCollider.transform.position,
                    pickupCollider.radius,
                    ~LayerMask.NameToLayer("Shell"));

        if (Input.GetButtonDown("Action")) {
            // Drop current shell
            if (shell != null) {
                shell.transform.parent = null;

                Rigidbody shellRB = shell.GetComponent<Rigidbody>();
                shellRB.detectCollisions = true;
                shellRB.isKinematic = false;

                Vector2 xzAngle = Random.insideUnitCircle.normalized * 400f;
                shellRB.AddForce(new Vector3(xzAngle.x, 300f, xzAngle.y));
                shellRB.angularVelocity = new Vector3(Random.Range(100f, 300f), Random.Range(100f, 300f), Random.Range(100f, 300f));

                shell.layer = LayerMask.NameToLayer("Shell");
                shell = null;
            }

            // Pick up shell
            if (shells.Length > 0) {
                // For now, just pick a random shell in the returned array.
                // In the future, we might want to pick the closest shell, and also
                // display a highight around that closest shell when the player comes close.
                shell = shells[0].gameObject;

                // Set the shell's transform to the appropriate preset position
                shell.transform.parent = transform;
                Transform presetPos = bottleCapPos;
                Shell.ShellType shellType = shell.GetComponent<Shell>().Type;
                switch (shellType) {
                    case Shell.ShellType.BottleCap: presetPos = bottleCapPos; break;
                    case Shell.ShellType.Can: presetPos = canPos; break;
                }
                shell.transform.localPosition = presetPos.localPosition;
                shell.transform.localRotation = presetPos.localRotation;

                shell.layer = LayerMask.NameToLayer("Default");

                Rigidbody shellRB = shell.GetComponent<Rigidbody>();
                shellRB.detectCollisions = false;
                shellRB.isKinematic = true;
            }
        }
    }
}
