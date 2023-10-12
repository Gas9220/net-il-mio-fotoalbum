using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public string Sender { get; set; }
        [Required]
        public string Content { get; set; }

        public int? PhotoId { get; set; }
        public Photo? Photo { get; set; }

        public Message() { }
        public Message(string sender, string content)
        {
            Sender = sender;
            Content = content;
        }
    }
}
