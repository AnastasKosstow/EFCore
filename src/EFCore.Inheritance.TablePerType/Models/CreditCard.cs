namespace EFCore.Inheritance.TablePerType.Models;

public record CreditCard : BillingDetail
{
    public string CardType { get; set; }
}