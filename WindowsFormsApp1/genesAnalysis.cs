using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WindowsFormsApp1
{
    class genesAnalysis
    {
        public static void genesCpG()
        {
            string[] List1 = formatCSV.OpenVectorFile();
            string[,] List2 = formatCSV.OpenMatrixFileStringCSV("Longer List");

            int y0 = List1.GetLength(0);
            int y1 = List2.GetLength(0);
            int x1 = List2.GetLength(1);

            Console.WriteLine(y0.ToString());
            Console.WriteLine(x1.ToString() + " " + y1.ToString());


            TextWriter tw = new StreamWriter("genesCpG.csv");

            int count = 0;
            for (int i = 0; i < y0; i++)
            {
                for (int j = 0; j < y1; j++)
                {
                    string[] tokens = List2[j, 1].Split(';');
                    for (int k = 0; k < tokens.GetLength(0); k++)
                    {
                        if (List1[i] == tokens[k])
                        {
                            tw.WriteLine(List1[i] + ',' +
                                List2[j, 0]);

                            count++;
                        }
                    }
                }

            }
            tw.Close();

            Console.WriteLine(count);
        }

        public static void reduceSeriesMatrix()
        {
            string[] featureList = formatCSV.OpenVectorFile("reduced Features");
            string[] reducedList = formatCSV.OpenVectorFile("all Features");
            string[,] data = formatCSV.OpenMatrixFileStringCSV("CSV Matrix");

            int y = data.GetLength(0);
            int x = data.GetLength(1);

            int yFList = featureList.GetLength(0);
            int rFList = reducedList.GetLength(0);

            Console.WriteLine(x.ToString() + " " + y.ToString());
            Console.WriteLine(yFList.ToString() + " " + rFList.ToString());

            int count = 0;
            TextWriter tw = new StreamWriter("features.csv");
            for (int i = 0; i < rFList; i++)
            {
                for (int j = 0; j < yFList; j++)
                {

                    if (reducedList[i] == featureList[j])
                    {
                        tw.WriteLine(reducedList[i]);
                        Console.WriteLine(reducedList[i]);
                        count++;
                    }

                }
            }
            tw.Close();
            Console.WriteLine("count " + count);

            string[,] reduceData = new string[count, x];
            int index = 0;

            for (int i = 0; i < rFList; i++)
            {
                for (int j = 0; j < yFList; j++)
                {
                    if (reducedList[i] == featureList[j])
                    {
                        for (int k = 0; k < x; k++)
                        {
                            

                            if ((data[j, k] == "") || (data[j, k] == "NA"))
                            {
                                reduceData[index, k] = "0";
                            }
                            else
                            {
                                reduceData[index, k] = data[j, k];
                            }


                        }
                        index++;
                    }

                }
            }



            string[,] reduceDataT = new
               string[reduceData.GetLength(1), reduceData.GetLength(0)];

            for (int i = 0; i < reduceData.GetLength(0); i++)
            {
                for (int j = 0; j < reduceData.GetLength(1); j++)
                {
                    reduceDataT[j, i] = reduceData[i, j]
;
                }

            }

            printFile.printMatrix(reduceDataT, "reduceData");
        }


        public static void idsValues()
        {
            string[] List1 = formatCSV.OpenVectorFile("reduced ids");
            string[] List2 = formatCSV.OpenVectorFile("ids");
            string[,] values = formatCSV.OpenMatrixFileStringCSV("values");

            int y0 = List1.GetLength(0);
            int y1 = values.GetLength(0);
            int x1 = values.GetLength(1);

            Console.WriteLine(y0.ToString());
            Console.WriteLine(x1.ToString() + " " + y1.ToString());



            TextWriter tw = new StreamWriter("features.csv");

            int count = 0;
            for (int i = 0; i < y0; i++)
            {
                for (int j = 0; j < y1; j++)
                {
                    if (List1[i] == List2[j])
                    {
                        count++;
                        tw.WriteLine(List1[i]);
                    }
                        
                }

            }

            tw.Close();

            Console.WriteLine(count);

            string[,] outMatrix = new string[count, x1];

            count = 0;
            for (int i = 0; i < y0; i++)
            {
                for (int j = 0; j < y1; j++)
                {
                    if (List1[i] == List2[j])
                    {
                        for (int k = 0; k < x1; k++)
                        {
                            outMatrix[count, k] = values[j, k];
                        }
                        count++;
                    }
                }

            }

            printFile.printMatrix(outMatrix, "valuesReduced");

        }

        public static void featuresValues()
        {
            string[] List1 = formatCSV.OpenVectorFile("reduced features");
            string[] List2 = formatCSV.OpenVectorFile("features");
            double[,] values = formatCSV.OpenMatrixFile("values");

            int x0 = List1.GetLength(0);
            int y1 = values.GetLength(0);
            int x1 = values.GetLength(1);

            Console.WriteLine(x0.ToString());
            Console.WriteLine(x1.ToString() + " " + y1.ToString());



            TextWriter tw = new StreamWriter("features.csv");

            int count = 0;
            for (int i = 0; i < x0; i++)
            {
                for (int j = 0; j < x1; j++)
                {
                    if (List1[i] == List2[j])
                    {
                        count++;
                        tw.WriteLine(List1[i]);
                    }
                    
                }

            }

            tw.Close();

            Console.WriteLine(count);

            double[,] outMatrix = new double[y1,count];

            count = 0;
            for (int i = 0; i < x0; i++)
            {
                for (int j = 0; j < x1; j++)
                {
                    if (List1[i] == List2[j])
                    {
                        for (int k = 0; k < y1; k++)
                        {
                            outMatrix[k,count] = values[ k,j];
                        }
                        count++;
                    }
                }

            }

            printFile.printMatrix(outMatrix, "valuesReduced");

        }
    }
}
