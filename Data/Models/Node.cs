using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace TestTask.Data.Models
{
    public class Node
    {
        [BindNever]
        public long Id { get; set; }
        [Display(Name = "Enter your title: ")]
        [MaxLength(20)]
        [MinLength(3)]
        [Required(ErrorMessage = "Max lenght is 20 and min length is 3.")]
        [DataType(DataType.Text)]
        public string Title { get; set; }
        [BindNever]
        public long? parentId { get; set; }
        [BindNever]
        public bool IsExpanded { get; set; }
    }
}
