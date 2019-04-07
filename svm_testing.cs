using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Accord.MachineLearning;
using edu.stanford.nlp.ie.crf;
using Accord.IO;
using Accord.MachineLearning.VectorMachines;
using edu.stanford.nlp.process;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.trees;
using edu.stanford.nlp.parser.lexparser;
using Accord.Statistics.Analysis;
using Accord.Statistics.Models.Regression;
using Accord.Statistics.Models.Regression.Fitting;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Math.Optimization.Losses;
using Accord.Statistics;
using Accord.Statistics.Kernels;
using Accord.Controls;


namespace ner
{

    class Program
    {

        public static string[] GetStringInBetween(string strBegin, string strEnd, string strSource, bool includeBegin, bool includeEnd)
        {
            //double a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20, a21, a22, a23, a24, a25, a26, a27;

            string[] result = { "", "" };
            int iIndexOfBegin = strSource.IndexOf(strBegin);
            if (iIndexOfBegin != -1)
            {
                // include the Begin string if desired
                if (includeBegin)
                    iIndexOfBegin -= strBegin.Length;
                strSource = strSource.Substring(iIndexOfBegin
                    + strBegin.Length);
                int iEnd = strSource.IndexOf(strEnd);
                if (iEnd != -1)
                {
                    // include the End string if desired
                    if (includeEnd)
                        iEnd += strEnd.Length;
                    result[0] = strSource.Substring(0, iEnd);
                    // advance beyond this segment
                    if (iEnd + strEnd.Length < strSource.Length)
                        result[1] = strSource.Substring(iEnd
                            + strEnd.Length);
                }
            }
            else
                // stay where we are
                result[1] = strSource;
            return result;
        }
        public static int isContain(string arg1, string arg2)
        {
            string[] parts = arg1.Split(' ');
            foreach (string i in parts)
            {
                if (arg2.Equals(i))
                    return 1;
            }
            return 2;

        }
        static void Main()
        {
            // Path to the folder with classifiers models
            var jarRoot = @"\Users\devir\OneDrive\Documents\Visual Studio 2015\Projects\ner";
            var classifiersDirecrory = jarRoot + @"\classifiers";

            // Loading 3 class classifier model
            var classifier = CRFClassifier.getClassifierNoExceptions(
                classifiersDirecrory + @"\english.muc.7class.distsim.crf.ser.gz");

            var s1 = " She got up this morning at 9:00 am and went to a shop to spend five dollars to buy a 50% off toothbrush.";


            var s2 = "Tell the latest on olympics from the New York.";
            Console.WriteLine("{0}\n", classifier.classifyToCharacterOffsets(s1));
            Console.WriteLine("{0}\n", classifier.classifyWithInlineXML(s1));

            //MUNCULIN NER SATU SATU
            string result = classifier.classifyWithInlineXML(s1);
            String substr1 = "TIME";
            String substr2 = "LOCATION";
            String substr3 = "PERSON";
            String substr4 = "ORGANIZATION";
            String substr5 = "MONEY";
            String substr6 = "Percent";
            String substr7 = "Date";
            string total1, total2, total3, total4, total5, total6, total7;

            //if (result.Contains(substr1))
            //{
            //    string[] hasiltime = GetStringInBetween("<TIME>", "</TIME>", result, false, false);
            //    string output_time = hasiltime[0];
            //    string next_time = hasiltime[1];
            //    total1 = output_time;
            //   // Console.WriteLine(output_time);
            //}
            //if (result.Contains(substr2))
            //{
            //    string[] hasillocation = GetStringInBetween("<LOCATION>", "</LOCATION>", result, false, false);
            //    string output_location = hasillocation[0];
            //    string next_loc = hasillocation[1];
            //    //Console.WriteLine(output_location);
            //    total2 = output_location;
            //}
            //if (result.Contains(substr3))
            //{
            //    string[] hasilperson = GetStringInBetween("<PERSON>", "</PERSON>", result, false, false);
            //    string output_person = hasilperson[0];
            //    string next_person = hasilperson[1];
            //    //Console.WriteLine(hasilperson);
            //    total3 = output_person;
            //}
            //if (result.Contains(substr4))
            //{
            //    string[] hasilORGANIZATION = GetStringInBetween("<ORGANIZATION>", "</ORGANIZATION>", result, false, false);
            //    string output_ORGANIZATION = hasilORGANIZATION[0];
            //    string next_ORGANIZATION = hasilORGANIZATION[1];
            //    //Console.WriteLine(output_ORGANIZATION);
            //    total4 = output_ORGANIZATION;
            //}
            //if (result.Contains(substr5))
            //{
            //    string[] hasilMONEY = GetStringInBetween("<MONEY>", "</MONEY>", result, false, false);
            //    string output_MONEY = hasilMONEY[0];
            //    string next_MONEY = hasilMONEY[1];
            //    // Console.WriteLine(output_MONEY);
            //    total5 = output_MONEY;
            //}
            //if (result.Contains(substr6))
            //{
            //    string[] hasilPercent = GetStringInBetween("<Percent>", "</Percent>", result, false, false);
            //    string output_Percent = hasilPercent[0];
            //    string next_Percent = hasilPercent[1];
            //    //Console.WriteLine(output_Percent);
            //    total6 = output_Percent;
            //}
            //if (result.Contains(substr7))
            //{
            //    string[] hasilDate = GetStringInBetween("<Date>", "</Date>", result, false, false);
            //    string output_Date = hasilDate[0];
            //    string next_Date = hasilDate[1];
            //    //Console.WriteLine(output_Date);
            //    total7 = output_Date;

            //}

            string[] hasiltime = GetStringInBetween("<TIME>", "</TIME>", result, false, false);
            string output_time = hasiltime[0];
            string next_time = hasiltime[1];
            total1 = output_time;
            //Console.WriteLine(output_time);

            string[] hasillocation = GetStringInBetween("<LOCATION>", "</LOCATION>", result, false, false);
            string output_location = hasillocation[0];
            string next_loc = hasillocation[1];
            //Console.WriteLine(output_location);
            total2 = output_location;

            string[] hasilperson = GetStringInBetween("<PERSON>", "</PERSON>", result, false, false);
            string output_person = hasilperson[0];
            string next_person = hasilperson[1];
            //Console.WriteLine(hasilperson);
            total3 = output_person;

            string[] hasilORGANIZATION = GetStringInBetween("<ORGANIZATION>", "</ORGANIZATION>", result, false, false);
            string output_ORGANIZATION = hasilORGANIZATION[0];
            string next_ORGANIZATION = hasilORGANIZATION[1];
            //Console.WriteLine(output_ORGANIZATION);
            total4 = output_ORGANIZATION;

            string[] hasilMONEY = GetStringInBetween("<MONEY>", "</MONEY>", result, false, false);
            string output_MONEY = hasilMONEY[0];
            string next_MONEY = hasilMONEY[1];
            // Console.WriteLine(output_MONEY);
            total5 = output_MONEY;

            string[] hasilPercent = GetStringInBetween("<Percent>", "</Percent>", result, false, false);
            string output_Percent = hasilPercent[0];
            string next_Percent = hasilPercent[1];
            //Console.WriteLine(output_Percent);
            total6 = output_Percent;

            string[] hasilDate = GetStringInBetween("<Date>", "</Date>", result, false, false);
            string output_Date = hasilDate[0];
            string next_Date = hasilDate[1];
            //Console.WriteLine(output_Date);
            total7 = output_Date;


            //BOW
            string semua = total1 + ";" + total2 + ";" + total3 + ";" + total4 + ";" + total5 + ";" + total6 + ";" + total7 + ";";
            Console.WriteLine(semua);
            string[] gabungan = { total1, total2, total3, total4, total5, total6, total7 };

            foreach (var a in gabungan)
            {
                Console.WriteLine(a);

            }
            string[][] words = gabungan.Tokenize();
            //var codebook = new TFIDF()
            //{
            //    Tf = TermFrequency.Log,
            //    Idf = InverseDocumentFrequency.Default
            //};
            var codebook = new BagOfWords()
            {
                MaximumOccurance = 1 // the resulting vector will have only 0's and 1's
            };
            codebook.Learn(words);
            double[] bow1 = codebook.Transform(words[0]);
            double[] bow2 = codebook.Transform(words[1]);
            double[] bow3 = codebook.Transform(words[2]);
            double[] bow4 = codebook.Transform(words[3]);
            double[] bow5 = codebook.Transform(words[4]);
            double[] bow6 = codebook.Transform(words[5]);
            double[] bow7 = codebook.Transform(words[6]);
            double[][] keseluruhanBOW1 = { bow1, bow2, bow3, bow4, bow5, bow6, bow7 };

            //coba 
            bool quitNow = false;
            while (!quitNow)
            {
                string val;
                Console.Write("Enter question: ");
                val = Console.ReadLine();
                string[] textss =
            {
                val,
            };



                string[][] wordss = textss.Tokenize();
                //var codebook2 = new TFIDF()
                //{
                //    Tf = TermFrequency.Log,
                //    Idf = InverseDocumentFrequency.Default
                //};
                var codebook2 = new BagOfWords()
                {
                    MaximumOccurance = 1 // the resulting vector will have only 0's and 1's
                };
                codebook2.Learn(wordss);
                double[] c1 = codebook2.Transform(wordss[0]);
                string path = @"C:\Users\devir\OneDrive\Documents\Visual Studio 2015\Projects\ner";
                //var load_svm_model = Serializer.Load<MulticlassClassifierBase>(Path.Combine(path, "pelatihanSVMbayardanpergi.bin"));
                

                //LibSvmModel modela = LibSvmModel.Load(Path.Combine(path, "pelatihanSVMbayardanpergi.bint"));
                //int jawaban = load_svm_model.Decide( c1); // answer will be 2.
                // Now, we can use the model class to create the equivalent Accord.NET SVM:
                
                //Console.WriteLine(jawaban);
                LibSvmModel model = LibSvmModel.Load(Path.Combine(path, "pelatihanSVMbayardanpergi.txt"));

                // Now, we can use the model class to create the equivalent Accord.NET SVM:
                SupportVectorMachine svm = model.CreateMachine();

                // Compute classification error
                bool predicted = svm.Decide( c1 );

                // var machine = teacher.Learn(inputs, outputs);

                if (predicted == false)
                {
                    Console.WriteLine("BAYAR");
                };
                if (predicted == true)
                {
                    Console.WriteLine("PERGI");
                };
                Console.ReadLine();




            }

            // In order to convert any 2d array to jagged one
            // let's use a generic implementation

        }

    }
}