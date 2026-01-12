using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notification_services.application.DTOs
{
    public class EnviarCorreoReservaExitosaDto
    {
        public string Destinatario { get; set; }
        public string CantidadTickets { get; set; }
        public string NombreEvento { get; set; }
        public string IdReserva { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
