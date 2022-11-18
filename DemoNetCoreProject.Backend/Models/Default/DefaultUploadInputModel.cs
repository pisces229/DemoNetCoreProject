using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DemoNetCoreProject.Backend.Models.Default
{
    public class DefaultUploadInputModel
    {
        [BindProperty(Name = "file")]
        [Required]
        public IFormFile File { get; set; } = null!;
        [BindProperty(Name = "name")]
        [Required]
        public string Name { get; set; } = null!;
    }
}
