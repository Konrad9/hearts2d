using UnityEngine;
using System; 
using System.Collections;
using System.Collections.Generic;


public class Card : MonoBehaviour {
	///0 = clubs, 1 = diamonds, 2 = hearts, 3 = spades
	public string _suit;
	public int _faceValue;
	private Vector3 _displayPosition;
	private string _colliderTag; 
	public int _belongsTohand; 


	public Card(string suit, int face_value)
	{
		_suit  = suit;
		_faceValue = face_value;
	}
	public string suit  { get { return _suit; } }
	public int faceValue { get { return _faceValue; } }
	public Vector3 displayPosition { get { return _displayPosition; } }

	public void setSuit(string suit)
	{
		_suit = suit;
	}

	public void setFaceValue(int faceValue)
	{
		_faceValue = faceValue;
	}

	public void setOwnerHand(int belongsTohand)
	{
		_belongsTohand = belongsTohand;
	}

	public int getOwnerHand()
	{
		return _belongsTohand;
	}


	public void setDisplayPosition(Vector3 disPos)
	{
		//Debug.Log ("setDisplayPosition " + disPos);
		_displayPosition = disPos;
		gameObject.transform.position = _displayPosition;
	}

	public void resetPosition()
	{
//		Debug.Log ("reset Position");
		gameObject.transform.position = _displayPosition;
	}




}
