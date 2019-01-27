using UnityEngine;

public class WalkingCrab : MonoBehaviour {
    [SerializeField] Transform[] positions;
    [SerializeField] float startT;
    [SerializeField] float walkSpeed;
    [SerializeField] GameObject crab;
    float t = 0;
    int posIndex = 0;

    void Awake() {
        foreach (Transform pos in positions) {
            pos.gameObject.SetActive(false);
            pos.transform.parent = null;
        }
    }
    void Update() {
        t += Time.deltaTime;
        if (t > startT && posIndex < positions.Length) {
            Vector3 target = positions[posIndex].position;
            float spd = walkSpeed * Time.deltaTime;
            float dist = Vector3.Distance(crab.transform.position, target);
            if (dist <= spd) {
                spd = dist;
                posIndex++;
            }
            crab.transform.position = Vector3.MoveTowards(crab.transform.position, target, spd);
        }
    }
}
