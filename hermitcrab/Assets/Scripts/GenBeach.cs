using UnityEngine;

public class GenBeach : MonoBehaviour {
    [SerializeField] GameObject[] shellTemplates;
    [SerializeField] int[] numShells;

    [SerializeField] BoxCollider bounds1;
    [SerializeField] BoxCollider bounds2;

    void Awake() {
        for (var i = 0; i < shellTemplates.Length; i++) {
            GameObject template = shellTemplates[i];
            for (var j = 0; j < numShells[i]; j++) {
                GameObject shell = Instantiate(template);

                float size = bounds1.size.x / 2;
                if (i == 4 || i == 5 || i == 6 || i == 7) {
                    size = bounds2.size.x / 2;
                }
                shell.transform.position = new Vector3(
                    transform.position.x + Random.Range(-size, size),
                    template.transform.position.y,
                    transform.position.z + Random.Range(-size, size));

                // Randomize first 4 shells
                if (i == 0 || i == 1 || i == 2 || i == 3) {
                    shell.transform.Rotate(Vector3.up, Random.Range(0f, 360f));
                    shell.transform.Rotate(Vector2.right, Random.Range(-60f, 60f));
                    shell.transform.position += Vector3.down * Random.Range(0f, 0.2f);
                }

                // Randomize last 2 shells
                if (i == 4 || i == 5) {
                    shell.transform.Rotate(Vector3.up, Random.Range(-60f, 60f));
                }

                // Randomize towel
                if (i == 6) {
                    shell.transform.Rotate(Vector3.up, Random.Range(0f, 360f));
                }

                // Randomize radio
                if (i == 7) {
                    shell.transform.Rotate(Vector3.up, Random.Range(-60f, 60f));
                }

                // Randomize rocks and seaweed
                if (i == 8 || i == 9) {
                    shell.transform.Rotate(Vector3.up, Random.Range(0f, 360f));
                    shell.transform.Rotate(Vector2.right, Random.Range(0, 360f));
                    float scale = Random.Range(1f, 2f);
                    shell.transform.localScale = new Vector3(scale, scale, scale);
                }
            }

            Destroy(template);
        }
    }
}
