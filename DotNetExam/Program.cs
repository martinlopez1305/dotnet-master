using System;
using System.Linq;
using System.Data.Entity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Text;

public class Program
{
    public static void Main()
    {
        using (var ctx = new DatabaseContext())
        {
            InitializeDB(ctx);

            //creo un listado para el reporte
            var data = OrganizeData(ctx);

            PrintReport(data);
            
            Console.ReadLine();
        }
    }
    public static List<ReportData> OrganizeData(DatabaseContext ctx)
    {
        List<ReportData> data = new List<ReportData>();

        //filtro los clientes que nacieron antes del 2000 y agrupo por cliente
        foreach (var costumer in ctx.Customers.Where(x => x.DateOfBirth.Year < 2000))
        {
            var purchases = ctx.Purchases.Where(x => x.CustomerId == costumer.CustomerId).ToList();
            data.Add(new ReportData(costumer, purchases));
        }
        return data;
    }

    public static void PrintReport(List<ReportData> data)
    {
        foreach (var row in data)
        {
            Console.WriteLine(DataFormatter.ToStringFullRow(row));
        }
    }
    public static void InitializeDB(DatabaseContext ctx)
    {
        if (ctx.Customers.Count() == 0)
        {
            ctx.Customers.Add(new Customer() { CustomerId = 1, FullName = "Sanchez Mario", DateOfBirth = new DateTime(1985, 10, 18) });
            ctx.Customers.Add(new Customer() { CustomerId = 2, FullName = "Gimenez Pedro", DateOfBirth = new DateTime(2010, 01, 10) });
            ctx.Customers.Add(new Customer() { CustomerId = 3, FullName = "Gomez Ricardo", DateOfBirth = new DateTime(1993, 11, 25) });
            ctx.Customers.Add(new Customer() { CustomerId = 4, FullName = "Araujo María", DateOfBirth = new DateTime(2009, 12, 2) });
    
            ctx.Purchases.Add(new Purchase() { PurchaseId = 1001, PurchaseDateUTC = new DateTime(2021, 2, 2, 15, 22, 35), Total = 255m, CustomerId = 1 });
            ctx.Purchases.Add(new Purchase() { PurchaseId = 1002, PurchaseDateUTC = new DateTime(2021, 2, 7, 12, 07, 45), Total = 888m, CustomerId = 3 });
            ctx.Purchases.Add(new Purchase() { PurchaseId = 1003, PurchaseDateUTC = new DateTime(2021, 2, 9, 9, 00, 10), Total = 672m, CustomerId = 1 });
            ctx.Purchases.Add(new Purchase() { PurchaseId = 1004, PurchaseDateUTC = new DateTime(2021, 1, 2, 10, 12, 32), Total = 1000m, CustomerId = 1 });
            ctx.Purchases.Add(new Purchase() { PurchaseId = 1005, PurchaseDateUTC = new DateTime(2021, 1, 4, 2, 25, 55), Total = 56m, CustomerId = 2 });
            ctx.Purchases.Add(new Purchase() { PurchaseId = 1006, PurchaseDateUTC = new DateTime(2021, 1, 7, 3, 12, 57), Total = 75m, CustomerId = 2 });
            ctx.Purchases.Add(new Purchase() { PurchaseId = 1007, PurchaseDateUTC = new DateTime(2021, 1, 12, 1, 17, 12), Total = 987m, CustomerId = 3 });
            ctx.Purchases.Add(new Purchase() { PurchaseId = 1008, PurchaseDateUTC = new DateTime(2021, 1, 15, 8, 55, 00), Total = 12000m, CustomerId = 3 });
            ctx.Purchases.Add(new Purchase() { PurchaseId = 1009, PurchaseDateUTC = new DateTime(2021, 1, 25, 10, 43, 10), Total = 1m, CustomerId = 3 });
            ctx.Purchases.Add(new Purchase() { PurchaseId = 1010, PurchaseDateUTC = new DateTime(2021, 2, 2, 17, 32, 22), Total = 100m, CustomerId = 4 });
            ctx.Purchases.Add(new Purchase() { PurchaseId = 1011, PurchaseDateUTC = new DateTime(2021, 2, 2, 15, 22, 35), Total = 256m, CustomerId = 1 });
            ctx.Purchases.Add(new Purchase() { PurchaseId = 1012, PurchaseDateUTC = new DateTime(2021, 2, 7, 12, 07, 45), Total = 887m, CustomerId = 3 });
            ctx.Purchases.Add(new Purchase() { PurchaseId = 1013, PurchaseDateUTC = new DateTime(2021, 2, 9, 9, 00, 10), Total = 673m, CustomerId = 1 });
            ctx.Purchases.Add(new Purchase() { PurchaseId = 1014, PurchaseDateUTC = new DateTime(2021, 1, 12, 1, 17, 12), Total = 987m, CustomerId = 3 });
            ctx.Purchases.Add(new Purchase() { PurchaseId = 1015, PurchaseDateUTC = new DateTime(2021, 1, 15, 8, 55, 00), Total = 12000m, CustomerId = 3 });
            ctx.Purchases.Add(new Purchase() { PurchaseId = 1016, PurchaseDateUTC = new DateTime(2021, 1, 25, 10, 43, 10), Total = 1m, CustomerId = 3 });
            ctx.Purchases.Add(new Purchase() { PurchaseId = 1017, PurchaseDateUTC = new DateTime(2021, 1, 25, 12, 43, 10), Total = 111m, CustomerId = 3 });
            ctx.Purchases.Add(new Purchase() { PurchaseId = 1018, PurchaseDateUTC = new DateTime(2021, 1, 25, 16, 43, 10), Total = 10m, CustomerId = 3 });
            ctx.SaveChanges();
        }

    }
}

