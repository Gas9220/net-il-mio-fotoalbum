using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models
{
    public class Photo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title can't be empty")]
        [StringLength(20, ErrorMessage = "Max 20 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description can't be empty")]
        public string Description { get; set; }

        public byte[]? Image { get; set; }

        [Required(ErrorMessage = "IsVisible is required")]
        public bool IsVisible { get; set; }

        public List<Category>? Categories { get; set; }

        public Photo() { }

        public Photo(string title, string description, byte[] image, bool isVisible)
        {
            Title = title;
            Description = description;
            Image = image;
            IsVisible = isVisible;
        }
    }
}
