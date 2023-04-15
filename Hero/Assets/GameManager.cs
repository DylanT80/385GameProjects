using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour
{
    public int planeCount = 10;                            // How many planes should be in the game
    [System.NonSerialized] public int planeCounter = 0;    // Keeps track of # of planes
    [System.NonSerialized] public int eggHit = 0;          // Keeps track of # of eggs destorying planes

    private CameraSupport s;
    // Start is called before the first frame update
    void Start()
    {
        s = Camera.main.GetComponent<CameraSupport>();
        Assert.IsTrue(s != null);
    }

    // Update is called once per frame
    void Update()
    {
        // Create new plane
        if (planeCounter < planeCount)
        {
            GameObject e = Instantiate(Resources.Load("Prefabs/Plane") as GameObject); // Prefab MUST BE locaed in Resources/Prefab folder!
            Vector3 pos;
            pos.x = (float) ((s.GetWorldBound().min.x + Random.value * s.GetWorldBound().size.x) * .9);     // Multiply .9 for 90% within world bounds
            pos.y = (float) ((s.GetWorldBound().min.y + Random.value * s.GetWorldBound().size.y) * .9);
            pos.z = 0;
            e.transform.localPosition = pos;

            // Update amount of planes
            planeCounter++;
        }

        if (Input.GetKeyDown(KeyCode.Q))
            Application.Quit();
    }
}
