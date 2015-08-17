using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace MarkovGen.Models
{
    public class Generator
    {
        private readonly Regex r = new Regex(
            "^(.*) @ [0-9]{4}\\.[0-9]{2}\\.[0-9]{2} [0-9]{2}:[0-9]{2}:[0-9]{2}$", RegexOptions.Compiled);
        Dictionary<string, MarkovChain> chains = new Dictionary<string, MarkovChain>();
        Random rnd = new Random();

        public void Init()
        {
            var filename = "input.txt";
            var wc = new WebClient();
            var url = ConfigurationManager.AppSettings["InputUrl"];
            wc.DownloadFile(url, filename);
            var lines = File.ReadAllLines(filename);
            
            var currentUser = "";
            var phrasesDict = new Dictionary<string, List<string>>();
            foreach (var line in lines)
            {
                if (line == "[photo_100]") continue;
                var m = r.Match(line);
                if (m.Success)
                {
                    currentUser = m.Groups[1].Value;
                }
                else
                {
                    List<string> phrases;
                    if (phrasesDict.TryGetValue(currentUser, out phrases))
                    {
                        phrases.Add(line);
                    }
                    else
                    {
                        phrasesDict[currentUser] = new List<string> { line };
                    }
                }
            }

            foreach (var corpus in phrasesDict)
            {
                if (corpus.Value.Count() > 1000)
                {
                    var chain = new MarkovChain();
                    chains[corpus.Key] = chain;
                    foreach (var phrase in corpus.Value)
                    {
                        var cleanPhrase = phrase
                            .Replace(")", " ")
                            .Replace("(", " ")
                            .Replace(".", " ");

                        var words = cleanPhrase.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                        if (words.Length >= 3)
                        {
                            for (var i = 2; i < words.Length; i++)
                            {
                                chain.Add(words[i - 2], words[i - 1], words[i]);
                            }
                        }
                    }
                }
            }
        }

        public string Generate()
        {
            var randomKey = chains.Keys.ToList()[rnd.Next(chains.Keys.Count)];
            var chain = chains[randomKey];

            var example = chain.GetPhrase(20);
            return String.Format("{0}: {1}", randomKey, example);
        }
    }
}