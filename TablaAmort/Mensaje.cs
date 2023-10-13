using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TablaAmort
{
    public class Mensaje
    {
        public static void MostrarMensaje(string mensaje, string title)
        {
            MessageBox.Show(mensaje, title, MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
    }

    
}
