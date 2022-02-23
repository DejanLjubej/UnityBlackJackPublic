using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class DeckShuffleAndDeal : MonoBehaviour
{
    string[] cardDeck;
    List<GameObject> cardsDelat;
    public string path;
    public Image imageOfCard;
    public Sprite spritesse;

    int nextIndexToDeal;

    public void PutCardsInAarray()
    {
        Object[] assets = Resources.LoadAll("PlayingCards");
        cardDeck = new string[assets.Length];
        int index = 0;


        foreach (var item in assets)
        {
            if (item != null)
            {
                cardDeck[index] = item.name;
            }
            index++;
        }

        ShuffleDeck();
    }

    public void ShuffleDeck()
    {
        nextIndexToDeal = 0;
        for (int i = 0; i < cardDeck.Length; i++)
        {
            int randomNumber = Random.Range(0, cardDeck.Length - 1);
            string temp = cardDeck[i];
            cardDeck[i] = cardDeck[randomNumber];
            cardDeck[randomNumber] = temp;
        }
    }

    public string DealCards(GameObject cardGrid)
    {
        string cardName = cardDeck[nextIndexToDeal];
        imageOfCard.GetComponent<Image>().sprite = Resources.Load<Sprite>("PlayingCards/" + cardName);

        PlaceCards(cardGrid);

        nextIndexToDeal++;

        return cardName;
    }
    void PlaceCards(GameObject cardGrid)
    {
        Image instatiatedImage = Instantiate(imageOfCard);
        cardsDelat.Add(instatiatedImage.gameObject);
        instatiatedImage.transform.SetParent(cardGrid.transform);

    }
    public void RemoveCardsFromPlay()
    {
        foreach (GameObject card in cardsDelat)
        {
            Destroy(card);
        }
        cardsDelat.Clear();
    }

    public void ChangeParentForSplit(GameObject cardGridRight)
    {
        cardsDelat[2].transform.SetParent(cardGridRight.transform);
    }

    void Awake()
    {
        cardsDelat = new List<GameObject>();
        nextIndexToDeal = 0;
    }
}
