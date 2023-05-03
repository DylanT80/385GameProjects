using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float offset;
    public GameObject projectile;
    public Transform shotPoint;
    private GameObject player;

    private float timeBtwShots;
    public float startTimeBtwShots;

    enum Direction {North, East, South, West};
    private Direction dir;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        dir = Direction.South;
    }

    // Update is called once per frame
    void Update()
    {
        // If not selected
        if (!GetComponent<PickUp>().selected) {
            GetComponent<BoxCollider2D>().enabled = true;
            return;
        }

        // Put in front of where player is moving
        float vertical = (float) (player.GetComponent<Transform>().position.y + Input.GetAxis("Vertical")/2);
        float horizontal = (float) (player.GetComponent<Transform>().position.x + Input.GetAxis("Horizontal")/2);

        // if (Input.GetKey(KeyCode.W)) {
        //     dir = Direction.North;
        // } else if (Input.GetKey(KeyCode.A)) {
        //     dir = Direction.West;
        // } else if (Input.GetKey(KeyCode.S)) {
        //     dir = Direction.South;
        // } else if (Input.GetKey(KeyCode.D)) {
        //     dir = Direction.East;
        // }

        // No input, default is to right for now
        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0) {
            horizontal += (float) 0.5;
        }
        Vector2 newPos = new Vector2(horizontal, vertical);
        GetComponent<Transform>().position = newPos;

        GetComponent<BoxCollider2D>().enabled = false;      // Avoid picking another gun again when it moves
        
        // Weapon stuff // 

        // Rotating the weapon
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; // direction = destination (cursor) - origin (weapon)
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
    
        

        // Weapon fire rate
        if (timeBtwShots <= 0) {
            if (Input.GetMouseButtonDown(0)) {
                Instantiate(projectile, shotPoint.position, transform.rotation);
                timeBtwShots = startTimeBtwShots;
            }
        }
        else {
            timeBtwShots -= Time.deltaTime;
        }
    }
}
