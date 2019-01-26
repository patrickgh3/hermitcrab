using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour {
    [SerializeField] SphereCollider pickupCollider;
    [SerializeField] BoxCollider wallCollider;
    [SerializeField] Transform bottleCapPos;
    [SerializeField] Transform canPos;

    GameObject shell;
    float pickupTime;
    float pickupDuration = 3f;
    Transform pickupTargetPos;

    float lookAngle = 90;

    enum State {
        Walking,
        Growing,
    }
    State state = State.Walking;

    float growTime;
    float growDuration = 2f;

    void Awake() {
        bottleCapPos.gameObject.SetActive(false);
        canPos.gameObject.SetActive(false);
    }

    void Update() {
        // Get input
        Vector2 inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        /*
        float inputAngle = Util.Vector2ToDegrees(inputVector);
        float cameraAngle = Util.Vector2ToDegrees(Camera.main.transform.eulerAngles);
        Debug.Log("inputAngle: " + inputAngle.ToString());
        Debug.Log("cameraAngle: " + cameraAngle.ToString());
        //inputVector = Util.DegreeToVector2(inputAngle);
        */

        // Move
        Vector3 deltaPos = new Vector3(inputVector.x, 0, inputVector.y) * 8f * Time.deltaTime;
        Vector3 deltaPosX = new Vector3(deltaPos.x, 0, 0);
        Vector3 deltaPosZ = new Vector3(0, 0, deltaPos.z);
        if (!Physics.CheckBox(
                wallCollider.transform.position + deltaPosX,
                wallCollider.size,
                Quaternion.identity,
                (1<<LayerMask.NameToLayer("Wall")))) {
            transform.position += deltaPosX;
        }
        if (!Physics.CheckBox(
                wallCollider.transform.position + deltaPosZ,
                wallCollider.size,
                Quaternion.identity,
                (1 << LayerMask.NameToLayer("Wall")))) {
            transform.position += deltaPosZ;
        }

        /*
        Collider[] colls = Physics.OverlapSphere(
                    pickupCollider.transform.position,
                    pickupCollider.radius,
                    ~(1<<LayerMask.NameToLayer("Wall")));
        for (int i = 0; i < colls.Length; i++) {
            Debug.Log(colls[i].gameObject.name);
        }
        */

        // Rotate
        if (inputVector.magnitude != 0) {
            float inputAngle = Util.Vector2ToDegrees(inputVector);
            lookAngle = Mathf.LerpAngle(lookAngle, inputAngle, 0.15f);
            transform.forward = new Vector3(Mathf.Cos(lookAngle * Mathf.Deg2Rad), 0, Mathf.Sin(lookAngle * Mathf.Deg2Rad));
        }

        // Find nearby shells
        Collider[] shells = Physics.OverlapSphere(
                    pickupCollider.transform.position,
                    pickupCollider.radius,
                    (1<<LayerMask.NameToLayer("Shell")));

        if (state == State.Walking && Input.GetButtonDown("Action")) {
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

            // Pick up shell if there is one
            if (shells.Length > 0) {
                // For now, just pick a random shell in the returned array.
                // In the future, we might want to pick the closest shell, and also
                // display a highight around that closest shell when the player comes close.
                shell = shells[0].gameObject;

                pickupTime = Time.time;

                // Set the shell's transform to the appropriate preset position
                shell.transform.parent = transform;
                Shell.ShellType shellType = shell.GetComponent<Shell>().Type;
                switch (shellType) {
                    case Shell.ShellType.BottleCap: pickupTargetPos = bottleCapPos; break;
                    case Shell.ShellType.Can: pickupTargetPos = canPos; break;
                    default: Debug.LogWarning("Missing ShellType case"); break;
                }

                shell.layer = LayerMask.NameToLayer("Default");

                Rigidbody shellRB = shell.GetComponent<Rigidbody>();
                shellRB.detectCollisions = false;
                shellRB.isKinematic = true;
            }
        }

        // Testing key to grow
        if (state == State.Walking && Input.GetKeyDown(KeyCode.Alpha1)) {
            state = State.Growing;
            growTime = Time.time;
        }

        if (state == State.Growing) {

        }

        // Update shell transform moving to the preset position on the crab's back
        if (shell != null) {
            if (Time.time - pickupTime < pickupDuration) {
                float lerpT = (Time.time - pickupTime) / pickupDuration;
                shell.transform.localPosition = Vector3.Slerp(
                        shell.transform.localPosition, pickupTargetPos.localPosition, lerpT);
                shell.transform.localRotation = Quaternion.Slerp(
                        shell.transform.localRotation, pickupTargetPos.localRotation, lerpT);
            }
            else {
                shell.transform.localPosition = pickupTargetPos.localPosition;
                shell.transform.localRotation = pickupTargetPos.localRotation;
            }
        }
    }
}
