using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUp : MonoBehaviour
{
    public GameObject ItemButton;
    private Inventory inventory;
    private Quaternion iRotation;
    [System.NonSerialized] public bool selected;
    [System.NonSerialized] public bool picked;
    // Reduce opacity and then delete
    private Color iColor;
    private float reduction;


    // Start is called before the first frame update
    void Start()
    {
       inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>(); 
       Invoke();

        // Calcualte reduction
       iColor = GetComponent<SpriteRenderer>().color;
       reduction = (float) (iColor.a * .00034);

       selected = false;
       picked = false;

       iRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // if not selected
        if (!picked) {
            // Lower opacity every frame
            Color c = GetComponent<SpriteRenderer>().color;
            c.a -= reduction;
            GetComponent<SpriteRenderer>().color = c;
            // Rotation is normal
            transform.rotation = iRotation;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        // Player picks up object
        if (other.tag == "Player") {
            inventory.AddItem(ItemButton, gameObject);
        }
    }

    // Public timer to destroy 
    public void Invoke() {
        Invoke("DestroyMe", 10);
    }

    void DestroyMe() {
        Destroy(gameObject);
    }

    public void TurnActive(bool a) {
        selected = a;
    }
}
