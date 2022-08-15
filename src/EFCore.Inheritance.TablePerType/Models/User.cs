namespace EFCore.Inheritance.TablePerType.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public int BillingDetailId { get; set; }
    public virtual BillingDetail BillingInfo { get; set; }
}
