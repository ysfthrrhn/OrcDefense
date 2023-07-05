using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public Transform[] waypoints;

    private void Awake()
    {
        waypoints = new Transform[transform.childCount];

        for (var i =0; i< waypoints.Length; i++)
        {
            waypoints[i] = transform.GetChild(i);
        }
    }
}
