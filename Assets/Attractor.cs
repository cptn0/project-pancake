using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    GridXY grid;
    Rigidbody rb;

    [SerializeField]
    private GameObject objectToAttract;

    private Vector2 cellSize = new Vector3(10, 10);
    private Vector3 directionToAttractor;

    private void Awake() {
        rb = objectToAttract.GetComponent<Rigidbody>();
    }

    void Start() {
        
        /*
        grid = new GridXY(cellCount, cellCount, cellSize, new Vector3(-cellCount * .5f * cellSize.x, -cellCount * .5f * cellSize.y));
        for (int i = 0; i < cellCount * .5f; i++) {
            for (int x = 0 + i; x < cellCount - i; x++) {
                for (int y = 0 + i; y < cellCount - i; y++) {
                    grid.SetValue(x, y, startValue);
                }
            }
            startValue += addedValue;
        }
        */
    }

    void Update() {
        int dis = Mathf.RoundToInt(Vector3.Distance(transform.position, objectToAttract.transform.position) * .1f);

        Vector3 vectorToAttractor = transform.position - objectToAttract.transform.position;
        
        if (vectorToAttractor.magnitude > .1f) directionToAttractor = vectorToAttractor.normalized * Mathf.Clamp(30 - dis, 0, 40);
        else directionToAttractor = Vector3.zero;
    }

    void FixedUpdate() {
        rb.AddForce(directionToAttractor, ForceMode.Impulse);
    }
}
