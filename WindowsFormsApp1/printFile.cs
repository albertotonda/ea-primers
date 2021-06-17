using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WindowsFormsApp1
{
    class printFile
    {
        /// <summary>
        /// Print Matrix to a file.
        /// </summary>
        /// <param name="matrix">matrix to print</param>
        /// <param name="name">name of the file</param>
        public static void printMatrix(int[,] matrix, string name)
        {

            TextWriter tw = new StreamWriter(name + ".csv");
            //tw.WriteLine(matrix.GetLength(0).ToString());
            //tw.WriteLine(matrix.GetLength(1).ToString());

            for (int i = 0; i < matrix.GetLength(0); i++)
            {


                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (j != matrix.GetLength(1) - 1)
                        tw.Write(matrix[i, j] + ",");
                    else
                        tw.Write(matrix[i, j]);
                }

                tw.WriteLine();
            }

            tw.Close();

        }
        /// <summary>
        /// Print Matrix to a file.
        /// </summary>
        /// <param name="matrix">matrix to print</param>
        /// <param name="name">name of the file</param>
        public static void printMatrix(double[,] matrix, string name)
        {

            TextWriter tw = new StreamWriter(name + ".csv");
            //tw.WriteLine(matrix.GetLength(0).ToString());
            //tw.WriteLine(matrix.GetLength(1).ToString());



            for (int i = 0; i < matrix.GetLength(0); i++)
            {

                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (j != matrix.GetLength(1) - 1)
                        tw.Write(matrix[i, j] + ",");
                    else
                        tw.Write(matrix[i, j]);
                }

                tw.WriteLine();
            }

            tw.Close();

        }
        /// <summary>
        /// Print a matrix file without name , using the date as name.
        /// </summary>
        /// <param name="matrix">matrix to print</param>
        public static void printMatrix(double[,] matrix)
        {
            string datetimeString = string.Format("{0:yyyy-MM-dd_hh-mm-ss-fffff-tt}.matrix", DateTime.Now);
            TextWriter tw = new StreamWriter(datetimeString + ".csv");
            //tw.WriteLine(matrix.GetLength(0).ToString());
            //tw.WriteLine(matrix.GetLength(1).ToString());

            for (int i = 0; i < matrix.GetLength(0); i++)
            {

                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (j != matrix.GetLength(1) - 1)
                        tw.Write(matrix[i, j] + ",");
                    else
                        tw.Write(matrix[i, j]);
                }

                tw.WriteLine();
            }

            tw.Close();

        }
        /// <summary>
        /// Print a matrix file without name , using the date as name.
        /// </summary>
        /// <param name="matrix">matrix to print</param>
        public static void printMatrix(int[,] matrix)
        {
            string datetimeString = string.Format("{0:yyyy-MM-dd_hh-mm-ss-fffff-tt}.matrix", DateTime.Now);

            TextWriter tw = new StreamWriter(datetimeString + ".csv");
            //tw.WriteLine(matrix.GetLength(0).ToString());
            //tw.WriteLine(matrix.GetLength(1).ToString());

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (j != matrix.GetLength(1) - 1)
                        tw.Write(matrix[i, j] + ",");
                    else
                        tw.Write(matrix[i, j]);
                }

                tw.WriteLine();
            }

            tw.Close();
        }
        /// <summary>
        /// Print a vector to a file.
        /// </summary>
        /// <param name="vector">vector to print</param>
        /// <param name="name">name of the file</param>
        public static void printVector(int[] vector, string name)
        {

            TextWriter tw = new StreamWriter(name + ".csv");
            // Output in matrix format
            //tw.WriteLine(vector.Length.ToString());
            for (int i = 0; i < vector.GetLength(0); i++)
            {
                tw.WriteLine(vector[i].ToString());

            }
            tw.Close();

        }
        /// <summary>
        /// Print a vector to a file.
        /// </summary>
        /// <param name="vector">vector to print</param>
        /// <param name="name">name of the file</param>
        public static void printVector(double[] vector, string name)
        {

            TextWriter tw = new StreamWriter(name + ".csv");
            // Output in matrix format
            //tw.WriteLine(vector.Length.ToString());
            for (int i = 0; i < vector.GetLength(0); i++)
            {
                tw.WriteLine(vector[i].ToString());

            }
            tw.Close();

        }
        /// <summary>
        /// Print a vector to a file.
        /// </summary>
        /// <param name="vector">vector to print</param>
        /// <param name="name">name of the file</param>
        public static void printVector(double[] vector)
        {
            string datetimeString = string.Format("{0:yyyy-MM-dd_hh-mm-ss-fffff-tt}.matrix", DateTime.Now);
            TextWriter tw = new StreamWriter(datetimeString + ".csv");
            // Output in matrix format
            //tw.WriteLine(vector.Length.ToString());
            for (int i = 0; i < vector.GetLength(0); i++)
            {
                tw.WriteLine(vector[i].ToString());

            }
            tw.Close();

        }
        /// <summary>
        /// Print a vector to a file.
        /// </summary>
        /// <param name="vector">vector to print</param>
        /// <param name="name">name of the file</param>
        public static void printVector(int[] vector)
        {

            string datetimeString = string.Format("{0:yyyy-MM-dd_hh-mm-ss-fffff-tt}.matrix", DateTime.Now);
            TextWriter tw = new StreamWriter(datetimeString + ".csv");
            // Output in matrix format
            //tw.WriteLine(vector.Length.ToString());
            for (int i = 0; i < vector.GetLength(0); i++)
            {
                tw.WriteLine(vector[i].ToString());

            }
            tw.Close();

        }
        /// <summary>
        /// Print String Matrix as csv
        /// </summary>
        /// <param name="matrix">String Matrix</param>
        /// <param name="name">name</param>
        public static void printMatrix(string[,] matrix, string name)
        {
            string datetimeString = string.Format("{0:yyyy-MM-dd_hh-mm-ss-fffff-tt}.matrix", DateTime.Now);

            TextWriter tw = new StreamWriter(name + ".csv");
            //tw.WriteLine(matrix.GetLength(0).ToString());
            //tw.WriteLine(matrix.GetLength(1).ToString());

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (j != matrix.GetLength(1) - 1)
                        tw.Write(matrix[i, j] + ",");
                    else
                        tw.Write(matrix[i, j]);
                }

                tw.WriteLine();
            }

            tw.Close();
        }
        /// <summary>
        /// Print a vector to a file.
        /// </summary>
        /// <param name="vector">vector to print</param>
        /// <param name="name">name of the file</param>
        public static void printVector(string[] vector, string name)
        {

            TextWriter tw = new StreamWriter(name + ".csv");
            //tw.WriteLine(vector.Length.ToString());
            for (int i = 0; i < vector.GetLength(0); i++)
            {
                tw.WriteLine(vector[i]);

            }
            tw.Close();

        }

    }
}
