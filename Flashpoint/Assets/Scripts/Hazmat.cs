using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazmat : MonoBehaviour
{
	public int x, y; 

	public void MoveTo(int x, int y)
	{
		//Switch transform position and state to the selected space
		GameObject target = BoardManager.Instance.GetSpace(x, y);
		gameObject.transform.position = target.transform.position;
		this.x = x;
		this.y = y;
		
		//Delete the hazmat if its been moved outside
		if (IsOutside())
		{
			BoardManager.Instance.RemoveHazmat(gameObject);
			Destroy(gameObject);
		}
	}

	public bool IsOutside()
	{
		return BoardManager.Instance.IsOutside(new int[] { x, y });
	}

	public void InitHazmat(int x, int y)
	{
		this.x = x;
		this.y = y;
		MoveTo(x, y);
	}





	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
