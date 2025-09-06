public static string FilterPeopleFromXml(string xmlData)
{
	// Null/boş veri için boş sonuç döndür
	if (string.IsNullOrWhiteSpace(xmlData))
	{
		return JsonSerializer.Serialize(new
		{
			Names = new List<string>(),
			TotalSalary = 0,
			AverageSalary = 0,
			MaxSalary = 0,
			Count = 0
		});
	}

	// XML'i yükle (geçersiz XML durumunda yakalanabilir; burada basitlik adına try/catch ekleniyor)
	XDocument doc;
	try
	{
		doc = XDocument.Parse(xmlData);
	}
	catch
	{
		// Geçersiz XML gelirse boş istatistik döndür
		return JsonSerializer.Serialize(new
		{
			Names = new List<string>(),
			TotalSalary = 0,
			AverageSalary = 0,
			MaxSalary = 0,
			Count = 0
		});
	}

	// XML → Person listesi
	var people = ParsePeople(doc);

	// Filtre kriterleri:
	// Age > 30, Department == "IT", Salary > 5000, HireDate < 2019-01-01
	var limitDate = new DateTime(2019, 1, 1);
	var eligible = people
		.Where(p => p.Age > 30
					&& string.Equals(p.Department, "IT", StringComparison.OrdinalIgnoreCase)
					&& p.Salary > 5000
					&& p.HireDate < limitDate)
		.ToList();

	// İsimler alfabetik
	var names = eligible.Select(p => p.Name)
						.OrderBy(n => n, StringComparer.Ordinal)
						.ToList();

	// Toplam/ortalama/max maaş ve kişi sayısı
	int count = eligible.Count;
	int total = count > 0 ? eligible.Sum(p => p.Salary) : 0;
	int average = count > 0 ? total / count : 0; // örnekler integer verdigi için tamsayı
	int max = count > 0 ? eligible.Max(p => p.Salary) : 0;

	// JSON çıktısı (istenen alan adlarıyla)
	var payload = new
	{
		Names = names,
		TotalSalary = total,
		AverageSalary = average,
		MaxSalary = max,
		Count = count
	};

	return JsonSerializer.Serialize(payload);
}

// XML Person listesine dönüştüren yardımcı metot
private static List<Person> ParsePeople(XDocument doc)
{
	var list = new List<Person>();

	// <People><Person>...</Person></People> yapısını bekliyoruz
	foreach (var p in doc.Descendants("Person"))
	{
		// Güvenli parse: alan yoksa varsayılan değerlere düş
		string name = (string?)p.Element("Name") ?? "";
		int age = SafeParseInt((string?)p.Element("Age"));
		string dept = (string?)p.Element("Department") ?? "";
		int salary = SafeParseInt((string?)p.Element("Salary"));
		DateTime hire = SafeParseDate((string?)p.Element("HireDate"));

		list.Add(new Person
		{
			Name = name,
			Age = age,
			Department = dept,
			Salary = salary,
			HireDate = hire
		});
	}

	return list;
}

// Yardımcı: int parse (başarısızsa 0)
private static int SafeParseInt(string? s)
	=> int.TryParse(s, out var v) ? v : 0;

// Yardımcı: DateTime parse (başarısızsa MinValue)
private static DateTime SafeParseDate(string? s)
	=> DateTime.TryParse(s, out var d) ? d : DateTime.MinValue;
	}