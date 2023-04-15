using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class PlaneBehavior : MonoBehaviour
{
    private float reduction;                            // Amount to reduce alpha value by
    private Color colorTemp;                            // To change color of plane

    GameManager gManager;                               // Game manager for plane counter
    // Start is called before the first frame update
    void Start()
    {
        // Finding the right reduction for alpha every step
        colorTemp = GetComponent<SpriteRenderer>().color;
        reduction = (float) (colorTemp.a * .25);

        GameObject go = GameObject.FindWithTag("GameController");
        gManager = go.GetComponent<GameManager>();
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
}
