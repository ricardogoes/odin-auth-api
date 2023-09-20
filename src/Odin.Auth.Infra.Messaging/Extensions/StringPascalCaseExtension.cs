﻿using System.Text.RegularExpressions;

namespace Odin.Auth.Infra.Messaging.Extensions
{
    public static class StringPascalCaseExtension
    {
        public static string ToPascalCase(this string original)
        {
            Regex invalidCharsRgx = new Regex("[^_a-zA-Z0-9]", RegexOptions.None, TimeSpan.FromMilliseconds(100));
            Regex whiteSpace = new Regex(@"(?<=\s)", RegexOptions.None, TimeSpan.FromMilliseconds(100));
            Regex startsWithLowerCaseChar = new Regex("^[a-z]", RegexOptions.None, TimeSpan.FromMilliseconds(100));
            Regex firstCharFollowedByUpperCasesOnly = new Regex("(?<=[A-Z])[A-Z0-9]+$", RegexOptions.None, TimeSpan.FromMilliseconds(100));
            Regex lowerCaseNextToNumber = new Regex("(?<=[0-9])[a-z]", RegexOptions.None, TimeSpan.FromMilliseconds(100));
            Regex upperCaseInside = new Regex("(?<=[A-Z])[A-Z]+?((?=[A-Z][a-z])|(?=[0-9]))", RegexOptions.None, TimeSpan.FromMilliseconds(100));

            // replace white spaces with undescore, then replace all invalid chars with empty string
            var pascalCase = invalidCharsRgx.Replace(whiteSpace.Replace(original, "_"), string.Empty)
                // split by underscores
                .Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                // set first letter to uppercase
                .Select(w => startsWithLowerCaseChar.Replace(w, m => m.Value.ToUpper()))
                // replace second and all following upper case letters to lower if there is no next lower (ABC -> Abc)
                .Select(w => firstCharFollowedByUpperCasesOnly.Replace(w, m => m.Value.ToLower()))
                // set upper case the first lower case following a number (Ab9cd -> Ab9Cd)
                .Select(w => lowerCaseNextToNumber.Replace(w, m => m.Value.ToUpper()))
                // lower second and next upper case letters except the last if it follows by any lower (ABcDEf -> AbcDef)
                .Select(w => upperCaseInside.Replace(w, m => m.Value.ToLower()));

            return string.Concat(pascalCase);
        }
    }
}
