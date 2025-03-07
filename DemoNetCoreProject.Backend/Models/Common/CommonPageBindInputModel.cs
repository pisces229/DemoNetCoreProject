using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DemoNetCoreProject.Backend.Models.Common
{
    public class CommonPageBindInputModel
    {
        [BindProperty(Name = "pageSize")]
        [Required]
        public int PageSize { get; set; }
        [BindProperty(Name = "pageNo")]
        [Required]
        public int PageNo { get; set; }
    }
}