public class Purchase
{
    public int PurchaseId { get; set; }
    public DateTime PurchaseDateUTC { get; set; }
    public Decimal Total { get; set; }
    public int CustomerId { get; set; }
    public virtual Customer Customer { get; set; }
}

public class Customer
{
    public int CustomerId { get; set; }
    public string FullName { get; set; }
    public DateTime DateOfBirth { get; set; }
}
//clase para organizar la data del reporte
public class ReportData
{
    public ReportData(Customer customer, List<Purchase> purchases)
    {
        Customer = customer;
        Purchases = purchases;
    }

    public Customer Customer { get; set; }
    public List<Purchase> Purchases { get; set; }
}
//clase para darle un formato al reporte en consola
public static class DataFormatter
{
    //metodo para cada fila del cliente
    public static string ToStringFullRow(ReportData row)
    {
        //
        StringBuilder response = new StringBuilder();

        //agrego la cabecera del reporte
        response.Append(NameAndAge(row.Customer.FullName, row.Customer.DateOfBirth));
        response.AppendLine();
        response.Append(Separator("=", 30));
        response.AppendLine();
        
        //agrego la compras realizadas ordenadas por fecha en orden descendiente
        foreach ( var purchase in row.Purchases.OrderByDescending(x => x.PurchaseDateUTC))
        {
            response.Append(PurchaseRow(purchase.PurchaseDateUTC, purchase.Total));
            response.AppendLine();
        }
        response.Append(Separator("=", 30));
        response.AppendLine();

        //agrego el total
        response.Append(TotalRow(row.Purchases.Select(x => x.Total).Sum()));
        response.AppendLine();

        return response.ToString();
    }
    public static string NameAndAge(string name, DateTime dateOfBirth)
    {
        int age = DateTime.Now.Year - dateOfBirth.Year;
        if (dateOfBirth > DateTime.Now.AddYears(-age)) age--;

        return $"{name}     (Edad: {age})";
    }
    public static string PurchaseRow(DateTime purchaseDate, Decimal total)
    {
        TimeZoneInfo t = TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time");
        string fecha = TimeZoneInfo.ConvertTimeFromUtc(purchaseDate, t).ToString("dd/MM/yyyy");
        string monto = string.Format("{0:0.##}", total);
        string s = Separator(" ", (10 - monto.Count()));
        monto = string.Concat(s, monto);

        return $"{fecha} -------$ {monto}";
    }

    public static string TotalRow(decimal total)
    {
        return $"TOTAL: ${string.Format("{0:0.##}", total)}";
    }
    public static string Separator(string s, int cant)
    {
        return string.Concat(Enumerable.Repeat(s, cant));
    } 
}


public class DatabaseContext : DbContext
{
    public DatabaseContext() : base()
    {

    }

    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<Customer> Customers { get; set; }
}
