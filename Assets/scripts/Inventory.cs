using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {
	public int porcentaje;
	public bool esPocima;
	public bool asignado;
	public int type;//0 piedra 1 cantimplora
	// Use this for initialization
	void Start () {
		int valor = Random.Range(1,100);
		if(porcentaje<=valor)
			esPocima=true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
