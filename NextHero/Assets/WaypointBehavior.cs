using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointBehavior : MonoBehaviour
{
    public GameObject NextWaypoint; // This waypoint's next target for sequential plane movement                     

    private CameraSupport s;        // World bounds
    private Color c;                // Color component
    private float alphaI;           // Initial value of alpha value
    private float reduction;        // Reduction of alpha value every hit
    private PlaneBehavior plane;
    private GameManager gManager;

    // Start is called before the first frame update
    void Start()
    {
        s = Camera.main.GetComponent<CameraSupport>();
        c = GetComponent<SpriteRenderer>().color;
        alphaI = c.a;
        reduction = (float) (c.a * .25);
    }

    // Update is called once per frame
    void Update()
    {
        // Move waypoint when alpha is 0 and reset alpha value, not deleting it
        if (c.a <= 0)
        {
            // Get pos and store it as well
            Vector3 pos = transform.position;
            Vector3 posI = pos;
            // x and y adds (includes 60% of worlds bounds checker so that waypoint doesnt move outside of camera view)
            float x = Random.Range(-2f, 2f);
            float y = Random.Range(-2f, 2f);
            pos.x += x;
            pos.y += y;
            transform.position = pos;

            // Check if new position is in bounds
            Renderer rend = GetComponent<Renderer>();
            while (!(s.isInside(rend.bounds)))          // Keep looping until in bounds
            {
                // Restart and do process again
                pos = posI;
                x = Random.Range(-2f, 2f);
                y = Random.Range(-2f, 2f);
                pos.x += x;
                pos.y += y;
                transform.position = pos;
            }
            
            // Reset alpha value
            c.a = alphaI;
            GetComponent<SpriteRenderer>().color = c;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Plane collision
        if (other.tag == "Target")
        {
            plane = other.gameObject.GetComponent<PlaneBehavior>();                             // Get the plane component and change its target to the next waypoint
            gManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();    // Get bool that determines if sequential or random switching

            // Choose sequentially
            if (gManager.sequential)
                plane.targetedWaypoint = NextWaypoint;
            // Choose randomly
            else
            {
                // Choose random index
                int i = Random.Range(0, plane.waypoints.Length);
                // Be sure its not the same waypoint
                while (plane.waypoints[i] == gameObject)
                    i = Random.Range(0, plane.waypoints.Length);
                // Change targeted waypoint to randomly selected waypoint
                plane.targetedWaypoint = plane.waypoints[i];
            }
        }

        // Egg projectile hit
        if (other.tag == "Projectile" && GetComponent<SpriteRenderer>().enabled)
        {
            // Takes 4 steps to reduce alpha value to 0
            c.a -= reduction;
            GetComponent<SpriteRenderer>().color = c;
            Debug.Log("Egg hit waypoint!");
        }
    }
}
