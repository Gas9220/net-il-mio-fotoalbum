using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title can't be empty")]
        [StringLength(20, ErrorMessage = "Max 20 characters")]
        public string Name { get; set; }

        public List<Photo> Photos { get; set; }

        public Category() { }

        public Category(string name)
        {
            Name = name;
        }
    }
}
