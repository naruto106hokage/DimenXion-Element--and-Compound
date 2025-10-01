using UnityEngine;
using UmetyDatabase;
using System;

public class DummyDataEntry : MonoBehaviour
{
	
	private int a = 1;

	public void MakeDummyDataEntry ( )
	{

		DatabaseManager.dbm.AddEntry ( a.ToString ( ) , a.ToString ( ) , a , a , true , false );

		a = a + 1;

	}

}