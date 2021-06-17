using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class formatCSV
    {
        //openFileDialog.InitialDirectory = "c:\\";
        //openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
        //openFileDialog.FilterIndex = 2;
        /// <summary>
        /// Open a double *.matrix File
        /// </summary>
        /// <returns></returns>
        public static double[,] OpenMatrixFile()
        {

            double[,] K = null;
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Filter = "txt files(*.txt)|*.txt|csv files(*.csv)|*.csv|tsv files(*.tsv)|*.tsv";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.Title = "Select a Matrix File";

            int y = 0;
            int x = 0;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);

                do
                {
                    string dummString = sr.ReadLine();
                    if (y == 0)
                    {
                        string[] dummyStringEntries = dummString.Split(' ', '\t', ',');
                        x = dummyStringEntries.GetLength(0);
                    }

                    y++;

                } while (!sr.EndOfStream);

                sr.Close();
                sr = new System.IO.StreamReader(openFileDialog1.FileName);
                K = new double[y, x];
                for (int i = 0; i < y; i++)
                {
                    string rowString = sr.ReadLine();
                    string[] rowStringEntries = rowString.Split(' ', '\t', ',');



                    for (int j = 0; j < x; j++)
                    {

                        K[i, j] = Convert.ToDouble(rowStringEntries[j]);

                    }

                }

                sr.Close();

            }
            else
            {
                MessageBox.Show("No file");
            }
            return K;
        }

        public static double[,] OpenMatrixFile(string title)
        {

            double[,] K = null;
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Filter = "txt files(*.txt)|*.txt|csv files(*.csv)|*.csv";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.Title = "Select a Matrix File " + title;

            int y = 0;
            int x = 0;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);
                int maxString = 0;
                do
                {
                    string dummString = sr.ReadLine();
                    
                    string[] dummyStringEntries = dummString.Split(' ', '\t', ',');
                    x = dummyStringEntries.GetLength(0);
                    if (x > maxString)
                        maxString = x;

                y++;

                } while (!sr.EndOfStream);
                x = maxString;

                sr.Close();
                sr = new System.IO.StreamReader(openFileDialog1.FileName);
                K = new double[y, x];
                for (int i = 0; i < y; i++)
                {
                    string rowString = sr.ReadLine();
                    string[] rowStringEntries = rowString.Split(' ', '\t', ',');

                    int stringLength = rowStringEntries.GetLength(0);

                    for (int j = 0; j < rowStringEntries.GetLength(0); j++)
                    {
                        if (rowStringEntries[j].Length>0)
                        K[i, j] = Convert.ToDouble(rowStringEntries[j]);

                    }

                }

                sr.Close();

            }
            else
            {
                MessageBox.Show("No file");
            }
            return K;
        }

        public static string[] OpenVectorFile()
        {

            string[] K = null;
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Filter = "txt files(*.txt)|*.txt|csv files(*.csv)|*.csv";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.Title = "Select a Vector File";

            int y = 0;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);

                do
                {
                    string dummString = sr.ReadLine();


                    y++;

                } while (!sr.EndOfStream);

                sr.Close();
                sr = new System.IO.StreamReader(openFileDialog1.FileName);
                K = new string[y];
                for (int i = 0; i < y; i++)
                {
                    string rowString = sr.ReadLine();

                    K[i] = rowString;



                }

                sr.Close();

            }
            else
            {
                MessageBox.Show("No file");
            }
            return K;
        }

        public static string[] OpenVectorFile(string title)
        {

            string[] K = null;
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Filter = "txt files(*.txt)|*.txt|csv files(*.csv)|*.csv";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.Title = "Select a Vector File " + title;

            int y = 0;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);

                do
                {
                    string dummString = sr.ReadLine();


                    y++;

                } while (!sr.EndOfStream);

                sr.Close();
                sr = new System.IO.StreamReader(openFileDialog1.FileName);
                K = new string[y];
                for (int i = 0; i < y; i++)
                {
                    string rowString = sr.ReadLine();

                    K[i] = rowString;



                }

                sr.Close();

            }
            else
            {
                MessageBox.Show("No file");
            }
            return K;
        }

        public static double[] OpenDoubleVectorFile(string title)
        {

            double[] K = null;
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Filter = "txt files(*.txt)|*.txt|csv files(*.csv)|*.csv";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.Title = "Select a Vector File " + title;

            int y = 0;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);

                do
                {
                    string dummString = sr.ReadLine();


                    y++;

                } while (!sr.EndOfStream);

                sr.Close();
                sr = new System.IO.StreamReader(openFileDialog1.FileName);
                K = new double[y];
                for (int i = 0; i < y; i++)
                {
                    string rowString = sr.ReadLine();

                    K[i] = Convert.ToDouble(rowString);



                }

                sr.Close();

            }
            else
            {
                MessageBox.Show("No file");
            }
            return K;
        }

        public static string[,] OpenMatrixFileStringCSV(string title)
        {

            string[,] K = null;
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Filter = "txt files(*.txt)|*.txt|csv files(*.csv)|*.csv";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.Title = "Select a Matrix File " + title;

            int y = 0;
            int x = 0;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);

                do
                {
                    string dummString = sr.ReadLine();
                    if (y == 0)
                    {
                        string[] dummyStringEntries = dummString.Split(',');
                        x = dummyStringEntries.GetLength(0);
                    }
                    else
                    {
                        string[] dummyStringEntries = dummString.Split(',');
                        if (dummyStringEntries.GetLength(0) > x)
                            x = dummyStringEntries.GetLength(0);
                    }
                    y++;

                } while (!sr.EndOfStream);

                sr.Close();
                sr = new System.IO.StreamReader(openFileDialog1.FileName);
                K = new string[y, x];
                for (int i = 0; i < y; i++)
                {
                    string rowString = sr.ReadLine();
                    string[] rowStringEntries = rowString.Split(',');



                    for (int j = 0; j < rowStringEntries.GetLength(0); j++)
                    {

                        K[i, j] = (rowStringEntries[j]);

                    }

                }

                sr.Close();

            }
            else
            {
                MessageBox.Show("No file");
            }
            return K;
        }

        public static string[,] OpenMatrixFileStringTSV(string title)
        {

            string[,] K = null;
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Filter = "txt files(*.txt)|*.txt|csv files(*.csv)|*.csv|tsv files(*.tsv)|*.tsv";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.Title = "Select a Matrix File " + title;

            int y = 0;
            int x = 0;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);

                do
                {
                    string dummString = sr.ReadLine();
                    if (y == 0)
                    {
                        string[] dummyStringEntries = dummString.Split('\t');
                        x = dummyStringEntries.GetLength(0);
                    }

                    y++;

                } while (!sr.EndOfStream);

                sr.Close();
                sr = new System.IO.StreamReader(openFileDialog1.FileName);
                K = new string[y, x];
                for (int i = 0; i < y; i++)
                {
                    string rowString = sr.ReadLine();
                    string[] rowStringEntries = rowString.Split('\t');



                    for (int j = 0; j < x; j++)
                    {

                        K[i, j] = (rowStringEntries[j]);

                    }

                }

                sr.Close();

            }
            else
            {
                MessageBox.Show("No file");
            }
            return K;
        }

        public static string[,] OpenMatrixFileStringTSVFileName(string title)
        {

            string[,] K = null;
           

            int y = 0;
            int x = 0;

            
                System.IO.StreamReader sr = new System.IO.StreamReader(title);

                do
                {
                    string dummString = sr.ReadLine();
                    if (y == 0)
                    {
                        string[] dummyStringEntries = dummString.Split('\t');
                        x = dummyStringEntries.GetLength(0);
                    }

                    y++;

                } while (!sr.EndOfStream);

                sr.Close();
                sr = new System.IO.StreamReader(title);
                K = new string[y, x];
                for (int i = 0; i < y; i++)
                {
                    string rowString = sr.ReadLine();
                    string[] rowStringEntries = rowString.Split('\t');



                    for (int j = 0; j < x; j++)
                    {

                        K[i, j] = (rowStringEntries[j]);

                    }

                }

                sr.Close();

            
            return K;
        }

        public static string[,] OpenMatrixFileStringCSVFileName(string title)
        {

            string[,] K = null;


            int y = 0;
            int x = 0;


            System.IO.StreamReader sr = new System.IO.StreamReader(title);

            do
            {
                string dummString = sr.ReadLine();
                if (y == 0)
                {
                    string[] dummyStringEntries = dummString.Split(',');
                    x = dummyStringEntries.GetLength(0);
                }

                y++;

            } while (!sr.EndOfStream);

            sr.Close();
            sr = new System.IO.StreamReader(title);
            K = new string[y, x];
            for (int i = 0; i < y; i++)
            {
                string rowString = sr.ReadLine();
                string[] rowStringEntries = rowString.Split(',');



                for (int j = 0; j < x; j++)
                {

                    K[i, j] = (rowStringEntries[j]);

                }

            }

            sr.Close();


            return K;
        }

        public static int[] OpenIntVectorFile(string title)
        {

            int[] K = null;
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Filter = "txt files(*.txt)|*.txt|csv files(*.csv)|*.csv";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.Title = "Select a Vector File " + title;

            int y = 0;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);

                do
                {
                    string dummString = sr.ReadLine();


                    y++;

                } while (!sr.EndOfStream);

                sr.Close();
                sr = new System.IO.StreamReader(openFileDialog1.FileName);
                K = new int[y];
                for (int i = 0; i < y; i++)
                {
                    string rowString = sr.ReadLine();

                    K[i] = Convert.ToInt32(rowString);



                }

                sr.Close();

            }
            else
            {
                MessageBox.Show("No file");
            }
            return K;
        }
    }
}
