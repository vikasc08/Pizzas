﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Pizzas
{
    class Program
    {
        static void Main(string[] args)
        {
            //Dictionary is the logical choice of collection for this requirement since
            //Dictionary is a key value pair, populate the key with the Pizza Toppings and the count in the value
            //Once the collection is populated, simply sort by descending, and take top 20
            Dictionary<string, int> Pizzas = new Dictionary<string, int>();

            //Reading the JSON array from the given file
            var jarray = JArray.Parse(File.ReadAllText("pizzas.json"));

            //Loop through the JArray to parse each JObject
            foreach (var item in jarray.Children<JObject>())
            {
                //Get the JSON Token value for each JObject
                item.TryGetValue("toppings", out JToken value);
                var key = FormatString(value.ToString());


                //If the JSON Token already exists, get the value of the Token and increment else add new pair to the collection
                if (Pizzas.ContainsKey(key))
                {
                    Pizzas.TryGetValue(key, out int currentCount);
                    Pizzas[key] = currentCount+1;
                }
                else
                    Pizzas.Add(key, 1);
            }

            //Selecting the Top 20 from the collection in Descending order of the value
            var top20 = Pizzas.OrderByDescending(k => k.Value)
                        .Take(20);
            int Rank = 1;
            foreach (var item in top20)
            {
                Console.WriteLine(@"Popular Pizza Toppings: {0}, Count of Orders: {1}, Rank: {2}", item.Key, item.Value, Rank);
                Rank++;
            }
        }

        public static string FormatString(string key)
        {
            key = string.Join("", System.Text.RegularExpressions.Regex.Split(key, @"(?:\r\n|\n\r|\n|\r|\[|\])"));
            string[] stringArray = key.Split(',');
            Array.Sort(stringArray);
            string returnValue = "";
            for (int i = stringArray.GetLowerBound(0); i <= stringArray.GetUpperBound(0); i++)
            {
                returnValue = returnValue + stringArray[i].Trim() + ",";
            }
            return returnValue.Remove(returnValue.Length - 1, 1);
            //return key.Trim();
        }
    }
}
