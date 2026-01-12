using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notification_services.domain.ValueObjects
{
    public class DestinatarioNotificacionVO
    {
        public string destinatarioNotificacion { get; set; }

        public DestinatarioNotificacionVO(string destinatarioNotificacion)
        {
            this.destinatarioNotificacion = destinatarioNotificacion;
        }
    }
}
