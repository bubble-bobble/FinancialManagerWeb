using System.Collections.Generic;
using System.Linq;

namespace FinancialManager.Models
{
    public class IndexAccountViewModel
    {
        public string AccountType { get; set; }

        public IEnumerable<AccountViewModel> Accounts { get; set; }

        public decimal Balance => Accounts.Sum(x => x.Balance);
    }
}
