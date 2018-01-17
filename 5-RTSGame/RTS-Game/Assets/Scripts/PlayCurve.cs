using UnityEngine;
using System.Collections;

public class PlayCurve : MonoBehaviour {

    // The Curve
    public AnimationCurve curve;
        
    IEnumerator sample() {
        // go through curve time
        for (float t=0; t<curve.keys[curve.length-1].time; t+=Time.deltaTime) {
            // scale a bit
            transform.localScale = Vector3.one * (1 + curve.Evaluate(t));
    
            // come back next Update
            yield return null;
        }
    }
    
    public void Toggle() {
        StartCoroutine("sample");
    }
}