using UnityEngine;

public class DirectionFinder : MonoBehaviour {

    // this field is used in Unity to specify where the arrow should be pointing at
    public Transform destination;
	
	// Update is called once per frame
	void Update () {
        // find the nearest target object
        destination = FindTarget();
        // if there is a nearest target object..
        if (destination != null)
        {
            // .. set the target position to be the position returned by FindTarget() method
           Vector3 targetPosition = destination.transform.position;
            // rotate only around the Y axis
            targetPosition.y = transform.position.y;
            // direct the arrow towards the position of the nearest object
            transform.LookAt(targetPosition);
        }
	}

    // this method finds the nearest target object
    public Transform FindTarget()
    {
        // find all objects with a tag "Pick Up"
        GameObject[] candidates = GameObject.FindGameObjectsWithTag("Pick Up");
        // instance fields for the shortest distance and the closest object
        float minDistance = Mathf.Infinity;
        Transform closest;
        // if there is no objects with the tag Pick Up, return null
        if (candidates.Length == 0)
            return null;
        // at the beginning, set the closet object to the first one in the array
        closest = candidates[0].transform;
        // iterate over the array of Pick Up objects
        for (int i = 1; i < candidates.Length; ++i)
        {
            // find the distance between the Player (Camera (eye) specifically) and the Pick Up object at current array index
            float distance = (candidates[i].transform.position - transform.position).sqrMagnitude;
            // if the distance is smaller than the minimum distance..
            if (distance < minDistance)
            {
                // .. set the new closest object to the current Pick Up object in the array
                closest = candidates[i].transform;
                // set the minimum distance to the distance between the Player (Camera (eye) specifically) and 
                // the Pick Up object at current index in the array
                minDistance = distance;
            }
        }
        // return the closest object out of all in the Pick Up array 
        return closest;
    }
    
}
