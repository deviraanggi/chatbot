using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using ConsoleQLearning;

namespace rnn
{

    public partial class Form1 : Form
    {
        public string hasil_jawaban;
        public int index_jawaban;
        
        public int isContain(string arg1, string arg2)
        {
            string[] parts = arg1.Split(' ');
            foreach (string i in parts)
            {
                if (arg2.Equals(i))
                    return 1;
            }
            return 2;
            
        }
        
        public string tfidfcosine(string query)
        {
            //tujuan, harga, terima_kasih, selamat_pagi, dealing
            //daftarkan pertanyaan dan jawaban
            string[] pertanyaan = { "wanna go to", "how much", "thankyou", "good morning", "i will take it" };
            string[] jawaban = { "tujuan", "harga", "terima_kasih", "selamat_pagi", "dealing" };

            query = query.ToLower();
            query = query.Replace("i am", "");
            query = query.Replace("this", "");
            string[] query_parts = query.Split(' ');
            //buat daftar kata
            List<string> unique_words = new List<string>();
            foreach (string i in pertanyaan)
            {
                string[] i_parts = i.Split(' ');
                foreach (string j in i_parts)
                {
                    if (!unique_words.Contains(j))
                    {
                        unique_words.Add(j);
                    }
                }
            }
            string[] q_parts = query.Split(' ');
            foreach (string i in q_parts)
            {
                if (!unique_words.Contains(i))
                {
                    unique_words.Add(i);
                }
            }

            //foreach (string i in unique_words) Console.WriteLine(i);


            //hitung nilai tf
            String fcontent = "PERHITUNGAN TF\n";
            List<List<int>> tf = new List<List<int>>();
            foreach (string kata in unique_words)
            {
                List<int> tfkata = new List<int>();
                if (isContain(query, kata) == 1)
                {
                    fcontent += "nilai tf kata " + kata + "dengan kalimat " + query + "\t\t=1\n";
                    tfkata.Add(1);
                }
                else
                {
                    tfkata.Add(0);
                    fcontent += "nilai tf kata " + kata + "dengan kalimat " + query + "\t\t=0\n";
                }
                foreach (string D in pertanyaan)
                {
                    if (isContain(D, kata) == 1)
                    {
                        fcontent += "nilai tf kata " + kata + "dengan kalimat " + D + "\t\t=1\n";
                        tfkata.Add(1);
                    }
                    else
                    {
                        fcontent += "nilai tf kata " + kata + "dengan kalimat " + D + "\t\t=0\n";
                        tfkata.Add(0);
                    }
                }
                tf.Add(tfkata);

            }
            Console.WriteLine(fcontent);


            fcontent = "PERHITUNGAN DF\n";
            int ind = 0;
            //hitung nilai df
            List<int> df = new List<int>();
            foreach (List<int> i in tf)
            {
                int total = 0;
                foreach (int j in i)
                {
                    total += j;
                }
                df.Add(total);
                ind++;
                fcontent += "Nilai DF " + ind.ToString() + " = " + total.ToString() + "\n";
            }
            Console.WriteLine(fcontent);
            /*
            foreach (int i in df)
            {
                Console.WriteLine(i);
            }*/



            //hitung nilai idf
            fcontent = "PERHITUNGAN IDF\n";
            ind = 0;
            List<double> idf = new List<double>();
            foreach (int i in df)
            {
                idf.Add((Math.Log10((double)pertanyaan.Length + 1) / (double)i) + 1);
                ind++;
                fcontent += "Nilai IDF " + ind.ToString() + " = " + ((Math.Log10((double)pertanyaan.Length + 1) / (double)i) + 1).ToString() + "\n";
            }
            Console.WriteLine(fcontent);
            //hitung nilai wdt;
            fcontent = "PERHITUNGAN WDT\n";
            ind = 1;
            int ind2 = 1;
            List<List<double>> wdt = new List<List<double>>();
            int index_idf = 0;
            foreach (List<int> i in tf)
            {
                List<double> wdtkata = new List<double>();
                foreach (int j in i)
                {
                    if (j == 1)
                    {
                        fcontent += "nilai wdt kolom " + ind.ToString() + " baris " + ind2.ToString() + " = " + (idf[index_idf]).ToString() + "\n";
                        wdtkata.Add(idf[index_idf]);
                        ind++;
                    }
                    else
                    {
                        fcontent += "nilai wdt kolom " + ind.ToString() + " baris " + ind2.ToString() + " = " + (0).ToString() + "\n";
                        wdtkata.Add(0);
                        ind++;
                    }
                }
                wdt.Add(wdtkata);
                index_idf++;
                ind2++;
                ind = 1;
            }
            Console.WriteLine(fcontent);
            //hitung nilai wd
            fcontent = "PERHITUNGAN WD\n";
            ind = ind2 = 1;
            List<List<double>> wd = new List<List<double>>();
            foreach (List<double> i in wdt)
            {
                List<double> wdkata = new List<double>();
                int index = 0;
                double index_val = 0;
                foreach (double j in i)
                {
                    if (index == 0)
                    {
                        index_val = j;
                    }
                    else
                    {
                        fcontent += "nilai wd kolom " + ind.ToString() + " baris " + ind2.ToString() + " = " + (j * index_val).ToString() + "\n";
                        wdkata.Add(j * index_val);
                    }
                    index++;
                }
                wd.Add(wdkata);
                ind2++;
                ind = 1;
            }
            Console.WriteLine(fcontent);
            List<double> total_wd = new List<double>();
            foreach (List<double> i in wd)
            {
                foreach (double j in i)
                {
                    total_wd.Add(0);
                }
                break;
            }

            foreach (List<double> i in wd)
            {
                int index = 0;
                foreach (double j in i)
                {
                    total_wd[index] += j;
                    index++;

                }
            }
            fcontent = "TOTAL WD\n";
            ind = 1;
            foreach (double total in total_wd)
            {
                fcontent = "Total WD kolom " + ind.ToString() + " = " + total.ToString() + "\n";
            }
            Console.WriteLine(fcontent);
            //hitung nilai panjang vektor
            fcontent = "PERHITUNGAN PANJANG VEKTOR\n";
            ind = ind2 = 1;
            List<List<double>> panjang_vektor = new List<List<double>>();
            foreach (List<double> i in wdt)
            {
                List<double> vektor = new List<double>();
                foreach (double j in i)
                {
                    vektor.Add(j * j);
                    fcontent += "nilai panjang vektor kolom " + ind.ToString() + " baris " + ind2.ToString() + " = " + (j * j).ToString() + "\n";
                    ind++;
                }
                panjang_vektor.Add(vektor);
                ind = 1;
                ind2++;
            }
            Console.WriteLine(fcontent);

            //hitung total panjang vektor
            fcontent = "PERHITUNGAN TOTAL PANJANG VEKTOR\n";
            ind = ind2 = 1;
            List<double> total_panjang_vektor = new List<double>();
            foreach (List<double> i in panjang_vektor)
            {
                foreach (double j in i)
                {
                    total_panjang_vektor.Add(0);
                }
            }
            foreach (List<double> i in panjang_vektor)
            {
                int index = 0;
                foreach (double j in i)
                {
                    total_panjang_vektor[index] += j;
                    index++;
                }
            }
            foreach (double total in total_panjang_vektor)
            {
                fcontent = "Total Panjang Vektor kolom " + ind.ToString() + " = " + total.ToString() + "\n";
                ind++;
            }
            Console.WriteLine(fcontent);


            fcontent = "PERHITUNGAN AKAR PANJANG VEKTOR\n";
            ind = 1;
            //hitung akar panjang vektor
            List<double> akar_panjang_vektor = new List<double>();
            foreach (double i in total_panjang_vektor)
            {
                akar_panjang_vektor.Add(Math.Sqrt(i));
                fcontent = "Akar Panjang Vektor Kolom " + ind.ToString() + " = " + (Math.Sqrt(i)).ToString() + "\n";
                ind++;
            }
            Console.WriteLine(fcontent);


            fcontent = "PERHITUNGAN COSINE\n";
            ind = 1;
            //hitung nilai cosine
            List<double> cosine = new List<double>();
            //string debug_txt = "";
            for (int i = 0; i < pertanyaan.Length; i++)
            {
                cosine.Add(total_wd[i] / (akar_panjang_vektor[0] * akar_panjang_vektor[i + 1]));
                fcontent = "Cosine Kolom " + ind.ToString() + " = " + (total_wd[i] / (akar_panjang_vektor[0] * akar_panjang_vektor[i + 1])).ToString() + "\n";
                ind++;
                //debug_txt += (total_wd[i] / (akar_panjang_vektor[0] * akar_panjang_vektor[i + 1])).ToString();
                //debug_txt += "\n";
            }
            Console.WriteLine(fcontent);

            Console.WriteLine("Bobot");
            for (int i = 0; i < pertanyaan.Length; i++)
            {
                Console.Write(pertanyaan[i] + ":");
                Console.WriteLine(cosine[i]);
            }


            //hitung nilai tertinggi
            //label2.Text = cosine.Max().ToString();
            int index_tertinggi = cosine.IndexOf(cosine.Max());
            if (cosine.Max() == 0)
            {
                //hasil_jawaban = "Maaf saya tidak mengerti apa yang kamu inginkan mohon ucapkan kembali";
                //index_jawaban = index_tertinggi;
                index_jawaban = jawaban.Length - 1;
                index_tertinggi = index_jawaban;
                return jawaban[index_tertinggi];
                //return "Maaf saya tidak mengerti apa yang kamu inginkan mohon ucapkan kembali";
            }
            else
            {
                hasil_jawaban = jawaban[index_tertinggi];
                index_jawaban = index_tertinggi;
                Console.WriteLine(index_jawaban);

                string answer = jawaban[index_tertinggi];
                ind = 0;
                foreach (double c in cosine)
                {
                    if (c == cosine.Max() && ind != index_tertinggi)
                    {
                        answer += ";" + jawaban[ind];
                    }
                    ind++;
                }

                //return jawaban[index_tertinggi];
                return answer;

            }
        }
        // Given an input string, finds a response 
        public Form1()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {

            
            string query = textBox1.Text;
            string hasil_cosine = tfidfcosine(query);
            string[] parts1 = hasil_cosine.Split(';');
            string hasilkalimat;

            if (parts1.Length > 1)
            {
                label2.Text = "Hasil Metode TF-IDF-COSINE Similarity 1: " + parts1[0];
                hasilkalimat = parts1[0];
            }
            else {
                label2.Text = "Hasil Metode TF-IDF-COSINE Similarity: " + parts1[0];
                hasilkalimat = parts1[0];
            }

           
            

            //label1.Text = tfidfcosine(query);
            //label1.Text = jaccard_similarity(query);
            //WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();

            //if (index_jawaban == 0)
            //{
            //    wplayer.URL = "obat_minum.mp3";
            //    wplayer.controls.play();
            //}
            //if (index_jawaban == 0)
            //{
            //    wplayer.URL = "obat_minum.mp3";
            //    wplayer.controls.play();
            //}
            //if (index_jawaban == 1)
            //{
            //    wplayer.URL = "obat.mp3";
            //    wplayer.controls.play();
            //}
            //if (index_jawaban == 2)
            //{
            //    wplayer.URL = "nonton_minum.mp3";
            //    wplayer.controls.play();
            //}
            //if (index_jawaban == 3)
            //{
            //    wplayer.URL = "nonton.mp3";
            //    wplayer.controls.play();
            //}
            //if (index_jawaban == 3)
            //{
            //    wplayer.URL = "remot.mp3";
            //    wplayer.controls.play();
            //}
            //if (index_jawaban == 4)
            //{
            //    wplayer.URL = "mandi_minum.mp3";
            //    wplayer.controls.play();
            //}
            //if (index_jawaban == 5)
            //{
            //    wplayer.URL = "sabun.mp3";
            //    wplayer.controls.play();
            //}
            //if (index_jawaban == 6)
            //{
            //    wplayer.URL = "minum.mp3";
            //    wplayer.controls.play();
            //}




            ///REINFORCEMENT
            QLearning q = new QLearning
            {
                Episodes = 1000,
                Alpha = 0.1,
                Gamma = 0.9,
                MaxExploreStepsWithinOneEpisode = 1000
            };

            var opponentStyles = new List<double[]>();
            // kemungkinan model final state (opponent)
            //tujuan,harga,terimakasih,selamatpagi,dealing
            opponentStyles.Add(new[] { 0.2, 0.2, 0.2, 0.2, 0.2 }); //begin
            opponentStyles.Add(new[] { 0.1, 0.6, 0.1, 0.1, 0.1 }); //tujuan
            opponentStyles.Add(new[] { 0.1, 0.1, 0.1, 0.1, 0.6 }); //harga
            opponentStyles.Add(new[] { 0.1, 0.6, 0.1, 0.1, 0.1 });//terimakasih
            opponentStyles.Add(new[] { 0.1, 0.1, 0.6, 0.1, 0.1 });//selamat pagi
            opponentStyles.Add(new[] { 0.1, 0.1, 0.1, 0.1, 0.6});//dealing
            int index = new Random().Next(opponentStyles.Count);
            var opponent = opponentStyles[index];

            // opponent probability pick 
            double tujuanOpponent = opponent[0];
            double hargaOpponent = opponent[1];
            double terimakasihOpponent = opponent[2];
            double selamatpagiOpponent = opponent[3];
            double dealingOpponent = opponent[4];

            QAction fromTo;
            QState state;
            string stateName;
            string stateNameNext;

            // ----------- Begin Insert the path setup here -----------
            // insert the end states here
            q.EndStates.Add(StateNameEnum.tujuan.EnumToString());
            q.EndStates.Add(StateNameEnum.harga.EnumToString());
            q.EndStates.Add(StateNameEnum.terima_kasih.EnumToString());
            q.EndStates.Add(StateNameEnum.selamat_pagi.EnumToString());
            q.EndStates.Add(StateNameEnum.dealing.EnumToString());

            // State Begin sembarang ini
            stateName = StateNameEnum.tujuan.EnumToString();
            q.AddState(state = new QState(stateName, q));

            //Console.Write("Masukkan perintah: ");
            //name = Console.ReadLine();
            //Console.WriteLine("Perintah = {0}", name);

            //string testString;
            //Console.Write("Enter a string - ");
            //testString = Console.ReadLine();
            //Console.WriteLine("You entered '{0}'", testString);

            // action Pusing
            //if (name == "pusing")

            if(hasilkalimat == "tujuan")
            {
                stateNameNext = StateNameEnum.tujuan.EnumToString();
                state.AddAction(fromTo = new QAction(stateName, new QActionName(stateName, stateNameNext)));
                // action outcome probability
                //NAMA NEXT NYA
                fromTo.AddActionResult(new QActionResult(fromTo, StateNameEnum.tujuan.EnumToString(),
                    tujuanOpponent)
                { Reward = 0 });
                fromTo.AddActionResult(new QActionResult(fromTo, StateNameEnum.harga.EnumToString(),
                    hargaOpponent)
                { Reward = 10 });
                fromTo.AddActionResult(new QActionResult(fromTo, StateNameEnum.terima_kasih.EnumToString(),
                    terimakasihOpponent)
                { Reward = -10 });
                fromTo.AddActionResult(new QActionResult(fromTo, StateNameEnum.selamat_pagi.EnumToString(),
                    selamatpagiOpponent)
                { Reward = -10 });
                fromTo.AddActionResult(new QActionResult(fromTo, StateNameEnum.dealing.EnumToString(),
                    dealingOpponent)
                { Reward = -10 });
                q.RunTraining();
                q.PrintQLearningStructure();
                q.ShowPolicy();

                //Console.WriteLine("\n** Opponent style **");
                Console.WriteLine(string.Format("style is tujuan {0} harga {1} terimakasih {2} selamatpagi {3} dealing",
                    opponent[0].Pretty(), opponent[1].Pretty(), opponent[2].Pretty(), opponent[3].Pretty(), opponent[4].Pretty()));
                //label5.Text= string.Format("Model: Pusing {0} Panas {1} Minum {2} Mandi {3} Bosan {4} Remote {5} Obat {6}",
                //opponent[0].Pretty(), opponent[1].Pretty(), opponent[2].Pretty(), opponent[3].Pretty(), opponent[4].Pretty(), opponent[5].Pretty(), opponent[6].Pretty());
                
                    double max = Double.MinValue;
                    string actionName = "nothing";
                foreach (QAction action in state.Actions)
                {
                    foreach (QActionResult actionResult in action.ActionsResult)
                    {
                        if (actionResult.QEstimated > 0.15)
                        {
                            max = actionResult.QEstimated;
                            actionName = action.ActionName.ToString();
                            Console.WriteLine("Jawaban= " + actionResult + "?");
                            label5.Text = "Jawaban= " + actionResult + "?" + "\n";
                            //textBox2.Text = "Apakah anda ingin " + actionResult + " ? " + "\n";
                        }


                    }

                }
                    ////dibukani
                    //Console.WriteLine(string.Format("From state {0} do action {1}, max QEstimated is {2}",
                    //  state.StateName, actionName, max.Pretty()));
                    //Console.WriteLine(string.Format("Dari State {0} melakukan aksi {1} didapatkan Estimasi Nilai Q maksimal = {2}",
                     //  state.StateName, actionName, max.Pretty()));
                }




            //// action Panas
            if (hasilkalimat == "harga")
            {
                stateNameNext = StateNameEnum.harga.EnumToString();
                state.AddAction(fromTo = new QAction(stateName, new QActionName(stateName, stateNameNext)));
                // action outcome probability
                //NAMA NEXT NYA
                fromTo.AddActionResult(new QActionResult(fromTo, StateNameEnum.tujuan.EnumToString(),
                    tujuanOpponent)
                { Reward = 0 });
                fromTo.AddActionResult(new QActionResult(fromTo, StateNameEnum.harga.EnumToString(),
                    hargaOpponent)
                { Reward = 10 });
                fromTo.AddActionResult(new QActionResult(fromTo, StateNameEnum.terima_kasih.EnumToString(),
                    terimakasihOpponent)
                { Reward = -10 });
                fromTo.AddActionResult(new QActionResult(fromTo, StateNameEnum.selamat_pagi.EnumToString(),
                    selamatpagiOpponent)
                { Reward = -10 });
                fromTo.AddActionResult(new QActionResult(fromTo, StateNameEnum.dealing.EnumToString(),
                    dealingOpponent)
                { Reward = -10 });
                q.RunTraining();
                q.PrintQLearningStructure();
                q.ShowPolicy();

                //Console.WriteLine("\n** Opponent style **");
                Console.WriteLine(string.Format("style is tujuan {0} harga {1} terimakasih {2} selamatpagi {3} dealing",
                    opponent[0].Pretty(), opponent[1].Pretty(), opponent[2].Pretty(), opponent[3].Pretty(), opponent[4].Pretty()));
                //label5.Text= string.Format("Model: Pusing {0} Panas {1} Minum {2} Mandi {3} Bosan {4} Remote {5} Obat {6}",
                //opponent[0].Pretty(), opponent[1].Pretty(), opponent[2].Pretty(), opponent[3].Pretty(), opponent[4].Pretty(), opponent[5].Pretty(), opponent[6].Pretty());

                double max = Double.MinValue;
                string actionName = "nothing";
                foreach (QAction action in state.Actions)
                {
                    foreach (QActionResult actionResult in action.ActionsResult)
                    {
                        if (actionResult.QEstimated > 0.15)
                        {
                            max = actionResult.QEstimated;
                            actionName = action.ActionName.ToString();
                            Console.WriteLine("Jawaban= " + actionResult + "?");
                            label5.Text = "Jawaban= " + actionResult + "?" + "\n";
                            //textBox2.Text = "Apakah anda ingin " + actionResult + " ? " + "\n";
                        }


                    }

                }
                ////dibukani
                //Console.WriteLine(string.Format("From state {0} do action {1}, max QEstimated is {2}",
                //  state.StateName, actionName, max.Pretty()));
                //Console.WriteLine(string.Format("Dari State {0} melakukan aksi {1} didapatkan Estimasi Nilai Q maksimal = {2}",
                //  state.StateName, actionName, max.Pretty()));
            }


            if (hasilkalimat == "terima kasih")
            {
                stateNameNext = StateNameEnum.terima_kasih.EnumToString();
                state.AddAction(fromTo = new QAction(stateName, new QActionName(stateName, stateNameNext)));
                // action outcome probability
                //NAMA NEXT NYA
                fromTo.AddActionResult(new QActionResult(fromTo, StateNameEnum.tujuan.EnumToString(),
                    tujuanOpponent)
                { Reward = -10 });
                fromTo.AddActionResult(new QActionResult(fromTo, StateNameEnum.harga.EnumToString(),
                    hargaOpponent)
                { Reward = -10 });
                fromTo.AddActionResult(new QActionResult(fromTo, StateNameEnum.terima_kasih.EnumToString(),
                    terimakasihOpponent)
                { Reward = 10 });
                fromTo.AddActionResult(new QActionResult(fromTo, StateNameEnum.selamat_pagi.EnumToString(),
                    selamatpagiOpponent)
                { Reward = 0 });
                fromTo.AddActionResult(new QActionResult(fromTo, StateNameEnum.dealing.EnumToString(),
                    dealingOpponent)
                { Reward = 0 });
                q.RunTraining();
                q.PrintQLearningStructure();
                q.ShowPolicy();

                //Console.WriteLine("\n** Opponent style **");
                Console.WriteLine(string.Format("style is tujuan {0} harga {1} terimakasih {2} selamatpagi {3} dealing",
                    opponent[0].Pretty(), opponent[1].Pretty(), opponent[2].Pretty(), opponent[3].Pretty(), opponent[4].Pretty()));
                //label5.Text= string.Format("Model: Pusing {0} Panas {1} Minum {2} Mandi {3} Bosan {4} Remote {5} Obat {6}",
                //opponent[0].Pretty(), opponent[1].Pretty(), opponent[2].Pretty(), opponent[3].Pretty(), opponent[4].Pretty(), opponent[5].Pretty(), opponent[6].Pretty());

                double max = Double.MinValue;
                string actionName = "nothing";
                foreach (QAction action in state.Actions)
                {
                    foreach (QActionResult actionResult in action.ActionsResult)
                    {
                        if (actionResult.QEstimated > 0.15)
                        {
                            max = actionResult.QEstimated;
                            actionName = action.ActionName.ToString();
                            Console.WriteLine("Jawaban= " + actionResult + "?");
                            label5.Text = "Jawaban= " + actionResult + "?" + "\n";
                            //textBox2.Text = "Apakah anda ingin " + actionResult + " ? " + "\n";
                        }


                    }

                }
                ////dibukani
                //Console.WriteLine(string.Format("From state {0} do action {1}, max QEstimated is {2}",
                //  state.StateName, actionName, max.Pretty()));
                //Console.WriteLine(string.Format("Dari State {0} melakukan aksi {1} didapatkan Estimasi Nilai Q maksimal = {2}",
                //  state.StateName, actionName, max.Pretty()));
            }


            if (hasilkalimat == "selamat pagi")
            {
                stateNameNext = StateNameEnum.selamat_pagi.EnumToString();
                state.AddAction(fromTo = new QAction(stateName, new QActionName(stateName, stateNameNext)));
                // action outcome probability
                //NAMA NEXT NYA
                fromTo.AddActionResult(new QActionResult(fromTo, StateNameEnum.tujuan.EnumToString(),
                    tujuanOpponent)
                { Reward = -10 });
                fromTo.AddActionResult(new QActionResult(fromTo, StateNameEnum.harga.EnumToString(),
                    hargaOpponent)
                { Reward = -10 });
                fromTo.AddActionResult(new QActionResult(fromTo, StateNameEnum.terima_kasih.EnumToString(),
                    terimakasihOpponent)
                { Reward = -10 });
                fromTo.AddActionResult(new QActionResult(fromTo, StateNameEnum.selamat_pagi.EnumToString(),
                    selamatpagiOpponent)
                { Reward = 10 });
                fromTo.AddActionResult(new QActionResult(fromTo, StateNameEnum.dealing.EnumToString(),
                    dealingOpponent)
                { Reward = -10 });
                q.RunTraining();
                q.PrintQLearningStructure();
                q.ShowPolicy();

                //Console.WriteLine("\n** Opponent style **");
                Console.WriteLine(string.Format("style is tujuan {0} harga {1} terimakasih {2} selamatpagi {3} dealing",
                    opponent[0].Pretty(), opponent[1].Pretty(), opponent[2].Pretty(), opponent[3].Pretty(), opponent[4].Pretty()));
                //label5.Text= string.Format("Model: Pusing {0} Panas {1} Minum {2} Mandi {3} Bosan {4} Remote {5} Obat {6}",
                //opponent[0].Pretty(), opponent[1].Pretty(), opponent[2].Pretty(), opponent[3].Pretty(), opponent[4].Pretty(), opponent[5].Pretty(), opponent[6].Pretty());

                double max = Double.MinValue;
                string actionName = "nothing";
                foreach (QAction action in state.Actions)
                {
                    foreach (QActionResult actionResult in action.ActionsResult)
                    {
                        if (actionResult.QEstimated > 0.15)
                        {
                            max = actionResult.QEstimated;
                            actionName = action.ActionName.ToString();
                            Console.WriteLine("Jawaban= " + actionResult + "?");
                            label5.Text = "Jawaban= " + actionResult + "?" + "\n";
                            //textBox2.Text = "Apakah anda ingin " + actionResult + " ? " + "\n";
                        }


                    }

                }
                ////dibukani
                //Console.WriteLine(string.Format("From state {0} do action {1}, max QEstimated is {2}",
                //  state.StateName, actionName, max.Pretty()));
                //Console.WriteLine(string.Format("Dari State {0} melakukan aksi {1} didapatkan Estimasi Nilai Q maksimal = {2}",
                //  state.StateName, actionName, max.Pretty()));
            }



            // action Bosan
            if (hasilkalimat == "dealing")
            {
                stateNameNext = StateNameEnum.dealing.EnumToString();
                state.AddAction(fromTo = new QAction(stateName, new QActionName(stateName, stateNameNext)));
                // action outcome probability
                //NAMA NEXT NYA
                fromTo.AddActionResult(new QActionResult(fromTo, StateNameEnum.tujuan.EnumToString(),
                    tujuanOpponent)
                { Reward = -10 });
                fromTo.AddActionResult(new QActionResult(fromTo, StateNameEnum.harga.EnumToString(),
                    hargaOpponent)
                { Reward = -10 });
                fromTo.AddActionResult(new QActionResult(fromTo, StateNameEnum.terima_kasih.EnumToString(),
                    terimakasihOpponent)
                { Reward = -10 });
                fromTo.AddActionResult(new QActionResult(fromTo, StateNameEnum.selamat_pagi.EnumToString(),
                    selamatpagiOpponent)
                { Reward = -10 });
                fromTo.AddActionResult(new QActionResult(fromTo, StateNameEnum.dealing.EnumToString(),
                    dealingOpponent)
                { Reward = 10 });
                q.RunTraining();
                q.PrintQLearningStructure();
                q.ShowPolicy();

                //Console.WriteLine("\n** Opponent style **");
                Console.WriteLine(string.Format("style is tujuan {0} harga {1} terimakasih {2} selamatpagi {3} dealing",
                    opponent[0].Pretty(), opponent[1].Pretty(), opponent[2].Pretty(), opponent[3].Pretty(), opponent[4].Pretty()));
                //label5.Text= string.Format("Model: Pusing {0} Panas {1} Minum {2} Mandi {3} Bosan {4} Remote {5} Obat {6}",
                //opponent[0].Pretty(), opponent[1].Pretty(), opponent[2].Pretty(), opponent[3].Pretty(), opponent[4].Pretty(), opponent[5].Pretty(), opponent[6].Pretty());

                double max = Double.MinValue;
                string actionName = "nothing";
                foreach (QAction action in state.Actions)
                {
                    foreach (QActionResult actionResult in action.ActionsResult)
                    {
                        if (actionResult.QEstimated > 0.15)
                        {
                            max = actionResult.QEstimated;
                            actionName = action.ActionName.ToString();
                            Console.WriteLine("Jawaban= " + actionResult + "?");
                            label5.Text = "Jawaban= " + actionResult + "?" + "\n";
                            //textBox2.Text = "Apakah anda ingin " + actionResult + " ? " + "\n";
                        }


                    }

                }
                ////dibukani
                //Console.WriteLine(string.Format("From state {0} do action {1}, max QEstimated is {2}",
                //  state.StateName, actionName, max.Pretty()));
                //Console.WriteLine(string.Format("Dari State {0} melakukan aksi {1} didapatkan Estimasi Nilai Q maksimal = {2}",
                //  state.StateName, actionName, max.Pretty()));
            }
            
            //// ----------- End Insert the path setup here -----------


        }

        private void label3_Click(object sender, EventArgs e)
        {
            
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
    
}
