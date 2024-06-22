using CurrencyToWordConverterApp.Server.Utilities;
using System.Text;

namespace CurrencyToWordConverterApp.Server.Services
{
    public class NumberToWordService : INumberToWordService
    {
        // Converts the given amount in string format to its word representation
        public string Convert(string amount)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(amount))
                {
                    return "Error: Amount cannot be null or empty.";
                }

                // Split the input string based on the comma separator to separate dollars and cents
                var parts = amount.Split(',');

                if (parts.Length > 2)
                {
                    return "Error: Amount format is invalid. Too many commas.";
                }

                // Separating the dollar and cent parts
                string dollarsPart = parts[0];
                string centsPart = parts.Length == 2 ? parts[1] : "00";

                // Checking the validity of the input
                foreach (char c in dollarsPart)
                {
                    if (!char.IsDigit(c))
                    {
                        return "Error: Dollars part contains invalid characters.";
                    }
                }

                foreach (char c in centsPart)
                {
                    if (!char.IsDigit(c))
                    {
                        return "Error: Cents part contains invalid characters.";
                    }
                }

                if (!long.TryParse(dollarsPart, out long dollars))
                {
                    return "Error: Dollars part is not a valid number.";
                }

                if (!int.TryParse(centsPart, out int cents))
                {
                    return "Error: Cents part is not a valid number.";
                }

                if (dollars < 0 || dollars > 999999999)
                {
                    return "Error: Dollars part is out of range. Must be between 0 and 999999999.";
                }

                if (cents < 0 || cents > 99)
                {
                    return "Error: Cents part is out of range. Must be between 0 and 99.";
                }

                string dollarsInWords = ConvertDollarsToWords(dollars);
                string centsInWords = ConvertCentsToWords(cents);

                if (dollars == 0 && cents == 0)
                {
                    return "zero dollars";
                }
                if (dollars == 0)
                {
                    return $"{centsInWords}";
                }
                if (cents == 0)
                {
                    return $"{dollarsInWords} dollars";
                }

                return $"{dollarsInWords} dollars and {centsInWords}";
            }
            catch (Exception ex)
            {
                return $"Error: An unexpected error occurred. Details: {ex.Message}";
            }
        }

        // Converts the given dollar amount to words
        private static string ConvertDollarsToWords(long number)
        {
            if (number == 0)
            {
                return "zero";
            }

            StringBuilder words = new StringBuilder();

            // Convert millions part
            if (number / 1000000 > 0)
            {
                words.Append(ConvertHundredsToWords(number / 1000000) + " million ");
                number %= 1000000;
            }

            // Convert thousands part
            if (number / 1000 > 0)
            {
                words.Append(ConvertHundredsToWords(number / 1000) + " thousand ");
                number %= 1000;
            }

            // Convert hundreds part
            if (number > 0)
            {
                words.Append(ConvertHundredsToWords(number));
            }

            return words.ToString().Trim();
        }

        // Converts numbers less than 1000 to words
        private static string ConvertHundredsToWords(long number)
        {
            StringBuilder words = new StringBuilder();

            // Convert hundreds part
            if (number / 100 > 0)
            {
                words.Append(NumberMappings.UnitsMap[number / 100] + " hundred ");
                number %= 100;
            }

            // Convert tens and units part
            if (number > 0)
            {
                if (number < 20)
                {
                    words.Append(NumberMappings.UnitsMap[number]);
                }
                else
                {
                    words.Append(NumberMappings.TensMap[(number / 10) * 10]);
                    if ((number % 10) > 0)
                    {
                        words.Append("-" + NumberMappings.UnitsMap[number % 10]);
                    }
                }
            }

            return words.ToString().Trim();
        }

        // Converts the given cent amount to words
        private static string ConvertCentsToWords(int number)
        {
            if (number == 0)
            {
                return "zero cents";
            }

            StringBuilder words = new StringBuilder();

            // Convert tens and units part
            if (number < 20)
            {
                words.Append(NumberMappings.UnitsMap[number]);
            }
            else
            {
                words.Append(NumberMappings.TensMap[(number / 10) * 10]);
                if ((number % 10) > 0)
                {
                    words.Append("-" + NumberMappings.UnitsMap[number % 10]);
                }
            }

            words.Append(number == 1 ? " cent" : " cents");
            return words.ToString().Trim();
        }
    }
}


