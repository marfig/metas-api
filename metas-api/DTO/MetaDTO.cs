using System;

namespace metas_api.DTO
{
    public class MetaDTO
    {
        public string Id { get; set; }
        public string Parentid { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Notas { get; set; }
        public decimal Porcentaje { get; set; }
        public int Eliminado { get; set; }
        public int Sincronizado { get; set; }
        public DateTime FechaAdd { get; set; }
    }
}
