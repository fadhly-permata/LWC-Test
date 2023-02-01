using Microsoft.EntityFrameworkCore;
using Repo.Context;

internal class Program
{
    private static void Main(string[] args)
    {
        using (var ctx = new TestLawenconSqliteContext(new DbContextOptions<TestLawenconSqliteContext>())) {
            Console.Write(CustomerTableFormat(ctx.Customers.ToList()));
        };

        Console.ReadKey(); 

        Environment.Exit(0);
    }

    static string CustomerTableFormat(List<Repo.Data.Entities.Customer> customers) {
        if (customers is null) throw new ArgumentNullException(nameof(customers));

        var result = "";
        foreach (var cust in customers)
        {
            var newLine = result.Length == 0 ? "" : "\n";
            result += $"{newLine}{CustConsolePattern(cust)}";
        }


        result = (
@$"
.:: CUSTOMER DATA ::.
 *=============================================================================================*
|| ID    ||  Name           ||  HP             ||  NIK            ||  Email                    ||
 *=============================================================================================*
{result}
 *=============================================================================================*
"
        );

        return result;
    }

    static string CustConsolePattern(Repo.Data.Entities.Customer data) {
        if (data is null) throw new ArgumentNullException(nameof(data));
        return $"|| {castStringWithSpace(data.Id.ToString(), 5)} || {castStringWithSpace(data.Name)} || {castStringWithSpace(data.Hp)} || {castStringWithSpace(data.Nik)} || {castStringWithSpace(data.Email, 25)} ||";
    }

    static string castStringWithSpace(string data, int maxLength = 15, char character = ' ') {
        if (data is null) throw new ArgumentNullException(nameof(data));
        return $"{data}{new string(character, maxLength - data.Length)}";
    }
}