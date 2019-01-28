using UnityEngine;

public class SmackCrab : MonoBehaviour {
    void OnTriggerEnter(Collider collider) {
        collider.gameObject.transform.parent = null;
        var rb = collider.gameObject.GetComponent<Rigidbody>();
        var bc = collider.gameObject.GetComponent<BoxCollider>();

        if (bc.isTrigger) {
            bc.isTrigger = false;

            rb.isKinematic = false;
            rb.useGravity = true;
            //rb.AddForce(new Vector3(0, 5000f, 0)); // decapitation is too dark
            //rb.angularVelocity = new Vector3(Random.Range(100f, 300f), Random.Range(100f, 300f), Random.Range(100f, 300f));

            rb.WakeUp();

            World.PlaySound(World.Sound.Smack);
        }
    }
}
