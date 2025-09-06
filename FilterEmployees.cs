public static string FilterEmployees(IEnumerable<(string Name, int Age, string Department, decimal Salary, DateTime HireDate)> employees)
{
	// Null veya boþ kaynak gelirse direkt boþ istatistik döndür
	if (employees == null || !employees.Any())
	{
		return JsonSerializer.Serialize(new
		{
			Names = new List<string>(),
			TotalSalary = 0m,
			AverageSalary = 0m,
			MinSalary = 0m,
			MaxSalary = 0m,
			Count = 0
		});
	}

	// Tarih eþiði: örnekleri saðlamak için 2017-01-01 sonrasý (strict)
	var hireDateLowerBound = new DateTime(2017, 1, 1);

	// Filtre: yaþ, departman, maaþ ve iþe giriþ tarihi
	var filtered = employees
		.Where(e =>
			e.Age >= 25 && e.Age <= 40 &&                                              // yaþ aralýðý (dahil)
			(e.Department.Equals("IT", StringComparison.OrdinalIgnoreCase) ||
			 e.Department.Equals("Finance", StringComparison.OrdinalIgnoreCase)) &&   // departman
			e.Salary >= 5000m && e.Salary <= 9000m &&                                  // maaþ aralýðý (dahil)
			e.HireDate > hireDateLowerBound                                            // 2017-01-01'den sonra
		)
		.ToList();

	// Ýsimleri: önce uzunluk (desc), sonra alfabetik (asc)
	var sortedNames = filtered
		.Select(e => e.Name)
		.OrderByDescending(n => n.Length)
		.ThenBy(n => n, StringComparer.Ordinal) // Türkçe karþýlaþtýrma gerekmiyorsa Ordinal yeterli
		.ToList();

	// Ýstatistikler
	int count = filtered.Count;
	decimal total = count > 0 ? filtered.Sum(e => e.Salary) : 0m;
	// Örnek çýktýlarda ortalama 2 ondalýkla yuvarlanmýþ (7166.67 vb.)
	decimal average = count > 0
		? Math.Round(total / count, 2, MidpointRounding.AwayFromZero)
		: 0m;
	decimal min = count > 0 ? filtered.Min(e => e.Salary) : 0m;
	decimal max = count > 0 ? filtered.Max(e => e.Salary) : 0m;

	// JSON payload (alan adlarý örneklerle birebir)
	var payload = new
	{
		Names = sortedNames,
		TotalSalary = total,
		AverageSalary = average,
		MinSalary = min,
		MaxSalary = max,
		Count = count
	};

	return JsonSerializer.Serialize(payload);
}