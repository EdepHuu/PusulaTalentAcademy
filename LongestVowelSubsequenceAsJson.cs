public static string LongestVowelSubsequenceAsJson(List<string> words)
{
	// E�er liste bo� veya null ise [] d�nd�r
	if (words == null || words.Count == 0)
		return JsonSerializer.Serialize(new List<object>());

	// Sesli harfleri tutan bir k�me (k���k/b�y�k harf fark etmez)
	var vowels = new HashSet<char>(new[]
	{
				'a','e','i','o','u','A','E','I','O','U'
			});

	// Sonu�lar� saklamak i�in bir liste (JSON�a �evrilecek)
	var results = new List<object>(words.Count);

	// Her kelimeyi tek tek incele
	foreach (var word in words)
	{
		// E�er kelime bo�/null ise sequence="" length=0
		if (string.IsNullOrEmpty(word))
		{
			results.Add(new { word = word ?? "", sequence = "", length = 0 });
			continue;
		}

		// En uzun sesli alt diziyi tutacak de�i�kenler
		int bestStart = -1, bestLen = 0;

		// �u an takip edilen sesli alt diziyi tutan de�i�kenler
		int curStart = -1, curLen = 0;

		// Kelimenin her karakterini gez
		for (int i = 0; i < word.Length; i++)
		{
			if (vowels.Contains(word[i]))
			{
				// Sesli harf bulundu, yeni bir blok ba�l�yorsa ba�lang�c� i�aretle
				if (curLen == 0) curStart = i;

				// Mevcut blok uzunlu�unu art�r
				curLen++;

				// �imdiye kadarki en uzun blok mu? Evetse g�ncelle
				if (curLen > bestLen)
				{
					bestLen = curLen;
					bestStart = curStart;
				}
			}
			else
			{
				// Sessiz harf mevcut blok s�f�rlan�r
				curLen = 0;
			}
		}

		// En uzun alt dizi bulunduysa ��kar, yoksa bo� string
		string sequence = bestLen > 0 ? word.Substring(bestStart, bestLen) : string.Empty;

		// Sonu� listesine ekle
		results.Add(new { word, sequence, length = bestLen });
	}

	// JSON�a �evir ve d�nd�r
	return JsonSerializer.Serialize(results);
}