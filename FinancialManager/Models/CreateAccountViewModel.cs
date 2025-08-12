using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FinancialManager.Models
{
    public class CreateAccountViewModel : AccountViewModel
    {
        public IEnumerable<SelectListItem> AccountTypes { get; set; }
    }
}
