using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Zona : MonoBehaviour {

	public int width;
	public int height;
	public Vector3 position;

	public IList<GameObject> _spawns;
	public IList<GameObject> _oasis;

	public Zona () {
		this._spawns = new List<GameObject> ();
		this._oasis = new List<GameObject> ();
	}

	public void SpawnOasis()
	{
		//MakeSpawns ();
		MakeOasis ();
	}

	void MakeOasis()
	{
		var totalOasis = Random.Range (2, 6);

		for(int i = 0; i < totalOasis; i++) {
			var x = Random.Range(0, width);
			var z = Random.Range(0, height);
			Vector3 randomPosition = new Vector3(position.x + x, 10000, position.z + z);
			
			RaycastHit hit = new RaycastHit();
			
			if (Physics.Raycast (randomPosition, -Vector3.up, out hit, 15000)) {
				randomPosition = new Vector3(position.x + x, hit.point.y, position.z + z);
			}

			var clon = (GameObject)Instantiate (Resources.Load ("oasis-"+Random.Range(1,7)),
			                                    randomPosition,
			                                    Quaternion.identity);
			
			if(i >= 0)
			{
				clon.GetComponent<Illusion>().isReal = true;
			}
			
			_oasis.Add(clon);

		}
	}
}