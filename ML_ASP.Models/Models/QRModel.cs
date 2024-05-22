using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.Models.Models
{
    public class QRModel
    {
        [Key]
        public int Id { get; set; }

        public string? QrCode {  get; set; }
    }
}
