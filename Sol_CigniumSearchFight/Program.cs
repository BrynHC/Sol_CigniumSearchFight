using Cignium.Entitites;
using Cignium.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sol_CigniumSearchFight
{
    class Program
    {
        static void Main(string[] args)
        {
            GoogleSearch oGoogleSearch = new GoogleSearch();
            BingSearch oBingSearch = new BingSearch();

            Console.WriteLine("Type programming languages and press enter:");

            ///GET DATA FROM INPUT
            string data = Console.ReadLine();

            ///SEPARATING DATA FROM INPUT BY SPACE
            var toSearchList = data.Split(' ');
            
            long googleSearchResult = default(long);
            long bingSearchResult = default(long);

            List<Result> googleResultList = new List<Result>();
            Result googleResult = new Result();

            List<Result> bingResultList = new List<Result>();
            Result bingResult = new Result();

            foreach (var item in toSearchList)
            {
                ///SEARCH IN GOOGLE
                googleResult = oGoogleSearch.GetGoogleSearchList(item);
                googleSearchResult = googleResult.ErrorMessage != string.Empty ? default(int) : googleResult.GoogleAmount;

                googleResultList.Add(googleResult);

                ///SEARCH IN BING
                bingResult = oBingSearch.GetBingSearchList(item);
                bingSearchResult = bingResult.ErrorMessage != string.Empty ? default(int) : bingResult.BingAmount;

                bingResultList.Add(bingResult);

                Console.WriteLine($"{item}:\n\tGoogle: {googleSearchResult}\n\tMSN Search: {bingSearchResult}\n");
            }

            string programmingLanguageGoogleWinner = googleResultList.FirstOrDefault(m => m.GoogleAmount == googleResultList.Max(x => x.GoogleAmount)).ProgrammingLanguage;
            string programmingLanguageBingWinner = bingResultList.FirstOrDefault(m => m.BingAmount == bingResultList.Max(x => x.BingAmount)).ProgrammingLanguage;

            Console.WriteLine($"Google winner: {programmingLanguageGoogleWinner}\n");
            Console.WriteLine($"MSN winner: {programmingLanguageBingWinner}\n");

            string totalWinnerSearch = googleResultList.Max(x => x.GoogleAmount) > bingResultList.Max(x => x.BingAmount) ? programmingLanguageGoogleWinner : programmingLanguageBingWinner;

            Console.WriteLine($"Total winner: {totalWinnerSearch}");

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}
