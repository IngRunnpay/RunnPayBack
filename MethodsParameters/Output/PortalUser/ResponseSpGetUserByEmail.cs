using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MethodsParameters.Output.PortalUser
{
    public class ResponseSpGetUserByEmail
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int TipoDocumento { get; set; }
        public string Documento {  get; set; }
        public DateTime FechaCreacion {  get; set; }
        public int IdEstadoUsuario { get; set; }
        public int IdPais { get; set; }
        public int IdCiudad {  get; set; }
        public string Celular {  get; set; }
        public string Correo { get; set; }
        public Guid IdAplicacion { get; set; }
        public string NombrePT { get; set; }
        public string NIT { get; set; }

    }
}
