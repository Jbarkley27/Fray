using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class EnemyMovementModule : MonoBehaviour
{
    public Rigidbody rb;
    public float rotateSpeed = 1.0f;
    public NavMeshAgent mNavMeshAgent;
    public LineRenderer movementLineRenderer;
    public LineRenderer finalLineRenderer;
    public int pointsPerSegment = 5;  // Number of new points per segment
    public GameObject pointPrefab;
    public List<Vector3> newPoints = new List<Vector3>();
    public List<GameObject> pointObjects = new List<GameObject>();
    public bool shouldStartCalculatingPath = false;
    public Vector3 playerPos;
    [Range(0, 100)]
    public int percentageOfPath = 50;
    public Transform pointRoot;



    // Start is called before the first frame update
    void Start()
    {
        mNavMeshAgent = GetComponent<NavMeshAgent>();
        StopMovement();
    }

    // Update is called once per frame
    void Update()
    {
        KeepTrackOfPlayerPosition();
        RotateTowards(TurnBasedManager.instance.movementSystem.gameObject.transform.position - transform.position);

        PathCalculation();
    }

    public void KeepTrackOfPlayerPosition()
    {
        playerPos = TurnBasedManager.instance.IsTimePaused() ? GlobalDataStore.instance.player.transform.position : playerPos;

        // if player has movement tracking enabled then set playerPos to keep tracking player position event during gameplay

        if (GetComponent<EnemyBaseComponent>().MovementWithTracking)
        {
            playerPos = GlobalDataStore.instance.player.transform.position;
        }
    }

    public void PathCalculation()
    {
        if (shouldStartCalculatingPath)
        {
            ShowDestination();
        }
    }

    public void RotateTowards(Vector3 targetDirection)
    {
        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        // Smoothly interpolate between current and target rotation
        Quaternion smoothedRotation = Quaternion.Slerp(
            rb.rotation,             // Current rotation
            targetRotation,          // Target rotation
            rotateSpeed * Time.deltaTime // Interpolation factor
        );

        // Apply the smooth rotation to the Rigidbody
        rb.MoveRotation(smoothedRotation);
    }

    public bool IsWithinRange()
    {
        return Vector3.Distance(transform.position, TurnBasedManager.instance.movementSystem.gameObject.transform.position) < mNavMeshAgent.stoppingDistance;
    }


    public List<Vector3> finalPointPath = new List<Vector3>();

    public void MoveToPlayer()
    {
        ResumeMovement();
        finalPointPath.Clear();
        finalPointPath.AddRange(newPoints);

        rb.DOPath(finalPointPath.ToArray(), 1.0f, PathType.CatmullRom);
    }

    public void StopMovement()
    {
        mNavMeshAgent.isStopped = true;
    }

    public void ResumeMovement()
    {
        // mNavMeshAgent.isStopped = false;
    }

    public void ShowDestination()
    {
        Debug.Log("Calculating path");

        // Get current path from enemy to player
        mNavMeshAgent.SetDestination(playerPos);


        // Setup first iteration of the line
        finalLineRenderer.gameObject.SetActive(true);
        finalLineRenderer.positionCount = mNavMeshAgent.path.corners.Length;
        finalLineRenderer.SetPosition(0, transform.position);
        finalLineRenderer.SetPositions(mNavMeshAgent.path.corners);

        // create a path of points
        CreateMorePointsForLine();
    }




    public void StartCalculatingPath()
    {
        Debug.Log("Starting to calculate path");
        shouldStartCalculatingPath = true;
    }

    public void StopCalculatingPath()
    {
        Debug.Log("Stopping to calculate path");
        shouldStartCalculatingPath = false;
    }



    public void CreateMorePointsForLine()
    {
        if (finalLineRenderer == null)
        {
            Debug.LogError("LineRenderer is not assigned.");
            return;
        }


        // Step 1: Get the current points of the LineRenderer
        Vector3[] originalPoints = new Vector3[finalLineRenderer.positionCount];
        finalLineRenderer.GetPositions(originalPoints);

        newPoints.Clear();

        // pointsPerSegment should refelct the length of the path
        float distance = Vector3.Distance(originalPoints[0], originalPoints[originalPoints.Length - 1]);

        pointsPerSegment = (int)distance / 2;

        Mathf.Clamp(pointsPerSegment, 30, 100);


        // Add the first point
        newPoints.Add(originalPoints[0]);

        // Step 3: Loop through the original points and insert new points between them
        for (int i = 0; i < originalPoints.Length - 1; i++)
        {
            Vector3 start = originalPoints[i];
            Vector3 end = originalPoints[i + 1];

            // Calculate the distance between the points
            float segmentLength = Vector3.Distance(start, end);

            // Calculate the distance between each new point
            float stepDistance = segmentLength / (pointsPerSegment + 1); // +1 because the first and last are already included

            // Generate new points between start and end
            for (int j = 1; j <= pointsPerSegment; j++)
            {
                // Calculate the interpolation factor (t) and find the new point
                float t = j / (float)(pointsPerSegment + 1);
                Vector3 newPoint = Vector3.Lerp(start, end, t);
                newPoints.Add(newPoint);
            }

            // Add the end point (final corner)
            if (i == originalPoints.Length - 2) 
            {
                newPoints.Add(originalPoints[i + 1]);
            }
        }

        // calculate the percentage of the path
        int pathPercentage = (int)(newPoints.Count * (percentageOfPath / 100.0f));

        // change newPoints to reflect the percentage of the path
        newPoints.RemoveRange(pathPercentage, newPoints.Count - pathPercentage);



        // This point we have newPoints which is a list Vector3, next we need to create the points
        ClearAllPoints();

        pointObjects.Clear();

        // Instantiate the point prefab at each new point
        for (int i = 0; i < newPoints.Count; i++)
        {
            GameObject point = Instantiate(pointPrefab, newPoints[i], Quaternion.identity, pointRoot);
            pointObjects.Add(point);
        }


        // newTarget = pointObjects[pointObjects.Count - 1].transform.position;

        // // print out all new points

        // // // Set the destination of the NavMeshAgent to the middle point
        // mNavMeshAgent.SetDestination(GlobalDataStore.instance.player.transform.position);

        // SetupFinalLine();

        // Set the position of the LineRenderer to the middle point
        // movementLineRenderer.positionCount = mNavMeshAgent.path.corners.Length;
        // movementLineRenderer.SetPosition(0, transform.position);
        // movementLineRenderer.SetPositions(mNavMeshAgent.path.corners);

        // Debug.Log("New points: " + newPoints.Count);
        // Debug.Log("Has Path: " + mNavMeshAgent.hasPath);
        // Debug.Log("Path corners: " + mNavMeshAgent.path.corners.Length);
    }

    public void ClearAllPoints()
    {
        // find all gameobjects with the tag PathPoint
        foreach(Transform child in pointRoot)
        {
            Destroy(child.gameObject);
        }
    }

    public void SetupFinalLine()
    {
        finalLineRenderer.gameObject.SetActive(true);
        finalLineRenderer.positionCount = mNavMeshAgent.path.corners.Length;
        finalLineRenderer.SetPosition(0, transform.position);
        finalLineRenderer.SetPositions(mNavMeshAgent.path.corners);
    }

    // public bool HasFullPathReady()
    // {
    //     return newPoints.Count > pointsPerSegment;
    // }
}
