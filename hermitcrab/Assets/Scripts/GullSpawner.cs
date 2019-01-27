using UnityEngine;

public class GullSpawner : MonoBehaviour {
    [SerializeField] Crab crab;
    [SerializeField] GameObject gullShadowTemplate;
    [SerializeField] public float dist = 125f;
    [SerializeField] public float[] spawnPeriods;
    float t;

    void Awake() {
        gullShadowTemplate.SetActive(false);

        // Spawn initial gull shadows
        int numToSpawn = 10;
        for (var i = 0; i < numToSpawn; i++) {
            float initDist = Mathf.Lerp(dist, 70f, i / (float)numToSpawn);
            SpawnGull(initDist);
        }
    }

    void SpawnGull(float dist) {
        GullShadow gullShadow = Instantiate(gullShadowTemplate).GetComponent<GullShadow>();
        gullShadow.gameObject.SetActive(true);

        int choice = Random.Range(0, 3);
        // Bottom
        if (choice == 0) {
            gullShadow.transform.position = new Vector3(
                    Random.Range(-dist, dist), gullShadowTemplate.transform.position.y, -dist);
            gullShadow.direction = 90f;
        }
        // Top
        else if (choice == 1) {
            gullShadow.transform.position = new Vector3(
                    Random.Range(-dist, dist), gullShadowTemplate.transform.position.y, dist);
            gullShadow.direction = 270f;
        }
        // Left
        else if (choice == 2) {
            gullShadow.transform.position = new Vector3(
                    -dist, gullShadowTemplate.transform.position.y, Random.Range(-dist, dist));
            gullShadow.direction = 0f;
        }
        // Left
        else if (choice == 3) {
            gullShadow.transform.position = new Vector3(
                    dist, gullShadowTemplate.transform.position.y, Random.Range(-dist, dist));
            gullShadow.direction = 180f;
        }
        gullShadow.direction += Random.Range(-35f, 35f);
        gullShadow.curve = Random.Range(-9f, 9f);
        gullShadow.speed = 9f;
    }

    void Update() {
        t += Time.deltaTime;
        if (t > spawnPeriods[crab.sizeIndex]) {
            t = 0;
            SpawnGull(dist);
        }
    }
}
