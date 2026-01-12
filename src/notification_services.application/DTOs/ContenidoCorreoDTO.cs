using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notification_services.application.DTOs
{
    public class ContenidoCorreoDTO
    {
        /// <summary>
        /// Atributo que corresponde al asunto del correo a enviar.
        /// </summary>
        public string Asunto { get; set; }
        /// <summary>
        /// Atributo que corresponde al contenido (En HTML) del correo a enviar.
        /// </summary>
        public string Html { get; set; }

    }
}
