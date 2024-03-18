namespace TopCard
{
    public static class UserInterface
    {
        public static void PrintWelcomeMessage()
        {
            Console.WriteLine("***TOP CARD***\n");
            Console.WriteLine($"The object of the game is to get as close to {Constants.WinningScore} without going over.");
            Console.WriteLine("Select a card from the board. The value of that card will be displayed.");
            Console.WriteLine("You can either take the revealed card or take the card from the top of the deck.");
            Console.WriteLine("If you take the top card, the card you selected from the board will be available as a future selection so remember it.\n");
            Console.WriteLine("Disclaimer: This game is a personal educational project created solely for non-commercial purposes. It is not affiliated with or endorsed by any official entities associated with the original game show. Any references to the original game show are purely for the purpose of nostalgia. No challenge of ownership or rights to the original game show's intellectual property is intended or implied. All trademarks and copyrights belong to their respective owners.");
        }

        public static void PrintScore(int currentScore)
        {
            Console.WriteLine($"Your score is {currentScore}");
        }

        public static void PrintTopCards(BoardCard[] topCards)
        {
            for (int i = 0; i < Constants.NumberOfCards; i++)
            {
                if (topCards[i].Taken)
                {
                    Console.WriteLine((i+1).ToString() + "   " + "TAKEN");
                }
                else
                {
                    Console.WriteLine((i+1).ToString() + "   " + "???");
                }
            }
        }

        public static int GetBoardCardSelection(BoardCard[] topCards)
        {
            bool validBoardSelection = false;
            int selection = 0;
            while (!validBoardSelection) {
                Console.Write("Which card do you want? ");
                string? topCardSelection = Console.ReadLine();
                if (Int32.TryParse(topCardSelection, out selection))
                {
                    if (selection >= 1 && selection <= Constants.NumberOfCards)
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

            return selection;
        }
    }
}