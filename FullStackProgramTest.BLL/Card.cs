using System.Collections.Generic;
using System.Threading.Tasks;
using FullStackProgramTest.Contracts;

namespace FullStackProgramTest.BLL
{
    public class Card : ICard
    {
        private const string _specialCardNote = "T";
        private const string _diamondCardNote = "D";
        private const string _spadeCardNote = "S";
        private const string _clubCardNote = "C";
        private const string _heartCardNote = "H";
        private readonly Dictionary<string, int> _alphabetsRanks;
        private readonly Dictionary<string, int> _specialCardsRanks;
        public Card()
        {
            _alphabetsRanks = new Dictionary<string, int>()
            {
                {"J",1},
                {"Q",2},
                {"K",3},
                {"A",4}
            };
            _specialCardsRanks = new Dictionary<string, int>()
            {
                {"4T",1},
                {"2T",2},
                {"ST",3},
                {"PT",4},
                {"RT",5}
            };
        }

        public Task<(string, string)> GetSortedCards(string cardsDeck)
        {
            List<string> specialCards = new List<string>();
            List<string> diamonds = new List<string>();
            List<string> spades = new List<string>();
            List<string> clubs = new List<string>();
            List<string> hearts = new List<string>();
            List<string> wrongCards = new List<string>();

            List<string> sortedCards = new List<string>();

            string[] cardsInDeck = cardsDeck.Split(",");

            foreach (string card in cardsInDeck)
            {
                if (card.Length <= 0
                    || card.Length > 3)
                {
                    wrongCards.Add(card);
                    continue;
                }

                if (card.Contains(_specialCardNote))
                {
                    specialCards.Add(card);
                    continue;
                }

                if (card.Contains(_diamondCardNote))
                {
                    diamonds.Add(card.Remove(card.Length - 1, 1));
                    continue;
                }

                if (card.Contains(_spadeCardNote))
                {
                    spades.Add(card.Remove(card.Length - 1, 1));
                    continue;
                }

                if (card.Contains(_clubCardNote))
                {
                    clubs.Add(card.Remove(card.Length - 1, 1));
                    continue;
                }

                if (card.Contains(_heartCardNote))
                {
                    hearts.Add(card.Remove(card.Length - 1, 1));
                    continue;
                }
            }

            if (specialCards.Count > 0)
            {
                sortedCards.Add(GetSpecialCardsSortedOrder(specialCards));
            }

            if (diamonds.Count > 0)
            {
                sortedCards.Add(GetSortedOrder(diamonds, _diamondCardNote));
            }

            if (spades.Count > 0)
            {
                sortedCards.Add(GetSortedOrder(spades, _spadeCardNote));
            }

            if (clubs.Count > 0)
            {
                sortedCards.Add(GetSortedOrder(clubs, _clubCardNote));
            }

            if (hearts.Count > 0)
            {
                sortedCards.Add(GetSortedOrder(hearts, _heartCardNote));
            }

            return Task.FromResult((string.Join(",", sortedCards), string.Join(",", wrongCards)));
        }

        private string GetSortedOrder(List<string> inputData, string cardsType)
        {
            List<int> numericsData = new List<int>();
            List<string> alphabetData = new List<string>();
            List<string> sortedData = new List<string>();

            foreach (string item in inputData)
            {
                if (int.TryParse(item, out int numaricValue))
                {
                    numericsData.Add(numaricValue);
                    continue;
                }

                alphabetData.Add(item);
            }

            string numaricSortedCards = GetNumaricSortedOrder(numericsData, cardsType);
            string alphabetSortedCards = GetAlphabetSortedOrder(alphabetData, cardsType);

            if (!string.IsNullOrEmpty(numaricSortedCards))
            {
                sortedData.Add(numaricSortedCards);
            }

            if (!string.IsNullOrEmpty(alphabetSortedCards))
            {
                sortedData.Add(alphabetSortedCards);
            }

            return string.Join(",", sortedData);
        }

        private string GetNumaricSortedOrder(List<int> inputData, string cardsType)
        {
            int tempValue;
            string sortedCardsSet = string.Empty;

            for (int outerLoopIndex = 0; outerLoopIndex <= inputData.Count - 2; outerLoopIndex++)
            {
                for (int innerLoopIndex = 0; innerLoopIndex <= inputData.Count - 2; innerLoopIndex++)
                {
                    if (inputData[innerLoopIndex] > inputData[innerLoopIndex + 1])
                    {
                        tempValue = inputData[innerLoopIndex + 1];
                        inputData[innerLoopIndex + 1] = inputData[innerLoopIndex];
                        inputData[innerLoopIndex] = tempValue;
                    }
                }
            }

            foreach (int sortNumber in inputData)
            {
                if (!string.IsNullOrEmpty(sortedCardsSet))
                {
                    sortedCardsSet = $"{sortedCardsSet},";
                }
                sortedCardsSet = $"{sortedCardsSet}{sortNumber}{cardsType}";
            }

            return sortedCardsSet;
        }

        private string GetAlphabetSortedOrder(List<string> inputData, string cardsType)
        {
            string tempValue;
            string sortedCardsSet = string.Empty;

            for (int outerLoopIndex = 0; outerLoopIndex <= inputData.Count - 2; outerLoopIndex++)
            {
                for (int innerLoopIndex = 0; innerLoopIndex <= inputData.Count - 2; innerLoopIndex++)
                {
                    if (!_alphabetsRanks.TryGetValue(inputData[innerLoopIndex], out int firstItem))
                    {
                        continue;
                    }

                    if (!_alphabetsRanks.TryGetValue(inputData[innerLoopIndex + 1], out int secondItem))
                    {
                        continue;
                    }

                    if (firstItem > secondItem)
                    {
                        tempValue = inputData[innerLoopIndex + 1];
                        inputData[innerLoopIndex + 1] = inputData[innerLoopIndex];
                        inputData[innerLoopIndex] = tempValue;
                    }
                }
            }

            foreach (string sortItem in inputData)
            {
                if (!string.IsNullOrEmpty(sortedCardsSet))
                {
                    sortedCardsSet = $"{sortedCardsSet},";
                }
                sortedCardsSet = $"{sortedCardsSet}{sortItem}{cardsType}";
            }

            return sortedCardsSet;
        }

        private string GetSpecialCardsSortedOrder(List<string> inputData)
        {
            string tempValue;
            string sortedCardsSet = string.Empty;

            for (int outerLoopIndex = 0; outerLoopIndex <= inputData.Count - 2; outerLoopIndex++)
            {
                for (int innerLoopIndex = 0; innerLoopIndex <= inputData.Count - 2; innerLoopIndex++)
                {
                    if (!_specialCardsRanks.TryGetValue(inputData[innerLoopIndex], out int firstItem))
                    {
                        continue;
                    }

                    if (!_specialCardsRanks.TryGetValue(inputData[innerLoopIndex + 1], out int secondItem))
                    {
                        continue;
                    }

                    if (firstItem > secondItem)
                    {
                        tempValue = inputData[innerLoopIndex + 1];
                        inputData[innerLoopIndex + 1] = inputData[innerLoopIndex];
                        inputData[innerLoopIndex] = tempValue;
                    }
                }
            }

            foreach (string sortItem in inputData)
            {
                if (!string.IsNullOrEmpty(sortedCardsSet))
                {
                    sortedCardsSet = $"{sortedCardsSet},";
                }
                sortedCardsSet = $"{sortedCardsSet}{sortItem}";
            }

            return sortedCardsSet;
        }
    }
}
