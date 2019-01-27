﻿using UnityEngine;

public class GullSpawner : MonoBehaviour {
    [SerializeField] Crab crab;
    [SerializeField] GameObject gullShadowTemplate;
    [SerializeField] public float dist = 125f;
    [SerializeField] public float[] spawnPeriods;
    float t;

    void Awake() {
        gullShadowTemplate.SetActive(false);
        //Destroy(gullShadowTemplate);
    }

    void Update() {
        //GullShadow[] gulls = FindObjectsOfType<GullShadow>();
        //Debug.Log(gulls.Length);

        t += Time.deltaTime;

        if (t > spawnPeriods[crab.sizeIndex]) {
            t = 0;
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
            gullShadow.direction += Random.Range(-45f, 45f);
            gullShadow.curve = Random.Range(-10f, 10f);
            gullShadow.speed = 10f;
        }
    }
}