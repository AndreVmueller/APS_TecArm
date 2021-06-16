using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Collections;

namespace APS_TecArm
{
    class Program
    {
        static void Main(string[] args)
        {
            bool loop = true;
            while (loop)
            {
                Logs.logs = new List<string>();

                Typerwriter.PrintMenu();

                if (Typerwriter.Algoritmo <= 3
                    && Typerwriter.ValorMinimo < Typerwriter.ValorMaximo
                    && Typerwriter.TamanhoLista < Typerwriter.ValorMaximo)
                {
                    var t = new Timer();
                    var data = RandData.DataGenerator(Typerwriter.TamanhoLista, Typerwriter.ValorMinimo, Typerwriter.ValorMaximo);

                    Logs.Log("TamanhoLista: " + Typerwriter.TamanhoLista.ToString());
                    Logs.Log("ValorMinimo: " + Typerwriter.ValorMinimo.ToString());
                    Logs.Log("ValorMaximo: " + Typerwriter.ValorMaximo.ToString());

                    Logs.Log(String.Format("Entrada: {0}.", string.Join(", ", data)));
                    Logs.Log(String.Format("Início ordenação: {0}", t.Init()));
                    var result = Algorithms.Selecionar(Typerwriter.Algoritmo, data);
                    Logs.Log(String.Format("Fim ordenação: {0} segundos.", t.End()));

                    var formated = string.Join(", ", result);

                    Logs.Log(String.Format("Saída: {0}.", formated));

                    IOFiles.Write(Logs.LstLogs() , Algorithms.Nome);

                    Console.WriteLine("\n****FIM*****\n");
  
                }else {
                    Console.WriteLine("\nDitite outros valores. \nREGRAS\n");
                    Console.WriteLine("> Algoritmo <= 3");
                    Console.WriteLine("> ValorMinimo < ValorMaximo");
                    Console.WriteLine("> TamanhoLista < ValorMaximo");
                }

                Console.WriteLine("\nDigite 0 para Finalizar.\nDigite 1 para continuar. ");

                loop = Typerwriter.Read() == 1;

            }
        }
    }

    public static class Algorithms
    {
        public static string Nome {get;set;}

        public static int[] Selecionar(int Algoritmo, int[] A)
        {

            switch (Algoritmo)
            {
                case 1:

                    A = InsertionSort(A);
                    Nome = " InsertionSort";
                    break;
                case 2:

                    A = CountSort(A);
                    Nome = " CountSort";
                    break;
                case 3:

                    A = BucketSort(A);
                    Nome = " BucketSort";
                    break;
                default:

                    Console.Write("Implementação não finalizada!");

                    break;
            }
            return A;
        }


        public static int[] CountSort(int[] A)
        {

            int min = A.Min();
            int range = A.Max() - min + 1;
            int[] C = new int[range];
            int[] B = new int[A.Length];

            for (int i = 0; i < A.Length; i++)
                C[A[i] - min]++;

            for (int i = 1; i < C.Length; i++)
                C[i] += C[i - 1];

            for (int i = A.Length - 1; i >= 0; i--)
            {
                B[C[A[i] - min] - 1] = A[i];
                C[A[i] - min]--;
            }
            for (int i = 0; i < A.Length; i++)
                A[i] = B[i];


            return A;
        }
        // D = A.Max().ToString().Length;
        public static int[] RadixSort(int[] A, int D)
        {
            if (D == 0)
                return A;
            //bublesort ...
            for (int j = 0; j < A.Length; j++)
            {
                int a = A[j] % D;
                int b = A[j + 1] % D;
                if (a > b)
                {
                    a = A[j];
                    A[j] = A[j + 1];
                    A[j + 1] = a;
                }
            }
            return RadixSort(A, D - 1);
        }

        public static int[] BucketSort(int[] A)
        {
            int size = A.Length;
            int[][] B = new int[size][];
            int max = int.Parse("1" + "".PadRight(A.Max().ToString().Length, '0'));

            for (int i = 0; i < size; i++)
            {
                B[i] = new int[size];
            }

            for (int i = 0; i < size; i++)
            {
                int indexB = (A[i] * size) / max;
                int indeBB = 0;

                for (int j = 0; j < size; j++)
                {
                    if (B[indexB][j] == 0)
                    {
                        indeBB = j;
                        break;
                    }
                }
                B[indexB][indeBB] = A[i];
            }

            for (int i = 0; i < size; i++)
                InsertionSort(B[i]);

            var myAL = new ArrayList();
            for (int i = 0; i < size; i++)
                myAL.AddRange(B[i].AsQueryable().Where(x => x > 0).ToArray());

            return (int[])myAL.ToArray(typeof(int));
        }

