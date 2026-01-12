using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notification_services.domain.ValueObjects
{
    public class AsuntoNotificacionVO
    {
        public string asuntoNotificacion { get; set; }

        public AsuntoNotificacionVO(string asuntoNotificacion)
        {
            this.asuntoNotificacion = asuntoNotificacion;
        }
    }
}
