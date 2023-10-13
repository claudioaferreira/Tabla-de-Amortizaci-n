using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TablaAmort
{
    public class Calculos
    {

        public double tasaInteresMensual { get; set; }
        public double TasaInteresAnual { get; set; }
        public double TotalInteres { get; set; }
        public int tiempoEnMeses { get; set; }
        public double MontoPrestamo { get; set; }
        public double cuotas { get; set; }
        public double calculo { get; set; }
        public double resultado { get; set; }
        public double saldofinal { get; set; }

        public Calculos()
        {

        }


        public double CalculoInteresMensual()
        {
            return tasaInteresMensual = (TasaInteresAnual / 12) / 100;
        }

        public void CalculoTotalIntereses()
        {
            CalculoInteresMensual();
            TotalInteres = (MontoPrestamo * tasaInteresMensual * tiempoEnMeses) - MontoPrestamo;
        }

        public double CalculoCuotas()
        {
            calculo = (1 - Math.Pow(1 + tasaInteresMensual, tiempoEnMeses * -1)) / tasaInteresMensual;

            return calculo;
        }

        public double CalculoCuotas(double calculo, double MontoPrestamo)
        {
            cuotas = MontoPrestamo / calculo;
            return cuotas;
        }
    }
}
