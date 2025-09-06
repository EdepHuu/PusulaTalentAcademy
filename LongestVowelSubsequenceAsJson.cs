public static string LongestVowelSubsequenceAsJson(List<string> words)
{
	// Eðer liste boþ veya null ise [] döndür
	if (words == null || words.Count == 0)
		return JsonSerializer.Serialize(new List<object>());

	// Sesli harfleri tutan bir küme (küçük/büyük harf fark etmez)
	var vowels = new HashSet<char>(new[]
	{
				'a','e','i','o','u','A','E','I','O','U'
			});

	// Sonuçlarý saklamak için bir liste (JSON’a çevrilecek)
	var results = new List<object>(words.Count);

	// Her kelimeyi tek tek incele
	foreach (var word in words)
	{
		// Eðer kelime boþ/null ise sequence="" length=0
		if (string.IsNullOrEmpty(word))
		{
			results.Add(new { word = word ?? "", sequence = "", length = 0 });
			continue;
		}

		// En uzun sesli alt diziyi tutacak deðiþkenler
		int bestStart = -1, bestLen = 0;

		// Þu an takip edilen sesli alt diziyi tutan deðiþkenler
		int curStart = -1, curLen = 0;

		// Kelimenin her karakterini gez
		for (int i = 0; i < word.Length; i++)
		{
			if (vowels.Contains(word[i]))
			{
				// Sesli harf bulundu, yeni bir blok baþlýyorsa baþlangýcý iþaretle
				if (curLen == 0) curStart = i;

				// Mevcut blok uzunluðunu artýr
				curLen++;

				// Þimdiye kadarki en uzun blok mu? Evetse güncelle
				if (curLen > bestLen)
				{
					bestLen = curLen;
					bestStart = curStart;
				}
			}
			else
			{
				// Sessiz harf mevcut blok sýfýrlanýr
				curLen = 0;
			}
		}

		// En uzun alt dizi bulunduysa çýkar, yoksa boþ string
		string sequence = bestLen > 0 ? word.Substring(bestStart, bestLen) : string.Empty;

		// Sonuç listesine ekle
		results.Add(new { word, sequence, length = bestLen });
	}

	// JSON’a çevir ve döndür
	return JsonSerializer.Serialize(results);
}