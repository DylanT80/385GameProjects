using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class PlaneBehavior : MonoBehaviour
{
    public float speed = 0.2f;                                                  // Speed of plane
    private float t;

    private float reduction;                                                    // Amount to reduce alpha value by
    private Color colorTemp;                                                    // To change color of plane
    [System.NonSerialized] public GameObject[] waypoints;                       // Array of the waypoints
    [System.NonSerialized] public GameObject targetedWaypoint;                  // Waypoint to fly towards

    GameManager gManager;                                                       // Game manager for plane counter
    // Start is called before the first frame update
    void Start()
    {
        // Finding the right reduction for alpha every step
        colorTemp = GetComponent<SpriteRenderer>().color;
        reduction = (float) (colorTemp.a * .25);

        GameObject go = GameObject.FindWithTag("GameController");
        gManager = go.GetComponent<GameManager>();

        // Initially, choose random waypoint upon spawning
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        targetedWaypoint = waypoints[Random.Range(0, waypoints.Length)];

        t = speed;
    }

    // Update is called once per frame
    void Update()
    {
        // Plane is dead!
        if (colorTemp.a <= 0)
        {
            Destroy(gameObject);
            gManager.planeCounter -= 1;                 // Reducing counter will create a new plane by the < planeCount check
            gManager.eggHit++;                          // Increase egg destroy counter
        }

        // Plane movement to targetedWaypoint, is updated every frame to always follow the "current" target

        // Move towards target
        Vector3 currentPos = transform.position;
        Vector3 targetPos = targetedWaypoint.transform.position;
        transform.position = Vector3.MoveTowards(currentPos, Vector3.Lerp(currentPos, targetPos, t), speed);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        // Egg projectile hit
        if (other.tag == "Projectile")
        {
            // Takes 4 steps to reduce alpha value to 0
            colorTemp.a -= reduction;
            GetComponent<SpriteRenderer>().color = colorTemp;
            Debug.Log("Egg hit!");
        }
    }
    
    // For plane to look at target waypoint (rotation)
    private void FixedUpdate()
    {
        if (targetedWaypoint != null)
        {
            Vector3 vectorToTarget = targetedWaypoint.transform.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 20);
        }

    }
}
