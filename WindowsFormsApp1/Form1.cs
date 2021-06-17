using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// This code will open a series matrix and process it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeriesMatrixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formatSeriesMatrix.OpenMatrixFile();
        }
        /// <summary>
        ///  This code is to open big Series Matrix and divide them
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeriesMatrixBigDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formatSeriesMatrix.OpenMatrixFileHeavy();
        }
        /// <summary>
        /// This code is to get the headers and variables and a demo of 5 of the data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeriesMatrixBigDataHeadersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formatSeriesMatrix.OpenMatrixFileHeavyIds();
        }

        private void GenesToCpGPositionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            genesAnalysis.genesCpG();

        }

        private void ReduceByCPGsSeriesMatrixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            genesAnalysis.reduceSeriesMatrix();
        }

        private void ReduceBySampleIdsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            genesAnalysis.idsValues();
        }

        private void Compare2ListsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[,] List1 = formatCSV.OpenMatrixFileStringCSV("machine 1");
            string[,] List2 = formatCSV.OpenMatrixFileStringCSV("machine 2");

            int count = 0;
            TextWriter tw = new StreamWriter("List3.csv");
            for (int i = 0; i < List1.GetLength(0); i++)
            {
                for (int j = 0; j < List2.GetLength(0); j++)
                {
                    if (List1[i, 0] == List2[j, 0])
                    {
                        tw.WriteLine(List1[i, 0] + ',' +
                            List1[i, 1] + ',' + List2[j, 1]);
                        count++;
                    }
                }
            }
            tw.Close();
            Console.WriteLine(count);
        }


        private void ReduceBySampleFeaturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            genesAnalysis.featuresValues();
        }

        private void Compare2ListsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string[,] List1 = formatCSV.OpenMatrixFileStringCSV("machine 1");
            string[,] List2 = formatCSV.OpenMatrixFileStringCSV("machine 2");

            int count = 0;
            TextWriter tw = new StreamWriter("List3.csv");
            for (int i = 0; i < List1.GetLength(0); i++)
            {
                for (int j = 0; j < List2.GetLength(0); j++)
                {
                    if (List1[i, 0] == List2[j, 0])
                    {
                        tw.WriteLine(List1[i, 0]);
                        count++;
                    }
                }
            }
            tw.Close();
            Console.WriteLine(count);
        }

        private void PValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[,] casesXGenes = formatCSV.OpenMatrixFile("data");
            double[] labels = formatCSV.OpenDoubleVectorFile("Labels");

            int countControl = 0;
            int countDisease = 0;

            for (int i = 0; i < labels.GetLength(0); i++)
            {
                if (labels[i] == 0)
                    countControl++;
                if (labels[i] == 1)
                    countDisease++;

            }

            double[,] diseaseMatrix = new double[countDisease,
                casesXGenes.GetLength(1)];
            double[,] controlMatrix = new double[countControl,
                casesXGenes.GetLength(1)];

            countControl = 0;
            countDisease = 0;

            for (int i = 0; i < labels.GetLength(0); i++)
            {
                if (labels[i] == 0)
                {
                    for (int j = 0; j < casesXGenes.GetLength(1); j++)
                    {
                        controlMatrix[countControl, j] =
                            casesXGenes[i, j];
                    }
                    countControl++;
                }

                if (labels[i] == 1)

                {
                    for (int j = 0; j < casesXGenes.GetLength(1); j++)
                    {
                        diseaseMatrix[countDisease, j] =
                            casesXGenes[i, j];
                    }
                    countDisease++;
                }


            }

            //printFile.printMatrix(controlMatrix, "controlMatrix");
            //printFile.printMatrix(diseaseMatrix, "diseaseMatrix");

            double[,] resultsMatrix = new double
                [casesXGenes.GetLength(1), 8];

            for (int i = 0; i < casesXGenes.GetLength(1); i++)
            {
                double[] controlVector =
                    new double[controlMatrix.GetLength(0)];
                double[] diseaseVector =
                    new double[diseaseMatrix.GetLength(0)];

                for (int j = 0; j < controlMatrix.GetLength(0); j++)
                {
                    controlVector[j] = controlMatrix[j, i];
                }
                for (int j = 0; j < diseaseMatrix.GetLength(0); j++)
                {
                    diseaseVector[j] = diseaseMatrix[j, i];
                }
                //results[0] = meanX;
                //results[1] = meanY;
                //results[2] = t;
                //results[3] = df;
                //results[4] = p;
                //results[i,5]= fold change (B-A)/A
                //controlMatrix.GetLength(0);
                //diseaseMatrix.GetLength(0);
                double[] results =
                TTest.TTestProgram.TTest(controlVector, diseaseVector);

                for (int j = 0; j < 5; j++)
                {
                    resultsMatrix[i, j] = results[j];
                }

                resultsMatrix[i, 6] = controlMatrix.GetLength(0);
                resultsMatrix[i, 7] = diseaseMatrix.GetLength(0);
            }

            for (int i = 0; i < casesXGenes.GetLength(1); i++)
            {
                double value = (resultsMatrix[i, 1] - resultsMatrix[i, 0]) / resultsMatrix[i, 0];
                Console.WriteLine(resultsMatrix[i, 0] + "\t" + resultsMatrix[i, 1] + "\t" + value);

                if (value < 0)
                {
                    value = -(resultsMatrix[i, 0] - resultsMatrix[i, 1]) / resultsMatrix[i, 1];
                }

                resultsMatrix[i, 5] = value;
            }
            string[] variables = new string[6];

            variables[0] = "meanX";
            variables[1] = " meanY";
            variables[2] = " t";
            variables[3] = "df";
            variables[4] = "p";
            variables[5] = "fold change(B - A) / A";
            printFile.printMatrix(resultsMatrix, "resultsMatrix");
            printFile.printVector(variables, "variables");
        }

        private void CompareFeaturesInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] List1 = formatCSV.OpenVectorFile();
            string[,] List2 = formatCSV.OpenMatrixFileStringCSV("Longer List");

            int y0 = List1.GetLength(0);
            int y1 = List2.GetLength(0);
            int x1 = List2.GetLength(1);

            Console.WriteLine(y0.ToString());
            Console.WriteLine(x1.ToString() + " " + y1.ToString());

            TextWriter tw = new StreamWriter("features.csv");
            int count = 0;
            for (int i = 0; i < y0; i++)
            {
                for (int j = 0; j < y1; j++)
                {


                    if (List1[i] == List2[j, 0])
                    {
                        Console.WriteLine(
                            List1[i] + '\t' +
                            List2[j, 1]
                            );
                        tw.Write('\n' + List1[i] + ',');
                        for (int k = 1; k < List2.GetLength(1) - 1; k++)
                        {
                            tw.Write(List2[j, k] + ',');
                        }
                        tw.Write(List2[j, List2.GetLength(1) - 1]);
                        count++;
                    }

                }

            }
            tw.Close();
            Console.WriteLine(count);
        }

        private void CountValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[,] List2 = formatCSV.OpenMatrixFileStringCSV("Longer List");
            List<string> values = new List<string>();
            for (int i = 0; i < List2.GetLength(0); i++)
            {
                for (int j = 0; j < List2.GetLength(1); j++)
                {
                    if (!values.Contains(List2[i, j]))
                    {
                        values.Add(List2[i, j]);
                    }
                }
            }

            int[] valuesCount = new int[values.Count];

            for (int i = 0; i < List2.GetLength(0); i++)
            {
                for (int j = 0; j < List2.GetLength(1); j++)
                {
                    for (int k = 0; k < values.Count; k++)
                    {
                        if (List2[i, j] == values[k])
                            valuesCount[k]++;
                    }
                }
            }

            for (int k = 0; k < values.Count; k++)
            {
                Console.WriteLine(values[k] + "\t" + valuesCount[k]);
            }

        }

        private void GenerateMttrixByClassifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] feats = formatCSV.OpenVectorFile("Features");
            string[,] classifier = formatCSV.OpenMatrixFileStringCSV("Classifier List");


            string[,] outList = new string[feats.GetLength(0), 2];
            for (int j = 0; j < feats.GetLength(0); j++)
            {
                outList[j, 1] = "0";
            }
            for (int i = 0; i < feats.GetLength(0); i++)
            {
                outList[i, 0] = feats[i];
                for (int j = 0; j < classifier.GetLength(0); j++)
                {

                    if (classifier[j, 0] == feats[i])
                    {
                        outList[i, 1] = classifier[j, 1];
                    }
                }
            }

            printFile.printMatrix(outList, "List");


        }

        private void CompareListWith2FeatsListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] List1 = formatCSV.OpenVectorFile("machine 1");
            string[,] List2 = formatCSV.OpenMatrixFileStringCSV("machine 2");

            int count = 0;
            TextWriter tw = new StreamWriter("List3.csv");
            for (int i = 0; i < List1.GetLength(0); i++)
            {
                for (int j = 0; j < List2.GetLength(0); j++)
                {
                    if (List1[i] == List2[j, 0])
                    {
                        tw.WriteLine(List1[i] + ',' + ',' + List2[j, 1]);
                        count++;
                    }
                }
            }
            tw.Close();
            Console.WriteLine(count);
        }

        private void TransposeMatrixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[,] values = formatCSV.OpenMatrixFile("values");
            int y = values.GetLength(0);
            int x = values.GetLength(1);

            double[,] valuesT = new double[x, y];

            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    valuesT[j, i] = values[i, j];
                }
            }

            printFile.printMatrix(valuesT);
        }

        private void LoadManifestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[,] manifest = formatCSV.OpenMatrixFileStringTSV("algo");

            string type = "WT";
            string folder = "E:\\exeTemp\\DATA\\" + type + "\\";

            double[,] RC = new double[manifest.GetLength(0) - 1, 1881];
            double[,] RPM = new double[manifest.GetLength(0) - 1, 1881];
            string[,] labels = new string[manifest.GetLength(0) - 1, 3];


            for (int i = 1; i < manifest.GetLength(0); i++)
            {
                string file = folder + manifest[i, 0] + "\\" + manifest[i, 1];
                string[,] matrix = formatCSV.OpenMatrixFileStringTSVFileName(file);
                for (int j = 1; j < matrix.GetLength(0); j++)
                {
                    RC[i - 1, j - 1] = Convert.ToDouble(matrix[j, 1]);
                    RPM[i - 1, j - 1] = Convert.ToDouble(matrix[j, 2]);
                }
                labels[i - 1, 0] = manifest[i, 4];
                labels[i - 1, 1] = manifest[i, 6];
                labels[i - 1, 2] = manifest[i, 7];
            }

            printFile.printMatrix(RC, "RC_" + type);
            printFile.printMatrix(RPM, "RPM_" + type);
            printFile.printMatrix(labels, "labels_" + type);
        }

        private void MergeMatrixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Filter = "txt files(*.txt)|*.txt|csv files(*.csv)|*.csv|tsv files(*.tsv)|*.tsv";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.Multiselect = true;
            openFileDialog1.ShowDialog();

            int examples = 0;
            int columns = 0;
            for (int i = 0; i < openFileDialog1.FileNames.GetLength(0); i++)
            {
                //Console.WriteLine(openFileDialog1.FileNames[i]);
                string[,] matrix = formatCSV.OpenMatrixFileStringCSVFileName(openFileDialog1.FileNames[i]);
                Console.WriteLine(matrix.GetLength(0));
                Console.WriteLine(matrix.GetLength(1));
                examples += matrix.GetLength(0);
                columns = matrix.GetLength(1);
            }

            string[,] outputMerge = new string[examples, columns];
            int index = 0;
            for (int i = 0; i < openFileDialog1.FileNames.GetLength(0); i++)
            {
                //Console.WriteLine(openFileDialog1.FileNames[i]);
                string[,] matrix = formatCSV.OpenMatrixFileStringCSVFileName(openFileDialog1.FileNames[i]);
                for (int j = 0; j < matrix.GetLength(0); j++)
                {

                    for (int k = 0; k < matrix.GetLength(1); k++)
                    {
                        outputMerge[index, k] = matrix[j, k];
                    }
                    index++;
                }
            }

            printFile.printMatrix(outputMerge, "labels");

        }

        private void ExcludeCasesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[,] exclude = formatCSV.OpenMatrixFileStringCSV("exclude list");
            string[,] labels = formatCSV.OpenMatrixFileStringCSV("labels list");

            int count = 0;

            for (int i = 0; i < labels.GetLength(0); i++)
            {

                for (int j = 0; j < exclude.GetLength(0); j++)
                {
                    if (labels[i, 0].Contains(exclude[j, 0]))
                    {
                        Console.WriteLine(labels[i, 0]);
                        Console.WriteLine(exclude[j, 0]);
                        count++;
                    }
                }
            }



            Console.WriteLine(count);

            int recount = labels.GetLength(0) - count;
            string[,] RC = formatCSV.OpenMatrixFileStringCSV("RC");
            string[,] RPM = formatCSV.OpenMatrixFileStringCSV("RPM");

            string[,] RCClean = new string[recount, RC.GetLength(1)];
            string[,] RPMClean = new string[recount, RPM.GetLength(1)];
            string[,] labelsClean = new string[recount, labels.GetLength(1)];

            int counter = 0;



            for (int i = 0; i < labels.GetLength(0); i++)
            {
                bool contains = false;
                for (int j = 0; j < exclude.GetLength(0); j++)
                {
                    if (labels[i, 0].Contains(exclude[j, 0]))
                    {
                        contains = true;
                    }
                }
                if (contains == false)
                {
                    for (int k = 0; k < RC.GetLength(1); k++)
                    {
                        RCClean[counter, k] = RC[i, k];
                        RPMClean[counter, k] = RPM[i, k];
                    }
                    for (int k = 0; k < labels.GetLength(1); k++)
                    {
                        labelsClean[counter, k] = labels[i, k];
                    }
                    counter++;

                }

            }

            printFile.printMatrix(RCClean, "RCClean");
            printFile.printMatrix(RPMClean, "RPMClean");
            printFile.printMatrix(labelsClean, "labelsClean");
        }

        private void Compare2ListsInverseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[,] List1 = formatCSV.OpenMatrixFileStringCSV("machine 1");
            string[,] List2 = formatCSV.OpenMatrixFileStringCSV("machine 2");

            int count = 0;
            TextWriter tw = new StreamWriter("List3Inv.csv");
            for (int i = 0; i < List1.GetLength(0); i++)
            {
                bool exists = false;
                for (int j = 0; j < List2.GetLength(0); j++)
                {
                    if (List1[i, 0] == List2[j, 0])
                    {
                        exists = true;
                    }
                }
                if (exists == false)
                {
                    tw.WriteLine(List1[i, 0]);
                    count++;
                }
            }
            tw.Close();
            Console.WriteLine(count);
        }

        private void reduceXAxisNoRepetitionToolStripMenuItem_Click(object sender, EventArgs e)
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
                        break;
                    }

                }

            }

            tw.Close();

            Console.WriteLine(count);

            double[,] outMatrix = new double[y1, count];

            count = 0;
            for (int i = 0; i < x0; i++)
            {
                for (int j = 0; j < x1; j++)
                {
                    if (List1[i] == List2[j])
                    {
                        for (int k = 0; k < y1; k++)
                        {
                            outMatrix[k, count] = values[k, j];
                        }
                        count++;
                        break;
                    }
                }

            }

            printFile.printMatrix(outMatrix, "valuesReduced");
        }

        private void ReduceYAxisNoRepetitionToolStripMenuItem_Click(object sender, EventArgs e)
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
                        break;
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
                        break;
                    }
                }

            }

            printFile.printMatrix(outMatrix, "valuesReduced");
        }

        private void CompareListWith2FeatsListNoRepetitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] List1 = formatCSV.OpenVectorFile("machine 1");
            string[,] List2 = formatCSV.OpenMatrixFileStringCSV("machine 2");

            int count = 0;
            TextWriter tw = new StreamWriter("List3.csv");
            for (int i = 0; i < List1.GetLength(0); i++)
            {
                for (int j = 0; j < List2.GetLength(0); j++)
                {
                    if (List1[i] == List2[j, 0])
                    {
                        tw.WriteLine(List1[i] + ',' + ',' + List2[j, 1]);
                        count++;
                        break;
                    }
                }
            }
            tw.Close();
            Console.WriteLine(count);
        }

        private void OpenMatrixRemoveEmptyColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[,] matrix = formatCSV.OpenMatrixFileStringCSV("");
            int[] numbers = new int[matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                Boolean var = false;
                for (int j = 1; j < matrix.GetLength(0); j++)
                {
                    if (matrix[j, i] != "")
                        var = true;
                }
                numbers[i] = 1;
            }

            int columns = 0;

            for (int i = 0; i < numbers.Length; i++)
            {
                columns += numbers[i];
            }

            Console.WriteLine(columns);
            Console.WriteLine(matrix.GetLength(1));
        }

        private void RemoveColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[,] matrix = formatCSV.OpenMatrixFileStringCSV("");
            int[] numbers = new int[matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                Boolean var = false;
                for (int j = 1; j < matrix.GetLength(0); j++)
                {
                    if (matrix[j, i] != "")
                        var = true;
                }
                if (var)
                {
                    numbers[i] = 1;
                }
                else
                    Console.WriteLine(matrix[0, i]);

            }

            int columns = 0;

            for (int i = 0; i < numbers.Length; i++)
            {
                columns += numbers[i];
            }

            string[,] reduceMatrix = new string[matrix.GetLength(0), columns];

            int count = 0;
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (numbers[j] == 1)
                {
                    for (int i = 0; i < matrix.GetLength(0); i++)
                    {
                        reduceMatrix[i, count] = matrix[i, j];
                    }
                    count++;
                }


            }

            printFile.printMatrix(reduceMatrix, "reduceMatrix");

            Console.WriteLine(columns);
            Console.WriteLine(matrix.GetLength(1));
        }

        private void CountVauesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[,] matrix = formatCSV.OpenMatrixFileStringCSV("");
            int[] numbers = new int[matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                Boolean var = false;
                for (int j = 1; j < matrix.GetLength(0); j++)
                {
                    if (matrix[j, i] != "")
                        var = true;
                }
                if (var)
                {
                    numbers[i] = 1;
                }
                else
                    Console.WriteLine(matrix[0, i]);

            }

            int columns = 0;

            for (int i = 0; i < numbers.Length; i++)
            {
                columns += numbers[i];
            }

            string[,] reduceMatrix = new string[matrix.GetLength(0), columns];

            int count = 0;
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (numbers[j] == 1)
                {
                    for (int i = 0; i < matrix.GetLength(0); i++)
                    {
                        reduceMatrix[i, count] = matrix[i, j];
                    }
                    count++;
                }


            }

            printFile.printMatrix(reduceMatrix, "reduceMatrix");

            Console.WriteLine(columns);
            Console.WriteLine(matrix.GetLength(1));
        }

        private void TestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[,] K = null;
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Filter = "txt files(*.txt)|*.txt|csv files(*.csv)|*.csv|fasta files(*.fasta)|*.fasta";
            openFileDialog1.FilterIndex = 3;
            openFileDialog1.Title = "Select a Matrix File";

            int y = 0;

            string[] note = null;
            string[] dataDNA = null;
            int[] dataSize = null;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);

                do
                {
                    string dummString = sr.ReadLine();

                    y++;

                } while (!sr.EndOfStream);
                Console.WriteLine("file Size " + y.ToString());

                sr.Close();

                note = new string[y / 2];
                dataDNA = new string[y / 2];
                dataSize = new int[y / 2];

                y = 0;
                sr = new System.IO.StreamReader(openFileDialog1.FileName);

                int indexNote = 0;
                int indexData = 0;
                do
                {
                    string dummString = sr.ReadLine();
                    if (y % 2 == 0)
                    {
                        note[indexNote] = dummString;
                        indexNote++;
                    }
                    else
                    {
                        dataDNA[indexData] = dummString;
                        dataSize[indexData] = dummString.Length;
                        indexData++;
                    }

                    y++;
                } while (!sr.EndOfStream);

                printFile.printVector(note, "Notes.csv");
                printFile.printVector(dataDNA, "dataDNA.csv");
                printFile.printVector(dataSize, "dataSize.csv");

                int max = 0;
                for (int i = 0; i < y / 2; i++)
                {
                    if (dataSize[i] > max)
                        max = dataSize[i];
                }

                Console.WriteLine("max Size " + max.ToString());

                string[,] dataProcessed = new string[y / 2, max];
                double[,] dataProcessedDouble = new double[y / 2, max];

                for (int i = 0; i < y / 2; i++)
                {
                    for (int j = 0; j < dataSize[i]; j++)
                    {
                        string temp = dataDNA[i];
                        dataProcessed[i, j] = temp[j].ToString();
                        if (dataProcessed[i, j] == "C")
                            dataProcessedDouble[i, j] = 0.25;
                        else if (dataProcessed[i, j] == "T")
                            dataProcessedDouble[i, j] = 0.5;
                        else if (dataProcessed[i, j] == "G")
                            dataProcessedDouble[i, j] = 0.75;
                        else if (dataProcessed[i, j] == "A")
                            dataProcessedDouble[i, j] = 1.0;
                        else
                            dataProcessedDouble[i, j] = 0.0;
                    }
                }



                printFile.printMatrix(dataProcessed, "dataProcessed");
                printFile.printMatrix(dataProcessedDouble, "dataProcessedDouble");
            }
            else
            {
                MessageBox.Show("No file");
            }

        }

        private void FromDNAFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string[,] dataDNA = formatCSV.OpenMatrixFileStringCSV("dataDNA");

            Console.WriteLine(dataDNA.GetLength(0));
            Console.WriteLine(dataDNA.GetLength(1));

            int y = dataDNA.GetLength(0);

            int[] dataSize = new int[dataDNA.GetLength(0)];
            for (int i = 0; i < dataDNA.GetLength(0); i++)
            {
                dataSize[i] = dataDNA[i, 0].Length;
            }

            printFile.printVector(dataSize, "dataSize.csv");

            int max = 0;
            for (int i = 0; i < y; i++)
            {
                if (dataSize[i] > max)
                    max = dataSize[i];
            }

            Console.WriteLine("max Size " + max.ToString());

            //string[,] dataProcessed = new string[y, max];
            string[,] dataProcessedDouble = new string[y, max];




            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < dataSize[i]; j++)
                {
                    string temp = dataDNA[i, 0];
                    string temp2 = temp[j].ToString();
                    if (temp2 == "C")
                    {
                        dataProcessedDouble[i, j] = "0.25";
                    }

                    else if (temp2 == "T")
                    {
                        dataProcessedDouble[i, j] = "0.5";
                    }

                    else if (temp2 == "G")
                    {
                        dataProcessedDouble[i, j] = "0.75";
                    }

                    else if (temp2 == "A")
                    {
                        dataProcessedDouble[i, j] = "1.0";
                    }

                    else
                    {
                        dataProcessedDouble[i, j] = "0.0";
                    }

                }
            }

            printFile.printMatrix(dataProcessedDouble, "dataProcessedDouble");


        }

        private void FASTNCBIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[,] K = null;
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Filter = "txt files(*.txt)|*.txt|csv files(*.csv)|*.csv|fasta files(*.fasta)|*.fasta";
            openFileDialog1.FilterIndex = 3;
            openFileDialog1.Title = "Select a Matrix File";

            int examples = 0;
            int examples2 = 0;
            string[] examplesInfo = null;
            string[] examplesData = null;
            string[] examplesSize = null;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);

                do
                {
                    string dummString = sr.ReadLine();
                    if (dummString.Length > 0)
                    {
                        if (dummString[0] == '>')
                            examples++;

                    }
                    else
                    {
                        examples2++;
                    }

                } while (!sr.EndOfStream);
                Console.WriteLine("file Size " + examples.ToString());
                Console.WriteLine("file Size2 " + examples2.ToString());
                sr.Close();
                sr = new System.IO.StreamReader(openFileDialog1.FileName);
                examplesInfo = new string[examples];
                examplesData = new string[examples];
                examplesSize = new string[examples];

                int stringsDNA = 0;
                do
                {
                    string dummString = sr.ReadLine();
                    if (dummString.Length > 0)
                    {
                        if (dummString[0] == '>')
                        {
                            examplesInfo[stringsDNA] = dummString.Replace("/", ",");
                        }
                        else
                        {
                            dummString = dummString.Replace("a", "A");
                            dummString = dummString.Replace("c", "C");
                            dummString = dummString.Replace("t", "T");
                            dummString = dummString.Replace("g", "G");
                            dummString = dummString.Replace("n", "N");
                            examplesData[stringsDNA] += dummString;

                        }
                    }
                    else
                    {
                        stringsDNA++;
                    }



                } while (!sr.EndOfStream);
                Console.WriteLine("stringsDNA Size " + stringsDNA.ToString());

                sr.Close();

                for (int i = 0; i < examples; i++)
                {
                    examplesSize[i] = examplesData[i].Length.ToString();
                }

                printFile.printVector(examplesInfo, "examplesInfo");
                printFile.printVector(examplesData, "examplesData");
                printFile.printVector(examplesSize, "examplesSize");
            }

            else
            {
                MessageBox.Show("No file");
            }
        }

        private void ConcatMatrixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[,] mat1 = formatCSV.OpenMatrixFileStringCSV("mat1");
            string[,] mat2 = formatCSV.OpenMatrixFileStringCSV("mat2");

            int x1 = mat1.GetLength(1);
            int y1 = mat1.GetLength(0);
            int x2 = mat2.GetLength(1);
            int y2 = mat2.GetLength(0);

            int xT = Math.Max(x1, x2);
            int yT = y1 + y2;

            string[,] matT = new string[yT, xT];

            int count = 0;
            for (int i = 0; i < y1; i++)
            {
                for (int j = 0; j < x1; j++)
                {
                    matT[count, j] = mat1[i, j];
                }
                count++;
            }
            for (int i = 0; i < y2; i++)
            {
                for (int j = 0; j < x2; j++)
                {
                    matT[count, j] = mat2[i, j];
                }
                count++;
            }

            printFile.printMatrix(matT, "matT");

        }

        private void NotesNGDCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[,] notes = formatCSV.OpenMatrixFileStringCSV("Notes CSV");

            int samples = notes.GetLength(0);
            int columns = notes.GetLength(1);

            Console.WriteLine(samples);
            Console.WriteLine(columns);

            string[,] labels = new string[samples, 2];



            for (int i = 0; i < samples; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (notes[i, 0].Contains("OC43"))
                    {
                        labels[i, 0] = "HCoV-OC43";
                        labels[i, 1] = "2";
                    }

                    else if (notes[i, 0].Contains("Middle East respiratory syndrome"))
                    {

                        labels[i, 0] = "MERS-CoV";
                        labels[i, 1] = "1";
                    }
                    else if (notes[i, 0].Contains("229E"))
                    {
                        labels[i, 0] = "HCoV-229E";
                        labels[i, 1] = "2";
                    }
                    else if (notes[i, 0].Contains("4408"))
                    {
                        labels[i, 0] = "HCoV-4408";
                        labels[i, 1] = "2";
                    }
                    else if (notes[i, 0].Contains("betacoronavirus 2c"))
                    {
                        labels[i, 0] = "HCoV-EMC";
                        labels[i, 1] = "2";
                    }
                    else if (notes[i, 0].Contains("Betacoronavirus England 1"))
                    {
                        labels[i, 0] = "HCoV-EMC";
                        labels[i, 1] = "2";
                    }
                    else if (notes[i, 0].Contains("NL63"))
                    {
                        labels[i, 0] = "HCoV-NL63";
                        labels[i, 1] = "3";
                    }

                    else if (notes[i, 0].Contains("HKU1"))
                    {
                        labels[i, 0] = "HCoV-HKU1";
                        labels[i, 1] = "3";
                    }

                    else if (notes[i, 0].Contains("Severe acute respiratory syndrome coronavirus 2"))
                    {
                        labels[i, 0] = "SARS-CoV-2";
                        labels[i, 1] = "0";
                    }
                    else if (notes[i, 0].Contains("SARS-CoV-2"))
                    {
                        labels[i, 0] = "SARS-CoV-2";
                        labels[i, 1] = "0";
                    }
                    else if (notes[i, 0].Contains("SARS coronavirus Urbani isolate"))
                    {
                        labels[i, 0] = "SARS-CoV";
                        labels[i, 1] = "4";
                    }

                    else if (notes[i, 0].Contains("SARS coronavirus HKU-39849"))
                    {
                        labels[i, 0] = "SARS-CoV";
                        labels[i, 1] = "4";
                    }
                    else if (notes[i, 0].Contains("Severe acute respiratory syndrome-related coronavirus isolate Tor2"))
                    {
                        labels[i, 0] = "SARS-CoV";
                        labels[i, 1] = "4";
                    }
                    else if (notes[i, 0].Contains("SARS coronavirus GDH-BJH01"))
                    {
                        labels[i, 0] = "SARS-CoV";
                        labels[i, 1] = "4";
                    }
                    else if (notes[i, 0].Contains("SARS coronavirus P2"))
                    {
                        labels[i, 0] = "SARS-CoV";
                        labels[i, 1] = "4";
                    }

                    else
                        labels[i, 0] = notes[i, 0];
                }
            }

            printFile.printMatrix(labels, "labels");

        }

        void HsvToRgb(double h, double S, double V, out int r, out int g, out int b)
        {
            // ######################################################################
            // T. Nathan Mundhenk
            // mundhenk@usc.edu
            // C/C++ Macro HSV to RGB

            double H = h;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (V <= 0)
            { R = G = B = 0; }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {

                    // Red is the dominant color

                    case 0:
                        R = V;
                        G = tv;
                        B = pv;
                        break;

                    // Green is the dominant color

                    case 1:
                        R = qv;
                        G = V;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = V;
                        B = tv;
                        break;

                    // Blue is the dominant color

                    case 3:
                        R = pv;
                        G = qv;
                        B = V;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = V;
                        break;

                    // Red is the dominant color

                    case 5:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                    case 6:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // The color is not defined, we should throw an error.

                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        R = G = B = V; // Just pretend its black/white
                        break;
                }
            }
            r = Clamp((int)(R * 255.0));
            g = Clamp((int)(G * 255.0));
            b = Clamp((int)(B * 255.0));
        }

        /// <summary>
        /// Clamp a value to 0-255
        /// </summary>
        int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }

        private Color generateRGB(double X)
        {
            Color color;

            int red;
            int green;
            int blue;
            HsvToRgb(X * 360, 1, 1, out red, out green, out blue);

            color = Color.FromArgb(red, green, blue);

            return color;
        }

        public void matrixToImage(double [,] K, string name)
            {
            
            double[,] matrix = K;
        Bitmap myBitmap;
        int widthMatrix = matrix.GetLength(1);
            if (matrix.GetLength(1) < 5000)
            {

                myBitmap = new Bitmap(widthMatrix, matrix.GetLength(0));

            }
            else
            {

                myBitmap = new Bitmap(5000, matrix.GetLength(0));

            }

double max = -1e6;
double min = 1e6;
            for (int i = 0; i<matrix.GetLength(0); i++)
            {
                for (int j = 0; j<widthMatrix; j++)
                {
                    if (matrix[i, j] > max)
                        max = matrix[i, j];
                    if (matrix[i, j] < min)
                        min = matrix[i, j];
                }
            }

            for (int i = 0; i<matrix.GetLength(0); i++)
            {
                for (int j = 0; j<widthMatrix; j++)
                {
                    matrix[i, j] = (matrix[i, j] - min) / (max - min);
                }
            }

            for (int Ycount = 0; Ycount<myBitmap.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount<myBitmap.Width; Xcount++)
                {
                    Color myRgbColor = new Color();
int value = Convert.ToInt32(matrix[Ycount, Xcount] * 255);
myRgbColor = Color.FromArgb(value, value, value);
                    // myBitmap.SetPixel(Xcount, Ycount, generateRGB(matrix[Ycount,Xcount]));
                    myBitmap.SetPixel(Xcount, Ycount, myRgbColor);
                }
            }

            myBitmap.Save(name+".jpg", ImageFormat.Jpeg);
            }

        private void CreateImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Filter = "txt files(*.txt)|*.txt|csv files(*.csv)|*.csv";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.Title = "Select a Matrix File ";
            openFileDialog1.Multiselect = true;
            

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                for(int l=0;l<openFileDialog1.FileNames.GetLength(0);l++)
                {
                    int y = 0;
                    int x = 0;
                    System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileNames[l]);
                    double[,] K = null;
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
                    sr = new System.IO.StreamReader(openFileDialog1.FileNames[l]);
                    K = new double[y, x];
                    for (int i = 0; i < y; i++)
                    {
                        string rowString = sr.ReadLine();
                        string[] rowStringEntries = rowString.Split(' ', '\t', ',');

                        int stringLength = rowStringEntries.GetLength(0);

                        for (int j = 0; j < rowStringEntries.GetLength(0); j++)
                        {
                            if (rowStringEntries[j].Length > 0)
                                K[i, j] = Convert.ToDouble(rowStringEntries[j]);

                        }

                    }

                    sr.Close();

                    matrixToImage(K,l.ToString());
                }
                
            }
            else
            {
                MessageBox.Show("No file");
            }




        }

        private void GetMaxPoolAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[,] matrix = formatCSV.OpenMatrixFile("mat");
            int numberWindows = 210;
            double[,] maxPool = new double[matrix.GetLength(0), numberWindows];
            int[,] posPool = new int[matrix.GetLength(0), numberWindows];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                //for progoma; size was 25
                int maxPool_windowSize = 148;
                int pad_left_HPool = 25;
                double max = -1e6;
                int index = pad_left_HPool;
                int position = -1;
                int indexMax = 0;
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] > max)
                    {
                        max = matrix[i, j];
                        position = j;
                    }
                    index++;
                    if (index == maxPool_windowSize || j == matrix.GetLength(1) - 1)
                    {
                        maxPool[i, indexMax] = max;
                        posPool[i, indexMax] = position;
                        //Console.WriteLine(max.ToString() + " " + position.ToString());
                        max = -1e6;
                        position = -1;
                        index = 0;
                        indexMax++;
                    }


                }
            }
            printFile.printMatrix(posPool, "posPool");
            printFile.printMatrix(maxPool, "maxPool");

        }

        private void GetDNADATAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[,] matrix = formatCSV.OpenMatrixFile("OriginalDNADoubleData");
            double[,] posMatrix = formatCSV.OpenMatrixFile("posMaxPool");

            int[,] pos = new int[posMatrix.GetLength(0), posMatrix.GetLength(1)];

            for (int i = 0; i < posMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < posMatrix.GetLength(1); j++)
                {
                    pos[i, j] = Convert.ToInt32(posMatrix[i, j]);
                }
            }
            int numberFilters = 21;
            int padding = 10;
            double[,] dataDNA = new double[posMatrix.GetLength(0), 210 * numberFilters];

            for (int i = 0; i < posMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < posMatrix.GetLength(1); j++)
                {
                    int coef = pos[i, j];

                    for (int k = 0; k < padding + 1; k++)
                    {
                        if ((coef + k) < matrix.GetLength(1))
                            dataDNA[i, j * numberFilters + padding + k] = matrix[i, coef + k];
                        if ((coef - k) >= 0)
                            dataDNA[i, j * numberFilters + padding - k] = matrix[i, coef - k];
                    }

                }
            }

            string[,] dataDNAString = new string[posMatrix.GetLength(0), 210 * numberFilters];

            for (int i = 0; i < dataDNA.GetLength(0); i++)
            {
                for (int j = 0; j < dataDNA.GetLength(1); j++)
                {
                    if (dataDNA[i, j] == 0.25)
                    {
                        dataDNAString[i, j] = "C";
                    }
                    else if (dataDNA[i, j] == 0.50)
                    {
                        dataDNAString[i, j] = "T";
                    }
                    else if (dataDNA[i, j] == 0.75)
                    {
                        dataDNAString[i, j] = "G";
                    }
                    else if (dataDNA[i, j] == 1.00)
                    {
                        dataDNAString[i, j] = "A";
                    }
                    else
                    {
                        dataDNAString[i, j] = "N";
                    }
                }
            }

            printFile.printMatrix(dataDNAString, "dataDNAString");
            printFile.printMatrix(dataDNA, "dataDNA");

            string[,] dataDNAFeatures = new string[posMatrix.GetLength(0), 210];

            for (int i = 0; i < dataDNA.GetLength(0); i++)
            {
                int indexFeature = 0;
                int feature = 0;
                for (int j = 0; j < dataDNA.GetLength(1); j++)
                {

                    dataDNAFeatures[i, feature] += dataDNAString[i, j];
                    indexFeature++;
                    if (indexFeature == numberFilters)
                    {
                        feature++;
                        indexFeature = 0;
                    }

                }
            }

            printFile.printMatrix(dataDNAFeatures, "dataDNAFeatures");
        }

        private void ProcessSequencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[,] features = formatCSV.OpenMatrixFileStringCSV("features");

            string[,] sequences = formatCSV.OpenMatrixFileStringCSV("sequences");



            List<string> feats = new List<string>();
            for (int i = 0; i < features.GetLength(0); i++)
            {
                for (int j = 0; j < features.GetLength(1); j++)
                {
                    if (!(feats.Contains(features[i, j])))
                    {
                        if (!features[i, j].Contains("N"))
                            feats.Add(features[i, j]);
                    }

                }
            }



            Console.WriteLine(feats.Count);





            int[,] frequencies = new int[sequences.GetLength(0), feats.Count];

            for (int i = 0; i < sequences.GetLength(0); i++)
            {
                for (int j = 0; j < feats.Count; j++)
                {
                    //egex regex = new Regex(sequences[i, 0]);


                    int count = new Regex(Regex.Escape(feats[j])).Matches(sequences[i, 0]).Count;

                    frequencies[i, j] = count;
                    //frequencies[i,j]=regex.Matches(featsVector[j]).Count;
                }

            }

            List<string> featsRemove = new List<string>();

            for (int i = 0; i < sequences.GetLength(0); i++)
            {
                for (int j = 0; j < feats.Count; j++)
                {
                    if (frequencies[i, j] > 1)
                        if (!featsRemove.Contains(feats[j]))
                            featsRemove.Add(feats[j]);
                }

            }

            for (int i = 0; i < featsRemove.Count; i++)
            {
                feats.Remove(featsRemove[i]);
            }



            string[] featsVector = new string[feats.Count];

            for (int i = 0; i < feats.Count; i++)
            {
                featsVector[i] = feats[i];
            }
            frequencies = new int[sequences.GetLength(0), feats.Count];

            for (int i = 0; i < sequences.GetLength(0); i++)
            {
                for (int j = 0; j < feats.Count; j++)
                {
                    //egex regex = new Regex(sequences[i, 0]);


                    int count = new Regex(Regex.Escape(feats[j])).Matches(sequences[i, 0]).Count;

                    frequencies[i, j] = count;
                    //frequencies[i,j]=regex.Matches(featsVector[j]).Count;
                }

            }

            printFile.printVector(featsVector, "featsVector");

            printFile.printMatrix(frequencies, "frequencies");

            // 


        }

        private void MutateDATAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            double[,] matrixDNA = formatCSV.OpenMatrixFile("double");

            for (int i = 0; i < matrixDNA.GetLength(0); i++)
            {
                int positions = Convert.ToInt32(matrixDNA.GetLength(1) * 0.05);
                for (int j = 0; j < positions; j++)
                {
                    int position = rand.Next(0, matrixDNA.GetLength(1));

                    matrixDNA[i, position] = rand.Next(0, 5) * 0.25;


                }
            }

            printFile.printMatrix(matrixDNA, "mutated");
        }

        private void CreateImageDNAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[,] matrix = formatCSV.OpenMatrixFile("mat");
            Bitmap myBitmap;
            int widthMatrix = matrix.GetLength(1);
            if (matrix.GetLength(1) < 5000)
            {

                myBitmap = new Bitmap(widthMatrix, matrix.GetLength(0));

            }
            else
            {

                myBitmap = new Bitmap(5000, matrix.GetLength(0));

            }





            for (int Ycount = 0; Ycount < myBitmap.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < myBitmap.Width; Xcount++)
                {
                    Color myRgbColor = new Color();
                    double value = (matrix[Ycount, Xcount]);

                    if (value == 0.0)
                        myRgbColor = Color.Black;
                    else if (value == 0.25)
                        myRgbColor = Color.Blue;
                    else if (value == 0.50)
                        myRgbColor = Color.Green;
                    else if (value == 0.75)
                        myRgbColor = Color.Orange;
                    else if (value == 1.00)
                        myRgbColor = Color.Red;

                    myBitmap.SetPixel(Xcount, Ycount, myRgbColor);
                }
            }

            myBitmap.Save("image.jpg", ImageFormat.Jpeg);


        }

        private void CutMatrixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[,] matrix = formatCSV.OpenMatrixFile("values");

            int lengthCut = 1250;

            double[,] matrix0 = new double[matrix.GetLength(0), lengthCut];
            double[,] matrix1 = new double[matrix.GetLength(0), lengthCut];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                int index = 0;
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if ((j >= lengthCut) && (j < 2500))
                    {
                        matrix1[i, index] = matrix[i, j];
                        index++;
                    }
                    else if (j < lengthCut)
                    {
                        matrix0[i, j] = matrix[i, j];
                    }
                }
            }

            printFile.printMatrix(matrix0, "matrix0");
            printFile.printMatrix(matrix1, "matrix1");
        }

        private void AttachFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Multiselect = true;

            DialogResult dr = openFileDialog1.ShowDialog();

            TextWriter tw = new StreamWriter("merge.fasta");

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                // Read the files
                foreach (String file in openFileDialog1.FileNames)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(file);

                    do
                    {
                        string dummString = sr.ReadLine();
                        tw.WriteLine(dummString);

                    } while (!sr.EndOfStream);
                    tw.WriteLine();
                }
            }

            tw.Close();
        }

        private void AttachFilesTXTGISAIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Multiselect = true;

            DialogResult dr = openFileDialog1.ShowDialog();

            TextWriter tw = new StreamWriter("informationALL.txt");
            int count = 0;
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                // Read the files
                foreach (String file in openFileDialog1.FileNames)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(file);

                    do
                    {
                        string dummString = sr.ReadLine();
                        if (dummString.Contains("Additional host information:"))
                            count++;
                        tw.WriteLine(dummString);

                    } while (!sr.EndOfStream);
                    //tw.WriteLine();
                }
            }

            tw.Close();
            Console.WriteLine(count);
            string[,] matrixData = new string[count, 11];
            int indexCount = 0;
            tw = new StreamWriter("additionalHostInformation.txt");

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                // Read the files
                foreach (String file in openFileDialog1.FileNames)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(file);

                    do
                    {
                        string dummString = sr.ReadLine();
                        if (dummString.Contains("Additional host information:"))
                        {
                            dummString = dummString.Replace(",", "-");
                            matrixData[indexCount, 0] = dummString;
                            tw.WriteLine(dummString);
                            indexCount++;
                        }


                    } while (!sr.EndOfStream);
                    //tw.WriteLine();
                }
            }

            tw.Close();
            indexCount = 0;
            tw = new StreamWriter("Host.txt");

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                // Read the files
                foreach (String file in openFileDialog1.FileNames)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(file);

                    do
                    {
                        string dummString = sr.ReadLine();
                        if (dummString.Contains("Host:"))
                        {
                            dummString = dummString.Replace(",", "-");
                            matrixData[indexCount, 1] = dummString;
                            tw.WriteLine(dummString);
                            indexCount++;
                        }

                    } while (!sr.EndOfStream);
                    //tw.WriteLine();
                }
            }

            tw.Close();
            indexCount = 0;
            tw = new StreamWriter("AccessionID.txt");

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                // Read the files
                foreach (String file in openFileDialog1.FileNames)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(file);

                    do
                    {
                        string dummString = sr.ReadLine();
                        if (dummString.Contains("Accession ID:"))
                        {
                            dummString = dummString.Replace(",", "-");
                            matrixData[indexCount, 2] = dummString;
                            tw.WriteLine(dummString);
                            indexCount++;
                        }

                    } while (!sr.EndOfStream);
                    //tw.WriteLine();
                }
            }

            tw.Close();
            indexCount = 0;
            tw = new StreamWriter("Treatment.txt");

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                // Read the files
                foreach (String file in openFileDialog1.FileNames)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(file);

                    do
                    {
                        string dummString = sr.ReadLine();
                        if (dummString.Contains("Treatment:"))
                        {
                            dummString = dummString.Replace(",", "-");
                            matrixData[indexCount, 3] = dummString;
                            tw.WriteLine(dummString);
                            indexCount++;
                        }

                    } while (!sr.EndOfStream);
                    //tw.WriteLine();
                }
            }

            tw.Close();
            indexCount = 0;
            tw = new StreamWriter("PatientStatus.txt");

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                // Read the files
                foreach (String file in openFileDialog1.FileNames)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(file);

                    do
                    {
                        string dummString = sr.ReadLine();
                        if (dummString.Contains("Patient status:"))
                        {
                            dummString = dummString.Replace(",", "-");
                            matrixData[indexCount, 4] = dummString;
                            tw.WriteLine(dummString);
                            indexCount++;
                        }

                    } while (!sr.EndOfStream);
                    //tw.WriteLine();
                }
            }

            tw.Close();
            indexCount = 0;
            tw = new StreamWriter("SpecimenSource.txt");

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                // Read the files
                foreach (String file in openFileDialog1.FileNames)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(file);

                    do
                    {
                        string dummString = sr.ReadLine();
                        if (dummString.Contains("Specimen source:"))
                        {
                            dummString = dummString.Replace(",", "-");
                            matrixData[indexCount, 5] = dummString;
                            tw.WriteLine(dummString);
                            indexCount++;
                        }

                    } while (!sr.EndOfStream);
                    //tw.WriteLine();
                }
            }

            tw.Close();
            indexCount = 0;
            //Virus name:
            tw = new StreamWriter("VirusName.txt");

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                // Read the files
                foreach (String file in openFileDialog1.FileNames)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(file);

                    do
                    {
                        string dummString = sr.ReadLine();
                        if (dummString.Contains("Virus name:"))
                        {
                            dummString = dummString.Replace(",", "-");
                            matrixData[indexCount, 6] = dummString;
                            tw.WriteLine(dummString);
                            indexCount++;
                        }

                    } while (!sr.EndOfStream);
                    //tw.WriteLine();
                }
            }

            tw.Close();
            indexCount = 0;
            tw = new StreamWriter("Location.txt");

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                // Read the files
                foreach (String file in openFileDialog1.FileNames)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(file);

                    do
                    {
                        string dummString = sr.ReadLine();
                        if (dummString.Contains("Location:"))
                        {
                            dummString = dummString.Replace(",", "-");
                            matrixData[indexCount, 7] = dummString;
                            tw.WriteLine(dummString);
                            indexCount++;
                        }

                    } while (!sr.EndOfStream);
                    //tw.WriteLine();
                }
            }

            tw.Close();
            indexCount = 0;
            tw = new StreamWriter("PatientAge.txt");

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                // Read the files
                foreach (String file in openFileDialog1.FileNames)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(file);

                    do
                    {
                        string dummString = sr.ReadLine();
                        if (dummString.Contains("Patient age:"))
                        {
                            dummString = dummString.Replace(",", "-");
                            matrixData[indexCount, 8] = dummString;
                            tw.WriteLine(dummString);
                            indexCount++;
                        }

                    } while (!sr.EndOfStream);
                    //tw.WriteLine();
                }
            }

            tw.Close();
            indexCount = 0;
            tw = new StreamWriter("AdditionalLocationInformation.txt");

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                // Read the files
                foreach (String file in openFileDialog1.FileNames)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(file);

                    do
                    {
                        string dummString = sr.ReadLine();
                        if (dummString.Contains("Additional location information:"))
                        {
                            dummString = dummString.Replace(",", "-");
                            matrixData[indexCount, 9] = dummString;
                            tw.WriteLine(dummString);
                            indexCount++;
                        }

                    } while (!sr.EndOfStream);
                    //tw.WriteLine();
                }
            }

            tw.Close();
            indexCount = 0;

            printFile.printMatrix(matrixData, "matrixData");
            //
        }

        private void ProcessPositionsSequencesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string[] features = formatCSV.OpenVectorFile("features");

            string[] sequences = formatCSV.OpenVectorFile("sequences");

            double[,] freqMatrix = formatCSV.OpenMatrixFile("freqMatrix");

            string[] outputSequences = new string[freqMatrix.GetLength(0)];
            string[] outputNumbers = new string[freqMatrix.GetLength(0)];

            for (int i = 0; i < freqMatrix.GetLength(0); i++)
            {
                var numbers = new List<int>();
                string temp = "";
                string temp2 = "";
                for (int j = 0; j < freqMatrix.GetLength(1); j++)
                {
                    if (freqMatrix[i, j] == 1)
                    {
                        int val = sequences[i].IndexOf(features[j]);
                        for (int k = 0; k < 21; k++)
                        {
                            if (!numbers.Contains(val + k))
                                numbers.Add(val + k);
                        }

                        numbers.Sort();

                    }


                }

                for (int j = 0; j < numbers.Count; j++)
                {
                    temp = temp + sequences[i][numbers[j]];
                    temp2 = temp2 + "," + numbers[j];
                }

                outputSequences[i] = temp;
                outputNumbers[i] = temp2;
            }

            printFile.printVector(outputSequences, "outputSequences");
            printFile.printVector(outputNumbers, "outputNumbers");
        }

        private void ProcessFinalDataToolStripMenuItem_Click(object sender, EventArgs e)
        {


            string[] sequences = formatCSV.OpenVectorFile("sequences");

            int[] incomplete = new int[sequences.GetLength(0)];

            for (int i = 0; i < sequences.GetLength(0); i++)
            {

                int count = Regex.Matches(sequences[i], "N").Count;
                incomplete[i] = count;
            }

            printFile.printVector(incomplete, "incomplete");
        }

        private void ArrangeLocationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] sequences = formatCSV.OpenVectorFile("location");

            int[] tempInt = new int[sequences.GetLength(0)];
            string[] tempString = new string[sequences.GetLength(0)];

            for (int i = 0; i < sequences.GetLength(0); i++)
            {
                string[] words = sequences[i].Split('/');
                tempString[i] = words[0] + words[1];
            }

            printFile.printVector(tempString, "tempString");
        }

        private void FromDNAFileBigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[,] dataDNA = formatCSV.OpenMatrixFileStringCSV("dataDNA");

            Console.WriteLine(dataDNA.GetLength(0));
            Console.WriteLine(dataDNA.GetLength(1));

            int y = dataDNA.GetLength(0);

            int[] dataSize = new int[dataDNA.GetLength(0)];
            for (int i = 0; i < dataDNA.GetLength(0); i++)
            {
                dataSize[i] = dataDNA[i, 0].Length;
            }

            printFile.printVector(dataSize, "dataSize.csv");

            int max = 0;
            for (int i = 0; i < y; i++)
            {
                if (dataSize[i] > max)
                    max = dataSize[i];
            }

            Console.WriteLine("max Size " + max.ToString());

            //string[,] dataProcessed = new string[y, max];
            string[,] dataProcessedDouble = new string[y, max];




            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < dataSize[i]; j++)
                {
                    string temp = dataDNA[i, 0];
                    string temp2 = temp[j].ToString();
                    if (temp2 == "C")
                    {
                        dataProcessedDouble[i, j] = "0.25";
                    }

                    else if (temp2 == "T")
                    {
                        dataProcessedDouble[i, j] = "0.5";
                    }

                    else if (temp2 == "G")
                    {
                        dataProcessedDouble[i, j] = "0.75";
                    }

                    else if (temp2 == "A")
                    {
                        dataProcessedDouble[i, j] = "1.0";
                    }

                    else
                    {
                        dataProcessedDouble[i, j] = "0.0";
                    }

                }
            }

            printFile.printMatrix(dataProcessedDouble, "dataProcessedDouble");

        }

        private void GetAverageLabelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[,] matrix = formatCSV.OpenMatrixFile("matrix");
            int[] labels = formatCSV.OpenIntVectorFile("");

            int maxLabel = 0;

            for (int i = 0; i < labels.GetLength(0); i++)
            {
                if (labels[i] > maxLabel)
                    maxLabel = labels[i];
            }

            Console.WriteLine(maxLabel + 1);

            int labelSize = maxLabel + 1;

            double[,] avgMatrix = new double[labelSize, matrix.GetLength(1)];
            int[] count = new int[labelSize];

            for (int i = 0; i < labels.GetLength(0); i++)
            {
                count[labels[i]]++;
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    avgMatrix[labels[i], j] += matrix[i, j];
                }

            }

            for (int i = 0; i < count.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    avgMatrix[i, j] = avgMatrix[i, j] / count[i];
                }

            }

            printFile.printMatrix(avgMatrix, "avgMatrix");
            printFile.printVector(count, "count");
        }

        private void OpenFastaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[,] K = null;
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Filter = "txt files(*.txt)|*.txt|csv files(*.csv)|*.csv|fasta files(*.fasta)|*.fasta";
            openFileDialog1.FilterIndex = 3;
            openFileDialog1.Title = "Select a Matrix File";

            int examples = 0;
            int examples2 = 0;
            string[] examplesInfo = null;
            string[] examplesData = null;
            string[] examplesSize = null;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);

                do
                {
                    string dummString = sr.ReadLine();
                    if (dummString.Length > 0)
                    {
                        if (dummString[0] == '>')
                            examples++;

                    }
                    else
                    {
                        examples2++;
                    }

                } while (!sr.EndOfStream);
                Console.WriteLine("file Size " + examples.ToString());
                Console.WriteLine("file Size2 " + examples2.ToString());
                sr.Close();
                sr = new System.IO.StreamReader(openFileDialog1.FileName);
                examplesInfo = new string[examples];
                examplesData = new string[examples];
                examplesSize = new string[examples];

                int stringsDNA = -1;
                do
                {
                    string dummString = sr.ReadLine();
                    if (dummString.Length > 0)
                    {
                        if (dummString[0] == '>')
                        {

                            stringsDNA++;
                            examplesInfo[stringsDNA] = dummString.Replace("/", ",");
                            examplesInfo[stringsDNA] = examplesInfo[stringsDNA].Replace("|", ",");
                        }
                        else
                        {
                            dummString = dummString.Replace("a", "A");
                            dummString = dummString.Replace("c", "C");
                            dummString = dummString.Replace("t", "T");
                            dummString = dummString.Replace("U", "T");
                            dummString = dummString.Replace("g", "G");
                            dummString = dummString.Replace("n", "N");
                            examplesData[stringsDNA] += dummString;

                        }
                    }




                } while (!sr.EndOfStream);
                Console.WriteLine("stringsDNA Size " + stringsDNA.ToString());

                sr.Close();

                for (int i = 0; i < examples; i++)
                {
                    examplesSize[i] = examplesData[i].Length.ToString();
                }

                printFile.printVector(examplesInfo, openFileDialog1.FileName + "Info");
                printFile.printVector(examplesData, openFileDialog1.FileName + "Data");
                printFile.printVector(examplesSize, openFileDialog1.FileName + "Size");
            }

            else
            {
                MessageBox.Show("No file");
            }
        }

        private void ChangePrimersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] listPrimers = formatCSV.OpenVectorFile("primers");
            int numberPrimbers = listPrimers.GetLength(0);

            string[] reversePrimers = new string[numberPrimbers];

            for (int i = 0; i < listPrimers.Length; i++)
            {
                string s = listPrimers[i];
                char[] arr = s.ToCharArray();
                Array.Reverse(arr);
                reversePrimers[i] = new string(arr);

                reversePrimers[i] = reversePrimers[i].Replace("T", "0");
                reversePrimers[i] = reversePrimers[i].Replace("C", "1");
                reversePrimers[i] = reversePrimers[i].Replace("G", "2");
                reversePrimers[i] = reversePrimers[i].Replace("A", "3");

                reversePrimers[i] = reversePrimers[i].Replace("0", "A");
                reversePrimers[i] = reversePrimers[i].Replace("1", "G");
                reversePrimers[i] = reversePrimers[i].Replace("2", "C");
                reversePrimers[i] = reversePrimers[i].Replace("3", "T");
            }

            printFile.printVector(reversePrimers, "reversePrimers");
        }

        private void CheckFeaturesInStringsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string[,] features = formatCSV.OpenMatrixFileStringCSV("features");

            string[,] sequences = formatCSV.OpenMatrixFileStringCSV("sequences");



            List<string> feats = new List<string>();
            for (int i = 0; i < features.GetLength(0); i++)
            {
                for (int j = 0; j < features.GetLength(1); j++)
                {
                    if (!(feats.Contains(features[i, j])))
                    {
                        if (!features[i, j].Contains("N"))
                            feats.Add(features[i, j]);
                    }

                }
            }



            Console.WriteLine(feats.Count);





            int[,] frequencies = new int[sequences.GetLength(0), feats.Count];

            for (int i = 0; i < sequences.GetLength(0); i++)
            {
                for (int j = 0; j < feats.Count; j++)
                {
                    //egex regex = new Regex(sequences[i, 0]);


                    int count = new Regex(Regex.Escape(feats[j])).Matches(sequences[i, 0]).Count;

                    frequencies[i, j] = count;
                    //frequencies[i,j]=regex.Matches(featsVector[j]).Count;
                }

            }

            //List<string> featsRemove = new List<string>();

            //for (int i = 0; i < sequences.GetLength(0); i++)
            //{
            //    for (int j = 0; j < feats.Count; j++)
            //    {
            //        if (frequencies[i, j] > 1)
            //            if (!featsRemove.Contains(feats[j]))
            //                featsRemove.Add(feats[j]);
            //    }

            //}

            //for (int i = 0; i < featsRemove.Count; i++)
            //{
            //    feats.Remove(featsRemove[i]);
            //}



            string[] featsVector = new string[feats.Count];

            for (int i = 0; i < feats.Count; i++)
            {
                featsVector[i] = feats[i];
            }
            frequencies = new int[sequences.GetLength(0), feats.Count];

            for (int i = 0; i < sequences.GetLength(0); i++)
            {
                for (int j = 0; j < feats.Count; j++)
                {
                    //egex regex = new Regex(sequences[i, 0]);


                    int count = new Regex(Regex.Escape(feats[j])).Matches(sequences[i, 0]).Count;

                    frequencies[i, j] = count;
                    //frequencies[i,j]=regex.Matches(featsVector[j]).Count;
                }

            }

            printFile.printVector(featsVector, "featsVector");

            printFile.printMatrix(frequencies, "frequencies");

            // 


        }

        private void RemoveRedundantToolStripMenuItem_Click(object sender, EventArgs e)
        {


            double[,] frequencyTable = formatCSV.OpenMatrixFile("frequency Table");

            string[] features = formatCSV.OpenVectorFile("features");

            List<string> listRepeat = new List<string>();

            for (int i = 0; i < features.GetLength(0); i++)
            {
                double[] tempVector = new double[frequencyTable.GetLength(0)];

                for (int j = 0; j < frequencyTable.GetLength(0); j++)
                {
                    tempVector[j] = frequencyTable[j, i];
                }

                for (int k = 0; k < features.GetLength(0); k++)
                {
                    double[] tempVector2 = new double[frequencyTable.GetLength(0)];

                    for (int j = 0; j < frequencyTable.GetLength(0); j++)
                    {
                        tempVector2[j] = frequencyTable[j, k];
                    }

                    int suma = 0;
                    for (int j = 0; j < frequencyTable.GetLength(0); j++)
                    {
                        if (tempVector[j] == tempVector2[j])
                            suma++;
                    }
                    if (suma == frequencyTable.GetLength(0) && i != k && k > i)
                    {
                        Console.WriteLine(i.ToString() + "\t" + k.ToString());
                        Console.WriteLine(features[i] + "\t" + features[k]);
                        if (!listRepeat.Contains(features[k]))
                            listRepeat.Add(features[k]);
                    }

                }
            }

            string[] outputRepeatedList = new string[listRepeat.Count];
            for (int i = 0; i < listRepeat.Count; i++)
            {
                outputRepeatedList[i] = listRepeat[i];
            }
            string[] outputGoodList = new string[features.GetLength(0) - listRepeat.Count];
            int counter = 0;
            for (int i = 0; i < features.GetLength(0); i++)
            {
                if (!listRepeat.Contains(features[i]))
                {
                    outputGoodList[counter] = features[i];
                    counter++;
                }
            }
            printFile.printVector(outputGoodList, "outputGoodList");
            printFile.printVector(outputRepeatedList, "outputRepeatedList");


        }

        private void FrequencyTableToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void FindComplementaryMutationsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            double[,] frequencyTable = formatCSV.OpenMatrixFile("frequency Table");

            string[] features = formatCSV.OpenVectorFile("features");

            List<string> listRepeat = new List<string>();

            for (int i = 0; i < features.GetLength(0); i++)
            {
                double[] tempVector = new double[frequencyTable.GetLength(0)];

                for (int j = 0; j < frequencyTable.GetLength(0); j++)
                {
                    tempVector[j] = frequencyTable[j, i];
                }

                for (int k = 0; k < features.GetLength(0); k++)
                {
                    double[] tempVector2 = new double[frequencyTable.GetLength(0)];

                    for (int j = 0; j < frequencyTable.GetLength(0); j++)
                    {
                        tempVector2[j] = frequencyTable[j, k];
                    }

                    int suma = 0;
                    for (int j = 0; j < frequencyTable.GetLength(0); j++)
                    {
                        if (tempVector[j] != tempVector2[j])
                            suma++;
                    }
                    if (suma == frequencyTable.GetLength(0) && i != k && k > i)
                    {
                        Console.WriteLine(i.ToString() + "\t" + k.ToString());
                        Console.WriteLine(features[i] + "\t" + features[k]);
                        if (!listRepeat.Contains(features[k]))
                            listRepeat.Add(features[k]);
                    }

                }
            }

            string[] outputRepeatedList = new string[listRepeat.Count];
            for (int i = 0; i < listRepeat.Count; i++)
            {
                outputRepeatedList[i] = listRepeat[i];
            }
            string[] outputGoodList = new string[features.GetLength(0) - listRepeat.Count];
            int counter = 0;
            for (int i = 0; i < features.GetLength(0); i++)
            {
                if (!listRepeat.Contains(features[i]))
                {
                    outputGoodList[counter] = features[i];
                    counter++;
                }
            }
            printFile.printVector(outputGoodList, "outputGoodList");
            printFile.printVector(outputRepeatedList, "outputRepeatedList");
        }


        private void FrequencyTableToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            int[,] frequencies;
            string[] featsVector;
            getFrequencyTable(out frequencies, out featsVector);

            int[,] frequencyTable =(int[,]) frequencies.Clone();

            string[] features = (string[])featsVector.Clone();

            int windowSize = 21;
            string[,] sequences = formatCSV.OpenMatrixFileStringCSV("sequences");
            //for (int i = 0; i <3; i++)
            for (int i = 0; i < features.GetLength(0); i++)
            {
             

                int count0 = 0;
                int count1 = 0;

                
                for (int j = 0; j < frequencyTable.GetLength(0); j++)
                {
               
                    if (frequencyTable[j, i] == 1)
                        count1++;
                    if (frequencyTable[j, i] == 0)
                        count0++;
                }

                //Console.WriteLine(count1);
                //Console.WriteLine(count0);
                int countTotal = 0;
                List<string> listRepeat = new List<string>();
                for (int j = 0; j < windowSize; j++)
                {
                    for (int m = 0; m < 5; m++)
                    {
                        string feature = features[i];
                        StringBuilder sb = new StringBuilder(feature);
                        if (m == 0)
                            sb[j] = 'T';
                        if (m == 1)
                            sb[j] = 'C';
                        if (m == 2)
                            sb[j] = 'G';
                        if (m == 3)
                            sb[j] = 'A';
                        if (m == 4)
                            sb[j] = 'N';
                        string newFeature = sb.ToString();

                        int countFreq = 0;
                        for (int k = 0; k < sequences.GetLength(0); k++)
                        {
                            if (sequences[k,0].Contains(newFeature))
                                countFreq++;
                        }

                        if (countFreq > 0 && newFeature != feature)
                        {
                            Console.WriteLine(i.ToString()+"\t"+feature + "\t" + newFeature + "\t" 
                                + countFreq.ToString()+ "\t" + count0.ToString() + "\t" + count1.ToString());
                            countTotal += countFreq;
                            listRepeat.Add(newFeature);
                        }
                    }

                }
                  

            }

            for (int i = 0; i < features.GetLength(0); i++)
            {


                int count0 = 0;
                int count1 = 0;


                for (int j = 0; j < frequencyTable.GetLength(0); j++)
                {

                    if (frequencyTable[j, i] == 1)
                        count1++;
                    if (frequencyTable[j, i] == 0)
                        count0++;
                }

                //Console.WriteLine(count1);
                //Console.WriteLine(count0);
                int countTotal = 0;
                List<string> listRepeat = new List<string>();
                for (int j = 0; j < windowSize; j++)
                {
                    for (int m = 0; m < 5; m++)
                    {
                        string feature = features[i];
                        StringBuilder sb = new StringBuilder(feature);
                        if (m == 0)
                            sb[j] = 'T';
                        if (m == 1)
                            sb[j] = 'C';
                        if (m == 2)
                            sb[j] = 'G';
                        if (m == 3)
                            sb[j] = 'A';
                        if (m == 4)
                            sb[j] = 'N';
                        string newFeature = sb.ToString();

                        int countFreq = 0;
                        for (int k = 0; k < sequences.GetLength(0); k++)
                        {
                            if (sequences[k, 0].Contains(newFeature))
                                countFreq++;
                        }

                        if (countFreq > 0 && newFeature != feature)
                        {
                            //Console.WriteLine(i.ToString()+"\t"+feature + "\t" + newFeature + "\t" 
                            //    + countFreq.ToString()+ "\t" + count0.ToString() + "\t" + count1.ToString());
                            countTotal += countFreq;
                            listRepeat.Add(newFeature);
                        }
                    }

                }
                if (count0 == countTotal)
                {
                    //Console.WriteLine("countTotal " + i.ToString() + "\t" + count0.ToString() + "\t" + count1.ToString());
                    Console.WriteLine(features[i]);
                    for (int l = 0; l < listRepeat.Count; l++)
                    {
                        Console.WriteLine(listRepeat[l]);
                    }
                }


            }
        }

        private void CheckFeaturesInStringsWithNsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string[,] features = formatCSV.OpenMatrixFileStringCSV("features");

            string[,] sequences = formatCSV.OpenMatrixFileStringCSV("sequences");



            List<string> feats = new List<string>();
            for (int i = 0; i < features.GetLength(0); i++)
            {
                for (int j = 0; j < features.GetLength(1); j++)
                {
                    if (!(feats.Contains(features[i, j])))
                    {
                       feats.Add(features[i, j]);
                    }

                }
            }



            Console.WriteLine(feats.Count);





            int[,] frequencies = new int[sequences.GetLength(0), feats.Count];

            for (int i = 0; i < sequences.GetLength(0); i++)
            {
                for (int j = 0; j < feats.Count; j++)
                {
                    //egex regex = new Regex(sequences[i, 0]);


                    int count = new Regex(Regex.Escape(feats[j])).Matches(sequences[i, 0]).Count;

                    frequencies[i, j] = count;
                    //frequencies[i,j]=regex.Matches(featsVector[j]).Count;
                }

            }



            string[] featsVector = new string[feats.Count];

            for (int i = 0; i < feats.Count; i++)
            {
                featsVector[i] = feats[i];
            }
            frequencies = new int[sequences.GetLength(0), feats.Count];

            for (int i = 0; i < sequences.GetLength(0); i++)
            {
                for (int j = 0; j < feats.Count; j++)
                {
                    //egex regex = new Regex(sequences[i, 0]);


                    int count = new Regex(Regex.Escape(feats[j])).Matches(sequences[i, 0]).Count;

                    frequencies[i, j] = count;
                    //frequencies[i,j]=regex.Matches(featsVector[j]).Count;
                }

            }

            printFile.printVector(featsVector, "featsVector");

            printFile.printMatrix(frequencies, "frequencies");
        }

        private void CreatePatternsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] features = formatCSV.OpenVectorFile("features");
            double[,] freqTable = formatCSV.OpenMatrixFile("freqTable");

            string[] sequences = new string[freqTable.GetLength(0)];

            for (int i = 0; i < freqTable.GetLength(0); i++)
            {
                for (int j = 0; j < freqTable.GetLength(1); j++)
                {
                    if(freqTable[i,j]==1)
                    {
                        sequences[i] += features[j];
                    }
                    else
                    {
                        sequences[i] += "NNNNNNNNNNNNNNNNNNNNN";
                    }
                }
            }

            printFile.printVector(sequences, "sequences");
        }

        private void SanderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] lines = formatCSV.OpenVectorFile();

            Console.WriteLine(lines.GetLength(0));

            TextWriter tw = new StreamWriter("cleanLines" + ".csv");
            // Output in matrix format
            //tw.WriteLine(vector.Length.ToString());
            

            for (int i = 0; i < lines.GetLength(0); i++)
            {
                if (lines[i].Contains(",,,,,,,,,,,,,,,,,,,,,,,,,,,"))
                {

                    
                }
                else if(lines[i].Contains("1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,"))
                {

                }
                else if (lines[i].Contains("92974253,242011269,40974413,231468480,11126675,11159732,26346375,"))
                {

                }
                else if (lines[i].Contains("150602088,29446553,38230805,16482582,22930087,23241818,226033260,92529093,220220000,"))
                {

                }
                else
                {
                    tw.WriteLine(lines[i].ToString());
                }
                
            }

            tw.Close();
        }


        double costSeqs(string refSeq, string testSeq)
        {
            double cost = 0;

            for (int i = 0; i < refSeq.Length; i++)
            {
                if (refSeq[i] != testSeq[i])
                    cost++;
            }

            return cost;
        }

        string[] mateSeqs(string mother, string father, Random rand)
        {
            string[] sons = new string[2];


            int cut = rand.Next(mother.Length);
            int cut2= rand.Next(mother.Length);

            string son0 = mother;
            string son1 = father;

            for (int i = 0; i < mother.Length; i++)
            {
                if (i<cut)
                {
                    son0 = son0.Remove(i, 1).Insert(i, father[i].ToString());
                    son1 = son1.Remove(i, 1).Insert(i, mother[i].ToString());
                    //son1 = son1.Replace(i, mother[i]);
                }
                else
                {
                    son0 = son0.Remove(i, 1).Insert(i, mother[i].ToString());
                    son1 = son1.Remove(i, 1).Insert(i, father[i].ToString());
                }
            }

            sons[0] = son0;
            sons[1] = son1;
            return sons;
        }

        string[] mateSeqs2(string mother, string father, Random rand)
        {
            string[] sons = new string[2];


            int cut = rand.Next(mother.Length);
            int cut2 = rand.Next(mother.Length);

            int maxCut = Math.Max(cut2, cut);
            int minCut = Math.Min(cut2, cut);

            string son0 = mother;
            string son1 = father;

            for (int i = 0; i < mother.Length; i++)
            {
                if (i < minCut)
                {
                    son0 = son0.Remove(i, 1).Insert(i, father[i].ToString());
                    son1 = son1.Remove(i, 1).Insert(i, mother[i].ToString());

                }
                else if (i>=minCut && i<maxCut)
                {
                    son0 = son0.Remove(i, 1).Insert(i, mother[i].ToString());
                    son1 = son1.Remove(i, 1).Insert(i, father[i].ToString());
                }
                else
                {
                    son0 = son0.Remove(i, 1).Insert(i, father[i].ToString());
                    son1 = son1.Remove(i, 1).Insert(i, mother[i].ToString());
                }
            }

            sons[0] = son0;
            sons[1] = son1;
            return sons;
        }

        string mutateSeqs(string seq, Random rand, double percentage)
        {
            string newSeq = seq;

            if (rand.NextDouble()< percentage)
            {
                int newPos = rand.Next(seq.Length);
                double newBase = rand.NextDouble();
                if (newBase < 0.25)
                {
                    newSeq = newSeq.Remove(newPos, 1).Insert(newPos, "T");
                }
                else if (newBase >= 0.25 && newBase < 0.50)
                {
                    newSeq = newSeq.Remove(newPos, 1).Insert(newPos, "C");
                }
                else if (newBase >= 0.50 && newBase < 0.75)
                {
                    newSeq = newSeq.Remove(newPos, 1).Insert(newPos, "G");
                }
                else if (newBase >= 0.75)
                {
                    newSeq = newSeq.Remove(newPos, 1).Insert(newPos, "A");
                }
            }

            

            return newSeq;
        }

        string switchBase(Random rand)
        {
            string newBase="";
            int nBase=rand.Next(4);
            if (nBase == 0)
                newBase = "T";
            if (nBase == 1)
                newBase = "C";
            if (nBase == 2)
                newBase = "G";
            if (nBase == 3)
                newBase = "A";

            return newBase;
        }
        string mutateSeqs2(string seq, Random rand, double percentage)
        {
            int originalSize = seq.Length;
            string newSeq = seq;

            if (rand.NextDouble() < percentage)
            {
                int newPos = rand.Next(seq.Length);
                string firstBase = switchBase( rand);
                string secondBase = switchBase(rand);
                string thirdBase = switchBase(rand);

                string firstBase2 = switchBase(rand);
                string secondBase2 = switchBase(rand);
                string thirdBase2 = switchBase(rand);

                string concatBase = firstBase + secondBase + thirdBase;
                Console.WriteLine(concatBase);
                newSeq= newSeq.Insert(newPos, concatBase);
                newSeq = newSeq.Substring(0, originalSize);

            }



            return newSeq;
        }
        private void EAEvolveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] refSeqInput = formatCSV.OpenVectorFile("refSeq");
            string[] testSeqInput = formatCSV.OpenVectorFile("testSeq");

            string refSeq = refSeqInput[0];
            string testSeq = testSeqInput[0];

            Console.WriteLine(refSeq.Length);
            Console.WriteLine(testSeq.Length);

            int difSeq = refSeq.Length - testSeq.Length;
            Random rand = new Random();

            int population = 100;
            int tourSize = 5;
            double mutationRate = 0.50;

            string[] seqs = new string[population];
            double[] seqsCost= new double[population];

            for (int j = 0; j < population; j++)
            {
                testSeq = testSeqInput[0];
                for (int i = 0; i < difSeq; i++)
                {
                    int newPos = rand.Next(testSeq.Length);
                    double newBase = rand.NextDouble();
                    if (newBase < 0.25)
                    {
                        testSeq = testSeq.Insert(newPos, "T");
                    }
                    else if (newBase >= 0.25 && newBase < 0.50)
                    {
                        testSeq = testSeq.Insert(newPos, "C");
                    }
                    else if (newBase >= 0.50 && newBase < 0.75)
                    {
                        testSeq = testSeq.Insert(newPos, "G");
                    }
                    else if (newBase >= 0.75)
                    {
                        testSeq = testSeq.Insert(newPos, "A");
                    }

                }

                seqs[j] = testSeq;

                double costTemp = costSeqs(refSeq, testSeq);
                Console.WriteLine(j.ToString()+"\t"+costTemp.ToString());
                seqsCost[j] = costTemp;

            }
            string[] newSeqs = new string[population];
            double[] newCost = new double[population];

            TextWriter tw = new StreamWriter("genomeSeq" + ".csv");

                
            for (int gen = 0; gen < 1000; gen++)
            {
                for (int i = 0; i < population; i++)
                {
                    //mother
                    double minCost = 1e10;
                    int indexMinCost = -1;
                    for (int j = 0; j < tourSize; j++)
                    {
                        int indexRand = rand.Next(population);
                        if (seqsCost[indexRand] < minCost)
                        {
                            minCost = seqsCost[indexRand];
                            indexMinCost = indexRand;
                        }

                    }
                    string mother = seqs[indexMinCost];

                    //father
                    minCost = 1e10;
                    indexMinCost = -1;
                    for (int j = 0; j < tourSize; j++)
                    {
                        int indexRand = rand.Next(population);
                        if (seqsCost[indexRand] < minCost)
                        {
                            minCost = seqsCost[indexRand];
                            indexMinCost = indexRand;
                        }

                    }
                    string father = seqs[indexMinCost];

                    string[] sons = mateSeqs2(mother, father, rand);

                    //Console.WriteLine(mother);
                    //Console.WriteLine(father);
                    //Console.WriteLine(sons[0]);
                    //Console.WriteLine(sons[1]);
                    //Console.WriteLine(sons[0].Length);
                    //Console.WriteLine(sons[1].Length);

                    sons[0] = mutateSeqs2(sons[0], rand, mutationRate);
                    sons[1] = mutateSeqs2(sons[1], rand, mutationRate);

                    sons[0] = mutateSeqs(sons[0], rand, mutationRate);
                    sons[1] = mutateSeqs(sons[1], rand, mutationRate);

                    double costSon0 = costSeqs(refSeq, sons[0]);
                    double costSon1 = costSeqs(refSeq, sons[1]);



                    if (costSon0 <= costSon1)
                    {
                        newSeqs[i] = sons[0];
                        newCost[i] = costSon0;
                    }
                    else
                    {
                        newSeqs[i] = sons[1];
                        newCost[i] = costSon1;
                    }

                    Console.WriteLine(gen.ToString() + "\t" + i.ToString()
                        + "\t" + newCost[i].ToString() + "\t" + newSeqs[i].Length);

                }

                

                seqs = newSeqs;
                seqsCost = newCost;

                Array.Sort(newCost, newSeqs);

                string bestSeq = newSeqs[0];

                for (int k = 0; k < newSeqs[0].Length; k++)
                {
                    if (refSeq[k] != bestSeq[k])
                        bestSeq = bestSeq.Remove(k, 1).Insert(k, "*");
                }

                Console.WriteLine(bestSeq);

                tw.WriteLine(bestSeq);
            }

            


            tw.Close();


        }

        private void EA2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] refSeqInput = formatCSV.OpenVectorFile("refSeq");
            string[] testSeqInput = formatCSV.OpenVectorFile("testSeq");
            string[] testSeqInput2 = formatCSV.OpenVectorFile("testSeq2");

            string refSeq = refSeqInput[0];
            string testSeq = testSeqInput[0];
            string testSeq2 = testSeqInput2[0];

            Console.WriteLine("refSeq\t"+refSeq.Length);
            Console.WriteLine("testSeq\t" + testSeq.Length);
            Console.WriteLine("testSeq2\t" + testSeq2.Length);

            Random rand = new Random();

            int population = 100;
            int tourSize = 5;
            double mutationRate = 0.50;

            string[] seqs = new string[population];
            double[] seqsCost = new double[population];

            for (int j = 0; j < population; j++)
            {
                //*************************************************
                int difSeq = refSeq.Length - testSeqInput[0].Length;
                testSeq = testSeqInput[0];
                for (int i = 0; i < difSeq; i++)
                {
                    int newPos = rand.Next(testSeq.Length);
                    double newBase = rand.NextDouble();
                    if (newBase < 0.25)
                    {
                        testSeq = testSeq.Insert(newPos, "T");
                    }
                    else if (newBase >= 0.25 && newBase < 0.50)
                    {
                        testSeq = testSeq.Insert(newPos, "C");
                    }
                    else if (newBase >= 0.50 && newBase < 0.75)
                    {
                        testSeq = testSeq.Insert(newPos, "G");
                    }
                    else if (newBase >= 0.75)
                    {
                        testSeq = testSeq.Insert(newPos, "A");
                    }

                }
                //*************************************************
                difSeq = refSeq.Length - testSeqInput2[0].Length;
                testSeq2 = testSeqInput2[0];
                for (int i = 0; i < difSeq; i++)
                {
                    int newPos = rand.Next(testSeq2.Length);
                    double newBase = rand.NextDouble();
                    if (newBase < 0.25)
                    {
                        testSeq2 = testSeq2.Insert(newPos, "T");
                    }
                    else if (newBase >= 0.25 && newBase < 0.50)
                    {
                        testSeq2 = testSeq2.Insert(newPos, "C");
                    }
                    else if (newBase >= 0.50 && newBase < 0.75)
                    {
                        testSeq2 = testSeq2.Insert(newPos, "G");
                    }
                    else if (newBase >= 0.75)
                    {
                        testSeq2 = testSeq2.Insert(newPos, "A");
                    }

                }
                //*************************************************

                seqs[j] = mateSeqs2(testSeq, testSeq2, rand)[0];

                double costTemp = costSeqs(refSeq, seqs[j]);
                Console.WriteLine(j.ToString() + "\t" + costTemp.ToString());
                seqsCost[j] = costTemp;

            }
            string[] newSeqs = new string[population];
            double[] newCost = new double[population];

            TextWriter tw = new StreamWriter("genomeSeq" + ".csv");


            for (int gen = 0; gen < 1000; gen++)
            {
                for (int i = 0; i < population; i++)
                {
                    //mother
                    double minCost = 1e10;
                    int indexMinCost = -1;
                    for (int j = 0; j < tourSize; j++)
                    {
                        int indexRand = rand.Next(population);
                        if (seqsCost[indexRand] < minCost)
                        {
                            minCost = seqsCost[indexRand];
                            indexMinCost = indexRand;
                        }

                    }
                    string mother = seqs[indexMinCost];

                    //father
                    minCost = 1e10;
                    indexMinCost = -1;
                    for (int j = 0; j < tourSize; j++)
                    {
                        int indexRand = rand.Next(population);
                        if (seqsCost[indexRand] < minCost)
                        {
                            minCost = seqsCost[indexRand];
                            indexMinCost = indexRand;
                        }

                    }
                    string father = seqs[indexMinCost];

                    string[] sons = mateSeqs2(mother, father, rand);

                    //Console.WriteLine(mother);
                    //Console.WriteLine(father);
                    //Console.WriteLine(sons[0]);
                    //Console.WriteLine(sons[1]);
                    //Console.WriteLine(sons[0].Length);
                    //Console.WriteLine(sons[1].Length);

                    sons[0] = mutateSeqs2(sons[0], rand, mutationRate);
                    sons[1] = mutateSeqs2(sons[1], rand, mutationRate);

                    sons[0] = mutateSeqs(sons[0], rand, mutationRate);
                    sons[1] = mutateSeqs(sons[1], rand, mutationRate);

                    double costSon0 = costSeqs(refSeq, sons[0]);
                    double costSon1 = costSeqs(refSeq, sons[1]);



                    if (costSon0 <= costSon1)
                    {
                        newSeqs[i] = sons[0];
                        newCost[i] = costSon0;
                    }
                    else
                    {
                        newSeqs[i] = sons[1];
                        newCost[i] = costSon1;
                    }

                    Console.WriteLine(gen.ToString() + "\t" + i.ToString()
                        + "\t" + newCost[i].ToString() + "\t" + newSeqs[i].Length);

                }



                seqs = newSeqs;
                seqsCost = newCost;

                Array.Sort(newCost, newSeqs);

                string bestSeq = newSeqs[0];

                for (int k = 0; k < newSeqs[0].Length; k++)
                {
                    if (refSeq[k] != bestSeq[k])
                        bestSeq = bestSeq.Remove(k, 1).Insert(k, "*");
                }

                Console.WriteLine(bestSeq);

                tw.WriteLine(bestSeq);
            }




            tw.Close();
        }

        private void GetSeqyunMutationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] lines = formatCSV.OpenVectorFile();
            string refSeq= lines[0];

            int[] mutationsB1 = {
                3267, 5388, 6954, 23063, 23271, 23604, 23709, 24506, 24914, 27972, 
                28048, 28111, 28280, 28977
            };

            int[] mutations501 = {
                1059, 5230, 10323, 21614, 21801, 22206, 22299, 22813, 23012,
                23063, 23664, 22289, 25563, 25904, 26456, 28887
            };

            int[] mutationsP1 = {
                733, 3828,  5648, 17259, 21614, 21621, 21638, 21974, 22132,
                22812, 23012, 23063, 23525, 24642, 28167, 28512

            };

            int[] mutationsP1Set2 = {
                11288, 11296, 17259, 28269

            };

            int[] mutationsB1617 = {
                4965,  11201, 17523, 20396, 22022, 22917, 23012, 23604,
                25469, 27638, 28881, 29405

            };

            int[] mutationsMex = {
                3140, 19839, 22995, 23012, 23403, 23604, 23756


            };

            int[] mutationsB1672 = {
                21618, 22917, 22995, 23604, 24410,25469,26767,27638,27752,
                28461,28881,29402

            };

            for (int i = 0; i < mutationsB1.Length; i++)
            {
                int baseMut = mutationsB1[i] - 1;
                string sequence = "";
                for (int j = baseMut - 10; j < baseMut+11; j++)
                {
                    if (j== baseMut)
                    {
                        sequence += refSeq[j];
                    }
                    else
                    {
                        sequence += refSeq[j];
                    }
                    
                }
                Console.WriteLine(sequence);
            }
            Console.WriteLine("");
            for (int i = 0; i < mutations501.Length; i++)
            {
                int baseMut = mutations501[i] - 1;
                string sequence = "";
                for (int j = baseMut - 10; j < baseMut+11; j++)
                {
                    if (j == baseMut)
                    {
                        sequence += "x";
                    }
                    else
                    {
                        sequence += refSeq[j];
                    }

                }
                Console.WriteLine(sequence);
            }
            Console.WriteLine("mutationsP1");
            for (int i = 0; i < mutationsP1.Length; i++)
            {
                int baseMut = mutationsP1[i] - 1;
                string sequence = "";
                for (int j = baseMut - 10; j < baseMut + 11; j++)
                {
                    if (j == baseMut)
                    {
                        sequence += "x";
                    }
                    else
                    {
                        sequence += refSeq[j];
                    }

                }
                Console.WriteLine(sequence);
            }

            Console.WriteLine("");
            for (int i = 0; i < mutationsP1Set2.Length; i++)
            {
                int baseMut = mutationsP1Set2[i] - 1;
                string sequence = "";
                for (int j = baseMut - 10; j < baseMut + 11; j++)
                {
                    if (j == baseMut)
                    {
                        sequence += refSeq[j];
                    }
                    else
                    {
                        sequence += refSeq[j];
                    }

                }
                Console.WriteLine(sequence);
            }

            Console.WriteLine("");
            for (int i = 0; i < mutationsB1617.Length; i++)
            {
                int baseMut = mutationsB1617[i] - 1;
                string sequence = "";
                for (int j = baseMut - 10; j < baseMut + 11; j++)
                {
                    if (j == baseMut)
                    {
                        sequence += refSeq[j];
                    }
                    else
                    {
                        sequence += refSeq[j];
                    }

                }
                Console.WriteLine(sequence);
            }
            Console.WriteLine("");
            for (int i = 0; i < mutationsMex.Length; i++)
            {
                int baseMut = mutationsMex[i] - 1;
                string sequence = "";
                for (int j = baseMut - 10; j < baseMut + 11; j++)
                {
                    if (j == baseMut)
                    {
                        sequence += "x";
                    }
                    else
                    {
                        sequence += refSeq[j];
                    }

                }
                Console.WriteLine(sequence);
            }
            Console.WriteLine("");
            for (int i = 0; i < mutationsB1672.Length; i++)
            {
                int baseMut = mutationsB1672[i] - 1;
                string sequence = "";
                for (int j = baseMut - 10; j < baseMut + 11; j++)
                {
                    if (j == baseMut)
                    {
                        sequence += refSeq[j];
                    }
                    else
                    {
                        sequence += refSeq[j];
                    }

                }
                Console.WriteLine(sequence);
            }
        }

        private double calculateTm(string subsequence)
        {
            double Tm = 0;

            int tCount = Regex.Matches(subsequence, "T").Count;
            int cCount = Regex.Matches(subsequence, "C").Count;
            int gCount = Regex.Matches(subsequence, "G").Count;
            int aCount = Regex.Matches(subsequence, "A").Count;
            int nCount = Regex.Matches(subsequence, "N").Count;

            //Tm = 64.9 + 41 * (gCount + cCount - 16.4) /
            //    (aCount + tCount + gCount + cCount);
            double GC =(double) (gCount + cCount) / 21.0;

            Tm = 81.5 + 16.6*(Math.Log10(0.2)) + 41 * (GC) - 600 / 21;

            return Tm;
        }

        private double functionEval(string[] sequences, int[] labels, int[] seq)
        {
            double cost = 0;
            int totalSequences = sequences.GetLength(0);

            string subsequence = sequences[seq[0]].Substring(seq[1], 21);


            for (int i = 0; i < totalSequences; i++)
            {
                int contains = 0;
                if (sequences[i].Contains(subsequence))
                {
                    contains = 1;
                }
                if (labels[i] != contains)
                    cost++;

            }

            int tCount = Regex.Matches(subsequence, "T").Count;
            int cCount = Regex.Matches(subsequence, "C").Count;
            int gCount = Regex.Matches(subsequence, "G").Count;
            int aCount = Regex.Matches(subsequence, "A").Count;
            int nCount= Regex.Matches(subsequence, "N").Count;

            double NCost = (double) nCount * 1000.0;

            //50%
            double GC = (double)(gCount + cCount) / 21.0;
            double CGcountCost = Math.Abs(0.5-GC)*100;

            //Tm = 64.9 + 41 * (yG + zC - 16.4) / (wA + xT + yG + zC)
            double Tm = calculateTm(subsequence);

            double TmCost =  Math.Abs(60 - Tm);

            cost = cost+ CGcountCost + TmCost + NCost;



            return cost;
        }

        private void GECCOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] sequences = formatCSV.OpenVectorFile("sequences");
            int[] labels = formatCSV.OpenIntVectorFile("labels");
            int totalSequences = sequences.GetLength(0);
            int[] sequenceSize = new int[totalSequences];

            for (int i = 0; i < totalSequences; i++)
            {
                sequenceSize[i] = sequences[i].Length;
                //Console.WriteLine(sequenceSize[i]);
            }

            int population = 200;
            int tournament = 2;
            int runs = 100;
            double mutation = 0.15;

            int[][] genome = new int[population][];
            double[] costGenome = new double[population];
            for (int i = 0; i < population; i++)
            {
                genome[i] = new int[2];
            }

            Random randTop = new Random();

            for (int i = 0; i < population; i++)
            {
                genome[i][0] = randTop.Next(population);
                genome[i][1] = randTop.Next(sequenceSize[genome[i][0]] - 21);
                costGenome[i] = functionEval(sequences, labels, genome[i]);
                //string subsequence = sequences[genome[i][0]].Substring(genome[i][1], 21);
                //Console.WriteLine(subsequence + "\t" + costGenome[i].ToString());
            }

            int run = 0;
            int[][] genomeNew = new int[population][];
            double[] costGenomeNew = new double[population];

            //Limiting the maximum degree of parallelism to 2
            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 8
            };

            double allBestCost = int.MaxValue;
            int[] allBestGenome = new int[2];

            string datetimeString = string.Format("{0:yyyy-MM-dd_hh-mm-ss-fffff-tt}.results", DateTime.Now);
            TextWriter tw = new StreamWriter(datetimeString + ".tsv");
            
            while (run<runs )
            {
                double generationBestCost = int.MaxValue;
                int[] generationBestGenome = new int[2];


                Parallel.For(0, population / 2, options, i => {
                    Random rand = new Random();
                    //tournament

                    //create Mother
                    int bestIndex = rand.Next(population);
                    double bestCost = costGenome[bestIndex];
                    for (int j = 1; j < tournament; j++)
                    {
                        int randIndex = rand.Next(population);
                        if(costGenome[randIndex] <bestCost)
                        {
                            bestIndex = randIndex;
                            bestCost = costGenome[bestIndex];
                        }
                    }

                    int[] mother = genome[bestIndex];
                    double motherCost= costGenome[bestIndex];
                    //create Father
                    bestIndex = rand.Next(population);
                    bestCost = costGenome[bestIndex];
                    for (int j = 1; j < tournament; j++)
                    {
                        int randIndex = rand.Next(population);
                        if (costGenome[randIndex] < bestCost)
                        {
                            bestIndex = randIndex;
                            bestCost = costGenome[bestIndex];
                        }
                    }

                    int[] father = genome[bestIndex];
                    double fatherCost = costGenome[bestIndex];

                    //crossOver
                    int[] son0 = new int[2];
                    int[] son1 = new int[2];

                    son0[0] = mother[0];
                    son0[1] = father[1];



                    son1[0] = father[0];
                    son1[1] = mother[1];


                    //mutation
                    double mut0 = rand.NextDouble();
                    if (mut0 < mutation)
                        son0[0] = rand.Next(totalSequences);

                    double mut1 = rand.NextDouble();
                    if (mut1 < mutation)
                        son1[0] = rand.Next(totalSequences);

                    mut0 = rand.NextDouble();
                    if (mut0 < mutation)
                        son0[1] = rand.Next(sequenceSize[son0[0]]);

                     mut1 = rand.NextDouble();
                    if (mut1 < mutation)
                        son1[1] = rand.Next(sequenceSize[son1[0]]);

                    //note check to surpass maximum length of sequence
                    if (sequenceSize[son0[0]] < son0[1] + 21)
                        son0[1] = sequenceSize[son0[0]] - 21;

                    if (sequenceSize[son1[0]] < son1[1] + 21)
                        son1[1] = sequenceSize[son1[0]] - 21;

                    
                    double costSon0= functionEval(sequences, labels, son0);
                    double costSon1 = functionEval(sequences, labels, son1);

                    //string subsequenceTemp = sequences[son0[0]].Substring(son0[1], 21);
                    //Console.WriteLine(subsequenceTemp + "\t" + costSon0.ToString());

                    //subsequenceTemp = sequences[son1[0]].Substring(son1[1], 21);
                    //Console.WriteLine(subsequenceTemp + "\t" + costSon1.ToString());

                    genomeNew[i*2] = son0;
                    costGenomeNew[i * 2] = costSon0;

                    genomeNew[i * 2+1] = son1;
                    costGenomeNew[i * 2+1] = costSon1;

                });

                for (int i = 0; i < population; i++)
                {
                    if (costGenomeNew[i] < generationBestCost)
                    {
                        generationBestCost = costGenomeNew[i];
                        generationBestGenome = (int[])genomeNew[i].Clone();
                    }
                }

                if (generationBestCost<allBestCost)
                {
                    allBestCost = generationBestCost;
                    allBestGenome = (int[])generationBestGenome.Clone();
                }

                genome = (int[][])genomeNew.Clone();
                costGenome = (double[])costGenomeNew.Clone();

                string subsequence = sequences[generationBestGenome[0]].Substring(generationBestGenome[1], 21);
                string bestSequence= sequences[allBestGenome[0]].Substring(allBestGenome[1], 21);
                string commandLine = subsequence +
                    "\t" + generationBestCost.ToString() +
                    "\t" + calculateTm(subsequence) +
                    "\t" + bestSequence +
                    "\t" + allBestCost.ToString() +
                    "\t" + calculateTm(bestSequence);

                Console.WriteLine(commandLine);

                tw.WriteLine(commandLine);

                Console.WriteLine(run);
                run++;
            }

            tw.Close();
        }

        private void GECOO2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] sequences = formatCSV.OpenVectorFile("sequences");
            int[] labels = formatCSV.OpenIntVectorFile("labels");
            int totalSequences = sequences.GetLength(0);
            int[] sequenceSize = new int[totalSequences];

            for (int i = 0; i < totalSequences; i++)
            {
                sequenceSize[i] = sequences[i].Length;
                //Console.WriteLine(sequenceSize[i]);
            }

            int population = 200;
            int tournament = 2;
            int runs = 100;
            double mutation = 0.15;

            int[][] genome = new int[population][];
            double[] costGenome = new double[population];
            for (int i = 0; i < population; i++)
            {
                genome[i] = new int[2];
            }

            Random randTop = new Random();

            for (int i = 0; i < population; i++)
            {
                genome[i][0] = randTop.Next(population);
                genome[i][1] = randTop.Next(21500,25500);
                costGenome[i] = functionEval(sequences, labels, genome[i]);
                //string subsequence = sequences[genome[i][0]].Substring(genome[i][1], 21);
                //Console.WriteLine(subsequence + "\t" + costGenome[i].ToString());
            }

            int run = 0;
            int[][] genomeNew = new int[population][];
            double[] costGenomeNew = new double[population];

            //Limiting the maximum degree of parallelism to 2
            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 8
            };

            double allBestCost = int.MaxValue;
            int[] allBestGenome = new int[2];

            string datetimeString = string.Format("{0:yyyy-MM-dd_hh-mm-ss-fffff-tt}.results", DateTime.Now);
            TextWriter tw = new StreamWriter(datetimeString + ".tsv");

            while (run < runs)
            {
                double generationBestCost = int.MaxValue;
                int[] generationBestGenome = new int[2];


                Parallel.For(0, population / 2, options, i => {
                    Random rand = new Random();
                    //tournament

                    //create Mother
                    int bestIndex = rand.Next(population);
                    double bestCost = costGenome[bestIndex];
                    for (int j = 1; j < tournament; j++)
                    {
                        int randIndex = rand.Next(population);
                        if (costGenome[randIndex] < bestCost)
                        {
                            bestIndex = randIndex;
                            bestCost = costGenome[bestIndex];
                        }
                    }

                    int[] mother = genome[bestIndex];
                    double motherCost = costGenome[bestIndex];
                    //create Father
                    bestIndex = rand.Next(population);
                    bestCost = costGenome[bestIndex];
                    for (int j = 1; j < tournament; j++)
                    {
                        int randIndex = rand.Next(population);
                        if (costGenome[randIndex] < bestCost)
                        {
                            bestIndex = randIndex;
                            bestCost = costGenome[bestIndex];
                        }
                    }

                    int[] father = genome[bestIndex];
                    double fatherCost = costGenome[bestIndex];

                    //crossOver
                    int[] son0 = new int[2];
                    int[] son1 = new int[2];

                    son0[0] = mother[0];
                    son0[1] = father[1];



                    son1[0] = father[0];
                    son1[1] = mother[1];


                    //mutation
                    double mut0 = rand.NextDouble();
                    if (mut0 < mutation)
                        son0[0] = rand.Next(totalSequences);

                    double mut1 = rand.NextDouble();
                    if (mut1 < mutation)
                        son1[0] = rand.Next(totalSequences);

                    mut0 = rand.NextDouble();
                    if (mut0 < mutation)
                        son0[1] = randTop.Next(21500, 25500);

                    mut1 = rand.NextDouble();
                    if (mut1 < mutation)
                        son1[1] = randTop.Next(21500, 25500);


                    //note check to surpass maximum length of sequence
                    if (sequenceSize[son0[0]] < son0[1] + 21)
                        son0[1] = sequenceSize[son0[0]] - 21;

                    if (sequenceSize[son1[0]] < son1[1] + 21)
                        son1[1] = sequenceSize[son1[0]] - 21;


                    double costSon0 = functionEval(sequences, labels, son0);
                    double costSon1 = functionEval(sequences, labels, son1);

                    //string subsequenceTemp = sequences[son0[0]].Substring(son0[1], 21);
                    //Console.WriteLine(subsequenceTemp + "\t" + costSon0.ToString());

                    //subsequenceTemp = sequences[son1[0]].Substring(son1[1], 21);
                    //Console.WriteLine(subsequenceTemp + "\t" + costSon1.ToString());

                    genomeNew[i * 2] = son0;
                    costGenomeNew[i * 2] = costSon0;

                    genomeNew[i * 2 + 1] = son1;
                    costGenomeNew[i * 2 + 1] = costSon1;

                });

                for (int i = 0; i < population; i++)
                {
                    if (costGenomeNew[i] < generationBestCost)
                    {
                        generationBestCost = costGenomeNew[i];
                        generationBestGenome = (int[])genomeNew[i].Clone();
                    }
                }

                if (generationBestCost < allBestCost)
                {
                    allBestCost = generationBestCost;
                    allBestGenome = (int[])generationBestGenome.Clone();
                }

                genome = (int[][])genomeNew.Clone();
                costGenome = (double[])costGenomeNew.Clone();

                string subsequence = sequences[generationBestGenome[0]].Substring(generationBestGenome[1], 21);
                string bestSequence = sequences[allBestGenome[0]].Substring(allBestGenome[1], 21);
                string commandLine = subsequence +
                    "\t" + generationBestCost.ToString() +
                    "\t" + calculateTm(subsequence) +
                    "\t" + bestSequence +
                    "\t" + allBestCost.ToString() +
                    "\t" + calculateTm(bestSequence);

                Console.WriteLine(commandLine);

                tw.WriteLine(commandLine);

                Console.WriteLine(run);
                run++;
            }

            tw.Close();
        }

        private void CheckFeaturesInStringsALLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[,] frequencies;
            string[] featsVector;
            getFrequencyTable(out frequencies, out featsVector);

            printFile.printVector(featsVector, "featsVector");

            printFile.printMatrix(frequencies, "frequencies");

            int[] vectorSum = new int[frequencies.GetLength(1)];


            for (int j = 0; j < frequencies.GetLength(1); j++)
            {
                int sum = 0;
                for (int i = 0; i < frequencies.GetLength(0); i++)
                {
                    sum += frequencies[i, j];
                }
                vectorSum[j] = sum;
            }
            printFile.printVector(vectorSum, "vectorSum");
            // 

        }

        private static void getFrequencyTable(out int[,] frequencies, out string[] featsVector)
        {
            string[,] features = formatCSV.OpenMatrixFileStringCSV("features");

            string[,] sequences = formatCSV.OpenMatrixFileStringCSV("sequences");



            List<string> feats = new List<string>();
            for (int i = 0; i < features.GetLength(0); i++)
            {
                for (int j = 0; j < features.GetLength(1); j++)
                {

                    feats.Add(features[i, j]);

                }
            }



            Console.WriteLine(feats.Count);





            frequencies = new int[sequences.GetLength(0), feats.Count];
            for (int i = 0; i < sequences.GetLength(0); i++)
            {
                for (int j = 0; j < feats.Count; j++)
                {
                    //egex regex = new Regex(sequences[i, 0]);


                    int count = new Regex(Regex.Escape(feats[j])).Matches(sequences[i, 0]).Count;

                    frequencies[i, j] = count;
                    //frequencies[i,j]=regex.Matches(featsVector[j]).Count;
                }

            }

            //List<string> featsRemove = new List<string>();

            //for (int i = 0; i < sequences.GetLength(0); i++)
            //{
            //    for (int j = 0; j < feats.Count; j++)
            //    {
            //        if (frequencies[i, j] > 1)
            //            if (!featsRemove.Contains(feats[j]))
            //                featsRemove.Add(feats[j]);
            //    }

            //}

            //for (int i = 0; i < featsRemove.Count; i++)
            //{
            //    feats.Remove(featsRemove[i]);
            //}



            featsVector = new string[feats.Count];
            for (int i = 0; i < feats.Count; i++)
            {
                featsVector[i] = feats[i];
            }
            frequencies = new int[sequences.GetLength(0), feats.Count];

            for (int i = 0; i < sequences.GetLength(0); i++)
            {
                for (int j = 0; j < feats.Count; j++)
                {
                    //egex regex = new Regex(sequences[i, 0]);


                    int count = new Regex(Regex.Escape(feats[j])).Matches(sequences[i, 0]).Count;

                    frequencies[i, j] = count;
                    //frequencies[i,j]=regex.Matches(featsVector[j]).Count;
                }

            }
        }
    }
}
    



