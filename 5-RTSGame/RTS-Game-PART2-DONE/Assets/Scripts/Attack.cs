using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {
    // attack range
    public float range = 5;

    // attack interval
    public float interval = 1.5f;
    
    // tag of the unit that should be attacked
    public string enemyTag = "";

    // arrow prefab (to shoot at enemies)
    public GameObject arrow;

    // Use this for initialization
    void Start() {
        InvokeRepeating("Fire", interval, interval);
    }
    
    void Fire() {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag(enemyTag)) {
            // still alive?
            if (g != null) {
                // in attack range?				
                if (Vector3.Distance(g.transform.position, transform.position) <= range) {
                    // shoot arrow at it
                    GameObject a = (GameObject)Instantiate(arrow, 
                                                           transform.position, // default position
                                                           Quaternion.identity); // default rotation
                    
                    // set its target
                    a.GetComponent<Arrow>().target = g.transform;
                    
                    // animation
                    GetComponent<PlayCurve>().Toggle();
                    
                    // done for now
                    break;
                }
            }
        }
    }
}
