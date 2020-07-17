using UnityEngine;

public class FollowThePath : MonoBehaviour {

    public Transform[] waypoints;

    [SerializeField]
    private float moveSpeed = 1f;

    [HideInInspector]
    public int waypointIndex = 0;

    

    public bool moveAllowed = false;
    public bool backAllowed = false;

    // Use this for initialization
    private void Start () {
        transform.position = waypoints[waypointIndex].transform.position;
	}
	
	// Update is called once per frame
	private void Update () {
        if (moveAllowed)
            Move();

        if (backAllowed)
            Back();
	}

    private void Move()
    {
        if (waypointIndex <= waypoints.Length - 1)
        {
            transform.position = Vector2.MoveTowards(transform.position,
            waypoints[waypointIndex].transform.position,
            moveSpeed * Time.deltaTime);
            
            //Debug.Log("original"+transform.position);
            //Debug.Log("waypoint"+waypoints[waypointIndex].transform.position);
            //Debug.Log("index"+waypointIndex);

            if (transform.position == waypoints[waypointIndex].transform.position)
            {
                waypointIndex += 1;
                //Debug.Log("index added");
            }
        }
    }

    private void Back()
    {
        //Debug.Log("Back running");
        if (waypointIndex <= waypoints.Length - 1)
        {
            transform.position = Vector2.MoveTowards(transform.position,
            waypoints[waypointIndex].transform.position,
            moveSpeed * Time.deltaTime);

            //Debug.Log("original" + transform.position);
            //Debug.Log("waypoint" + waypoints[waypointIndex].transform.position);
            //Debug.Log("index" + waypointIndex);

            if (transform.position == waypoints[waypointIndex].transform.position)
            {
                Debug.Log("Entered");
                waypointIndex -= 1;
            }
        }
    }
}
