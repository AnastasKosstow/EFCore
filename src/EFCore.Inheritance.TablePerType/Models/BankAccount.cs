namespace EFCore.Inheritance.TablePerType.Models;

public record BankAccount : BillingDetail
{
    public string BankName { get; set; }
}