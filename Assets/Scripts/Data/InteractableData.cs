using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractableData
{
	public string MapName;
	public string TileName()
	{
		return PosX + "x" + PosY + "yTop";
	}

	public bool Single;
	public int PosX, PosY;
	
	public bool HasInteracted;
	public int State1;
	public int State2;
}
