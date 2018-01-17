using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
    // The target will be set by whoever uses the arrow.
    // The arrow will fly towards the target and deal damage.
    public Transform target;

    // The fly speed
    public float speed = 5;

    void FixedUpdate() {
        // Still has a Target?
        if (target) {
            // Fly towards it            
            Vector3 dir = target.position - transform.position;
            GetComponent<Rigidbody>().velocity = dir.normalized * speed;
            
            // Look at it
            transform.LookAt(target.position);
        } else {
            // Otherwise destroy self
            Destroy(gameObject);
        }
    }
    
    void OnTriggerEnter(Collider co) {
        // Hit the target?
        if (co.transform == target) {
            // Decrease target's health by 1
            --co.GetComponent<Health>().current;
            
            // Destroy self
            Destroy(gameObject);
        }
    }
}