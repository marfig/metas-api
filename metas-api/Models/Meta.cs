using System;

namespace metas_api.Models
{
    public class Meta
    {
        public string Id { get; set; }
        public string ApplicationId { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Notas { get; set; }
        public decimal Porcentaje { get; set; }
        public string ParentId { get; set; }
        public DateTime FechaAdd { get; set; }
        public DateTime? FechaDelete { get; set; }
    }
}