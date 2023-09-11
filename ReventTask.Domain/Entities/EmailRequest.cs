using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReventTask.Domain.Entities
{
    public class EmailRequest
    {
        public string EmailId = Guid.NewGuid().ToString();
        public string RecepientEmail { get; set; }
        public DateTime SentTime { get; set; }  = DateTime.Now;
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; } = new List<IFormFile>();

    }
}
