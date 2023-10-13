using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TablaAmort
{
    public static class Validacion
    {
        public static bool Validar(string texto)
        {
            return string.IsNullOrWhiteSpace(texto);
        }


    }
}
