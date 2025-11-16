using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wayPointFollower : MonoBehaviour
{
    public Transform[] waypoints;
    public int currentwayPointIndex;
    public float speed;
   

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(waypoints[currentwayPointIndex].position, transform.position) == 0f)
        {
            currentwayPointIndex++;
        }
        if(currentwayPointIndex == waypoints.Length)
        {
            currentwayPointIndex = 0;
        }
        transform.position = Vector2.MoveTowards(transform.position,
            waypoints[currentwayPointIndex].position, Time.deltaTime*speed);
    }
}
