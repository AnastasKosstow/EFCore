using EFCore.Inheritance.TablePerType;
using EFCore.Inheritance.TablePerType.Models;
using EFCore.Web.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Web.Services;

public class TablePerTypeService : ITablePerTypeService
{
    private readonly TablePerTypeDbContext _context;

    public TablePerTypeService(TablePerTypeDbContext context)
    {
        _context = context;
    }

    public async Task<IResponse> GetResult(CancellationToken cancellationToken)
    {
        if (!_context.Users.Any())
        {
            AddInitialData();
        }

        // To get all by Type: _context.Users.OfType<BankAccount>()

        var bankAccount = (await _context
            .Users?
            .FirstOrDefaultAsync(cancellationToken))
            .BillingInfo as BankAccount;

        return new TablePerTypeResponse($"" +
            $"{bankAccount.BankName} - " +
            $"{bankAccount.Number} - " +
            $"{bankAccount.Owner}");
    }

    #region InitialData
    private void AddInitialData()
    {
        _context.Users.Add(new User
        {
            FirstName = "A",
            LastName = "K",
            BillingInfo = new BankAccount()
            {
                Number = "9876543210",
                Owner = "A/K",
                BankName = "theBank",
            },
        });
    }
    #endregion
}
