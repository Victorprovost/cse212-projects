using System.Text.Json;
using System.Linq;

public static class SetsAndMaps
{
    /// <summary>
    /// The words parameter contains a list of two character 
    /// words (lower case, no duplicates). Using sets, find an O(n) 
    /// solution for returning all symmetric pairs of words.  
    ///
    /// For example, if words was: [am, at, ma, if, fi], we would return :
    ///
    /// ["am & ma", "if & fi"]
    ///
    /// The order of the array does not matter, nor does the order of the specific words in each string in the array.
    /// at would not be returned because ta is not in the list of words.
    ///
    /// As a special case, if the letters are the same (example: 'aa') then
    /// it would not match anything else (remember the assumption above
    /// that there were no duplicates) and therefore should not be returned.
    /// </summary>
    /// <param name="words">An array of 2-character words (lowercase, no duplicates)</param>
    public static string[] FindPairs(string[] words)
    {
        // Approach (step-by-step):
        // 1. Use a HashSet to store the remaining words for O(1) lookups and removals.
        // 2. Iterate the input array. For each word 'w':
        //    a. Skip if both characters are the same (e.g., "aa").
        //    b. Compute the reversed word 'rev'.
        //    c. If 'rev' exists in the remaining set, add a pair string and remove both
        //       words from the set to avoid duplicates.
        // 3. Collect pairs in a list and return as an array.

        if (words == null || words.Length == 0) return Array.Empty<string>();

        var remaining = new HashSet<string>(words);
        var pairs = new List<string>();

        foreach (var w in words)
        {
            if (!remaining.Contains(w))
                continue; // already paired/removed

            if (w.Length != 2)
                continue; // defensive: only handle 2-char words

            if (w[0] == w[1])
            {
                // special case: same letters do not form a pair
                remaining.Remove(w);
                continue;
            }

            var rev = new string(new[] { w[1], w[0] });
            if (remaining.Contains(rev))
            {
                // Add one representation of the pair. Order inside the string is not important.
                pairs.Add(rev + " & " + w);
                // Remove both to avoid duplicate reporting
                remaining.Remove(w);
                remaining.Remove(rev);
            }
        }

        return pairs.ToArray();
    }

    /// <summary>
    /// Read a census file and summarize the degrees (education)
    /// earned by those contained in the file.  The summary
    /// should be stored in a dictionary where the key is the
    /// degree earned and the value is the number of people that 
    /// have earned that degree.  The degree information is in
    /// the 4th column of the file.  There is no header row in the
    /// file.
    /// </summary>
    /// <param name="filename">The name of the file to read</param>
    /// <returns>fixed array of divisors</returns>
    public static Dictionary<string, int> SummarizeDegrees(string filename)
    {
        var degrees = new Dictionary<string, int>();
        foreach (var line in File.ReadLines(filename))
        {
            var fields = line.Split(",");
            // The degree/education field is the 4th column (index 3).
            if (fields.Length <= 3) continue;
            var degree = fields[3].Trim();
            if (string.IsNullOrEmpty(degree)) continue;

            if (degrees.ContainsKey(degree))
            {
                degrees[degree] += 1;
            }
            else
            {
                degrees[degree] = 1;
            }
        }

        return degrees;
    }

    /// <summary>
    /// Determine if 'word1' and 'word2' are anagrams.  An anagram
    /// is when the same letters in a word are re-organized into a 
    /// new word.  A dictionary is used to solve the problem.
    /// 
    /// Examples:
    /// is_anagram("CAT","ACT") would return true
    /// is_anagram("DOG","GOOD") would return false because GOOD has 2 O's
    /// 
    /// Important Note: When determining if two words are anagrams, you
    /// should ignore any spaces.  You should also ignore cases.  For 
    /// example, 'Ab' and 'Ba' should be considered anagrams
    /// 
    /// Reminder: You can access a letter by index in a string by 
    /// using the [] notation.
    /// </summary>
    public static bool IsAnagram(string word1, string word2)
    {
        // Plan:
        // 1. Normalize both inputs by removing whitespace and converting to lower-case.
        // 2. If the normalized lengths differ, they cannot be anagrams.
        // 3. Use a dictionary to count characters in the first word.
        // 4. Walk the second word, decrementing counts; if a character is missing or
        //    decremented below zero, they are not anagrams.
        // 5. If all counts cancel out, the words are anagrams.

        if (word1 == null || word2 == null) return false;

        // Normalize: remove spaces and make lower-case
        string Normalize(string s) => new string(s.Where(c => !char.IsWhiteSpace(c)).ToArray()).ToLowerInvariant();

        var a = Normalize(word1);
        var b = Normalize(word2);

        if (a.Length != b.Length) return false;

        var counts = new Dictionary<char, int>();
        foreach (var c in a)
        {
            if (counts.ContainsKey(c)) counts[c] += 1;
            else counts[c] = 1;
        }

        foreach (var c in b)
        {
            if (!counts.ContainsKey(c)) return false;
            counts[c] -= 1;
            if (counts[c] == 0) counts.Remove(c);
        }

        return counts.Count == 0;
    }

    /// <summary>
    /// This function will read JSON (Javascript Object Notation) data from the 
    /// United States Geological Service (USGS) consisting of earthquake data.
    /// The data will include all earthquakes in the current day.
    /// 
    /// JSON data is organized into a dictionary. After reading the data using
    /// the built-in HTTP client library, this function will return a list of all
    /// earthquake locations ('place' attribute) and magnitudes ('mag' attribute).
    /// Additional information about the format of the JSON data can be found 
    /// at this website:  
    /// 
    /// https://earthquake.usgs.gov/earthquakes/feed/v1.0/geojson.php
    /// 
    /// </summary>
    public static string[] EarthquakeDailySummary()
    {
        const string uri = "https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_day.geojson";
        using var client = new HttpClient();
        using var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        using var jsonStream = client.Send(getRequestMessage).Content.ReadAsStream();
        using var reader = new StreamReader(jsonStream);
        var json = reader.ReadToEnd();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var featureCollection = JsonSerializer.Deserialize<FeatureCollection>(json, options);
        if (featureCollection?.Features == null) return Array.Empty<string>();

        var summary = new List<string>();
        foreach (var f in featureCollection.Features)
        {
            var place = f?.Properties?.Place ?? "Unknown";
            var mag = f?.Properties?.Mag;
            var magStr = mag.HasValue ? mag.Value.ToString() : "0";
            summary.Add($"{place} - Mag {magStr}");
        }

        return summary.ToArray();
    }
}