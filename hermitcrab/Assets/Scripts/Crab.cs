using UnityEngine;

public class Crab : MonoBehaviour {
    void Update() {
        Vector3 inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        transform.position += inputVector * 0.1f;
    }
}
