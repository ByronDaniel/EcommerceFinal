using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Ecommerce.Domain.Entities
{
    public class BaseEntity
    {
        public DateTime DateCreation { get; set; } = DateTime.Now;
        public DateTime DateModification { get; set; }
        public DateTime DateDeleted { get; set; }
        public bool Status { get; set; } = true;
    }
}
