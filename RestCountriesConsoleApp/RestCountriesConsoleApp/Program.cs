using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;

namespace RestCountriesConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader oSr = new StreamReader("Countries.json");
            string sJson = "";
            using (oSr)
            {
                sJson = oSr.ReadToEnd();
            }

            JObject oJson = JObject.Parse(sJson);
            var oCountries = oJson["countries"].ToList();
            List<Country> lCountries = new List<Country>();
            for(int i=0; i<oCountries.Count;i++)
            {
                lCountries.Add(new Country
                {
                    sCode=(string)oCountries[i]["code"],
                    sName = (string)oCountries[i]["name"],
                    sCapital = (string)oCountries[i]["capital"],
                    nPopulation = (int)oCountries[i]["population"],
                    fArea = (float)oCountries[i]["area"]
                    
                });
            }

            for(int i=0;i<lCountries.Count;i++)
            {
                Console.WriteLine(lCountries[i].sCode + " " + lCountries[i].sName + " " + lCountries[i].sCapital + " " + lCountries[i].nPopulation + " " + lCountries[i].fArea);
            }

            Console.WriteLine(" ");

            var OrderByQuery = from c in lCountries.OrderBy(o => o.sCode) select c;
            List<Country> lSortedCountries = OrderByQuery.ToList();
            for(int i=0; i<lSortedCountries.Count;i++)
            {
                Console.WriteLine(lSortedCountries[i].sCode + " " + lSortedCountries[i].sName + " " + lSortedCountries[i].sCapital + " " + lSortedCountries[i].nPopulation + " " + lSortedCountries[i].fArea);
            }

            Console.ReadKey();
        }
    }
}
