using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Snake : MonoBehaviour {
    // Current Movement Direction
    // (by default it moves to the right)
    Vector2 dir = Vector2.right;

    // Keep Track of Tail
    List<Transform> tail = new List<Transform>();
    
    // Did the snake eat something?
    bool ate = false;

    //direction

    // Tail Prefab
    public GameObject tailPrefab;

    // Use this for initialization
    void Start () {
        // Move the Snake every 200ms
        InvokeRepeating("Move", 0.2f, 0.2f);    
    }
    
    // Update is called once per Frame
    void Update() {
        // Move in a new Direction?
        if (dir != Vector2.left && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)))
            dir = Vector2.right;
        else if (dir != Vector2.up && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)))
            dir = Vector2.down;    // '-up' means 'down'
        else if (dir != Vector2.right && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)))
            dir = Vector2.left; // '-right' means 'left'
        else if (dir != Vector2.down && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))
            dir = Vector2.up;
    }
    
    void Move() {
        // Save current position (gap will be here)
        Vector2 v = transform.position;

        // Move head into new direction (now there is a gap)
        transform.Translate(dir);

        // Ate something? Then insert new Element into gap
        if (ate) {
            // Load Prefab into the world
            GameObject g =(GameObject)Instantiate(tailPrefab,
                                                  v,
                                                  Quaternion.identity);

            // Keep track of it in our tail list
            tail.Insert(0, g.transform);

            // Reset the flag
            ate = false;
        }
        // Do we have a Tail?
        else if (tail.Count > 0) {
            // Move last Tail Element to where the Head was
            // Only moves one square instead of moving the entire snake but makes the snake appear to be moving
            tail.Last().position = v;

            // Add to front of list, remove from the back
            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count-1);
        }
    }
    
    void OnTriggerEnter2D(Collider2D col) {
        // Food?
        if (col.gameObject.tag == "Food") {
            // Get longer in next Move call
            ate = true;
            
            // Remove the Food
            Destroy(col.gameObject);
        }
        // Collided with Tail or Border
        else {
            // ToDo 'You lose' screen
            Debug.Log("Game Over");

            // Pause the game
            Time.timeScale = 0;
        }
    }
}