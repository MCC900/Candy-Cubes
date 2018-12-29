using System;
using UnityEngine;

[Serializable]
public class Array3DBool
{
	bool esNulo = true;

	public int largoX;
	public int largoY;
	public int largoZ;

	[SerializeField] bool[] data;
	[SerializeField] int XporY;

	public Array3DBool(int pLargoX, int pLargoY, int pLargoZ){
		esNulo = false;
		largoX = pLargoX;
		largoY = pLargoY;
		largoZ = pLargoZ;
		XporY = largoX * largoY;
		data = new bool[XporY * largoZ];
	}

	public Array3DBool(){
		esNulo = true;
	}

	public bool this[int x, int y, int z] {
		get { return data [x + y * largoX + z * XporY]; }
		set { data [x + y * largoX + z * XporY] = value; }
	}

	public bool esNull(){
		return esNulo;
	}
}

