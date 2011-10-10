using System.Collections.Generic;
using System.Linq;
using QCAPI.Model;

namespace QCAPI.Repositories
{
    public static class BugHelper
    {
        public static IEnumerable<Comment> ParseComments(string bgDevComments)
        {
            var sensibleString = PandocWrapper.PandocHelper.Convert(bgDevComments)
                            .Replace("<br />", string.Empty)
                            .Replace("&lt;", string.Empty)
                            .Replace("&gt;", string.Empty)
                            .Replace("'", string.Empty)
                            .Replace("_", string.Empty);

            return sensibleString.Split('\r')
                                .Where(str => !string.IsNullOrWhiteSpace(str))
                                .Select(x => new Comment
                                {
                                    Author = "root",
                                    //Author = WordBeforeComma(x),
                                    Value = InfoAfterColons(x)
                                });
        }

        public static string WordBeforeComma(string input)
        {
            var indexOfComma = input.IndexOf(',');
            var indexOfLastSpace = input.Substring(0, indexOfComma + 1).LastIndexOf(' ');

            return input.Substring(indexOfLastSpace + 1, indexOfComma - indexOfLastSpace - 1);
        }

        public static string InfoAfterColons(string input)
        {
            var indexOfColon = input.IndexOf(':');

            return input.Substring(indexOfColon + 2);
        }
    }
}