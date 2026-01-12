using notification_services.domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notification_services.domain.Entities
{
    public class Notification
    {
            /// <summary>
            /// Atributo que corresponde al destinatario de la notificación a enviar.
            /// </summary>
            public DestinatarioNotificacionVO destinatarioNotificacion { get; set; }

            /// <summary>
            /// Atributo que corresponde al contenido de la notificación a enviar.
            /// </summary>
            public ContenidoNotificacionVO contenidoNotificacion { get; set; }

            /// <summary>
            /// Atributo que corresponde al asunto de la notificación a enviar.
            /// </summary>
            public AsuntoNotificacionVO asuntoNotificacion { get; set; }

            public Notification(DestinatarioNotificacionVO destinatario, ContenidoNotificacionVO contenido,
                AsuntoNotificacionVO asunto)
            {
                destinatarioNotificacion = destinatario;
                contenidoNotificacion = contenido;
                asuntoNotificacion = asunto;
            }

        
    }
}
