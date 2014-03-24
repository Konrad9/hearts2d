using UnityEngine;
using System; 
using System.Collections;
using System.Collections.Generic;

public class heartsController : MonoBehaviour {

	public List<Texture2D> cardTextures = new List<Texture2D>();

	private string[] suits = new string[] {"c", "d", "h", "s"};

	private GameObject cardPlayed;

	/// <summary>
	/// The suit played by the trick leader
	/// </summary>
	private int trickSuit;

	private int trickNum;

	/// <summary>
	/// The player who threw down the first card in the trick
	/// </summary>
	private int trickLeader;

	/// <summary>
	/// The player who is up next in the trick
	/// </summary>
	private int currentTrickPlayer;

	private int handsPlayedInTrick;

	private bool cardSelected;

	private bool validCardSelected;

	private int cardToPlay;

	private bool showNewHand;

    private bool heartsBroken;

    private GameObject validCard; 

	private int shuffleSize;

	private int roundNum;

	private int tempCardLocation;

	public List<Hand> _hands; 

	public List<GameObject> deck; 

	private List<GameObject> playedCards;

	public GameObject cardObject;

    private int locationOfPlayedCard; 


	void Start () 
	{
        newGame();
		newTrick();
	}

	void newTrick()
	{
		///reset all of the per-trick variables		
		handsPlayedInTrick = 0;
		validCardSelected = false;
		cardToPlay = 15;
		trickNum++;
        if (trickNum > 11)
        {
            roundNum++; 
        }
		tempCardLocation = 0; 
		initializeDeck();
		shuffle();
		dealToHands();
        nextPlayersTurn(trickLeader);
	}

	void newGame()
	{
		///reset all of the per-game variables
		playedCards = new List<GameObject>();
		_hands = new List<Hand>();
		for (int i = 0; i < 4; ++i)
		{
			Hand tempHand = new Hand(); 
			_hands.Add (tempHand);
            
            playedCards.Add(cardObject);
		}	
		deck = new List<GameObject>();
		trickLeader = 0; 
		shuffleSize = 52;
		trickNum = 0; 
		roundNum = 0;
	}

	void initializeDeck()
	{
		int cardCounter = 0;
		///create a new deck, comprised of 4 suits with 13 cards each
		for (int faceValue = 1; faceValue < 14; faceValue++)
		{
			for (int suit = 0; suit < 4; suit++)
			{
				GameObject tempCard;
				tempCard = Instantiate(cardObject, new Vector3(2000, 2000, 0), Quaternion.identity) as GameObject;
				deck.Add(tempCard);
				Card cardScript = deck[cardCounter].GetComponent<Card>();
				cardScript.setFaceValue(faceValue);
				cardScript.setSuit(suits[suit]);				

				Texture newTexture = cardTextures[cardCounter];
				deck[cardCounter].renderer.material.mainTexture = newTexture;
				deck[cardCounter].renderer.materials[1].mainTexture = newTexture;
                cardCounter++;
			}
		}
	}

	void shuffle()
	{
		GameObject tempCard;
		int rand;
		while (shuffleSize > 1)
		{
			///shuffle the deck using the Fischer-Yates shuffle
			shuffleSize--;
			rand = UnityEngine.Random.Range(0, shuffleSize);
			tempCard = deck[rand];
			deck[rand] = deck[shuffleSize];
			deck[shuffleSize] = tempCard;
		}
	}

	void dealToHands()
	{
        int i = 0;
		while (deck.Count > 0)
		{
			///deal the deck out to each player
            Card cardScript = deck[0].GetComponent<Card>();
            if ((cardScript.suit == "c") && (cardScript.faceValue == 2))
            {
                trickLeader = i;
				currentTrickPlayer = i;
            }
            _hands[i].addCardToHand(deck[0]);			
			deck.RemoveAt(0);            
            if (++i > 3)
            {
                i = 0;
            }
		}
		Debug.Log ("Hi!");

		for (int x = 0; x < 4; x++)
		{
			_hands[x].sortCards();
		}
		_hands [0].displayCardsOnScreen ();
	}


	void Update()
	{
        if (validCardSelected == true)
        {
            _hands[currentTrickPlayer].removeCardFromDeck(validCard);
            playedCards[currentTrickPlayer] = validCard;
            
            Card cardScript = validCard.GetComponent<Card>();

            switch (currentTrickPlayer)
            {
                case 0:
                    cardScript.setDisplayPosition(new Vector3(-4.7f, 1.726f));
                    break;
                case 1:
                    cardScript.setDisplayPosition(new Vector3(-1.9f, 4.0f));
                    break;
                case 2:
                    cardScript.setDisplayPosition(new Vector3(-3.3f, 2.3f));
                    break;
                case 3:
                    cardScript.setDisplayPosition(new Vector3(-0.79f, 0.185f));
                    break;
            }       

			if (++currentTrickPlayer > 3)
			{
				currentTrickPlayer = 0;
			}
			
			if (++handsPlayedInTrick > 3)
			{
                newTrick();
                scoreRound();
            }
            validCardSelected = false;

            nextPlayersTurn(currentTrickPlayer);
        }
        else
        {
            nextPlayersTurn(currentTrickPlayer);
        }

	}

