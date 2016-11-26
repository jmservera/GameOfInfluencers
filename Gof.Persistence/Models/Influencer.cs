using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gof.Persistence.Service.Models
{
    public class Influencer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Index(IsUnique =true)]
        [StringLength(450)]
        public string ScreenName { get; set; }
    }
}
