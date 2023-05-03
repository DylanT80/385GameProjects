using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBehavior : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // If item is selcted in hotbar slot
        if (GetComponent<PickUp>().selected) {
            // Put in front of where player is moving, default is above
            float vertical = player.GetComponent<Transform>().position.y + Input.GetAxis("Vertical");
            float horizontal = player.GetComponent<Transform>().position.x + Input.GetAxis("Horizontal");
            if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0) {
                vertical += 1;
            }
            Vector2 newPos = new Vector2(horizontal, vertical);
            GetComponent<Transform>().position = newPos;
            GetComponent<BoxCollider2D>().enabled = false;      // Avoid picking another gun again when it moves

            // G uses its ability
            if (Input.GetKeyDown(KeyCode.E)) {
                AbilityBlink();
            }
        } else {
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    public void AbilityBlink() {
        Vector2 playerPos = new Vector2( player.GetComponent<Transform>().position.x + Input.GetAxis("Horizontal") * 2, player.GetComponent<Transform>().position.y + Input.GetAxis("Vertical") * 2);
        player.GetComponent<Transform>().position = playerPos;
    }
}
