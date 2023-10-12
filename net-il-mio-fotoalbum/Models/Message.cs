using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Email field required")]
        [EmailAddress(ErrorMessage = "Wrong format email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Message can't be empty")]
        public string Content { get; set; }

        public Message() { }
        public Message(string email, string content)
        {
            Email = email;
            Content = content;
        }
    }
}
