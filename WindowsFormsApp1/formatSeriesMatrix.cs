using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
{
    class formatSeriesMatrix
    {
        //openFileDialog.InitialDirectory = "c:\\";
        //openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
        //openFileDialog.FilterIndex = 2;
        /// <summary>
        /// Open a double *.matrix File
        /// </summary>
        /// <returns></returns>
        public static void OpenMatrixFile()
        {

            double[,] K = null;
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Filter = "txt files(*.txt)|*.txt|csv files(*.csv)|*.csv|tsv files(*.tsv)|*.tsv";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Title = "Select a Matrix File";

            int y = 0;
            int x = 0;

            TextWriter tw = new StreamWriter("Headers.tsv");
 
               
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);

                #region headers
                do
                {
                    string dummString = sr.ReadLine();
                    if (dummString.Length != 0)
                    {
                        if (dummString[0].Equals('!') )
                        {
                            tw.WriteLine(dummString);
                            //Console.WriteLine(dummString);
                        }
                        else
                        {
                            if (y == 0)
                            {
                                string[] dummyStringEntries = dummString.Split(' ', '\t', ',');
                                x = dummyStringEntries.GetLength(0);
                            }
                            
                            y++;
                            

                        }
                        
                    }

                } while (!sr.EndOfStream);

                sr.Close();
                Console.WriteLine(y);
                Console.WriteLine(x);
                tw.Close();
                #endregion

                sr = new System.IO.StreamReader(openFileDialog1.FileName);
                string[] ids = new string[x-1];
                string[] genes = new string[y - 1];
                K = new double[y - 1, x - 1];
                int z = 0;
                do
                {
                    string dummString = sr.ReadLine();
                    if (dummString.Length != 0)
                    {
                        if (dummString[0].Equals('!'))
                        {
                           
                        }
                        else
                        {
                            string[] rowStringEntries = dummString.Split(' ', '\t', ',');

                            if (z == 0)
                            {
                                for (int j = 1; j < x; j++)
                                {

                                    ids[j-1] = rowStringEntries[j].Replace("\"", "");

                                }
                                z++;
                            }
                            else
                            {
                                genes[z - 1] = rowStringEntries[0].Replace("\"", "");
                                for (int j = 1; j < x; j++)
                                {


                                    if ((rowStringEntries[j] != "") && (rowStringEntries[j] != "NA"))
                                        try
                                        {
                                            K[z - 1, j-1] = Convert.ToDouble(rowStringEntries[j]);
                                        }
                                        catch (Exception)
                                        {
                                            Console.WriteLine(rowStringEntries[j]);
                                            throw;
                                        }

                                }
                                z++;
                            }
                            


                        }

                    }

                } while (!sr.EndOfStream);

                sr.Close();

                printFile.printMatrix(K, "values");
                printFile.printVector(genes, "genes");
                printFile.printVector(ids, "ids");

            }
            else
            {
                MessageBox.Show("No file");
            }
            
        }

        /// <summary>
        /// Open a double *.matrix File
        /// </summary>
        /// <returns></returns>
        public static void OpenMatrixFileHeavy()
        {
            int sizeSample = 100;
            
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Filter = "txt files(*.txt)|*.txt|csv files(*.csv)|*.csv|tsv files(*.tsv)|*.tsv";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Title = "Select a Matrix File";

            int y = 0;
            int x = 0;

            TextWriter tw = new StreamWriter("Headers.tsv");
            double[,] K = null;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);

                #region headers
                do
                {
                    string dummString = sr.ReadLine();
                    if (dummString.Length != 0)
                    {
                        if (dummString[0].Equals('!'))
                        {
                            tw.WriteLine(dummString);
                            //Console.WriteLine(dummString);
                        }
                        else
                        {
                            if (y == 0)
                            {
                                string[] dummyStringEntries = dummString.Split(' ', '\t', ',');
                                x = dummyStringEntries.GetLength(0);
                            }

                            y++;


                        }

                    }

                } while (!sr.EndOfStream);

                sr.Close();
                Console.WriteLine(y);
                Console.WriteLine(x);
                tw.Close();
                #endregion

                
                string[] ids = new string[x - 1];
                string[] genes = new string[y - 1];

                int samples = (int) Math.Ceiling((double)(x - 1) / sizeSample);

                Console.WriteLine(samples);

                int lastsize = (x - 1) % sizeSample;

                Console.WriteLine(lastsize);
                
                for (int k = 0; k < samples; k++)
                {
                    sr = new System.IO.StreamReader(openFileDialog1.FileName);
                    int usedSize=0;
                    if (k != (samples - 1))
                        usedSize = sizeSample;
                    else
                        usedSize = lastsize;

                    K = new double[y - 1, usedSize];
                    int z = 0;
                    do
                    {
                        string dummString = sr.ReadLine();
                        if (dummString.Length != 0)
                        {
                            if (dummString[0].Equals('!'))
                            {

                            }
                            else
                            {
                                string[] rowStringEntries = dummString.Split(' ', '\t', ',');

                                if (z == 0)
                                {
                                    for (int j = 1; j < x; j++)
                                    {

                                        ids[j - 1] = rowStringEntries[j].Replace("\"", "");

                                    }
                                    z++;
                                }
                                else
                                {
                                    genes[z - 1] = rowStringEntries[0].Replace("\"", "");
                                    for (int j = 0; j < usedSize; j++)
                                    {
                                        int totalSize = j + k * sizeSample + 1;
                                        if ((rowStringEntries[totalSize] != "") && (rowStringEntries[totalSize] != "NA"))
                                            try
                                            {
                                                K[z - 1, j] = Convert.ToDouble(rowStringEntries[totalSize]);
                                            }
                                            catch (Exception)
                                            {
                                                Console.WriteLine(rowStringEntries[totalSize]);
                                                throw;
                                            }
                                            

                                    }
                                    z++;
                                }



                            }

                        }

                    } while (!sr.EndOfStream);

                    sr.Close();

                    printFile.printMatrix(K, "values_"+k.ToString());
                    
                    K = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();

                }

                printFile.printVector(genes, "genes");
                printFile.printVector(ids, "ids");
                

            }
            else
            {
                MessageBox.Show("No file");
            }

        }

        /// <summary>
        /// Open a double *.matrix File
        /// </summary>
        /// <returns></returns>
        public static void OpenMatrixFileHeavyIds()
        {
            int sizeSample = 5;

            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Filter = "txt files(*.txt)|*.txt|csv files(*.csv)|*.csv|tsv files(*.tsv)|*.tsv";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Title = "Select a Matrix File";

            int y = 0;
            int x = 0;

            TextWriter tw = new StreamWriter("Headers.tsv");
            double[,] K = null;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);

                #region headers
                do
                {
                    string dummString = sr.ReadLine();
                    if (dummString.Length != 0)
                    {
                        if (dummString[0].Equals('!'))
                        {
                            tw.WriteLine(dummString);
                            //Console.WriteLine(dummString);
                        }
                        else
                        {
                            if (y == 0)
                            {
                                string[] dummyStringEntries = dummString.Split(' ', '\t', ',');
                                x = dummyStringEntries.GetLength(0);
                            }

                            y++;


                        }

                    }

                } while (!sr.EndOfStream);

                sr.Close();
                Console.WriteLine(y);
                Console.WriteLine(x);
                tw.Close();
                #endregion


                string[] ids = new string[x - 1];
                string[] genes = new string[y - 1];

                int samples = (int)Math.Ceiling((double)(x - 1) / sizeSample);

                Console.WriteLine(samples);

                int lastsize = (x - 1) % sizeSample;

                Console.WriteLine(lastsize);

                for (int k = 0; k < 1; k++)
                {
                    sr = new System.IO.StreamReader(openFileDialog1.FileName);
                    int usedSize = 0;
                    if (k != (samples - 1))
                        usedSize = sizeSample;
                    else
                        usedSize = lastsize;

                    K = new double[y - 1, usedSize];
                    int z = 0;
                    do
                    {
                        string dummString = sr.ReadLine();
                        if (dummString.Length != 0)
                        {
                            if (dummString[0].Equals('!'))
                            {

                            }
                            else
                            {
                                string[] rowStringEntries = dummString.Split(' ', '\t', ',');

                                if (z == 0)
                                {
                                    for (int j = 1; j < x; j++)
                                    {

                                        ids[j - 1] = rowStringEntries[j];

                                    }
                                    z++;
                                }
                                else
                                {
                                    genes[z - 1] = rowStringEntries[0];
                                    for (int j = 0; j < usedSize; j++)
                                    {
                                        int totalSize = j + k * sizeSample + 1;
                                        if ((rowStringEntries[totalSize] != "") && (rowStringEntries[totalSize] != "NA"))
                                            try
                                            {
                                                K[z - 1, j] = Convert.ToDouble(rowStringEntries[totalSize]);
                                            }
                                            catch (Exception)
                                            {
                                                Console.WriteLine(rowStringEntries[totalSize]);
                                                throw;
                                            }


                                    }
                                    z++;
                                }



                            }

                        }

                    } while (!sr.EndOfStream);

                    sr.Close();

                    printFile.printMatrix(K, "values_" + k.ToString());

                    K = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();

                }

                printFile.printVector(genes, "genes");
                printFile.printVector(ids, "ids");


            }
            else
            {
                MessageBox.Show("No file");
            }

        }
    }
}
