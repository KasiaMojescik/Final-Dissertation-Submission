using UnityEngine;

public class NavLineCreator : MonoBehaviour
{
    // check whether it is the Line condition
    public bool drawLine;

	// For initialization
	void Start()
	{
        // if it is the Line condition, connect all objects at specified positions
        if (drawLine)
        {
            // find the position of every object (which is set to PickUp object in Unity)
            Vector3[] positions = new Vector3[this.transform.childCount];
            for (int i = 0; i < this.transform.childCount; i++)
            {
                positions[i] = this.transform.GetChild(i).position;
            }

            // draw the line connecting each position
            LineRenderer r = GetComponent<LineRenderer>();
            r.positionCount = this.transform.childCount;
            r.SetPositions(positions);

        }
	}
}
