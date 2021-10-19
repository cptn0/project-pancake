using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Coin : MonoBehaviour {
    public float pickUpRadius = 1.5f;
    public float rotationSpeed = 10;
    public int value = 5;

    SphereCollider myCollider;

    private void Awake() {
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = pickUpRadius;
        rotationSpeed += Random.Range(-25, 25);
    }

    private void Update() {
        transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickUpRadius);
    }

    private void OnTriggerEnter(Collider other) {
        PickUpCoin(other);
    }

    public void PickUpCoin(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player != null) 
                player.IncreaseScore(value);
            
            Destroy(this.gameObject);
        }
    }
}
