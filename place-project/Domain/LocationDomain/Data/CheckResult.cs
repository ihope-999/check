namespace place_project.Domain.LocationDomain.Data
{
    public class CitationCheck
    {
        public string givenName { get; set; } = string.Empty;
        public string givenAddress { get; set; } = string.Empty;
        public string givenPhoneNumber { get; set; } = string.Empty;

        public string givenCity { get; set; } = string.Empty;
        public double NameScore { get; set; }
        public double AddressScore { get; set; }

        public double PhoneNumberScore { get; set; }

        public double CityScore { get; set; }

        public bool IsNameAvailable { get; set; }
        public bool IsAddressAvailable { get; private set; }
        public bool IsCityAvailable { get; private set; }
        public bool IsPhoneAvailable { get; private set; }

        public double CalculateOverallScore(bool useNameAddressOnly = false)
        {
            if (useNameAddressOnly)
            {
                return NameScore * 0.5 + AddressScore * 0.5;
            }

            return NameScore * 0.4
                   + AddressScore * 0.30
                   + PhoneNumberScore * 0.30;
        }

        public double CalculateNameScore(string realName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(realName) || string.IsNullOrWhiteSpace(givenName))
                {
                    IsNameAvailable = false;
                    NameScore = 0;
                    return 0;
                }

                IsNameAvailable = true;
                string NormalizeString(string s) => s.Trim().ToLower().Replace(".", "").Replace(",", "");
                var jw = new JaroWinkler();
                realName = NormalizeString(realName);
                givenName = NormalizeString(givenName);
                NameScore = jw.Similarity(realName, givenName) * 100;
                if (NameScore != 100) Console.WriteLine("Missing name score");
                return NameScore;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); NameScore = 0; IsNameAvailable = false; }
            return 0;

        }
        public double CalculateCityScore(string realCity)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(realCity) || string.IsNullOrWhiteSpace(givenCity))
                {
                    IsCityAvailable = false;
                    CityScore = 0;
                    return 0;
                }

                IsCityAvailable = true;
                if (realCity.ToLower() == givenCity.ToLower())
                {
                    CityScore = 100;
                    return CityScore;
                }

            }
            catch (Exception ex)
            {
                CityScore = 0;
                IsCityAvailable = false;
                Console.WriteLine(ex.ToString());
            }

            return 0;




        }
        public double CalculateAddressScore(string realAddress)
        {
            if (string.IsNullOrWhiteSpace(realAddress) || string.IsNullOrWhiteSpace(givenAddress))
            {
                IsAddressAvailable = false;
                AddressScore = 0;
                return 0;
            }

            IsAddressAvailable = true;
            realAddress = NormalizeAddressForScoring(realAddress);
            givenAddress = NormalizeAddressForScoring(givenAddress);

            AddressScore = Fuzz.TokenSortRatio(realAddress, givenAddress);
            if (AddressScore != 100)
            {
                Console.WriteLine("Missing address score");
                Console.WriteLine("Real Address: " + realAddress);
                Console.WriteLine("Given Address: " + givenAddress);
            }
            return AddressScore;
        }

        private static string NormalizeAddressForScoring(string address)
        {
            if (string.IsNullOrWhiteSpace(address)) return string.Empty;

            string NormalizeString(string s) => s.Trim().ToLowerInvariant().Replace(".", "").Replace(",", "");

            address = Functions.NormalizeAddress(address);
            address = NormalizeString(address);
            address = RemoveCountryToken(address);
            address = string.Join(" ", address.Split(' ', StringSplitOptions.RemoveEmptyEntries));
            return address;
        }

        private static string RemoveCountryToken(string address)
        {
            if (string.IsNullOrWhiteSpace(address)) return string.Empty;
            var words = address.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .ToList();

            if (words.Count == 0) return address;

            var countryPhrases = new string[]
            {
                "united states of america",
                "united states",
                "united kingdom",
                "türkiye",
                "turkey",
                "australia",
                "canada",
                "germany",
                "france",
                "spain",
                "italy",
                "japan",
                "usa",
                "us",
                "uk"
            };

            foreach (var countryPhrase in countryPhrases)
            {
                var countryTokens = countryPhrase.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i <= words.Count - countryTokens.Length; i++)
                {
                    if (countryTokens.SequenceEqual(words.Skip(i).Take(countryTokens.Length), StringComparer.OrdinalIgnoreCase))
                    {
                        words.RemoveRange(i, countryTokens.Length);
                        i = Math.Max(-1, i - 1);
                    }
                }
            }

            return string.Join(" ", words);
        }

        public double CalculatePhoneNumberScore(string realPhoneNumber)
        {
            if (string.IsNullOrWhiteSpace(realPhoneNumber) || string.IsNullOrWhiteSpace(givenPhoneNumber))
            {
                IsPhoneAvailable = false;
                PhoneNumberScore = 0;
                return 0;
            }

            IsPhoneAvailable = true;
            realPhoneNumber = System.Text.RegularExpressions.Regex.Replace(realPhoneNumber ?? string.Empty, @"^\+\d{1,3}\s*", "");
            givenPhoneNumber = System.Text.RegularExpressions.Regex.Replace(givenPhoneNumber ?? string.Empty, @"^\+\d{1,3}\s*", "");
            string NormalizePhone(string s) => new string(s.Where(char.IsDigit).ToArray());
            string normalizedReal = NormalizePhone(realPhoneNumber);
            string normalizedGiven = NormalizePhone(givenPhoneNumber ?? string.Empty);

            PhoneNumberScore = normalizedReal == normalizedGiven ? 100 : 0;
            if (PhoneNumberScore != 100) Console.WriteLine("Missing phone number");
            return PhoneNumberScore;
        }

    }
}
