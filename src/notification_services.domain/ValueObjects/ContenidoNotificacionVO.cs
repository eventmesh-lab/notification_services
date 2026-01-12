using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notification_services.domain.ValueObjects
{
    public class ContenidoNotificacionVO
    {
        public string contenidoNotificacion { get; set; }

        public ContenidoNotificacionVO(string contenidoNotificacion)
        {
            this.contenidoNotificacion = contenidoNotificacion;
        }
    }
}
