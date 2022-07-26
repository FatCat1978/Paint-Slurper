using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintHelper
{
	public enum ColourValues
	{
	RED,
	GREEN,
	BLUE,
	YELLOW
	}

	public static Color ColourValueToColor(ColourValues colour)
	{
		switch(colour)
		{
			case ColourValues.RED:
				return Color.red;
			case ColourValues.GREEN:
				return Color.green;
			case ColourValues.BLUE:
				return Color.blue;
			case ColourValues.YELLOW:
				return Color.yellow;
			default:
				throw new System.Exception("TRYING TO FIND THE COLOUR VALUE FOR " + colour);
		}
	}

	public static Color AddColourValues(List<ColourValues> colours) //Todo, "Max Mix" greater than 3?
	{

		IDictionary<ColourValues, int> PrimaryValueCount = new Dictionary<ColourValues, int>();
		IDictionary<ColourValues, float> PrimaryValueShare = new Dictionary<ColourValues, float>();

		int totalColours = 0;
		foreach(ColourValues colour in colours)
        {
			totalColours++;
			if(PrimaryValueCount.Keys.Contains(colour))
            {
                PrimaryValueCount[colour]++;
            }
			else
            {
				PrimaryValueCount.Add(colour,1);
            }

        }

		foreach(ColourValues colour in PrimaryValueCount.Keys)
        {
			float x = PrimaryValueCount[colour];

			float y = totalColours;

			float x1 = x/y;

		//	Debug.Log("X:" + x);
		//	Debug.Log("Y:" + y);
		//	Debug.Log("x1:" + x1);
		
			PrimaryValueShare.Add(colour, x1);	
        }

		if(PrimaryValueCount.Keys.Count == 3) //if there's 3, we just iterate over one of each - 3 different colours, so the repeat issue doesn't exist.
        {
		//	Debug.Log("3 Keys! 3 Colours!");

			Color x = Color.white;
			foreach(ColourValues colour in PrimaryValueCount.Keys)
            {
				if (x == Color.white)
					x = PaintHelper.ColourValueToColor(colour);
				else
					x = Color.Lerp(x, ColourValueToColor(colour),0.5f);
            }
			return x;	
        }

		if(PrimaryValueCount.Keys.Count == 2)
        { //we've got only 2
			//Debug.Log("2 Keys! 2 Colours!");
			List<ColourValues> CompList = new List<ColourValues>();

			foreach(var key in PrimaryValueShare.Keys)
            {
				CompList.Add(key);
            }

			float Ashare = PrimaryValueShare[CompList[0]];

			//Debug.Log("Ashare is " + Ashare);

			Color MixedColor = Color.Lerp(PaintHelper.ColourValueToColor(CompList[1]),PaintHelper.ColourValueToColor(CompList[0]),Ashare);

			return MixedColor;

        }

        if(PrimaryValueCount.Keys.Count == 1)
        {
		//	Debug.Log("1 Keys! 1 Colours!");
			foreach (var Value in PrimaryValueShare.Keys)
				return PaintHelper.ColourValueToColor(Value);
        }




		//Alright, this is how this works.
		//we determine the amount of colours of a type present, and attach it a float based on how many of it exists proportional to the others
		//if it's 1/2, .5
		//2/3, .66
		//3/3 - 1


		//if it's .99, just return that 

		//if we have 3 different .33s, add them together with .5



		//Debug.Log("UH OH! SHOULDN'T BE HERE RETARD!");
		return Color.gray;
		/*
		Color BaseColor = Color.white;
		Debug.Log("PAINTHELPER: ADDING " + colours.Count + " COLOURS");
		foreach (ColourValues colour in colours)
        {
			if (BaseColor == Color.white)
				BaseColor = PaintHelper.ColourValueToColor(colour);
			else
				BaseColor = Color.Lerp(BaseColor, PaintHelper.ColourValueToColor(colour), 0.5f); //+= PaintHelper.ColourValueToColor(colour); //Color.Lerp(BaseColor, PaintHelper.ColourValueToColor(colour), 0.5f);
		}
		
		Debug.Log("Final Value: " + BaseColor.ToString());
		
		return BaseColor;
		/*/
	}

}

public class PaintHolder
{

	public List<PaintHelper.ColourValues> ColourList = new List<PaintHelper.ColourValues>();
	public int maxColours = 3;


	public Color CurrentColor;


    public PaintHolder(PaintHolder paintHolder)
    {
        foreach(var x in paintHolder.ColourList)
        {
			ColourList.Add(x);
        }
    }

    public PaintHolder()
    {
    }

    public void setRGB()
	{
		if(ColourList.Count <= 0)
        {
			throw new System.Exception("Tried to set colour from PaintHolder that has no Colours stored, or too many! Colours: " + ColourList.Count);
		}
		CurrentColor = PaintHelper.AddColourValues(ColourList);
		return;
		
		
	}

	public void Reset()
	{ 
		ColourList.Clear();
		
	}

	public void AddColour(PaintHelper.ColourValues toAdd)
	{
		ColourList.Add(toAdd);
		if(ColourList.Count > maxColours)
			ColourList.RemoveAt(0);

		setRGB();
	}

	public Color GetColor()
	{
		if (ColourList.Count > 0)
		{
			setRGB();
			//Debug.Log("Current stored colour in holder:" + CurrentColor.ToString());
			return CurrentColor;

		}
		else
			return Color.white;
	}

}