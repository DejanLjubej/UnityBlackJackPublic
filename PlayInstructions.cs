using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayInstructions : MonoBehaviour
{
    [Header("Card Spawn Game Objects")]
    public GameObject playerCardsGrid;
    public GameObject playerCardsRightGrid;
    public GameObject dealerCardsGrid;

    [Header("Points Of Entity")]
    public Text playerPoints;
    public Text playerPointsRight;
    public Text dealerPoints;
    
    [Header("Dependencies")]
    public DeckShuffleAndDeal deckManagement;

    [Header("Bet Screen Text")]
    public Text amoutToChangeText;
    public Text betAmountTextBetScreen;
    public Text playerMoneyBetScreen;

    [Header("Play Screen Text")]
    public Text betAmountPlayScreen;
    public Text playerMoneyPlayScreen;

    [Header("Other text")]
    public Text wonOrLostText;

    [Header("Panels")]
    public GameObject betPanel;
    public GameObject wonOrLostPanel;
    public GameObject playPanel;

    [Header("Buttons")]
    public Button hitButton;
    public Button standButton;
    public Button doubleButton;
    public Button splitButton;

    PlayerEntity player;
    EntityInGame dealer;
    RuleSet rules;

    int addOrReduceBy = 10;
    int indexForPlayersCard=0;
    bool firstOfSplit = false;
    bool secondCardInSplit = false;
    /// <summary>
    /// Dealing With Bet
    /// </summary>
    ///     /////////////////////////////////////////
    public void PlaceBet()
    {
        if(player.BetAmount > 0)
        {
            player.Money -= player.BetAmount;
            betAmountPlayScreen.text = player.BetAmount.ToString();
            playerMoneyPlayScreen.text = player.Money.ToString();
            betPanel.SetActive(false);

            deckManagement.ShuffleDeck();
            InitialDeal();
        }
    }

    public void IncreseOrDecreseBy(string number)
    {
        addOrReduceBy = int.Parse(number);
        amoutToChangeText.text = addOrReduceBy.ToString();
    }

    public void IncreseBet()
    {
        if(player.BetAmount+addOrReduceBy >= player.Money)
        {
            player.BetAmount = player.Money;
        }
        else
        {
            player.BetAmount += addOrReduceBy;
        }
        UpdateTextStatus();
    }

    public void DecreseBet()
    {
        if(player.BetAmount <= addOrReduceBy)
        {
            player.BetAmount = 0;
        }
        else
        {
            player.BetAmount -= addOrReduceBy;
        }
        UpdateTextStatus();
    }

    /// <summary>
    /// Cards
    /// </summary>
    ///     /////////////////////////////////////////

    void InitialDeal()
    {
        playPanel.SetActive(true);
        DealCardsToPlayer();
        DealCardsToDealer();
        DealCardsToPlayer();

        if (rules.IsCardValueLegibleForDouble(player.CardSum))
        {
            doubleButton.interactable = true;
        }
        if (rules.IsLegibleForSplit(player.FirstTwoCards[0], player.FirstTwoCards[1]))
        {
            splitButton.interactable = true;
        }
    }

    int GetCardNumber(string cardName)
    {
        string cardNumberString = cardName.Substring(cardName.Length - 2);
        int numberOfCard = int.Parse(cardNumberString);

        return numberOfCard;
    }

    public void DealCardsToPlayer()
    {
        string cardName; 
        int cardValue;

        if (!player.IsSplit || firstOfSplit)
        {
            cardName=deckManagement.DealCards(playerCardsGrid);
            cardValue=GetCardNumber(cardName);
            ManagePlayerPoint(cardValue);
        }
        else
        {
            cardName = deckManagement.DealCards(playerCardsRightGrid);
            cardValue = GetCardNumber(cardName);
            ManagePointsForSecondSet(cardValue);
        }
    }

    void DealCardsToDealer()
    {
        string cardName=deckManagement.DealCards(dealerCardsGrid);
        int cardValue = GetCardNumber(cardName);
        ManageDealerPoints(cardValue);
    }
   

    /// <summary>
    /// Points
    /// </summary>
    ///     /////////////////////////////////////////
    void DevidePointsForSplit()
    {
        if (player.NumberOfAces == 2)
        {
            player.SecondCardSum = player.CardSum = 11;
        }
        else
        {
            player.SecondCardSum = player.CardSum /= 2;
        }
        UpdateTextStatus();
    }

    void ManagePointsForSecondSet(int cardValue)
    {
        player.SecondCardSum += EstablishRealValue(cardValue, false, true);

        if (indexForPlayersCard == 1)
        {
            if (rules.IsBlackJack(player.SecondCardSum))
            {
                player.HasSecondBlackJack = true;
                if (dealer.CardSum < 10)
                {
                    player.SecondWin = true;
                    if (player.HasBlackJack)
                    {
                        PlayerWon();
                    }
                    else
                    {
                        DealersTurn();
                    }
                }
                else
                {
                    DealersTurn();
                }
            }
        }
        indexForPlayersCard++;

        if (rules.IsEntitiyBust(player.SecondCardSum))
        {
            if (player.SecondNumberOfAces > 0)
            {
                player.SecondCardSum -= 10;
                player.SecondNumberOfAces--;
            }
            else
            {
                player.SecondBust = true;
                if (player.IsBust)
                {
                    dealer.IsWinner = true;
                    PlayerLost();
                }
                else
                {
                    dealer.IsWinner = false;
                    DealersTurn();
                }
            }
        }
        UpdateTextStatus();
    }

    void ManagePlayerPoint(int cardValue)
    {
        player.CardSum +=EstablishRealValue(cardValue, false);

        if (indexForPlayersCard < 2)
        {
            player.FirstTwoCards[indexForPlayersCard] = cardValue;
        }
        
        if(indexForPlayersCard ==1)
        {
            if (rules.IsBlackJack(player.CardSum))
            {
                player.HasBlackJack = true;
                if( dealer.CardSum < 10)
                { 
                    player.IsWinner = true;
                    if (player.IsSplit)
                    {
                        firstOfSplit = false;
                    }
                    else
                    {
                        PlayerWon();
                    }
                }
                else
                {
                    if (player.IsSplit)
                    {
                        firstOfSplit = false;
                    }
                    else
                    {
                        DealersTurn();
                    }
                }
                
            }
        }
        indexForPlayersCard++;

        if (rules.IsEntitiyBust(player.CardSum))
        {
            if(player.NumberOfAces > 0)
            {
                player.CardSum -= 10;
                player.NumberOfAces--;
            }
            else
            {
                player.IsBust = true;
                if (!player.IsSplit)
                {
                    dealer.IsWinner = true;
                    PlayerLost();
                }
                else
                {
                    firstOfSplit = false;
                }
            }
        }

        UpdateTextStatus();
    }

    int EstablishRealValue(int cardValue, bool isThisDealer, bool secondAces = false)
    {
        if(cardValue > 10)
        {
            return 10;
        }else if(cardValue == 1 ){

            if (isThisDealer)
            {
                dealer.NumberOfAces++;
            }
            else
            {
                if (!secondAces)
                {
                    player.NumberOfAces++;
                }
                else
                {
                    player.SecondNumberOfAces++;
                }

            }
            return 11;
        }
        else
        {
            return cardValue;
        }
    }

    void ManageDealerPoints(int cardValue)
    {
        dealer.CardSum += EstablishRealValue(cardValue, true);

        if (rules.IsEntitiyBust(dealer.CardSum))
        {
            if (!player.IsSplit)
            {
                if (dealer.NumberOfAces > 0)
                {
                    dealer.CardSum -= 10;
                    dealer.NumberOfAces--;
                }
                else
                {
                    dealer.IsBust = true;
                    player.IsWinner = true;
                    PlayerWon();
                }
            }
            else
            {
                if(player.IsBust || player.SecondBust)
                {
                    player.Money -= player.BetAmount / 2;
                }
                PlayerWon();
            }

        }
        UpdateTextStatus();
    }

    /// <summary>
    /// Turns and Outcomes
    /// </summary>
    ///     /////////////////////////////////////////
    void DealersTurn()
    {
        DealCardsToDealer();
        if (rules.IsBlackJack(dealer.CardSum))
        {
            dealer.HasBlackJack = true;

            if (!player.IsSplit)
            {
                if (!player.HasBlackJack)
                {
                    PlayerLost();
                }
                else
                {
                    Draw();
                }
            }
            else
            {
                if (!player.HasBlackJack && !player.HasSecondBlackJack)
                {
                    PlayerLost();
                }else if(player.HasBlackJack || player.HasSecondBlackJack)
                {
                    player.Money -= player.BetAmount / 4;
                    Draw();
                }
                else
                {
                    Draw();
                }
            }
        }
        else
        {
            while (rules.DealMoreCardsToDealer(dealer.CardSum)&&dealer.CardSum<=player.CardSum)
            {
                DealCardsToDealer();
            }

            if (!player.IsSplit)
            {
                if (rules.DidPlayerWin(player.CardSum, dealer.CardSum))
                {
                    PlayerWon();
                }
                else if (rules.IsItADraw(player.CardSum, dealer.CardSum))
                {
                    Draw();
                }
                else
                {
                    PlayerLost();
                }
            }
            else
            {
                if ( player.CardSum > dealer.CardSum && player.SecondCardSum>dealer.CardSum)
                {
                    PlayerWon();
                }
                else if(player.CardSum< dealer.CardSum && player.SecondCardSum<dealer.CardSum)
                {
                    PlayerLost();
                }
                else if(player.CardSum<dealer.CardSum && player.SecondCardSum> dealer.CardSum ^ player.CardSum > dealer.CardSum && player.SecondCardSum < dealer.CardSum)
                {
                    Draw();
                }
                else if (player.CardSum > dealer.CardSum ^ player.SecondCardSum > dealer.CardSum)
                {
                    player.Money -= player.BetAmount / 2;
                    PlayerWon();
                }
                else
                {
                    player.Money += player.BetAmount / 2;
                    PlayerLost();
                }
            }
        }
    }

    public void Split()
    {
        player.Money -= player.BetAmount;
        player.BetAmount += player.BetAmount;

        firstOfSplit = player.IsSplit = secondCardInSplit = true;
        playerPointsRight.gameObject.SetActive(true);
        playerCardsRightGrid.SetActive(true);
        deckManagement.ChangeParentForSplit(playerCardsRightGrid);
        
        splitButton.interactable = false;
        doubleButton.interactable = false;

        indexForPlayersCard = 0;
        DevidePointsForSplit();
    }

    public void Double()
    {
        player.Money -= player.BetAmount;
        player.BetAmount += player.BetAmount;
        DealCardsToPlayer();
        Stand();
    }

    void PlayerLost()
    {
        dealer.IsWinner = true;
        DisplayWonOrLost();
        UpdateTextStatus();
    }

    void PlayerWon()
    {
        player.Money += player.BetAmount*2;
        player.IsWinner = true;
        DisplayWonOrLost();
        UpdateTextStatus();
    }

    void Draw()
    {
        player.Money += player.BetAmount;
        DisplayWonOrLost();
    }

    void DisplayWonOrLost()
    {
        wonOrLostPanel.SetActive(true);
        wonOrLostText.text = player.IsWinner ? "You Won" : !player.IsWinner && !dealer.IsWinner? "Draw" : "You Lost";
    }

    public void Continue()
    {
        wonOrLostPanel.SetActive(false);
        EndTurnManagement();
    }
    public void Stand()
    {
        indexForPlayersCard = 1;
        if (!player.IsSplit)
        {
            DealersTurn();
        }
        else
        {
            if (firstOfSplit)
            {
                firstOfSplit = false;
            }
            else
            {
                DealersTurn();
            }
        }
    }

    void EndTurnManagement()
    {
        if(player.BetAmount > player.Money)
        {
            player.BetAmount = player.Money;
        }
        //Set attributes player
        player.HasSecondBlackJack = false;
        player.HasBlackJack = false;
        player.SecondBust = false;
        player.SecondWin = false;
        player.IsWinner = false;
        player.IsSplit = false;
        player.IsBust = false;
        player.SecondNumberOfAces = 0;
        player.SecondCardSum = 0;
        player.NumberOfAces = 0;
        player.CardSum = 0;
        
        //setAttributes dealer
        dealer.HasBlackJack = false;
        dealer.IsWinner = false;
        dealer.IsBust = false;
        dealer.NumberOfAces = 0;
        dealer.NumberOfAces = 0;
        dealer.CardSum = 0;
        
        //Start New Round
        deckManagement.RemoveCardsFromPlay();
        ButtonInteractableStatusAtBegining();
        PanelActiveStatusAtBegining();
        UpdateTextStatus();

        indexForPlayersCard = 0;
        secondCardInSplit = false;
        firstOfSplit = false;

        playerPointsRight.gameObject.SetActive(false);
        playerCardsRightGrid.SetActive(false);

    }

    /// <summary>
    /// On Game start
    /// </summary>
    ///     /////////////////////////////////////////
    void PanelActiveStatusAtBegining()
    {
        wonOrLostPanel.SetActive(false);
        playPanel.SetActive(false);
        betPanel.SetActive(true);

    }
    void ButtonInteractableStatusAtBegining()
    {
        doubleButton.interactable = false;
        splitButton.interactable = false;
        standButton.interactable = true;
        hitButton.interactable = true;
    }
    void DefineObjects()
    {
        player = new PlayerEntity();
        dealer = new EntityInGame();
        rules = new RuleSet();
    }
    void UpdateTextStatus()
    {
        playerMoneyPlayScreen.text = playerMoneyBetScreen.text = player.Money.ToString();
        betAmountPlayScreen.text = betAmountTextBetScreen.text = player.BetAmount.ToString();
        
        playerPoints.text = player.CardSum.ToString();
        playerPointsRight.text = player.SecondCardSum.ToString();
        dealerPoints.text = dealer.CardSum.ToString();
    }

    void Awake()
    {
        PanelActiveStatusAtBegining();

        DefineObjects();

        UpdateTextStatus();

        ButtonInteractableStatusAtBegining();

        deckManagement.PutCardsInAarray();
        deckManagement.ShuffleDeck();

        indexForPlayersCard = 0;
        EndTurnManagement();
    }
}
