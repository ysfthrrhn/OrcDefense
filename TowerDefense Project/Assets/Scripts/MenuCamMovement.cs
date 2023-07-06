using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamMovement : MonoBehaviour
{
    public GameObject waypointHolder;

    private Transform _targetWaypoint;
    private int _currentWaypointIndex;

    public float camSpeed;
    private bool _noNeedToMove;
   
    // Sets moving camera to start
    private void Start()
    {
        _noNeedToMove = false;
        _targetWaypoint = waypointHolder.GetComponent<Waypoints>().waypoints[0];
        _currentWaypointIndex = 0;
    }

    // Move the cam
    private void Update()
    {
        if (_targetWaypoint != null && !_noNeedToMove)
        {
            FaceTheWay();
            Vector3 directions = _targetWaypoint.position - transform.position; // Directing the camera to the targetPoint
            transform.Translate(directions.normalized * Time.deltaTime * camSpeed, Space.World); // Moving the camera
            
            if (Vector3.Distance(transform.position, _targetWaypoint.position) < 0.2f) // Checking is camera reached to the targetPoint
            {
                GetNextWaypoint();
            }
        }

        
        
    }

    /// <summary>
    /// Make the camera look at the target.
    /// </summary>
    private void FaceTheWay()
    {
        var direction = _targetWaypoint.position - transform.position;
        var lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * camSpeed);   
    }
    
    /// <summary>
    /// Sets next point as a new target to Camera
    /// </summary>
    private void GetNextWaypoint()
    {
        if (_currentWaypointIndex >= waypointHolder.GetComponent<Waypoints>().waypoints.Length - 1) // eğer son hedefe ulaşıldıysa
        {
            _targetWaypoint = null;
            camSpeed = 0;
            _noNeedToMove = true;
        }
        else
        {
            _currentWaypointIndex++;
            _targetWaypoint = waypointHolder.GetComponent<Waypoints>().waypoints[_currentWaypointIndex];
        }
    }
}
