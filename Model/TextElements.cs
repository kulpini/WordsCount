using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace WordsCount.Model
{
    public class TextElements   // data class in Model, describing a set of text strings
    {
        public static List<TextElement> GetList(int[] stringID)
        {
            List<TextElement> elementList = new List<TextElement>();    // create new list
            foreach (int id in stringID)                                // look over string id`s
            {
                string responce = GetResponce(id.ToString());           //getting response from server with string id specifying 
                TextElement element = DeserializeJson(responce);        //deserialize response to an object
                elementList.Add(element);               // fill the list with TextElement object
            }
            return elementList;
        }
        private static TextElement DeserializeJson(string json)         // deserialize server answer to TextElement class object
        {
            TextElement element;
            try
            {
                element = JsonConvert.DeserializeObject<TextElement>(json);
            }
            catch
            {
                element = new TextElement(json);   // in case of trouble text string represents an error message
            }
            return element;
        }
        private static string GetResponce(string id)        //getting an answer from server as a string
        {
            string url = "http://tmgwebtest.azurewebsites.net/api/textstrings/" + id;
            string json = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Set("TMG-Api-Key", "0J/RgNC40LLQtdGC0LjQutC4IQ==");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                json = reader.ReadToEnd();
                stream.Close();
                reader.Close();
                response.Close();
            }
            catch (Exception e)
            {
                json = "Error: " + e.Message;  // in case of connection error return error description as a server answer
            }
            return json;
        }
    }
}
