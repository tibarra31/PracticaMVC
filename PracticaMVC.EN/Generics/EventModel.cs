using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaMVC.EN
{
    public class EventModel
    {
        public int IdFechaCalendario { get; set; }
        public int IdUsuario { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Fecha { get; set; }
    }

    public class EventModelDate
    {
        public DateTime Fecha { get; set; }
    }
}
