using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Hand : MonoBehaviour 
{
	private string[] suits = new string[] {"c", "d", "h", "s"};

	private int[] numOfCardsInSuits = new int[4];

	private List<GameObject> cardsInHand = new List<GameObject>(); 

	private Vector3 offScreenPos = new Vector3(2000, 2000, 0);

	public GameObject cardGO;

	private int cardCount; 

	//to declare and use whenever cardsInHand.Count would be called in a loop
	private int numCards;

	/// <summary>
	/// <returns>The of cards.</returns>
	public int numOfCards()
	{
		Debug.Log ("num of cards is " + cardsInHand.Count);
		return cardsInHand.Count;
	}

	/// <summary>
	/// Adds the card to hand.
	public void addCardToHand(GameObject card)
	{
		Debug.Log ("addCardToHand");
		cardsInHand.Add(card);
		cardCount++;

	}
	/// <summary>
	/// Removes the card from deck.
	/// if it doesn't exist, return false
	public bool removeCardFromDeck(GameObject cardToRemove)
	{
		Debug.Log ("removeCardFromDeck");
		///search for card and remove 
		return true; 
	}

	/// <summary>
	/// Need to add proper error handling here	
	public GameObject getCardFromDeck(int ind)
	{
		Debug.Log ("getCardFromDeck at index " + ind + ", num of cards is " + cardsInHand.Count);
		if (cardsInHand.Count >= ind)
		{
			return cardsInHand[ind];
		}
		return null;
	}

	public GameObject pickACard(int suitType, int aiType)
	{
		Debug.Log ("pick a card of suitType " + suitType);
		GameObject card = null;
		GameObject highCard = null;
		bool skipHighCard = false;

		int unnecessaryVariable = 13222;

		int startingPosition = 0; 
		
		for (int i = 0; i < suitType; i++)
		{
			startingPosition += numOfCardsInSuits[i];
		}

		foreach (GameObject pickCard in cardsInHand)
		{
			Card cardScript = pickCard.GetComponent<Card>();

			if (cardScript.faceValue == 0)
			{
				//skipHighCard = true;
				//highCard = pickCard;
			} else
			{
				return pickCard;
			}


		}

		unnecessaryVariable = 432123;

		//card = cardsInHand[startingPosition];
		
		//return card; 

		return card;




	}

    public GameObject returnLowestCardOfSuit(int suitType)
    {
		Debug.Log ("returnLowestCardOfSuit");
        GameObject card = null;

        int startingPosition = 0; 

        for (int i = 0; i < suitType; i++)
        {
            startingPosition += numOfCardsInSuits[i];
        }

        card = cardsInHand[startingPosition];
		Debug.Log ("Hi!");
        return card; 
    }

    public GameObject getTwoOfClubs()
    {
		Debug.Log ("getTwoOfClubs");
        GameObject card = null;

		for (int i = 0; i < cardCount; i++)
		{
			Card cardScript = cardsInHand[i].GetComponent<Card>();
			if (cardScript.faceValue == 2)
			{
				if (cardScript.suit == suits[0])
				{
					return cardsInHand[i];
				}
				break;
			}
		}       

        return card;
    }


	public void clearDeck()
	{
		Debug.Log ("clearDeck");
		cardsInHand.Clear();
	}

	

	/// <summary>
	/// Does the hand contain a card with the suit.
	public bool containsSuit(int suitCheck)
	{
		Debug.Log ("containsSuit? " + suitCheck);
		numCards = cardsInHand.Count;
		for (int i = 0; i < numCards; i++)
		{
			Debug.Log (i);
			Card cardScript = cardsInHand[i].GetComponent<Card>();
			Debug.Log("card in hand is " + cardScript.faceValue + " of " + cardScript.suit + " compared to suit " + suits[suitCheck]);
			if (cardScript.suit == suits[suitCheck])
			{
				return true;
			}
		}

		return false; 
	}

	/// <summary>
	/// Counts the number of cards in each suit.
	private void countSuits(int ind)
	{
		Debug.Log ("countSuits");
		Card cardScript = cardsInHand[ind].GetComponent<Card>();
		switch (cardScript.suit)
		{
		case "c":
			numOfCardsInSuits[0]++;
			break;
		case "d":
			numOfCardsInSuits[1]++;
			break;
		case "h":
			numOfCardsInSuits[2]++;
			break;
		case "s":
			numOfCardsInSuits[3]++;
			break;
		default: 
			break;

		}

		if (ind < (numCards - 1))
		{
			ind++;
			countSuits(ind);
		}



	}

	/// <summary>
	/// Checks to see if the previous card is of the same suit, and if so, checks to see if they should be switched (in ascending order)
	private void previousCardCheck(int ind)
	{
		Debug.Log ("previousCardCheck starting at index " + ind);
		GameObject bufferCard;

		Card cardScript = cardsInHand[ind].GetComponent<Card>();

		Card backCardScript = cardsInHand[ind - 1].GetComponent<Card>();
		if (cardScript.suit == backCardScript.suit)
		{
			if (cardScript.faceValue < backCardScript.faceValue)
			{
				bufferCard = cardsInHand[ind];
				cardsInHand[ind] = cardsInHand[ind - 1];
				cardsInHand[ind - 1] = bufferCard;
				if (ind >= 2)
				{
					Card backBackCardScript = cardsInHand[ind - 2].GetComponent<Card>();
					if (backCardScript.suit == backBackCardScript.suit)
					{
						previousCardCheck(ind - 1);
					}
				}
			}
		}
	}


	/// <summary>
	/// Sort the hand
	/// </summary>
	public void sortCards()
	{
		Debug.Log ("sortCards");
		
		numCards = cardsInHand.Count;
		countSuits(0);
		int numOfCardsInPreviousSuits = numOfCardsInSuits[0]; 
		///start at clubs, iterate through the other suits. 
		int sortingSuit = 0; 
		if (numOfCardsInSuits[0] == 0)
		{
			if (numOfCardsInSuits[1] == 0)
			{
				if (numOfCardsInSuits[2] == 0)
				{
					sortingSuit = 3;
				}
				else
				{
					sortingSuit = 2;
				}
			}
			else
			{
				sortingSuit = 1;
			}
		}
		///assume the hand is unsorted
		bool unsorted = true;
		///how many cards are currently in the hand


		///count the number of cards in each suit starting with the card at position 0

		///sort the deck starting with the card at position 0
		int currentCardPosition = 0;


		while (unsorted)
		{
			if (currentCardPosition >= numOfCardsInPreviousSuits)
			{
				///this was also not properly implemented, it has been corrected
				//have all of the cards in the suit been sorted? if so, move to the next suit
				sortingSuit++; 
				numOfCardsInPreviousSuits += numOfCardsInSuits[sortingSuit];
			}
			///if the card is not of the same suit as the suit currently being sorted
			/// this was mistakenly set to the SAME suit as currently being sorted, which didn't make any sense at all
			Card cardScript = cardsInHand[currentCardPosition].GetComponent<Card>();
			if (cardScript.suit != suits[sortingSuit])
			{
				///move the card to the end of the list
				cardsInHand.Add(cardsInHand[currentCardPosition]);
				cardsInHand.RemoveAt(currentCardPosition);
			}
			else if (currentCardPosition != 0)
			{
				///only do this on position 1 or after so it doesn't attempt to compare card 0 with -1
				///use the previousCardCheck to see if the card needs to be moved around
				previousCardCheck(currentCardPosition);	
				currentCardPosition++; 
				if (currentCardPosition >= numCards)
				{
					unsorted = false;
					break;
				}
				///have we moved past the last card? if so, don't continue sorting

			}
			else{
				currentCardPosition++;
			}
		}
	
	}

	/// <summary>
	/// Displays the cards on screen.
	public void displayCardsOnScreen()
	{
		Debug.Log ("displayCardsOnScreen");
		numCards = cardsInHand.Count;
		Vector3 displayPosition;
		float xPos = -8f; 
		float yPos = -4.5f; 
		float zPos = 0.0f;
		for (int i = 0; i < numCards; i++)
		{
			Card cardScript = cardsInHand[i].GetComponent<Card>();
			displayPosition= new Vector3(xPos, yPos, zPos); 
			cardsInHand[i].transform.position = displayPosition;
			cardScript.setDisplayPosition(displayPosition);
			xPos += 0.75f;
			zPos -= 0.1f;

		}
	}































}
