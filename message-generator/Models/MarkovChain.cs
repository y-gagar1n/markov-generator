using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace message_generator.Models
{
    internal class MarkovChain
    {
        private readonly Dictionary<string, List<string>> pairs = new Dictionary<string, List<string>>();
        private readonly Random r = new Random();

        public void Add(string key1, string key2, string value)
        {
            List<string> words = null;
            //var key = key1 + " " + key2;
            var key = key2;
            if (!pairs.TryGetValue(key, out words))
            {
                words = new List<string>();
                pairs[key] = words;
            }
            words.Add(value);
        }

        public string GetBegin()
        {
            string randomKey = null;
            string next = null;
            List<string> nextVariants;
            int attempts = 0;
            do
            {
                randomKey = GetRandom(pairs.Keys.ToList());

                pairs.TryGetValue(randomKey, out nextVariants);
                attempts++;
            } while (attempts < 50 && (nextVariants == null || nextVariants.Count() < 2));

            return randomKey;
        }

        public string Get(string key1, string key2)
        {
            //var key = key1 + " " + key2;
            var key = key2;
            return Get(key);
        }

        public string Get(string complexKey)
        {
            List<string> words;
            return pairs.TryGetValue(complexKey, out words) ? GetRandom(words) : null;
        }

        public string GetPhrase(int minLength)
        {
            string phrase = "";
            int attempts = 0;
            do
            {
                attempts++;
                var begin = GetBegin();
                phrase += begin;
                string word = begin;
                //var keys = word.Split(new char[] {' '}, 2);
                //string key1 = keys[0], key2 = keys[1];
                string key1 = null, key2 = word;
                int wordsCount = 1;
                do
                {
                    word = Get(key1, key2);
                    phrase += " " + word;
                    key1 = key2;
                    key2 = word;
                    wordsCount++;
                } while (word != null && wordsCount < 100);
            } while (phrase.Split(new char[] { ' ' }).Length < minLength && attempts < 50);

            return phrase;
        }

        private string GetRandom(List<string> input)
        {
            return input[r.Next(input.Count)];
        }
    }
}