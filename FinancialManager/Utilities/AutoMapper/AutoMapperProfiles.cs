using AutoMapper;
using FinancialManager.Models;

namespace FinancialManager.Utilities.AutoMapper;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AccountViewModel, CreateAccountViewModel>();
    }
}
