using UnityEngine;
using System.Collections;

public class MoveByPlayer : MonoBehaviour {
    // Selection Circle
    public GameObject circle;
    
    // Update is called once per frame
    void Update () {
        // Rightclicked while selected?
        if (Input.GetMouseButtonDown(1) && circle.activeSelf) {
            // Find out where the user clicked in the 3D world
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
                GetComponent<UnityEngine.AI.NavMeshAgent>().destination = hit.point;
        }
    }
}
