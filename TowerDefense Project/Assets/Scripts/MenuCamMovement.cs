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
    // Start is called before the first frame update
    private void Start()
    {
        _noNeedToMove = false;
        _targetWaypoint = waypointHolder.GetComponent<Waypoints>().waypoints[0];
        _currentWaypointIndex = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_targetWaypoint != null && !_noNeedToMove)
        {
            FaceTheWay();
            Vector3 directions = _targetWaypoint.position - transform.position; // kameraya hedefe yönlendirdik
            transform.Translate(directions.normalized * Time.deltaTime * camSpeed, Space.World); // kamerayı hareket ettiriyoruz
            
            if (Vector3.Distance(transform.position, _targetWaypoint.position) < 0.2f) // kameranın hedefe ulaşıp ulaşmadığını kontrol ediyoruz
            {
                GetNextWaypoint();
            }
        }

        
        
    }

    private void FaceTheWay()
    {
        //Make the camera look at the target
        var direction = _targetWaypoint.position - transform.position;
        var lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * camSpeed);
        
    }
    
    public void ChangeMovingStatus(bool status)
    {
        _noNeedToMove = status;
    }
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
