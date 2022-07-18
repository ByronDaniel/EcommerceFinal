using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Ecommerce.Domain.Entities
{
    public enum Status
    {
        Pendiente,
        Cancelado,
        Pagado,
        Rembolsado,
        Eliminado,
        Vigente
    }
}
