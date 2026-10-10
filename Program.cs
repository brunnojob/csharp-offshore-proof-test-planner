using System.Globalization;
using System.Text.Json;

static class Program
{
    static readonly CultureInfo Culture = CultureInfo.InvariantCulture;
    static double Numeric(string value)
    {
        double result = double.Parse(value, Culture);
        Require(double.IsFinite(result), "Finite numeric input required");
        return result;
    }
    static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidDataException(message);
    }
    static List<string[]> Input(int columns)
    {
        var rows = new List<string[]>();
        string? line;
        while ((line = Console.ReadLine()) is not null)
        {
            Require(rows.Count < 100000, "Input capacity exceeded");
            Require(line.Length <= 8192, "Input record too large");
            string[] fields = line.Split(',');
            Require(fields.Length == columns && fields.All(f => !string.IsNullOrWhiteSpace(f)), "Invalid CSV row");
            rows.Add(fields);
        }
        Require(rows.Count > 0, "Missing input");
        return rows;
    }
    static int Main(string[] args)
    {
        try
        {
            Require(args.Length == 0 || args.SequenceEqual(new[] { "--self-test" }), "Unsupported command arguments");
            if (args.SequenceEqual(new[] { "--self-test" })) { Test(); Console.WriteLine("Tests passed"); }
            else Run();
            return 0;
        }
        catch (Exception error) { Console.Error.WriteLine(error.Message); return 2; }
    }

    record Device(string Tag, double RatePerHour, double IntervalHours, double ElapsedHours, double Coverage);
    record Assessment(string Tag, double AveragePfd, double CurrentPfd, double OverdueHours, double Priority);
    static Assessment Assess(Device d)
    {
        Require(d.RatePerHour >= 0 && d.RatePerHour <= 0.001 && d.IntervalHours > 0 && d.ElapsedHours >= 0, "Invalid device parameters");
        Require(d.Coverage > 0 && d.Coverage <= 1, "Coverage must lie in (0,1]");
        double covered = d.RatePerHour * d.Coverage;
        double average = covered == 0 ? 0 : 1 - (1 - Math.Exp(-covered * d.IntervalHours)) / (covered * d.IntervalHours);
        double current = 1 - Math.Exp(-covered * d.ElapsedHours);
        double overdue = Math.Max(0, d.ElapsedHours - d.IntervalHours);
        return new Assessment(d.Tag, average, current, overdue, current * (1 + overdue / d.IntervalHours));
    }
    static void Test()
    {
        var zero = Assess(new Device("A", 0, 100, 200, 1)); Require(zero.CurrentPfd == 0 && zero.OverdueHours == 100, "Zero-rate mismatch");
        var a = Assess(new Device("A", 0.00001, 1000, 1000, 1));
        Require(Math.Abs(a.CurrentPfd - (1 - Math.Exp(-0.01))) < 1e-12, "Exponential model mismatch");
        Require(a.AveragePfd > 0 && a.AveragePfd < a.CurrentPfd, "Average PFD mismatch");
    }
    static void Run()
    {
        var devices = Input(5).Select(r => new Device(r[0], Numeric(r[1]), Numeric(r[2]), Numeric(r[3]), Numeric(r[4]))).ToList();
        Require(devices.Select(d => d.Tag).Distinct().Count() == devices.Count, "Duplicate device");
        foreach (var a in devices.Select(Assess).OrderByDescending(a => a.Priority).ThenBy(a => a.Tag)) Console.WriteLine(JsonSerializer.Serialize(a));
    }

}
