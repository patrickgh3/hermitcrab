using UnityEngine;
using UnityEngine.SceneManagement;

public class Crab : MonoBehaviour {
    [SerializeField] SphereCollider pickupCollider;
    [SerializeField] BoxCollider wallCollider;
    [SerializeField] GameObject crab;
    [SerializeField] Animator crabAnimator;
    [SerializeField] GameObject[] positions;

    enum State {
        Walking,
        Growing,
    }
    State state = State.Walking;

    float walkSpeed = 9f;

    float lookAngle = 90;

    public GameObject shell;
    float pickupTime;
    float pickupDuration = 3f;
    Transform pickupTargetPos;

    float growTime;
    float growDuration = 1f;
    float growStartScale = 1f;
    float growEndScale = 1f;
    public int sizeIndex = 0;

    public float periodicGrowTime;
    public float growPeriod = 12f;
    [SerializeField] float[] growPeriods;

    void Awake() {
        crab.SetActive(true);
        for (var i = 0; i < positions.Length; i++) {
            positions[i].SetActive(false);
        }
    }

    void Update() {
        // Get input
        Vector2 inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Move
        Vector3 deltaPos = new Vector3(inputVector.x, 0, inputVector.y) * walkSpeed * Time.deltaTime;
        Vector3 deltaPosX = new Vector3(deltaPos.x, 0, 0);
        Vector3 deltaPosZ = new Vector3(0, 0, deltaPos.z);
        if (!Physics.CheckBox(
                wallCollider.transform.position + deltaPosX,
                wallCollider.size,
                Quaternion.identity,
                1<<LayerMask.NameToLayer("Wall"))) {
            transform.position += deltaPosX;
        }
        if (!Physics.CheckBox(
                wallCollider.transform.position + deltaPosZ,
                wallCollider.size,
                Quaternion.identity,
                1 << LayerMask.NameToLayer("Wall"))) {
            transform.position += deltaPosZ;
        }

        if (deltaPos != Vector3.zero) {
            crabAnimator.SetInteger("Run", 1);
            World.SetWalking(true);
        }
        else {
            crabAnimator.SetInteger("Run", 0);
            World.SetWalking(false);
        }

        // Rotate
        if (inputVector.magnitude != 0) {
            float inputAngle = Util.Vector2ToDegrees(inputVector);
            lookAngle = Mathf.LerpAngle(lookAngle, inputAngle, 0.15f);
            Vector2 forwardXZ = Util.DegreeToVector2(lookAngle);
            transform.forward = new Vector3(forwardXZ.x, 0, forwardXZ.y);
        }

        // Find nearby shells
        Collider[] shells = Physics.OverlapSphere(
                    pickupCollider.transform.position,
                    pickupCollider.radius,
                    1<<LayerMask.NameToLayer("Shell"));

        // Press space bar
        if (state == State.Walking && Input.GetButtonDown("Action")) {
            DropShell();

            shell = pickShell(shells);

            if (shell == null && shells.Length > 0) {
                World.PlaySound(World.Sound.CantGrab);
            }
            else if (shell != null) {
                pickupTime = Time.time;

                // Find the shell's preset position
                shell.transform.parent = crab.transform;
                GameObject posObj = ShellTypeToPosObj(shell.GetComponent<Shell>().Type);
                pickupTargetPos = posObj.transform.Find("Shell");

                // Disable collisions on the shell
                shell.layer = LayerMask.NameToLayer("Default");
                Rigidbody shellRB = shell.GetComponent<Rigidbody>();
                shellRB.detectCollisions = false;
                shellRB.isKinematic = true;

                if (sizeIndex == 5) {
                    World.PlayLastShellMusic();
                }
                World.PlaySound(World.Sound.ShellGrab);
            }
        }

        // Grow periodically
        if (shell != null) {
            periodicGrowTime += Time.deltaTime;
        }
        if (sizeIndex < growPeriods.Length) {
            growPeriod = growPeriods[sizeIndex];
        }
        if (state == State.Walking && periodicGrowTime > growPeriod) {
            periodicGrowTime = 0;

            if (sizeIndex == positions.Length - 1) {
                SceneManager.LoadScene("Smack");
            }
            else {
                state = State.Growing;
                growTime = Time.time;

                sizeIndex++;
                growStartScale = growEndScale;
                growEndScale = positions[sizeIndex].transform.localScale.x;

                DropShell();

                float offsetScalar = 1f;
                if (sizeIndex == 3) offsetScalar = 1.25f;
                if (sizeIndex == 4) offsetScalar = 1.5f;
                if (sizeIndex == 5) offsetScalar = 2f;
                Camera.main.GetComponent<CameraMove>().offsetScalar = offsetScalar;

                if (sizeIndex == 3) walkSpeed = 12f;
                if (sizeIndex == 4) walkSpeed = 15f;
                if (sizeIndex == 5) walkSpeed = 18f;
            }
        }

        // Grow easing animation
        if (state == State.Growing) {
            float t = (Time.time - growTime) / growDuration;
            if (t >= 1) {
                t = 1;
                state = State.Walking;
            }
            float scale = Util.EaseOutElastic(growStartScale, growEndScale, t);
            crab.transform.localScale = new Vector3(scale, scale, scale);
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

        // Collision with gull shadows
        Collider[] gulls = Physics.OverlapBox(
                wallCollider.transform.position + deltaPosX,
                wallCollider.size,
                Quaternion.identity,
                1 << LayerMask.NameToLayer("GullShadow"));
        if (gulls.Length > 0) {
            GameObject gull = gulls[0].gameObject;
            SceneManager.LoadScene("Eat");
        }
    }

    GameObject ShellTypeToPosObj(Shell.ShellType shellType) {
        return transform.Find(shellType.ToString() + "Pos").gameObject;
    }

    void DropShell() {
        if (shell != null) {
            shell.transform.parent = null;

            // Enable collisions on the shell
            shell.layer = LayerMask.NameToLayer("Shell");
            Rigidbody shellRB = shell.GetComponent<Rigidbody>();
            shellRB.detectCollisions = true;
            shellRB.isKinematic = false;

            // Kick the shell in a random direction
            Vector2 xzAngle = Random.insideUnitCircle.normalized * 400f;
            shellRB.AddForce(new Vector3(xzAngle.x, 300f, xzAngle.y));
            shellRB.angularVelocity = new Vector3(Random.Range(100f, 300f), Random.Range(100f, 300f), Random.Range(100f, 300f));

            shell = null;

            World.PlaySound(World.Sound.ShellPop);
        }
    }

    // Find the first shell in the list of colliders which has the same
    // size as us
    GameObject pickShell(Collider[] shells) {
        foreach (Collider shell in shells) {
            int shellSize = 0;
            Shell.ShellType t = shell.GetComponent<Shell>().Type;
            switch (t) {
                case Shell.ShellType.BottleCap: shellSize = 0; break;
                case Shell.ShellType.Shell2: shellSize = 1; break;
                case Shell.ShellType.Shell1: shellSize = 2; break;
                case Shell.ShellType.Can: shellSize = 3; break;
                case Shell.ShellType.Skull: shellSize = 4; break;
                case Shell.ShellType.Cooler: shellSize = 5; break;
                default:
                    Debug.LogWarning("Mising case in pickShell");
                    shellSize = -1;
                    break;
            }

            if (shellSize == sizeIndex) {
                return shell.gameObject;
            }
        }
        return null;
    }
}
