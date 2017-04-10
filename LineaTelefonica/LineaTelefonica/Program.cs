using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LineaTelefonica.Entities;

namespace LineaTelefonica
{
    class Program
    {
        static void Main(string[] args)
        {
            //generamos 10 numeros aleatorios para trabajar con ellos
            List<int>listaNumeros = new List<int>();
            List<LineaTelefonica.Entities.LineaTelefonica> listaLineas = new List<LineaTelefonica.Entities.LineaTelefonica>();
            List<Venta> listaVentas = new List<Venta>();
            List<TipoEvaluacion> listaTipoEvaluacion = new List<TipoEvaluacion>();
            List<Evaluacion> listaEvaluacion = new List<Evaluacion>();
            List<TipoPlan> listatipoPlan = new List<TipoPlan>();
            List<TipoPago> listatipoPago = new List<TipoPago>();

            RandomNumber r = new RandomNumber();
            Random random= new Random();
            int numeroBase = r.getrandom();
            int numlineas = 40;
            int numlineasPostpago = random.Next(numlineas/2, numlineas); //dado que nos piden minimo 2 evaluaciones desaprobadas, necesitamos como minimo 2 lineas postpago
            int numlineasPrepago = numlineas - numlineasPostpago;

            for (int i = 0; i < numlineas; i++)
            {
                listaNumeros.Add(numeroBase);
                numeroBase++;
            }

            //creamos a un trabajador
            Trabajador trabajador = new Trabajador(TipoTrabajador.vendedor, "Sebastian", "Romero", "sromero", "2009104490");
            Cliente cliente = new Cliente("10231322", "Eduardo", "Gomero", "923121892", "egomeroc@usmp.pe", "direccion");

            for (int i = 0; i < numlineasPostpago; i++)
            {
                var linea= new LineaTelefonica.Entities.LineaTelefonica();
                linea.nroCelular = listaNumeros[i].ToString();
                linea.tipolinea = TipoLinea.postPago;
                listaLineas.Add(linea);
            }
            for (int i = numlineasPostpago; i < numlineas; i++)
            {
                var linea = new LineaTelefonica.Entities.LineaTelefonica();
                linea.nroCelular = listaNumeros[i].ToString();
                linea.tipolinea = TipoLinea.prepago;
                listaLineas.Add(linea);
            }

            //verificamos que se hayan creado correctamente las lineas
            //for (int i = 0; i < numlineas; i++)
            //{
            //    Console.WriteLine(listaLineas[i].nroCelular + "  " +listaLineas[i].tipolinea);
            //}

            //creamos una lista de Planes que seran usadas para construir las ventas, todo construido aleatoriamente

            for (int j = 0; j < numlineas; j++)
            {
                if (j < numlineas / 5) { listatipoPlan.Add(TipoPlan.control_35); }
                else if (j < numlineas / 3) { listatipoPlan.Add(TipoPlan.Premium_125); }
                else if (j < numlineas / 2) { listatipoPlan.Add(TipoPlan.control_45); }
                else { listatipoPlan.Add(TipoPlan.Libre_105); }
            }

            //lista de Evaluaciones tambien construido aleatoriamente
            for (int j = 0; j < numlineas; j++)
            {
                if ((j % 2) == 0)
                { listatipoPago.Add(TipoPago.contado); }
                else { listatipoPago.Add(TipoPago.credito); }
            }

            //tipo de pago, generado aleatoriamente

            for (int j = 0; j < numlineas; j++)
            {
                if (j < numlineas / 5) { listaTipoEvaluacion.Add(TipoEvaluacion.lineaNueva); }
                else if (j < numlineas / 3) { listaTipoEvaluacion.Add(TipoEvaluacion.portabilidad); }
                else { listaTipoEvaluacion.Add(TipoEvaluacion.renovacionContrato); }
            }

            //verificamos que se hayan creado aleatoriamente los tipos de evaluaciones para asignar
            //for (int i = 0; i < listaTipoEvaluacion.Count; i++)
            //{
            //    Console.WriteLine(listaTipoEvaluacion[i].ToString());
            //}


                for (int i = 0; i < numlineas; i++)
                {
                    if (listaLineas[i].tipolinea == TipoLinea.postPago)
                    {
                        Evaluacion evaluacion = new Evaluacion();
                        //asignamos tipoevaluacion
                        evaluacion.tipoevaluacion = listaTipoEvaluacion[i];
                        evaluacion.lineaTelefonica = listaLineas[i];
                        evaluacion.plan = new Plan(listatipoPlan[i],"Hasta cumplir Contrato");
                        evaluacion.trabajador = trabajador;
                        evaluacion.cliente=cliente;
                        //y si esta aprobada o no, lo consideramos aleatorio si es par o impar
                        if ((i % 2) == 0)
                        {
                            //si la evaluacion se considera aprobada, se hace la venta, asi que añadimos la informacion a una lista
                            //de ventas para mostrarse luego
                            evaluacion.estado = EstadoEvaluacion.aprobado;
                            Venta venta = new Venta();
                            venta.evaluacion=evaluacion;
                            venta.tipopago = listatipoPago[i];
                            venta.lineaTelefonica = evaluacion.lineaTelefonica;
                            listaVentas.Add(venta);
                        
                        }
                        else
                        {
                            evaluacion.estado = EstadoEvaluacion.desaprobado;
                        }

                        listaEvaluacion.Add(evaluacion);
                    }
                    else
                    {
                        Venta venta = new Venta();
                        venta.lineaTelefonica = listaLineas[i];
                        venta.tipopago = listatipoPago[i];
                        listaVentas.Add(venta);
                        
                    }

                }


            //Reportes

            //reporte de evaluaciones aprobadas por cada tipo
                Console.WriteLine("Reporte de Evaluaciones Aprobadas");
                Console.WriteLine("NroEvaluacion".PadRight(15) + "LineaTelefonica".PadRight(18) + "TipoEvaluacion".PadRight(22) + "EstadoEvaluacion".PadRight(15));
                int num = 1;
                for (int i = 0; i < listaEvaluacion.Count; i++)
                {
                    if (listaEvaluacion[i].estado == EstadoEvaluacion.aprobado)
                    {
                        Console.WriteLine((num).ToString().PadRight(15) + listaEvaluacion[i].lineaTelefonica.nroCelular.PadRight(18) + listaEvaluacion[i].tipoevaluacion.ToString().PadRight(22) + listaEvaluacion[i].estado.ToString().PadRight(15));
                        num++;
                    }
                }

            //reporte de Evaluaciones no Aprobadas
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("Reporte de Evaluaciones No Aprobadas");
                Console.WriteLine("NroEvaluacion".PadRight(15) + "LineaTelefonica".PadRight(18) + "TipoEvaluacion".PadRight(22) + "EstadoEvaluacion".PadRight(15));
                num = 1;
                for (int i = 0; i < listaEvaluacion.Count; i++)
                {
                    if (listaEvaluacion[i].estado == EstadoEvaluacion.desaprobado)
                    {
                        Console.WriteLine((num).ToString().PadRight(15) + listaEvaluacion[i].lineaTelefonica.nroCelular.PadRight(18) + listaEvaluacion[i].tipoevaluacion.ToString().PadRight(22) + listaEvaluacion[i].estado.ToString().PadRight(15));
                        num++;
                    }
                }


                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("Reporte de Total Lineas Vendidas " + listaVentas.Count);
                Console.WriteLine("NroLinea".PadRight(10) + "LineaTelefonica".PadRight(18) + "TipoLinea".PadRight(17) + "TipoPago".PadRight(15));
                num = 1;

                for (int i = 0; i < listaVentas.Count; i++)
                {
                    Console.WriteLine((num).ToString().PadRight(10) + listaVentas[i].lineaTelefonica.nroCelular.PadRight(18) + listaVentas[i].lineaTelefonica.tipolinea.ToString().PadRight(17) + listaVentas[i].tipopago.ToString().PadRight(15));
                    num++;
                }




        }
    }
}
