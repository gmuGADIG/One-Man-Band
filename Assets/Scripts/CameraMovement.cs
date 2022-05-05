using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public GameObject followingMe; // in the inspector drag the gameobject the will be following the player to this field
    public int followDistance;
    [SerializeField]
    private List<Vector3> storedPositions;
    void Start()
    {
        storedPositions = new List<Vector3>(); //create a blank list

        if (!followingMe)
        {
            Debug.Log("The FollowingMe gameobject was not set");
        }

        if (followDistance == 0)
        {
            Debug.Log("Please set distance higher then 0");
        }
        transform.SetParent(null);
    }

    // Update is called once per .2 seconds
    void FixedUpdate()
    {
        storedPositions.Add(player.transform.position); //store the position every frame

        if (storedPositions.Count > followDistance)
        {
            Vector3 pos = storedPositions[0];
            pos.z -= 10;
            followingMe.transform.position = pos; //move the player
            storedPositions.RemoveAt(0); //delete the position that player just move to
        }
    }
}
