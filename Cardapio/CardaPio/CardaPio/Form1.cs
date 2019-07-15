using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace CardaPio
{
    public partial class Form1 : Form
    {
        CommonClass cm = new CommonClass();
        string URI = "";
        int codigoProduto = 1;
        public int TempoProduto;
        public int Tempo;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.Text = "";
            listBox2.Text = "";
            listBox3.Text = "";
            listBox4.Text = "";

            Headers();
            LerCardapio();



        }

        public void Headers()
        {
            listView1.Clear();
            listView1.View = View.Details;
            listView1.Columns.Add("ITEM", 250, HorizontalAlignment.Center);
            listView1.Columns.Add("TEMPO PREPARO", 150, HorizontalAlignment.Center);
        }


        public void LerCardapio()
        {


            cm.prmPath = @"C:\temp\cardapio\";
            cm.prmArquivo = "cardapio.txt";

            string arquivo = cm.prmPath + cm.prmArquivo;

            if (File.Exists(arquivo))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(arquivo))
                    {
                        String linha;
                        // Lê linha por linha até o final do arquivo
                        while ((linha = sr.ReadLine()) != null)
                        {
                            Console.WriteLine(linha);
                            string[] line = linha.Split(';');

                            ListViewItem newList = new ListViewItem(line[0]);
                            newList.SubItems.Add(line[1]);
                            listView1.Items.Add(newList);

                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine(" O arquivo " + arquivo + "não foi localizado !");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }




        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            CommonClass cm = new CommonClass();
            int count = 0;
            string sucoGratis;


            if (listView1.SelectedIndices.Count <= 0)
            {
                return;
            }

            int intselectedindex = listView1.SelectedIndices[0];

            if (intselectedindex >= 0)
            {

                //COLETA O DADO DO CLICK DA LISTA
                String text = listView1.Items[intselectedindex].Text;

                //COLETA O TEMPO DO CLICK DA LISTA
                lblItem.Text = listView1.Items[intselectedindex].Text;

                ////ARMAZENA O TEMPO DO DADO DO CLICK DA LISTA
                TempoProduto = Convert.ToInt32(listView1.Items[intselectedindex].SubItems[1].Text);
                cm.CalculatempoPreparo(TempoProduto);

            }


            Cursor.Current = Cursors.Default;

        }
        /// <summary>
        /// CONCLUIR PEDIDO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            //Tempo

            //GERAR UM NUMERO DE PEDIDO
            var date = DateTime.Now;
            string plainData = date.ToString();// "filepathstring4030720191914";
            string hashedData = ComputeSha256Hash(plainData);
            string prmPedNum = "PED-" + hashedData.ToString().Substring(0, 3).ToUpper();


            listBox2.Items.Add(prmPedNum + " > " + Tempo);

            listBox1.Items.Clear();
            lblTempoPreparo.Text = "";
            Tempo = 0;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            Tempo = 0;
            lblTempoPreparo.Text = "";
        }


        /// <summary>
        /// ROTINA DE APRESENTAÇÃO DO CUPOM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click_1(object sender, EventArgs e)
        {

            if (lblItem.Text.ToString() != "")
            {

                
                string text = txtQtde.Text;

                if (text.ToString() != "")
                {


                    listBox1.Items.Add("+ " + lblItem.Text + "...................................................... X " + text.ToString());

                    if (Convert.ToInt32(text) >= 2 && lblItem.Text.ToString().Contains("Hamb"))
                    {
                        listBox1.Items.Add("+ SUCO GRATIS ......................X   1");
                    }

                    //CALCULO DO TEMPO DO PRODUTO
                    Tempo = Tempo + (TempoProduto * Convert.ToInt32(text));

                    lblTempoPreparo.Text = "TEMPO TOTAL DE PREPARO : " + Tempo + " Segs";
                }
                else
                {

                    System.Windows.Forms.MessageBox.Show("Informe a quantidade !");
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int count = 0;
            int text2 = listBox2.Items.Count;
            string text = listBox3.Text;
            var firstItem = "";

            //SE A COZINHA CONTIVER MENOS QUE 4 PEDIDOS, PEGA + 1
            if (text2 > 0)
            {
                if (listBox3.Items.Count <= 3)
                {
                    //SELECIONA PEDIDO NA FILA
                    foreach (string item in listBox2.Items)
                    {
                        firstItem = item;
                    }

                    //ENVIA UM PEDIDO PARA COZINHA
                    listBox3.Items.Add(firstItem);

                    //REMOVE DA FILA
                    listBox2.Items.Remove(firstItem);


                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            
            string tempo;
            int contador ;
            string MyStr;

            //PREPARA PEDIDO
            for (int i = 0; i < listBox3.Items.Count; i++)
            {


                //pego o pedido
                MyStr = listBox3.Items[i].ToString();

                Console.WriteLine(MyStr.Trim());

                //splito
                string[] line = MyStr.Split('>');



                contador = Convert.ToInt32(line[1]) - 1;


                if (contador == 1)
                {
                    listBox3.Items.Remove(MyStr.Trim());
                    listBox4.Items.Add(line[0].Trim());// + " > " + contador);
                    OrdenaEntrega();
                }
                else
                {

                    listBox3.Items.Remove(MyStr.Trim());
                    listBox3.Items.Add(line[0].Trim() + " > " + contador);
                    contador = 1;
                }

            }

            //OrdenaCozinha();

        }


        public void OrdenaCozinha()
        {

            int text = listBox3.Items.Count;

            //SE A COZINHA CONTIVER MENOS QUE 4 PEDIDOS, PEGA + 1
            if (text > 1)
            {

                String[] a = listBox3.Items.Cast<string>().ToArray();

                listBox3.Items.Clear();

                var ret = a.OrderBy(p => p);

                foreach (var item in ret)

                    listBox3.Items.Add(item.ToString());
            }
        }

        public void OrdenaEntrega()
        {

            int text = listBox4.Items.Count;

            //SE A COZINHA CONTIVER MENOS QUE 4 PEDIDOS, PEGA + 1
            if (text > 1)
            {

                String[] a = listBox4.Items.Cast<string>().ToArray();

                listBox4.Items.Clear();

                var ret = a.OrderBy(p => p);

                foreach (var item in ret)

                    listBox4.Items.Add(item.ToString());
            }
        }

        private async void GetAllProdutos()
        {
            URI = txtURI.Text;
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(URI))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        //clienteUri = response.Headers.Location;
                        var ProdutoJsonString = await response.Content.ReadAsStringAsync();
                        dgvDados.DataSource = JsonConvert.DeserializeObject<Produto[]>(ProdutoJsonString).ToList();
                    }
                    else
                    {
                        MessageBox.Show("Não foi possível obter o produto : " + response.StatusCode);
                    }
                }
            }
        }

        private void btnObterProdutos_Click(object sender, EventArgs e)
        {
            GetAllProdutos();
        }

        private void dgvDados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgvDados.Rows[e.RowIndex];

                //COLETA O PRODUTO
                lblItem.Text = row.Cells["Nome"].Value.ToString();


                //COLETA O TEMPO
                ////ARMAZENA O TEMPO DO DADO DO CLICK DA LISTA
                TempoProduto = Convert.ToInt32(row.Cells["Tempo"].Value.ToString());
            }
        }
    }


}


