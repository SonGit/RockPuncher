using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Constants
{
	private static readonly Constants _instance = new Constants();

	// Explicit static constructor to tell C# compiler
	// not to mark type as beforefieldinit
	static Constants()
	{
	}

	private Constants()
	{
	}

	public static Constants instance
	{
		get
		{
			return _instance;
		}
	}
	public static float defaultForce = 1000;
	public static float defaultRange = 1.1f;
	public static float startCharge = 1;
	public static float maxCharge = 10;
	public static float maxForce = 1200;
	public static float chargeToRangeConversionRate = 250;
	public static float chargeToForceConversionRate = 2;
} 
