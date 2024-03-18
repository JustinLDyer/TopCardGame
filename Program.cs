using System.Collections;
using System.Runtime.CompilerServices;
using DeckOfCardsLibrary;

namespace TopCard
{
    class TopCardGame
    {
        private static Deck deck = Deck.get();
    
        private static int numAces = 0;
        private static int currentScore = 0;

        private static BoardCard[] topCards = new BoardCard[Constants.NumberOfCards];

        private static void InitializeVariables()
        {
            deck.shuffle();

            for (int i = 0; i < Constants.NumberOfCards; i++)
            {
                var card = deck.draw();
                if (card != null) {
                    topCards[i] = new BoardCard();
                    topCards[i].DisplayText = GetDisplayText(card);
                    topCards[i].CardValue = GetCardValue(card.rank);
                    topCards[i].Taken = false;
                }
            }
        }

        private static string GetDisplayText(Card card)
        {
            return card.rank + " of " + card.suit;
        }

        private static int GetCardValue(Card.Rank rank)
        {
            int cardValue;

            switch (rank){
                case Card.Rank.Ace:
                  cardValue = 11;
                  break;
                case Card.Rank.King:
                case Card.Rank.Queen:
                case Card.Rank.Jack:
                   cardValue = 10;
                   break;
                default:
                    cardValue = (int)rank;
                    break;
            }

            return cardValue;
        }

        static void PlayTurn()
        {
            Console.WriteLine("");
            UserInterface.PrintScore(currentScore);
            Console.WriteLine("");

            UserInterface.PrintTopCards(topCards);
            int selection = UserInterface.GetBoardCardSelection(topCards);
            Console.WriteLine("The card is the " + topCards[selection - 1].DisplayText);

            bool validOptionTyped = false;
            string? optionTyped = "";
            while (!validOptionTyped)
            {
                Console.Write("Do you want to (K)eep that card or do you want the (T)op card? ");
                optionTyped = Console.ReadLine();
                optionTyped = optionTyped!.ToUpper();
                if (optionTyped == Constants.OptionToKeep || optionTyped == Constants.OptionTop)
                {
                    validOptionTyped = true;
                }
                else
                {
                    Console.WriteLine("Invalid selection");
                }
            }

            int cardSelectedValue = 0;
            if (optionTyped == Constants.OptionToKeep)
            {
                cardSelectedValue = topCards[selection - 1].CardValue;
                topCards[selection - 1].Taken = true;
            }
            else
            {
                var card = deck.draw();
                if (card != null)
                {
                    Console.WriteLine("The top card is the " + GetDisplayText(card));
                    cardSelectedValue = GetCardValue(card.rank);
                }
                else
                {
                    Console.WriteLine("All cards have been drawn. This should never happen in blackjack.");
                }
            }

            currentScore += cardSelectedValue;
            if (cardSelectedValue == 11)
            {
                numAces++;
            }
            if (currentScore > Constants.WinningScore)
            {
                if (numAces > 0)
                {
                    currentScore -= 10;
                    numAces--;
                }
            }
        }

        static void Main(string[] args)
        {
            InitializeVariables();
            UserInterface.PrintWelcomeMessage();
            bool gameOver = false;
            while (!gameOver)
            {
                PlayTurn();
                
                if (currentScore == Constants.WinningScore)
                {
                    Console.WriteLine($"You have reached {Constants.WinningScore}. You win!");
                    gameOver = true;
                }
                else if (currentScore > Constants.WinningScore)
                {
                    UserInterface.PrintScore(currentScore);
                    Console.WriteLine("You have busted.");
                    gameOver = true;
                }
                else
                {
                    UserInterface.PrintScore(currentScore);
                    string? keepPlaying = "";
                    while (keepPlaying! != "Y" && keepPlaying! != "N")
                    {
                        Console.Write("Would you like to keep playing? (Y/N) ");
                        keepPlaying = Console.ReadLine();
                        keepPlaying = keepPlaying!.ToUpper();
                    }
                    if (keepPlaying == "N")
                    {
                        gameOver = true;
                    }
                    else
                    {
                        Console.Clear();
                    }
                }
            }
        }
    }
}