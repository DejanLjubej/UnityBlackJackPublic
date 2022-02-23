using System.Collections;
using System.Collections.Generic;

public class EntityInGame
{

    public EntityInGame() {
        CardSum = 0;
        IsBust = false;
        IsWinner = false;
        NumberOfAces = 0;
        HasBlackJack = false;
    }

    public EntityInGame(int cardSumGot, bool bust, bool vicory, int numberOfAcesGot, bool hasGotBlasckJack) => 
        (CardSum, IsBust, IsWinner, NumberOfAces, HasBlackJack) = 
        (cardSumGot, bust, vicory, numberOfAcesGot, hasGotBlasckJack);

    public int CardSum { get; set; }
    public bool IsBust { get; set; }
    public bool IsWinner { get; set; }
    public int NumberOfAces { get; set; }
    public bool HasBlackJack { get; set; }
}

public class PlayerEntity : EntityInGame
{
    public PlayerEntity() 
    {
        Money = 10000;
        BetAmount = 0;
        SecondCardSum = 0;
        IsSplit = false;
        FirstTwoCards = new int[2];
        SecondNumberOfAces = 0;
    }

    public PlayerEntity(int moneyGot, int betAmount, int secondCardSumGot, bool isSplitDecision, int[] firstTwoCardsGot, int secondNumberOfAces, bool secondBust, bool secondWin, bool hasGotSecondBlackJack) => 
        (Money, BetAmount, SecondCardSum, IsSplit, FirstTwoCards, SecondNumberOfAces, SecondBust, SecondWin, HasSecondBlackJack) = 
        (moneyGot, betAmount, secondCardSumGot, isSplitDecision, firstTwoCardsGot, secondNumberOfAces, secondBust, secondWin, hasGotSecondBlackJack);

    public int Money { get; set; }
    public int BetAmount { get; set; }
    public bool IsSplit { get; set; }
    public int SecondCardSum { get; set; }
    public int[] FirstTwoCards  { get; set; }
    public int SecondNumberOfAces { get; set; }
    public bool SecondBust { get; set; }
    public bool SecondWin { get; set; }
    public bool HasSecondBlackJack { get; set; }
}
