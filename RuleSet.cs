using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleSet : MonoBehaviour
{
    //Check if points are exceeding 21

    public bool IsEntitiyBust(int pointSum)
    {
        if (pointSum > 21)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Check If dealer points are higher or equal than 17
    public bool DealMoreCardsToDealer(int pointSum)
    {
        if (pointSum < 17)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Check who's the winner
    public bool DidPlayerWin(int playerPoints, int dealerPoints)
    {
        if (playerPoints > dealerPoints)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Check if Draw
    public bool IsItADraw(int playerPoint, int dealerPoints)
    {
        if (playerPoint == dealerPoints)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Check if BlackJack
    public bool IsBlackJack(int cardSum)
    {
        if (cardSum == 21)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Check if legible for split
    public bool IsLegibleForSplit(int firstCardValue, int secondCardValue)
    {
        if(firstCardValue == secondCardValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Check if legible for double
    public bool IsCardValueLegibleForDouble(int cardSum)
    {
        if (cardSum == 9 || cardSum == 10||cardSum == 11)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
