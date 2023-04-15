using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBehavior : MonoBehaviour
{
    private string mode;
    private int eggCounter;
    private int touchedEnemyCounter;
    private int enemyCounter;
    private int destroyedCounter;

    GameObject go;

    PlayerMovement pMovement;
    GameManager gManager;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Track via player
        go = GameObject.FindWithTag("Player");
        pMovement = go.GetComponent<PlayerMovement>();

        // Movement Mode
        if (pMovement.movementMode)
            mode = "Mouse";
        else
            mode = "Keyboard";

        // Eggs on screen counter
        eggCounter = pMovement.eggsOnScreen;
        // Destroyed by touch counter
        touchedEnemyCounter = pMovement.playerHit;
        
        // Track via manager
        go = GameObject.FindWithTag("GameController");
        gManager = go.GetComponent<GameManager>();

        // Enemy counter
        enemyCounter = gManager.planeCounter;
        // Destoryed counter
        destroyedCounter = gManager.eggHit + pMovement.playerHit;   // egg hit + player hit = total destruction counter
        
        // What's printed to screen
        GetComponent<UnityEngine.UI.Text>().text = "Movement Mode: " + mode + "         Enemies Touched: " + touchedEnemyCounter.ToString() +
         "          No. of Eggs on Screen " + eggCounter + "        No. of Enemies: " + enemyCounter.ToString() + "         No. of Enemies Destoryed: " + destroyedCounter;
    }
}
