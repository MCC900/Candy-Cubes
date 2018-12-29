using System;
using UnityEngine;

[Serializable]
public class Array3DInt
{
	bool esNulo = true;

	public int largoX;
	public int largoY;
	public int largoZ;

	[SerializeField] int[] data;
	[SerializeField] int XporY;

	public Array3DInt(int pLargoX, int pLargoY, int pLargoZ){
		esNulo = false;
		largoX = pLargoX;
		largoY = pLargoY;
		largoZ = pLargoZ;
		XporY = largoX * largoY;
		data = new int[XporY * largoZ];
	}

	public Array3DInt(){
		esNulo = true;
	}

	public int this[int x, int y, int z] {
		get { return data [x + y * largoX + z * XporY]; }
		set { data [x + y * largoX + z * XporY] = value; }
	}

	public bool esNull(){
		return esNulo;
	}
}

