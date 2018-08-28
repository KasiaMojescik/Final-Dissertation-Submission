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
            // find the position of every child of the object that this script is attached to
            // in Unity, this script is attached to CueHolder, therefore it connects all arrows
            Vector3[] positions = new Vector3[this.transform.childCount];
            for (int i = 0; i < this.transform.childCount; i++)
            {
                positions[i] = this.transform.GetChild(i).position;
            }

            // draw the line connecting the position of each cue
            LineRenderer r = GetComponent<LineRenderer>();
            r.positionCount = this.transform.childCount;
            r.SetPositions(positions);

        }
	}
}
