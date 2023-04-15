using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class EggAction : MonoBehaviour
{
    public float projectileSpeed = 5f;

    private GameObject player;
    private CameraSupport s;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");

        s = Camera.main.GetComponent<CameraSupport>();
        Assert.IsTrue(s != null);
    }

    // Update is called once per frame
    void Update()
    {
        // Keeps travelling up in direction
        transform.position += 1 * transform.up * projectileSpeed * Time.smoothDeltaTime;

        // Destory egg when out of world bounds
        if (transform.position.x > s.GetWorldBound().size.x / 2 || transform.position.x < -1 * s.GetWorldBound().size.x / 2)    // / 2 since world bounds is whole width and starting position is in middle
            DestoryProjectile();
        if (transform.position.y > s.GetWorldBound().size.y / 2 || transform.position.y < -1 * s.GetWorldBound().size.y / 2)    // / 2 since world bounds is whole height and starting position is in middle
            DestoryProjectile();
    }

    // Destroy projectile on collision
    private void OnTriggerEnter2D(Collider2D other) 
    {
        DestoryProjectile();
    }

    // Destroy projectile
    private void DestoryProjectile()
    {
        PlayerMovement pMovement = player.GetComponent<PlayerMovement>();
        pMovement.eggsOnScreen--;

        Destroy(gameObject);
    }
}
