using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using java.util;

using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.NumberWithUnit;
using Microsoft.Recognizers.Text.Choice;
using Microsoft.Recognizers.Text.Sequence;
using Newtonsoft.Json;


namespace Helpers
{
    public class NLPHelper
    {
        
        // This will use both Microsoft Recognizer 
        public List<NLPTextResults> NPLProcessing(string words)
        {
            var textresultsRecognizer = new List<NLPTextResults>()
            
            try
            {
                var results = ParseAll(words, defaultCulture).OrderBy(a => a.Start);                

                foreach (var item in results)
                {
                    try
                    {                        
                        textresultsRecognizer.Add(new NLPTextResults { position = item.Start, text = item.Text, type = item.TypeName, lemma = "n/a", processedBy = "Microsoft Recognizer" });                        
                    }
                    catch
                    {

                    }

                }
                
            }
            catch (Exception e)
            {                
                response += e.Message;
            }

            //    // Now use Stanford NLP                 

            return (textresultsRecognizer);
        }

        

        private static IEnumerable<ModelResult> ParseAll(string query, string culture)
        {
            return MergeResults(
                // Number recognizer will find any number from the input               
                NumberRecognizer.RecognizeNumber(query, culture),

                // Ordinal number recognizer will find any ordinal number                
                NumberRecognizer.RecognizeOrdinal(query, culture),

                // Percentage recognizer will find any number presented as percentage                
                NumberRecognizer.RecognizePercentage(query, culture),

                // Number Range recognizer will find any cardinal or ordinal number range                
                NumberRecognizer.RecognizeNumberRange(query, culture),

                // Age recognizer will find any age number presented                
                NumberWithUnitRecognizer.RecognizeAge(query, culture),

                // Currency recognizer will find any currency presented                
                NumberWithUnitRecognizer.RecognizeCurrency(query, culture),

                // Dimension recognizer will find any dimension presented                
                NumberWithUnitRecognizer.RecognizeDimension(query, culture),

                // Temperature recognizer will find any temperature presented                
                NumberWithUnitRecognizer.RecognizeTemperature(query, culture),

                // Datetime recognizer This model will find any Date even if its write in coloquial language                
                DateTimeRecognizer.RecognizeDateTime(query, culture),

                // PhoneNumber recognizer will find any phone number presented               
                SequenceRecognizer.RecognizePhoneNumber(query, culture),

                // Add IP recognizer - This recognizer will find any Ipv4/Ipv6 presented               
                SequenceRecognizer.RecognizeIpAddress(query, culture)                
                );
        }

        private static IEnumerable<ModelResult> MergeResults(params List<ModelResult>[] results)
        {
            return results.SelectMany(o => o);
        }

        public class NLPTextResults
        {
            public int position { get; set; }

            public string text { get; set; }

            public string type { get; set; }

            public string lemma { get; set; }

            public string processedBy { get; set; }
        }
                
    }
}
