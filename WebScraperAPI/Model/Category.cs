using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebScraperAPI.Model
{
    public class Category
    {
        public Category()
        {
            Categories= new HashSet<Category>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryLink { get; set; }
        public int? CategoryLevel { get; set; }
        public List<Filter>? Filters { get; set; }
        public int? ParentId { get; set; }
        public virtual Category? ParentCategory { get; set; }
        public virtual  ICollection<Category> Categories{ get; set; }


    }
}
