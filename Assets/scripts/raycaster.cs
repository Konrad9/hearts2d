﻿using UnityEngine;
using System; 
using System.Collections;
using System.Collections.Generic;


public class raycaster : MonoBehaviour {
	RaycastHit raycastHit;
	Vector3 gameObjectPos; 
	public bool activeCard; 
	GameObject selectedCard;


	// Use this for initialization
	void Start () {
		gameObjectPos = new Vector3 (0, 0, 0);
		activeCard = false;
		raycastHit = new RaycastHit();
	}

	void Update()
	{
		if ((activeCard == true) && Input.GetMouseButton(0)) 
		{
			gameObjectPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			gameObjectPos.z = 0.0f;
			raycastHit.transform.position = gameObjectPos;
		}
		else if (Input.GetMouseButtonDown(0))
		{
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit))
			{
				activeCard = true;
				selectedCard = raycastHit.transform.gameObject; 
			}
		}


		/*if ((activeCard == true) && Input.GetTouch (0).phase == TouchPhase.Moved) 
		{
			gameObjectPos = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
			gameObjectPos.z = 0.0f;
			raycastHit.transform.position = gameObjectPos; 
		} else if (Input.GetTouch (0).phase == TouchPhase.Began) {
			activeCard = true;
			selectedCard = raycastHit.transform.gameObject; 
		}*/


		if (Input.GetMouseButtonUp (0) /*|| Input.GetTouch(0).phase == TouchPhase.Canceled*/) 
		{
			if (activeCard)
			{
				Card cardScript = selectedCard.GetComponent<Card>();
				cardScript.resetPosition();
				activeCard = false;
				selectedCard = null;
			}
		}



	}

}
