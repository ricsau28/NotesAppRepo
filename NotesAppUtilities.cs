using System;
using System.Globalization;

namespace NotesApp
{
    public class NotesAppUtilities
    {
        public static string ExtractNotebookFromTag(string tag)
        {
            /*
              Given a string in the form of: system:notebook:Desktop
              returns Desktop
            */
            string searchStr = "notebook:";

            int index = tag.ToLower().IndexOf(searchStr);
            if (index > 0)
            {
                //return tag.Split(':')[2];
                return tag.Substring(index + searchStr.Length).Trim();

            }
            else
                return string.Empty;
        }

        /*
        public static string ReplaceSQLChars(string clause)
        {

            if (string.IsNullOrEmpty(clause))
                return string.Empty;

            string newClause = clause;

            Dictionary<string, string> special_chars = new Dictionary<string, string>();

            List<(string, string)> special_chars2 = new List<(string, string)>();

            (string, string) newTuple = ("'", "''");
            special_chars2.Add(newTuple);

            newTuple = ("\"", "\"\"");
            special_chars2.Add(newTuple);

            newTuple = ("\\", "\\\\");
            special_chars2.Add(newTuple);

            foreach ((string, string) tup in special_chars2)
            {
                newClause = newClause.Replace(tup.Item1, tup.Item2);
            }

            /*
            special_chars["'"] = "''";
            special_chars["\""] = "\"\"";
            special_chars["\\"] = "\\\\";

            foreach (var (key, value) in special_chars)
            {
                newClause.Replace(key, value);
            }
            */

        //return newClause;

        //}

        public static string GetDateAndTime()
        {
            //string cultureName = "en-US";
            //var culture = new CultureInfo(cultureName);
            //DateTime localDate = DateTime.Now;

            return DateTime.Now.ToString(new CultureInfo("en-US"));
        }


        public static string ReplaceSQLChars(string clause)
        {

            if (string.IsNullOrEmpty(clause))
                return string.Empty;

            string newClause = clause;

            //NB: A list of tuples containing chars and their replacements
            List<(string, string)> special_chars = new List<(string, string)>()
            {
                ("'", "''"), ("\"", "\"\""), ("\\", "\\\\")

            };

            foreach ((string, string) tup in special_chars)
            {
                newClause = newClause.Replace(tup.Item1, tup.Item2);
            }

            return newClause;

        }// end method


    }
}