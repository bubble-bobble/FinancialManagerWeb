using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace FinancialManager.Models;

public class AccountTypeViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    [Remote(controller: "AccountTypes", action: "ValidateAccountTypeName")]
    public string Name { get; set; }

    public int Sequence { get; set; }

    public int UserId { get; set; }
}