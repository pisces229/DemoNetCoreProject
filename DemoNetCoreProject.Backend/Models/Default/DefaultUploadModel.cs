using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DemoNetCoreProject.Backend.Models.Default
{
    public class DefaultUploadModel
    {
        [BindProperty(Name = "File")]
        [Required]
        public IFormFile File { get; set; } = null!;
        [BindProperty(Name = "Name")]
        [Required]
        public string Name { get; set; } = null!;
    }
}
