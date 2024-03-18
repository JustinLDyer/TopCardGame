using System.Collections;
using System.Runtime.CompilerServices;
using DeckOfCardsLibrary;

namespace TopCard
{
    class CardFromBoard
    {
        public string DisplayText = "";
        public int CardValue = 0;
        public bool Taken = false;
    }

    class Program
    {
        private const int NumberOfCards = 9;
        private const int WinningScore = 21;
        private const string OptionToKeep = "K";
        private const string OptionTop = "T";
        private static Deck deck = Deck.get();
    
        private static int numAces = 0;
        private static int currentScore = 0;

        private static CardFromBoard[] topCards = new CardFromBoard[NumberOfCards];

        private static void PrintWelcome()
        {
            Console.WriteLine("***TOP CARD***\n");
            Console.WriteLine($"The object of the game is to get as close to {WinningScore} without going over.");
            Console.WriteLine("Select a card from the board. The value of that card will be displayed.");
            Console.WriteLine("You can either take the revealed card or take the card from the top of the deck.");
            Console.WriteLine("If you take the top card, the card you selected from the board will be available as a future selection so remember it.\n");
            Console.WriteLine("Disclaimer: This game is a personal educational project created solely for non-commercial purposes. It is not affiliated with or endorsed by any official entities associated with the original game show. Any references to the original game show are purely for the purpose of nostalgia. No challenge of ownership or rights to the original game show's intellectual property is intended or implied. All trademarks and copyrights belong to their respective owners.");
        }

        private static void InitializeVariables()
        {
            deck.shuffle();

            for (int i = 0; i < 9; i++)
            {
                var card = deck.draw();
                if (card != null) {
                    topCards[i] = new CardFromBoard();
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

        static void PrintTopCards(bool debug)
        {
            for (int i = 0; i < 9; i++)
            {
                if (topCards[i].Taken)
                {
                    Console.WriteLine((i+1).ToString() + "   " + "TAKEN");
                }
                else
                {
                    Console.WriteLine((i+1).ToString() + 
                                       "   " + 
                                       (debug == true ? topCards[i].DisplayText : "???"));
                }
            }
        }

        static void PlayTurn()
        {
            Console.WriteLine("");
            Console.WriteLine($"Your current score is {currentScore}");
            Console.WriteLine("");

            PrintTopCards(false);

            bool validBoardSelection = false;
            int selection = 0;
            while (!validBoardSelection) {
                Console.Write("Which card do you want? ");
                string? topCardSelection = Console.ReadLine();
                if (Int32.TryParse(topCardSelection, out selection))
                {
                    if (selection >= 1 && selection <= 9)
                    {
                        if (topCards[selection - 1].Taken)
                        {
                            Console.WriteLine("That card has already been taken.");
                        }
                        else
                        {
                            validBoardSelection = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid selection.");
                }
            }

            Console.WriteLine("The card is the " + topCards[selection - 1].DisplayText);

            bool validOptionTyped = false;
            string? optionTyped = "";
            while (!validOptionTyped)
            {
                Console.Write("Do you want to (K)eep that card or do you want the (T)op card? ");
                optionTyped = Console.ReadLine();
                optionTyped = optionTyped!.ToUpper();
                if (optionTyped == OptionToKeep || optionTyped == OptionTop)
                {
                    validOptionTyped = true;
                }
                else
                {
                    Console.WriteLine("Invalid selection");
                }
            }

            int cardSelectedValue = 0;
            if (optionTyped == OptionToKeep)
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
            if (currentScore > WinningScore)
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
            PrintWelcome();
            bool gameOver = false;
            while (!gameOver)
            {
                PlayTurn();
                
                if (currentScore == WinningScore)
                {
                    Console.WriteLine("You have reached ${WinningScore}. You win!");
                    gameOver = true;
                }
                else if (currentScore > WinningScore)
                {
                    Console.WriteLine($"Your score is {currentScore}. You have busted.");
                    gameOver = true;
                }
                else
                {
                    Console.WriteLine($"Your score is {currentScore}");
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