using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OasisFactory : MonoBehaviour {
	public Transform player;
	public Terrain terrain;

	private int zonas = 4;
	private IList<Zona> _zones;

	// Use this for initialization
	void Start () {
		this._zones = new List<Zona>();

		var terrainSize = this.terrain.terrainData.size;
		var anchoZona = Mathf.RoundToInt(terrainSize.x / zonas);
		var largoZona = Mathf.RoundToInt(terrainSize.z / zonas);
		int i = 0;
		int j = 0;

		while(i<=terrainSize.x) {
			while(j<=terrainSize.z) {
				var zona = new Zona();

				zona.height = Mathf.RoundToInt(largoZona);
				zona.width = Mathf.RoundToInt(anchoZona);

				zona.position = new Vector3(i, 0, j);

				zona.SpawnOasis();

				_zones.Add(zona);

				j+=largoZona;
			}

			j=0;
			i+=anchoZona;
		}
	}
}