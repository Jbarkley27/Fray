using System;
using System.Collections;
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

    void Start()
    {
        // slow down time
        Invoke("SlowDownTime", 1);
    }

    public void SlowDownTime()
    {
        currentTimeState = TimeState.Pause;
        movementSystem.FreezeMovement();
        EnemyManager.instance.StopAllEnemyMovement();
        ChangeStarfieldSpeed(0.1f);
        EnemyManager.instance.GetAllNextIntention();
    }

    public void ResumeTime()
    {
        currentTimeState = TimeState.Normal;
        ChangeStarfieldSpeed(1f);
        movementSystem.UnfreezeMovement();
        EnemyManager.instance.ExecuteAllNextIntention();

        // invoke slow down time after 1 second
        Invoke("SlowDownTime", timeBetweenTurns);
    }

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