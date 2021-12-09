using Microsoft.EntityFrameworkCore;
using PowerTools.Models.Helpers;
using System.ComponentModel.DataAnnotations;

namespace PowerTools.Models;

public class Product : BindableBase
{
    private int id;
    private string? productName;
    private string? productCode;
    private string? search;
    private decimal price;
    private string? imageUrl;
    private string? contentDetails;
    private string? supplierName;
    private string? supplierIcon;
    private bool isUptodate = false;
    

    public int Id { get => id; set => SetProperty(ref id, value); }
    [Required]
    public string? ProductName { get => productName; set => SetProperty(ref productName, value); }
    [Required]
    public string? ProductCode { get => productCode; set => SetProperty(ref productCode, value); }
    public string? Search { get => search; set => SetProperty(ref search, value); }
    [Precision(14, 2)]
    public decimal Price { get => price; set => SetProperty(ref price, value); }
    public string? ImageUrl { get => imageUrl; set => SetProperty(ref imageUrl, value); }
    public string? ContentDetails { get => contentDetails; set => SetProperty(ref contentDetails, value); }
    public string? SupplierName { get => supplierName; set => SetProperty(ref supplierName, value); }
    public string? SupplierIcon { get => supplierIcon; set => SetProperty(ref supplierIcon, value); }
    public bool IsUptodate { get => isUptodate; set => SetProperty(ref isUptodate, value); }


}
