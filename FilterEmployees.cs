public static string FilterEmployees(IEnumerable<(string Name, int Age, string Department, decimal Salary, DateTime HireDate)> employees)
{
	// Null veya bo� kaynak gelirse direkt bo� istatistik d�nd�r
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

	// Tarih e�i�i: �rnekleri sa�lamak i�in 2017-01-01 sonras� (strict)
	var hireDateLowerBound = new DateTime(2017, 1, 1);

	// Filtre: ya�, departman, maa� ve i�e giri� tarihi
	var filtered = employees
		.Where(e =>
			e.Age >= 25 && e.Age <= 40 &&                                              // ya� aral��� (dahil)
			(e.Department.Equals("IT", StringComparison.OrdinalIgnoreCase) ||
			 e.Department.Equals("Finance", StringComparison.OrdinalIgnoreCase)) &&   // departman
			e.Salary >= 5000m && e.Salary <= 9000m &&                                  // maa� aral��� (dahil)
			e.HireDate > hireDateLowerBound                                            // 2017-01-01'den sonra
		)
		.ToList();

	// �simleri: �nce uzunluk (desc), sonra alfabetik (asc)
	var sortedNames = filtered
		.Select(e => e.Name)
		.OrderByDescending(n => n.Length)
		.ThenBy(n => n, StringComparer.Ordinal) // T�rk�e kar��la�t�rma gerekmiyorsa Ordinal yeterli
		.ToList();

	// �statistikler
	int count = filtered.Count;
	decimal total = count > 0 ? filtered.Sum(e => e.Salary) : 0m;
	// �rnek ��kt�larda ortalama 2 ondal�kla yuvarlanm�� (7166.67 vb.)
	decimal average = count > 0
		? Math.Round(total / count, 2, MidpointRounding.AwayFromZero)
		: 0m;
	decimal min = count > 0 ? filtered.Min(e => e.Salary) : 0m;
	decimal max = count > 0 ? filtered.Max(e => e.Salary) : 0m;

	// JSON payload (alan adlar� �rneklerle birebir)
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