        public static int[] InsertionSort(int[] A)
        {
            for (int j = 1; j < A.Length; j++)
            {
                int key = A[j];
                int i = j - 1;

                while (i >= 0 && A[i] > key)
                {
                    A[i + 1] = A[i];
                    i = i - 1;
                }
                A[i + 1] = key;
            }

            return A;
        }

    }

    public static class Typerwriter
    {
        public static int Algoritmo { get; set; }
        public static int TipoEntrada { get; set; }
        public static int TamanhoLista { get; set; }
        public static int ValorMinimo { get; set; }
        public static int ValorMaximo { get; set; }

        public static void PrintMenu()
        {

            Console.WriteLine("SELECIONE O ALGORITMO DE ORDENAÇÃO E FORMA DE ENTRADA DE DADOS.");
            Console.WriteLine("SELCIONE o ALGORITMO");
            Console.WriteLine("ALGORITMO 1");
            Console.WriteLine("- InsertionSort");
            Console.WriteLine("ALGORITMO 2");
            Console.WriteLine("- CountSort");
            Console.WriteLine("ALGORITMO 3");
            Console.WriteLine("- BucketSort");
            Console.WriteLine("ALGORITMO 4");
            Console.WriteLine("- RadixSort");
            Algoritmo = Read();

            Console.WriteLine("TIPO DE ENTRADA (não implementado).");
            Console.WriteLine("ENTRADA 1");
            //Console.WriteLine("ENTRADA 2");
            //Console.WriteLine("ENTRADA 3");
            //TipoEntrada = Read();

            Console.WriteLine("SELIONE O TAMANHO DA LISTA:");
            TamanhoLista = Read();

            Console.WriteLine("VALOR MÍNIMO");
            ValorMinimo = Read();

            Console.WriteLine("VALOR MÁXIMO");
            ValorMaximo = Read();

        }

        public static int Read()
        {
            string read = Console.ReadLine().ToString();
            int iRead = 0;
            int.TryParse(read, out iRead);

            return iRead;
        }

    }


    public static class Logs
    {
        public static List<String> logs = new List<string>();

        public static void Log(string line)
        {
            Console.WriteLine(line);
            logs.Add(line);
        }

        public static List<string> LstLogs()
        {
            return logs;
        }
    }

    public static class IOFiles
    {

        public static void Read(string path_file)
        {
            String line;
            try
            {
                var sr = new StreamReader(path_file);
                line = sr.ReadLine();
                while (line != null)
                {
                    Console.WriteLine(line);
                    line = sr.ReadLine();
                }
                sr.Close();
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("");
            }
        }

        public static void Write(List<string> dados, string nome)
        {
            try
            {
                nome = string.Format("{0}.txt", nome + "_" + DateTime.Now.ToString("yyyyMMddhhmmss"));
                var sw = new StreamWriter(nome);

                foreach (var dado in dados)
                {
                    sw.WriteLine(dado);
                }
                
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Arquivo Salvo: " + nome);
            }
        }
    }

    public static class RandData
    {
        public static int[] DataGenerator(int size, int min_value, int max_value)
        {
            Console.WriteLine("Aguarde. Os dados estão sendo gerados para ordenação.");
            var r = new Random();
            var result = new ArrayList();
            if (size < max_value - min_value)
            {
                int conta = 0;
                while (result.Count <= size && conta <= size * 2)
                {
                    int number = r.Next(min_value, max_value);
                    if (!result.Contains(number))
                        result.Add(number);
                    conta++;
                }
            }
            Console.WriteLine("Dados gerados.");
            return (int[])result.ToArray(typeof(int));
        }
    }


    public class Timer
    {
        private Stopwatch timer = new Stopwatch();
        public string TimeExec = "0";

        public string Init()
        {
            TimeExec = "0";
            timer.Start();
            return TimeExec;
        }

        public string End()
        {
            timer.Stop();
            TimeExec = timer.Elapsed.Seconds.ToString();
            timer.Reset();
            return TimeExec;
        }
    }

}


