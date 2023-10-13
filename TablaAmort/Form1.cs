using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;


namespace TablaAmort
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void btnCalcular_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            if (Validacion.Validar(txtMontoPrestamo.Text))
            {
                string mensaje = "El campo Prestamo Anual está vacío";
                string title = "Advertencia";
                Mensaje.MostrarMensaje(mensaje, title);
                return;
            }


            if (Validacion.Validar(txtInteresAnual.Text))
            {
                string mensaje = "El campo Interes está vacío";
                string title = "Advertencia";
                Mensaje.MostrarMensaje(mensaje, title);
                return;
            }


            if (Validacion.Validar(txtTiempo.Text))
            {
                string mensaje = "El campo Tiempo está vacío";
                string title = "Advertencia";
                Mensaje.MostrarMensaje(mensaje, title);
                return;
            }



            //Instancia de la clase Calculos
            Calculos calculos = new Calculos();


            //Declaracion variable
            double tasaInteresMensual;
            double TasaInteresAnual;
            double MontoPrestamo;
            int tiempoEnMeses;
            double cuotas;
            double calculo;
            double tinteres = 0;
            double tcuota = 0;
            double saldoinicial = 0;
            double abonocapital = 0;
            double Ttotal = 0;

            //Asignacion de los txt a las variables
            TasaInteresAnual = Convert.ToDouble(txtInteresAnual.Text);
            tiempoEnMeses = Convert.ToInt32(txtTiempo.Text);
            MontoPrestamo = Convert.ToDouble(txtMontoPrestamo.Text);

            // Asignar los valores desde los cuadros de texto a las propiedades
            calculos.TasaInteresAnual = Convert.ToDouble(txtInteresAnual.Text);
            calculos.tiempoEnMeses = Convert.ToInt32(txtTiempo.Text);
            calculos.MontoPrestamo = Convert.ToDouble(txtMontoPrestamo.Text);
            calculos.saldofinal = calculos.MontoPrestamo;


            tasaInteresMensual = calculos.CalculoInteresMensual();
            txtinteresmensual.Text = tasaInteresMensual.ToString();

            //Calculo de las cuotas
            calculo = calculos.CalculoCuotas();

            //Calculo de cuotas
            cuotas = calculos.MontoPrestamo / calculo;
            txtpagocuotas.Text = cuotas.ToString();

            //Recorrido 
            for (int i = 1; i < tiempoEnMeses + 1; i++)
            {
                //calculo de los interes aplicado al mes
                double interes = Math.Round(calculos.saldofinal * tasaInteresMensual, 2);
                //acumulacion de los interes
                tinteres += interes;
                //saldo inicial = saldo final
                saldoinicial = calculos.saldofinal;
                //calcular el capital a pagar
                calculos.MontoPrestamo = Math.Round(cuotas - interes, 2);
                abonocapital += calculos.MontoPrestamo;
                calculos.saldofinal -= calculos.MontoPrestamo;
                tcuota += cuotas;

                dataGridView1.Rows.Add(i.ToString(), calculos.saldofinal.ToString("C2"), cuotas.ToString("C2"), calculos.MontoPrestamo.ToString("C2"), interes.ToString("C2"));
            }

            Ttotal = tinteres + abonocapital;

            //Actualizacion Label SUMARIO
            lblTotalInteres.Text = tinteres.ToString("C2");
            lblTotal.Text = abonocapital.ToString("C2");
            lblPagoFijo.Text = cuotas.ToString("C2");
            lblTtotal.Text = Ttotal.ToString("C2");

            btnImprimir.Enabled = true;

        }

        private void Limpiar()
        {
            txtInteresAnual.Clear();
            txtinteresmensual.Clear();
            txtMontoPrestamo.Clear();
            txtpagocuotas.Clear();
            txtTiempo.Clear();
            dataGridView1.Rows.Clear();
            lblPagoFijo.Text = "$0.00";
            lblTotal.Text = "$0.00";
            lblTotalInteres.Text = "$0.00";
        }
        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            // Crea un objeto PrintDocument
            PrintDocument doc = new PrintDocument();
            doc.DefaultPageSettings.Landscape = false;

            // Abre el diálogo de impresión para que el usuario seleccione la impresora
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                // Asigna la impresora seleccionada al objeto PrintDocument
                doc.PrinterSettings.PrinterName = printDialog.PrinterSettings.PrinterName;

                // Crea un objeto PrintPreviewDialog
                PrintPreviewDialog ppd = new PrintPreviewDialog { Document = doc };
                ((Form)ppd).WindowState = FormWindowState.Maximized;

                // Define el evento PrintPage
                doc.PrintPage += delegate (object ev, PrintPageEventArgs ep)
                {
                    const int DGV_ALTO = 35;
                    int left = ep.MarginBounds.Left;
                    int top = ep.MarginBounds.Top;

                    // TITULO
                    string titulo = "Tabla de Amortización"; // Cambia esto a tu título
                    ep.Graphics.DrawString(titulo, new Font("Segoe UI", 20, FontStyle.Bold), Brushes.Black, left, top);
                    top += 40; // Ajusta la posición vertical del título

                    // FOOTER
                    string textoFinal = "Claudio A. Ferreira"; // Reemplaza con tu propio texto
                    int margenInferior = 20; // Ajusta la distancia desde el borde inferior
                    int margenIzquierdo = 20; // Ajusta la distancia desde el borde izquierdo
                    float yTextoFinal = ep.MarginBounds.Bottom - margenInferior;
                    float xTextoFinal = ep.MarginBounds.Left + margenIzquierdo;

                    ep.Graphics.DrawString(textoFinal, new Font("Segoe UI", 8), Brushes.Black, xTextoFinal, yTextoFinal);

                    // Dibujar línea a la izquierda y en la parte superior
                    ep.Graphics.DrawLine(Pens.Gray, left, top, left, top + 43 + dataGridView1.RowCount * DGV_ALTO);
                    ep.Graphics.DrawLine(Pens.Gray, left, top, left + dataGridView1.Columns.Cast<DataGridViewColumn>().Sum(col => col.Width), top);

                    int lineLeft = ep.MarginBounds.Left;
                    int lineRight = left;
                    // COLUMNAS
                    foreach (DataGridViewColumn col in dataGridView1.Columns)
                    {
                        lineRight = lineLeft + col.Width; // Actualizar la posición del lado derecho de la columna
                        ep.Graphics.DrawString(col.HeaderText, new Font("Segoe UI", 16, FontStyle.Bold), Brushes.DeepSkyBlue, lineLeft, top);

                        if (col.Index < dataGridView1.ColumnCount - 1)
                        {
                            ep.Graphics.DrawLine(Pens.Gray, lineRight, top, lineRight, top + 43 + dataGridView1.RowCount * DGV_ALTO);
                        }
                        lineLeft = lineRight; // Mover el lado izquierdo de la columna al lado derecho para la siguiente columna
                    }

                    // Línea gruesa en la parte inferior de las columnas
                    ep.Graphics.FillRectangle(Brushes.Black, ep.MarginBounds.Left, top + 40, lineRight - ep.MarginBounds.Left, 3);

                    // Dibujar la línea vertical al final de la última columna
                    ep.Graphics.DrawLine(Pens.Gray, lineRight, top, lineRight, top + 43 + dataGridView1.RowCount * DGV_ALTO);

                    top += 43;

                    // FILAS
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Index == dataGridView1.RowCount) break;
                        left = ep.MarginBounds.Left;
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            ep.Graphics.DrawString(Convert.ToString(cell.Value), new Font("Segoe UI", 13), Brushes.Black, left, top + 4);
                            left += cell.OwningColumn.Width;
                        }
                        top += DGV_ALTO;

                        if (row.Index < dataGridView1.RowCount - 1)
                        {
                            ep.Graphics.DrawLine(Pens.Gray, ep.MarginBounds.Left, top, left, top);
                        }
                    }

                    // Dibujar línea en la parte inferior
                    ep.Graphics.DrawLine(Pens.Gray, ep.MarginBounds.Left, top, left, top);

                    // Ahora agregamos el contenido de las etiquetas
                    int labelsTop = top + 40; // Deja un espacio entre la tabla y el contenido de las etiquetas
                    top = ImprimirLabelsContent(ep, labelsTop);
                };

                // Muestra el cuadro de vista previa e impresión
                ppd.ShowDialog();
            }

        }

        private int ImprimirLabelsContent(PrintPageEventArgs ep, int top)
        {
            int left = ep.MarginBounds.Left;
            Font font = new Font("Segoe UI", 12);
            Brush brush = Brushes.Black;
            int cuadroAncho = 225; // Ancho del cuadro
            int cuadroAlto = 40; // Alto del cuadro (ajusta según tus necesidades)

            // Título "Sumario"
            string titulo = "Sumario";
            RectangleF tituloRect = new RectangleF(left, top, cuadroAncho, cuadroAlto);
            ep.Graphics.DrawString(titulo, new Font("Segoe UI", 12, FontStyle.Bold), brush, tituloRect);

            top += cuadroAlto; // Incrementar la posición vertical para el contenido

            // Definir un array de etiquetas y textos correspondientes
            string[] etiquetas = { "Pago Fijo:", "Total Interés:", "Total Capital:", "Total:" };
            string[] textos = { lblPagoFijo.Text, lblTotalInteres.Text, lblTotal.Text, lblTtotal.Text };

            for (int i = 0; i < etiquetas.Length; i++)
            {
                // Dibujar el cuadro para la etiqueta
                ep.Graphics.DrawRectangle(Pens.Black, left, top, cuadroAncho / 2, cuadroAlto);

                // Imprimir la etiqueta en el cuadro izquierdo
                string labelText = etiquetas[i];
                RectangleF labelRect = new RectangleF(left, top, cuadroAncho / 2, cuadroAlto);
                ep.Graphics.DrawString(labelText, font, brush, labelRect);

                // Dibujar el cuadro para el valor
                ep.Graphics.DrawRectangle(Pens.Black, left + cuadroAncho / 2, top, cuadroAncho / 2, cuadroAlto);

                // Imprimir el valor en el cuadro derecho
                string montoText = textos[i];
                RectangleF montoRect = new RectangleF(left + cuadroAncho / 2, top, cuadroAncho / 2, cuadroAlto);
                ep.Graphics.DrawString(montoText, font, brush, montoRect);

                top += cuadroAlto; // Incrementar la posición vertical
            }

            return top;
        }

    }
}