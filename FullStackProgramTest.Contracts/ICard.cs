using System.Threading.Tasks;

namespace FullStackProgramTest.Contracts
{
    public interface ICard
    {
        Task<(string sortedCards, string wrongCards)> GetSortedCards(string cardsDeck);
    }
}
