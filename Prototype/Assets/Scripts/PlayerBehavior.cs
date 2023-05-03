using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{
    public float speed = 3f;
    private Vector3 pos;
    private Rigidbody2D rb;
    private GameObject selectedSlot;
    private Inventory inventory;
    private int i;
    private Color iColor;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>(); 
        i = -1;
        iColor = GameObject.FindGameObjectWithTag("Slot").GetComponent<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {
        // WASD Movement
        transform.position = new Vector2(transform.position.x + Input.GetAxis("Horizontal")*speed*Time.smoothDeltaTime, transform.position.y + Input.GetAxis("Vertical")*speed*Time.smoothDeltaTime);

        // Scroll wheel and left/right arrow hotbar selection
        if (Input.mouseScrollDelta.y > 0 || Input.GetKeyDown(KeyCode.RightArrow)) {
            // Loop to beginning
            if (i >= inventory.getSlots().Length - 1) {
                i = 0;
            } else {
                i++;
            }

            selectedSlot = inventory.getSlots()[i];

            // Make normal configuration
            foreach (GameObject s in inventory.getSlots()) { 
                s.GetComponent<Image>().color = iColor;                                                                 // Color for slot image
                // Only if there is an item present in slot
                if (s.GetComponent<SlotBehavior>().item) {
                    s.GetComponent<SlotBehavior>().item.GetComponent<Transform>().position = new Vector2(10000, 10000); // Position
                    s.GetComponent<SlotBehavior>().item.GetComponent<PickUp>().TurnActive(false);                       // If there is item, set it to false
                }
            }
        } else if (Input.mouseScrollDelta.y < 0 || Input.GetKeyDown(KeyCode.LeftArrow)) {
            // Loop to end
            if (i <= 0) {
                i = inventory.getSlots().Length - 1;
            } else {
                i--;
            }

            selectedSlot = inventory.getSlots()[i];
            
            // Make normal configuration
            foreach (GameObject s in inventory.getSlots()) { 
                s.GetComponent<Image>().color = iColor;
                if (s.GetComponent<SlotBehavior>().item) {
                    s.GetComponent<SlotBehavior>().item.GetComponent<Transform>().position = new Vector2(10000, 10000); // Position
                    s.GetComponent<SlotBehavior>().item.GetComponent<PickUp>().TurnActive(false);
                }
            }
        }

        // Selected configuration
        if (selectedSlot) {
            // Change UI slot Color
            Color32 color32 = new Color32(179, 168, 167, 255);
            Color color = color32;
            selectedSlot.GetComponent<Image>().color = color; 

            // Active item
            if (selectedSlot.GetComponent<SlotBehavior>().item) {
                selectedSlot.GetComponent<SlotBehavior>().item.GetComponent<PickUp>().TurnActive(true);
            }
        }

        // Only if there is an item in the slot, Press G to drop item
        if (selectedSlot && selectedSlot.GetComponent<SlotBehavior>().item && Input.GetKeyDown(KeyCode.G)) {
            // Drpped so turn active item off
            selectedSlot.GetComponent<SlotBehavior>().item.GetComponent<PickUp>().TurnActive(false);

            // Drop item
            selectedSlot.GetComponent<SlotBehavior>().DropItem(); 
        }
    }
}
