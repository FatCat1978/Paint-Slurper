using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursiveParticleColour : MonoBehaviour
{

	//deletes the prefab after a set time, sets the colours

	[SerializeField] float lifetime = 3; //seconds
	public Color actualColour = Color.green;

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	private void OnValidate()
	{
		RecursivelySetColour(this.gameObject);
	}

	void RecursivelySetColour(GameObject target)
	{
		Material material = target.GetComponent<Material>();
		material.color = actualColour;
	}
}
