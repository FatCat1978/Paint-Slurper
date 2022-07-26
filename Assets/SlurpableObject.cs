using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))] //we need a sprite renderer for this to work.
public class SlurpableObject : MonoBehaviour
{
	
	// Start is called before the first frame update

	//these have to be public, they're accessed by other scripts.
	public PaintHelper.ColourValues currentValue = PaintHelper.ColourValues.RED; //this lets us change it at will

	

	void Start()
	{
		setColourToSelection();
	}
	private void OnValidate()
	{
		setColourToSelection();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	void setColourToSelection()
	{
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.color = PaintHelper.ColourValueToColor(currentValue);
	}

}
