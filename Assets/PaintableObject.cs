using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PaintableObject : MonoBehaviour
{

	[SerializeField] public UnityEvent callOnMatchingColours; //this gets called when the colours match.
	[SerializeField] public UnityEvent callOnNoLongerMatching; //this gets called when the colours match.

	[SerializeField] PaintHelper.ColourValues[] ColoursToMatch; //a list of colours that need to be in the Pholder before callOnMatchingColours is called.

	//Max of 3
	[SerializeField] List<PaintHelper.ColourValues> StartingColours; //max of 3 - enforced!

	[SerializeField] bool CheckForColours = false; //don't check by default.
	[SerializeField] public bool canBePaintedByUser = true; //if this is false, spitting doesn't change the colour.

	private bool isMatching = false;


	PaintHolder Pholder;
	// Start is called before the first frame update
	void Start()
	{
		
	}

    private void OnValidate()
    {
		if (StartingColours == null)
			return;
		if (StartingColours.Count > 3)
			StartingColours.RemoveRange(2, StartingColours.Count);

		Pholder = new PaintHolder();
		Pholder.ColourList = new List<PaintHelper.ColourValues>(StartingColours.ToArray());
		//StartingColours.CopyTo(Pholder.ColourList);
		attemptColourObject(this.gameObject, Pholder.GetColor());

    }


    // Update is called once per frame
    void Update()
	{
		
	}

	internal void applyPaint(Color color, PaintHolder paintHolder)
	{
		Debug.Log("Applying paint to." + gameObject.name);
		Pholder = new PaintHolder(paintHolder);
		//see if we have sprites first. if so, paint those.
		attemptColourObject(this.gameObject,color);
	
	}

	void attemptColourObject(GameObject toColour, Color colorToPaint)
    {
			SpriteRenderer SR = toColour.GetComponent<SpriteRenderer>();
			if (SR != null)
			{
				SR.color = colorToPaint;
				UpdatePaint();
				return;
			}

			Renderer r = toColour.GetComponent<Renderer>();
			if (r != null)
			{
				r.material.color = colorToPaint;
				UpdatePaint();
				return;
			}

			foreach(Transform t in toColour.transform)
            {
				attemptColourObject(t.gameObject,colorToPaint);
            }

		
	}
	void UpdatePaint()
	{
		//check to see if we're accurate.

		//if there's only one requirement, we see if it's the only thing present in the list in our helper

		//for everything else, we check to see if the lists are equlivalant

		//ae. BLUE + BLUE + RED != BLUE+RED || JUST BLUE

		//if we DO have it checked out, we trigger the callOnMatchingColours event, set our current var to true, and go off that
		//likewise if we're no longer true! we trigger the relevant event.
		
		if(CheckForColours) //Do we even need to check
		{
			if ((ColoursToMatch.Length == 0) && Pholder.ColourList.Count == 0)
            {
				ActivateEvents(true);
				return;
			}

			//first we do a niaeve check before doing any extra processing
			bool isEqual = Enumerable.SequenceEqual(ColoursToMatch.OrderBy(e => e), Pholder.ColourList.OrderBy(e => e));
			if (isEqual)
			{
				ActivateEvents(true);
				return;
			} //this will catch them assuming lengths are equal

			List<PaintHelper.ColourValues> CurrentUniqueList = new List<PaintHelper.ColourValues>(); //what we have

			foreach (var colour in Pholder.ColourList)
			{
				if (!CurrentUniqueList.Contains(colour))
					CurrentUniqueList.Add(colour);
			}

			List<PaintHelper.ColourValues> TargetUniqueList = new List<PaintHelper.ColourValues>(); //what we have

			foreach (var colour in ColoursToMatch)
            {
				if(!TargetUniqueList.Contains(colour))
					TargetUniqueList.Add(colour);
            }

			if (TargetUniqueList.Count == CurrentUniqueList.Count)
            {
               if(TargetUniqueList.Count > 0)
                    if (CurrentUniqueList.Contains(TargetUniqueList[0]))
                    {
						
						return;
                    }
                
            }





			

		}
		//first off, check to see if the unique number of colours is equal, if it's not we don't need to do more

		//second off, if the number of unique colours is one, just check to see if it's in the criteria at all

		//third off, if it's higher, see if the two arrays are equal in terms of content.

		// R G B = G R B

		// R G = G R


		ActivateEvents(false);
		return;
	}

	void ActivateEvents(bool matching)
	{
		if(isMatching == true && matching == false)
        {
			isMatching = false;
			callOnNoLongerMatching.Invoke();
			Debug.Log("Activating \"No longer matching\" events!");
		}

		if(isMatching == false && matching == true)
        {
			callOnMatchingColours.Invoke();
			Debug.Log("Activating \"now matching\" events!");
			isMatching = true;

		}
		
		
	}
}
