using UnityEngine;
using System.Collections;

public class Illusion : MonoBehaviour {
	public float rangeViewDisappear = 10f;
	public float rangeViewAppear = 25f;
	public bool isReal = false;

	private Transform player;

	private Renderer[] _renderers;

	// Use this for initialization
	void Start () {
		_renderers = GetComponentsInChildren<Renderer>();
		player = GameObject.FindGameObjectWithTag ("Player").transform;

		if (!isReal) {
			var _colliders = GetComponentsInChildren<Collider> ();

			foreach (var collider in _colliders) {
				collider.enabled = false;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!isReal) {
						Disappear ();
				} else
						Test();
	}

	void Test(){
		if (Vector3.Distance (transform.position, player.position) < rangeViewDisappear)
						player.GetComponent<userStates> ().sed = 0;
	}

	void Disappear()
	{
		if (Vector3.Distance (transform.position, player.position) < rangeViewDisappear) {
			foreach(var rendererChildren in _renderers)
			{
				/*var color = rendererChildren.material.color;

				if(color.a > 0f)
				{
					color.a -= 0.005f;
					rendererChildren.material.color = color;
				}*/

				Destroy(gameObject);
			}
		}
		else if (Vector3.Distance (transform.position, player.position) >= rangeViewAppear) {
			foreach(var rendererChildren in _renderers)
			{
				var color = rendererChildren.material.color;

				if(color.a < 1f)
				{
					color.a += 0.5f;				
					rendererChildren.material.color = color;
				}		
			}
		}
	}
}