    void nextPlayersTurn(int playerToGo)
    {
        
        if (playerToGo != 0)
        {
            switch (playerToGo)
            {
                case 1:
                    playAI1();
                    break;
                case 2:
                    playAI2();
                    break;
                case 3:
                    playAI3();
                    break;
            }            
        }
    }

	void scoreRound()
	{
		int score = 3002;
	}

	void playCard(GameObject playedCard)
	{
		Card cardScript = playedCard.GetComponent<Card>();
        
		if (trickNum > 1) 
		{
            if (handsPlayedInTrick > 0)
            {
                if (cardScript.suit == suits[trickSuit])
                {
                    ///if the hand played a card with the same suit as the trick leader, the card is valid
                    validCardSelected = true;
                }
                else if (_hands[currentTrickPlayer].containsSuit(trickSuit) == true)
                {
                    if (cardScript.suit == suits[trickSuit])
                    {
                        ///if the player has a card of the same suite but doesn't play it, throw an error
                    }
                }
                else
                {
                    if (heartsBroken)
                    {
                        validCardSelected = true;
                    }
                    else if (cardScript.suit == "h")
                    {
                        ///error, hearts not broken
                    }
                }
            }
            else
            {
                if (heartsBroken)
                {
                    ///if hearts is broken, can lead with any card
                    validCardSelected = true;
                }
                else
                {
                    if (cardScript.suit != "h")
                    {
                        if ((cardScript.suit == "c") && (cardScript.faceValue == 12))
                        {
                            heartsBroken = true;
                        }
                        validCardSelected = true;
                    }
                    else
                    {
                        ///cannot lead with a hearts card if hearts has not yet been broken
                    }
                }
            }
		}else
		{
			if (handsPlayedInTrick == 0)
			{
                if (cardScript.suit == "c")
                {
                    if (cardScript.faceValue == 2)
                    {
                        ///first player MUST play the 2 of clubs
                        validCardSelected = true;
                    }
                }else
                {
                    ///must play 2 of clubs
                }
            } 
            else if (_hands[currentTrickPlayer].containsSuit(trickSuit) == true)
            {
                ///if you are not the player with the 2 of clubs...
                ///if you have clubs, you must play clubs
                if (cardScript.suit == "c")
                {
                    validCardSelected = true; 
                }
            } else if (cardScript.suit != "h")
            {
                if ((cardScript.suit == "s") && (cardScript.faceValue != 12))
                {
                    validCardSelected = true;
                    ///cannot play the 12 of clubs on the first trick
                }
                else
                {
                    validCardSelected = true; 
                }
            }
		}

        if (validCardSelected == true)
        {
            validCard = playedCard;
        }

	}


	public void OnTriggerEnter(Collider collider)
	{
		Debug.Log (collider.tag);
		playCard (collider.gameObject);
	}

    private void playAI1()
    {
		int randNum;
		GameObject localCardToPlay = new GameObject();
		
		////if trick leader
		if (trickLeader == 1)
		{
			///if it is NOT the first trick
			if (trickNum > 1)
			{
				///if hearts is broken                
				if (heartsBroken)
				{
					localCardToPlay = returnAnyCard(1); 
					///play whatever
				}
				else
				{
					localCardToPlay = returnNonHeartsCard(1);
					///play non-hearts
				}
			}
			else
			{
				///if AI is the trick leader and it is the first trick, must play 2 of clubs
				localCardToPlay = _hands[1].getTwoOfClubs();
			}
		}
		else
		{
			if (trickNum > 1)
			{
				if (_hands[1].containsSuit(trickSuit))
				{
					localCardToPlay = _hands[1].returnLowestCardOfSuit(trickSuit);
				}
				else if (heartsBroken)
				{
					//randNum = UnityEngine.Random.Range(0, _hands[3].numOfCards());
					localCardToPlay = returnAnyCard(1);
					///play whatever
				}
				else
				{
					localCardToPlay = returnNonHeartsCard(1);
					///play non-hearts
				}
			}
			else
			{
				if (_hands[1].containsSuit(0))
				{
					///play clubs
					localCardToPlay = _hands[1].returnLowestCardOfSuit(0);
				}
				else
				{
					localCardToPlay = returnNonClubsNonHeartsCard(1);
					///play non-clubs, non-hearts
				}
			}
		}
		
		playCard(localCardToPlay);
	}
	
