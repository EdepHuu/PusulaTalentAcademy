public static string MaxIncreasingSubArrayAsJson(List<int> numbers)
{
	// G�venlik: null veya bo� liste
	if (numbers == null || numbers.Count == 0)
		return JsonSerializer.Serialize(new List<int>());

	// En iyi alt dizi ve toplam�
	List<int> best = new List<int>();
	int bestSum = int.MinValue;

	// Ge�ici (art�� s�rd�k�e b�y�t�len) alt dizi ve toplam�
	List<int> current = new List<int> { numbers[0] };
	int currentSum = numbers[0];

	for (int i = 1; i < numbers.Count; i++)
	{
		// Art�� devam ediyor mu?
		if (numbers[i] > current[^1])
		{
			current.Add(numbers[i]);
			currentSum += numbers[i];
		}
		else
		{
			// Art�� bozuldu o ana kadarki seriyi de�erlendir
			if (currentSum > bestSum)
			{
				bestSum = currentSum;
				best = new List<int>(current);
			}

			// Yeni seriyi ba�lat
			current.Clear();
			current.Add(numbers[i]);
			currentSum = numbers[i];
		}
	}

	// Sondaki seriyi de de�erlendir
	if (currentSum > bestSum)
	{
		best = new List<int>(current);
	}

	return JsonSerializer.Serialize(best);
}