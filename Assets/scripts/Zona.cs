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
		var totalOasis = Random.Range (2, 8);

		for(int i = 0; i < totalOasis; i++) {
			var x = Random.Range(0, width);
			var z = Random.Range(0, height);
			Vector3 randomPosition = new Vector3(position.x + x, 10000, position.z + z);
			
			RaycastHit hit = new RaycastHit();
			
			if (Physics.Raycast (randomPosition, -Vector3.up, out hit, 15000)) {
				randomPosition = new Vector3(position.x + x, hit.point.y + 1, position.z + z);
			}

			var clon = (GameObject)Instantiate (Resources.Load ("OasisPrefab"),
			                                    randomPosition,
			                                    Quaternion.identity);
			
			if(i == 0)
			{
				clon.GetComponent<Illusion>().isReal = true;

				//var tf = (GameObject)Instantiate(Resources.Load("Cubez"), clon.transform.position, Quaternion.identity);

			}
			
			_oasis.Add(clon);

		}
	}

	/*void MakeOasis()
	{
		int maximo = Mathf.RoundToInt((float)_spawns.Count * 0.2f);

		var cantidadOasis = Mathf.RoundToInt(Random.Range(1, maximo));

		for (int i = 0; i < cantidadOasis; i++) {
			var indexOasis = Mathf.RoundToInt(Random.Range(0, _spawns.Count - 1));

			var clon = (GameObject)Instantiate (Resources.Load ("OasisPrefab"),
			                                    _spawns[i].transform.position,
			                                    Quaternion.identity);

			if(i == 0)
				clon.GetComponent<Illusion>().isReal = true;
			
			_oasis.Add(clon);
		}
	}

	void MakeSpawns()
	{
		var i = position.x;
		var j = position.z;
		
		while (i < width) {
			while(j < height) {

				Vector3 randomPosition = new Vector3(i, 10000, j);

				RaycastHit hit = new RaycastHit();

				if (Physics.Raycast (randomPosition, -Vector3.up, out hit, 15000)) {
					randomPosition = new Vector3(i, hit.point.y + 1, j);
				}

				var spawn = (GameObject)Instantiate(Resources.Load("OasisSpawnPrefab"), 
				                                    randomPosition, 
				                                    Quaternion.identity);

				Destroy(spawn, 3f);
				
				_spawns.Add(spawn);				
				j += Mathf.RoundToInt(Random.Range(1, 10));
			}
			
			j = position.z;
			i += Mathf.RoundToInt(Random.Range(1, 50));
		}

		Debug.Log (_spawns.Count);
	}*/
}