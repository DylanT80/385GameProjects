using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotBehavior : MonoBehaviour
{
    private Inventory inventory;
    [System.NonSerialized] public GameObject item;  // Item slot contains
    [System.NonSerialized] public int i;            // Index for this slot in array
    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount <= 0) {
            inventory.getFull()[i] = false;
        }
    }

    // Delete button and spawn dropped item on current slot
    public void DropItem() {
        // Delete the UI image
        foreach (Transform child in transform)
        {
            child.GetComponent<Spawn>().SpawnDroppedItem(item); // Drop the item
            Destroy(child.gameObject);                  
        }
        item.GetComponent<PickUp>().picked = false;             // No longer picked
        item = null;                                            // No item in slot now
    }
}