	private void playAI2()
	{
		int randNum;
		GameObject localCardToPlay = new GameObject();
		
		////if trick leader
		if (trickLeader == 2)
		{
			///if it is NOT the first trick
			if (trickNum > 1)
			{
				///if hearts is broken                
				if (heartsBroken)
				{
					localCardToPlay = returnAnyCard(2); 
					///play whatever
				}
				else
				{
					localCardToPlay = returnNonHeartsCard(2);
					///play non-hearts
				}
			}
			else
			{
				///if AI is the trick leader and it is the first trick, must play 2 of clubs
				localCardToPlay = _hands[2].getTwoOfClubs();
			}
		}
		else
		{
			if (trickNum > 1)
			{
				if (_hands[2].containsSuit(trickSuit))
				{
					localCardToPlay = _hands[2].returnLowestCardOfSuit(trickSuit);
				}
				else if (heartsBroken)
				{
					//randNum = UnityEngine.Random.Range(0, _hands[2].numOfCards());
					localCardToPlay = returnAnyCard(2);
					///play whatever
				}
				else
				{
					localCardToPlay = returnNonHeartsCard(2);
					///play non-hearts
				}
			}
			else
			{
				if (_hands[2].containsSuit(0))
				{
					///play clubs
					localCardToPlay = _hands[2].returnLowestCardOfSuit(0);
				}
				else
				{
					localCardToPlay = returnNonClubsNonHeartsCard(2);
					///play non-clubs, non-hearts
				}
			}
		}
		
		playCard(localCardToPlay);
	}
	
	private void playAI3()
	{
		int randNum;
		GameObject localCardToPlay = new GameObject();
		
		////if trick leader
		if (trickLeader == 3)
		{
			///if it is NOT the first trick
			if (trickNum > 1)
			{
				///if hearts is broken                
				if (heartsBroken)
				{
					localCardToPlay = returnAnyCard(3); 
					///play whatever
				}
				else
				{
					localCardToPlay = returnNonHeartsCard(3);
					///play non-hearts
				}
			}
            else
            {
                ///if AI is the trick leader and it is the first trick, must play 2 of clubs
                localCardToPlay = _hands[3].getTwoOfClubs();
            }
        }
        else
        {
            if (trickNum > 1)
            {
                if (_hands[3].containsSuit(trickSuit))
                {
					localCardToPlay = _hands[3].returnLowestCardOfSuit(trickSuit);
				}
                else if (heartsBroken)
                {
                    //randNum = UnityEngine.Random.Range(0, _hands[3].numOfCards());
					localCardToPlay = returnAnyCard(3);
					///play whatever
                }
                else
                {
					localCardToPlay = returnNonHeartsCard(3);
					///play non-hearts
                }
            }
            else
            {
                if (_hands[3].containsSuit(0))
                {
                    ///play clubs
					localCardToPlay = _hands[3].returnLowestCardOfSuit(0);
				}
				else
                {
					localCardToPlay = returnNonClubsNonHeartsCard(3);
					///play non-clubs, non-hearts
                }
            }
        }

        playCard(localCardToPlay);
    }


	
	
	private GameObject returnNonClubsNonHeartsCard(int handNum)
	{
		GameObject localCardToPlay = null;
		
		if (_hands[handNum].containsSuit(1))
		{						
			localCardToPlay = _hands[handNum].returnLowestCardOfSuit(1);
		}else if (_hands[handNum].containsSuit(3))
		{						
			localCardToPlay = _hands[handNum].returnLowestCardOfSuit(3);
		}
		
		return localCardToPlay;
	}

	private GameObject returnNonHeartsCard(int handNum)
	{
		GameObject localCardToPlay = null; 
		if (_hands[handNum].containsSuit(0))
		{						
			localCardToPlay = _hands[handNum].returnLowestCardOfSuit(0);
		}else if (_hands[handNum].containsSuit(1))
		{						
			localCardToPlay = _hands[handNum].returnLowestCardOfSuit(1);
		}else if (_hands[handNum].containsSuit(3))
		{						
			localCardToPlay = _hands[handNum].returnLowestCardOfSuit(3);
		}		

		return localCardToPlay;
	}
	
	private GameObject returnAnyCard(int handNum)
	{
		GameObject localCardToPlay = null;
		if (_hands[handNum].containsSuit(0))
		{						
			localCardToPlay = _hands[handNum].returnLowestCardOfSuit(0);
		}else if (_hands[handNum].containsSuit(1))
		{						
			localCardToPlay = _hands[handNum].returnLowestCardOfSuit(1);
		}else if (_hands[handNum].containsSuit(2))
		{						
			localCardToPlay = _hands[handNum].returnLowestCardOfSuit(2);
		}else if (_hands[handNum].containsSuit(3))
		{						
			localCardToPlay = _hands[handNum].returnLowestCardOfSuit(3);
		}

		return localCardToPlay;
	}
	
	
	







}
