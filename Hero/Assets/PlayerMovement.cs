using UnityEngine;	
using System.Collections;
// Used for absolute value for velocity (magnitude of velocity is used)
using System;

public class PlayerMovement : MonoBehaviour {
	public float kHeroSpeed = 1f;
	private float kHeroRotateSpeed = 90f/2f;                // 90-degrees in 2 seconds

    [System.NonSerialized] public bool movementMode = true; // Keep track of movement mode, public to be accessible in other scripts
    [System.NonSerialized] public int playerHit = 0;        // Keep track of player hitting target
    [System.NonSerialized] public int eggsOnScreen = 0;     // Keep track of no. of eggs on screen

    public float cooldown = 3;                              // Cooldown to not spam firing prefab 
    private float counter = 0;                              // Counter keeps track of cooldown

    // States of the hero movement
    private Direction currDir = Direction.Start;            // Start: Hero is frozen at beginning 

    GameManager gManager;                                   // Game manager for plane counter

	// Use this for initialization
	void Start () {

	}

    // Movements of the hero, allows the hero to constantly be moving based on speed/direction
    private enum Direction {
        Up,
        Down,
        Start
    }
	
	// Update is called once per frame
	void Update () 
    {
        // Switch modes when M is pressed
        if (Input.GetKeyUp(KeyCode.M))
            movementMode = !movementMode;
        
        // Mouse mode
        if (movementMode)
        {
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(cursorPos.x, cursorPos.y, 0);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        // Keyboard mode
        else
        {
            // Moving up
            if (currDir == Direction.Up)
                transform.position += 1  * transform.up * (Math.Abs(kHeroSpeed) * Time.smoothDeltaTime);    // 1 replaces the GetAxis() since this is meant for moving up
            // Moving down
            if (currDir == Direction.Down)
                transform.position += -1  * transform.up * (Math.Abs(kHeroSpeed) * Time.smoothDeltaTime);   // -1 replaces GetAxis(), meant for moving down
            
            // Acceleration handling
            if (Input.GetKey(KeyCode.W))
            {
                kHeroSpeed += 0.02f;
                // Only change direction once velocity is a positive force
                if (kHeroSpeed > 0)
                    currDir = Direction.Up;
            }
            if (Input.GetKey(KeyCode.S))
            {
                kHeroSpeed -= 0.02f;
                // Only change direction once velocity is a negative force
                if (kHeroSpeed < 0)
                    currDir = Direction.Down;
            }

            // Rotaional movement                            
            // Fire1 is a
            // Fire2 is d
            float angle = Input.GetAxis("Fire1") * (kHeroRotateSpeed * Time.smoothDeltaTime); 
            angle -= Input.GetAxis("Fire2") * (kHeroRotateSpeed * Time.smoothDeltaTime); 
                            
            transform.Rotate(transform.forward, angle);
        }

        // Projectile mechanic

        // Shoot with space
        if (Input.GetKey(KeyCode.Space))
        {
            // instantiate prefab only if counter is past its cooldown
            if (counter <= 0)
            {
                Instantiate(Resources.Load("Prefabs/Egg") as GameObject, transform.position, transform.rotation);
                // After firing, reset cooldown
                counter = cooldown;

                eggsOnScreen++;    // Update no. of eggs on screen counter 
            }
        }
        // Counting down cooldown
        counter -= (float) 1.15 * Time.deltaTime;
	}

    // Player collision with target
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Find and access plane counter
        GameObject go = GameObject.FindWithTag("GameController");
        GameManager gManager = go.GetComponent<GameManager>();
        gManager.planeCounter -= 1;
        Destroy(other.gameObject);
        // Update hit counter
        playerHit++;
    }
}
