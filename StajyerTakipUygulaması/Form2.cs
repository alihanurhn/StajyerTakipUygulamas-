using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb; //Data OleDB'nin tanımlanması.
using System.IO; //Giriş Çıkış işlemleri için tanımlanması gereken kütüphane.
using System.Text.RegularExpressions; //Regex kütüphanesinin tanımlanması.

namespace StajyerTakipUygulaması
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        // Veritabanı dosya yolu ve provider nesnesinin belirlenmesi
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.Ace.OleDb.12.0;Data Source=personel.accdb");

        private void kullanicilari_goster()
        {
            try
            {
                baglanti.Open();
                OleDbDataAdapter kullanicilari_listele =new OleDbDataAdapter
                ("select tcno AS[TC KİMLİK NO], ad AS[ADI], soyad AS[SOYADI], yetki AS[YETKİ],parola AS[PAROLA] from kullanicilar Order By ad ASC", baglanti);
                DataSet dshafiza = new DataSet();
                kullanicilari_listele.Fill(dshafiza);
                dataGridView1.DataSource = dshafiza.Tables[0];
                baglanti.Close();
            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message, "Eyüpsultan Belediyesi Bilgi İşlem Müdürlüğü", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglanti.Close();
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            kullanicilari_goster(); 
        }
    }
}
