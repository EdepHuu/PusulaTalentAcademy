public static string MaxIncreasingSubArrayAsJson(List<int> numbers)
{
	// Güvenlik: null veya boþ liste
	if (numbers == null || numbers.Count == 0)
		return JsonSerializer.Serialize(new List<int>());

	// En iyi alt dizi ve toplamý
	List<int> best = new List<int>();
	int bestSum = int.MinValue;

	// Geçici (artýþ sürdükçe büyütülen) alt dizi ve toplamý
	List<int> current = new List<int> { numbers[0] };
	int currentSum = numbers[0];

	for (int i = 1; i < numbers.Count; i++)
	{
		// Artýþ devam ediyor mu?
		if (numbers[i] > current[^1])
		{
			current.Add(numbers[i]);
			currentSum += numbers[i];
		}
		else
		{
			// Artýþ bozuldu o ana kadarki seriyi deðerlendir
			if (currentSum > bestSum)
			{
				bestSum = currentSum;
				best = new List<int>(current);
			}

			// Yeni seriyi baþlat
			current.Clear();
			current.Add(numbers[i]);
			currentSum = numbers[i];
		}
	}

	// Sondaki seriyi de deðerlendir
	if (currentSum > bestSum)
	{
		best = new List<int>(current);
	}

	return JsonSerializer.Serialize(best);
}