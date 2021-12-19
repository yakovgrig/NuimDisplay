using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace NuimDisplay
{
    class Program
    {
        static string[] maleDigitMap = new[] { "אפס", "אחד", "שניים", "שלושה", "ארבעה", "חמישה", "שישה", "שבעה", "שמונה", "תשעה" };
        static string[] femaleDigitMap = new[] { "אפס", "אחת", "שתיים", "שלוש", "ארבע", "חמש", "שש", "שבע", "שמונה", "תשע" };
        
        //static string[] tdigit = new[] { "", "", "", "שלושת", "ארבעת", "חמשת", "ששת", "שבעת", "שמונת", "תשעת", "עשרת" };

        static (string Word, bool? Plural)[] maleFirstNumbersMap = new (string, bool?)[] {
            ("", null),
            ("אחד", false),
            ("שניים", false),
            ("שלושה", false),
            ("ארבעה", false),
            ("חמישה", false),
            ("שישה", false),
            ("שבעה", false),
            ("שמונה", false),
            ("תשעה", false),
            ("עשרה", false),
            ("אחד עשר", false),
            ("שניים עשר", false),
            ("שלושה עשר", false),
            ("ארבעה עשר", false),
            ("חמישה עשר", false),
            ("שישה עשר", false),
            ("שבעה עשר", false),
            ("שמונה עשר", false),
            ("תשעה עשר", false),
        };

        static (string Word, bool? Plural)[] femaleFirstNumbersMap = new (string, bool?)[] {
            ("", null),
            ("אחת", false),
            ("שתיים", false),
            ("שלוש", false),
            ("ארבע", false),
            ("חמש", false),
            ("שש", false),
            ("שבע", false),
            ("שמונה", false),
            ("תשע", false),
            ("עשר", false),
            ("אחת עשרה", false),
            ("שתיים עשרה", false),
            ("שלוש עשרה", false),
            ("ארבע עשרה", false),
            ("חמיש עשרה", false),
            ("שיש עשרה", false),
            ("שבע עשרה", false),
            ("שמונה עשרה", false),
            ( "תשע עשרה", false),
        };

        static (string Word, bool? Plural)[] thousandFirstNumbersMap = new (string, bool?)[] {
            ("", null),
            ("אלף", null),
            ("אלפיים", null),
            ("שלושת", true),
            ("ארבעת", true),
            ("חמשת", true),
            ("ששת", true),
            ("שבעת", true),
            ("שמונת", true),
            ("תשעת", true),
            ("עשרת", true),
            ("אחד עשר", false),
            ("שניים עשר", false),
            ("שלושה עשר", false),
            ("ארבעה עשר", false),
            ("חמישה עשר", false),
            ("שישה עשר", false),
            ("שבעה עשר", false),
            ("שמונה עשר", false),
            ("תשעה עשר", false),
        };

        static string[] tensMap = new[] {
            "",
            "",
            "עשרים",
            "שלושים",
            "ארבעים",
            "חמישים",
            "שישים",
            "שבעים",
            "שמונים",
            "תשעים",
        };

        class NamedNumber
        {
            public (string Word, bool? Plural)[] FirstNumbersMap;
            public string[] DigitMap;
            public string EntityWord;
            public string EntitiesWord;

            public NamedNumber() { }
            public NamedNumber((string Word, bool? Plural)[] firstNumbersMap, string[] digitMap, string entityWord, string entitiesWord) 
            {
                this.FirstNumbersMap = firstNumbersMap;
                this.DigitMap = digitMap;
                this.EntityWord = entityWord;
                this.EntitiesWord = entitiesWord;
            }

        }

        static List<NamedNumber> list = new List<NamedNumber>
            {
                null,
                new(thousandFirstNumbersMap, maleDigitMap, "אלף", "אלפים"),
                new(maleFirstNumbersMap, maleDigitMap, "מיליון", "מיליונים"),
                new(maleFirstNumbersMap, maleDigitMap, "מיליארד", "מיליארדים"),
                new(maleFirstNumbersMap, maleDigitMap, "טריליון", "טריליונים"),
                new(maleFirstNumbersMap, maleDigitMap, "קוודריליון", "קוודריליונים"),
                new(maleFirstNumbersMap, maleDigitMap, "קווינטיליון", "קווינטיליונים"),
                new(maleFirstNumbersMap, maleDigitMap, "סקסטיליון", "סקסטיליונים"),
                new(maleFirstNumbersMap, maleDigitMap, "ספּטיליון", "ספּטיליונים"),
            };

        static void Main(string[] args)
        {
            bool forMale = true;

            var digitMap = forMale ? maleDigitMap : femaleDigitMap;
            var firstNumbersMap = forMale ? maleFirstNumbersMap : femaleFirstNumbersMap;

            list[0] = new NamedNumber(firstNumbersMap, digitMap, null, null);

            long num = 12144560557778030;
            string res = "";

            long n = num;
            int i = 0;

            bool resAndPresent = false;

            do
            {
                var namedNumber = list[i];
                int r = (int)(n % 1000);

                if (r > 0)
                {
                    string s = Hundreds(out bool andPresent, r, namedNumber.DigitMap, namedNumber.FirstNumbersMap, namedNumber.EntityWord!=null,  namedNumber.EntityWord, namedNumber.EntitiesWord);
                    res = s + (string.IsNullOrEmpty(res) ? "" : " ") + (!string.IsNullOrEmpty(res) && !resAndPresent && !andPresent ? "ו" : "") + res;
                    resAndPresent = resAndPresent || andPresent;
                }

                n = n / 1000;
                ++i;
            }
            while (n > 0);

            Debug.WriteLine(res);
        }


        static string Hundreds(
            out bool andPresent, 
            int d, 
            string[] digitsMap,
            (string Word, bool? Plural)[] firstNumbersMap,
            bool entityPresent = false,
            string entityWord = null,
            string entitiesWord = null)
        {
            StringBuilder sb = new StringBuilder();
            
            int c1 = d / 100;
            string s1 = "";

            switch (c1)
            {
                case 0: 
                    break;
                
                case 1:
                    s1 = "מאה"; 
                    break;
                
                case 2: 
                    s1 = "מאתיים"; 
                    break;

                default: 
                    s1 = $"{femaleDigitMap[c1]} מאות"; 
                    break;
            }

            int c2 = d % 100;
            string s2 = Tenths(out andPresent, c2, digitsMap, firstNumbersMap, entityPresent, entityWord, entitiesWord);

            if (!string.IsNullOrEmpty(s1))
            {
                sb.Append(s1);
            }

            if (string.IsNullOrEmpty(s2))
            {
                if (entityPresent)
                {
                    sb.Append($" {entitiesWord}");
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(s1) && !string.IsNullOrEmpty(s2))
                {
                    sb.Append(andPresent ? " " : " ו");

                    andPresent = true;
                }

                if (!string.IsNullOrEmpty(s2))
                {
                    sb.Append(s2);
                }
            }

            return sb.ToString();
        }

        static string Tenths(
            out bool andPresent, 
            int d, 
            string [] digitsMap, 
            (string Word, bool? Plural)[] firstNumbersMap, 
            bool entityPresent = false, 
            string entityWord = null, 
            string entitiesWord = null
            )
        {
            andPresent = false;

            StringBuilder sb = new StringBuilder();

            if (d < firstNumbersMap.Length)
            {
                var map = firstNumbersMap[d];
                
                sb.Append(map.Word);
                
                if (entityPresent && map.Plural != null) // if map.Plural is null then don't append either entityWord nor entitiesWord
                {
                    sb.Append(map.Plural.Value ? $" {entitiesWord}" : $" {entityWord}");
                }
            }
            else 
            {
                int tens = d / 10;
                int units = d % 10;

                if (units == 0)
                {
                    sb.Append(tensMap[tens]);
                }
                else 
                {
                    andPresent = true;

                    sb.Append($"{tensMap[tens]} ו{digitsMap[units]}");
                }

                if (entityPresent)
                {
                    sb.Append($" {entityWord}");
                }
            }

            return sb.ToString();
        }
    }
}
