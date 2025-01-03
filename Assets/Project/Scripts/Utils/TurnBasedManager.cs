using System.Collections.Generic;
using UnityEngine;

public class TurnBasedManager : MonoBehaviour
{
    public float timeBetweenTurns = 1.0f;
    public enum TimeState
    {
        Normal,
        Pause
    }
    public List<ParticleSystem> starFieldVFX = new List<ParticleSystem>();
    public TimeState currentTimeState = TimeState.Normal;
    public static TurnBasedManager instance;
    public MovementSystem movementSystem;
    public int turnCount = 0;

   private void Awake()
    {
        // if there is already an instance of this object
        if (instance != null)
        {
            // log an error message
            Debug.LogError("Found an Turn Based Manager object, destroying new one");
            // destroy the new object
            Destroy(gameObject);
            // return from this method
            return;
        }
        // set the instance to this object
        instance = this;
        // don't destroy this object when loading a new scene
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        turnCount = 0;
        Invoke("SlowDownTime", 1);
        Debug.Log("Starting Battle");
    }





    // TIME HANDLING ---------------------------------------------------------
    private void SlowDownTime()
    {
        // set the current time state to pause
        currentTimeState = TimeState.Pause;

        // increment the turn count
        turnCount++;

        // freeze movements of all combatants and environment
        Debug.Log("Stopping all movement");
        EnemyManager.instance.StopAllEnemyMovement();
        ChangeStarfieldSpeed(0.1f);

        // have all enemies calculate their next move
        Debug.Log("Enemies calculating next move");
        EnemyManager.instance.GetAllNextIntention();
    }


    public void ResumeTime()
    {
        // set the current time state to normal
        Debug.Log("Resuming time");
        currentTimeState = TimeState.Normal;

        // unfreeze movements of all combatants and environment
        ChangeStarfieldSpeed(1f);

        // have all enemies execute their next move
        EnemyManager.instance.ExecuteAllNextIntention();

        // invoke slow down time after 1 second
        Invoke("SlowDownTime", timeBetweenTurns);
    }







    // HELPERS ---------------------------------------------------------------
    public bool IsTimePaused()
    {
        return currentTimeState == TimeState.Pause;
    }

    public void ChangeStarfieldSpeed(float speed)
    {
        foreach (ParticleSystem starField in starFieldVFX)
        {
            var main = starField.main;
            main.simulationSpeed = speed;
        }
    }
}