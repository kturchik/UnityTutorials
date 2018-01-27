using UnityEngine;
using System.Collections;

public class SunCollect : MonoBehaviour {
    // Global score
    public static int score = 100;

    void OnMouseDown() {
        // Increase Score
        score += 20;

        // Destroy Sun
        Destroy(gameObject);
    }
}