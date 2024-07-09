using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb; //Data OleDB Kütüphanesinin tanımlanması.
using System.Text.RegularExpressions; //Regex Kütüphanesinin tanımlanması.
using System.IO; //Input Output Kütüphanesinin tanımlanması.


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
                MessageBox.Show("Veritabanı bağlantısı başarılı!");
                OleDbDataAdapter kullanicilari_listele = new OleDbDataAdapter
                ("select tcno AS[TC KİMLİK NO], ad AS[ADI], soyad AS[SOYADI], yetki AS[YETKİ],parola AS[PAROLA] from kullanicilar Order By ad ASC", baglanti);
                DataSet dshafiza = new DataSet();
                kullanicilari_listele.Fill(dshafiza);
                dataGridView1.DataSource = dshafiza.Tables[0];
                baglanti.Close();
            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message, "Eyüpsultan Belediyesi Bilgi İşlem Müdürlüğü", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
;
            }
        }

        private void personel_goster()
        {
            try
            {
                baglanti.Open();
                MessageBox.Show("Veritabanı bağlantısı başarılı!");
                OleDbDataAdapter personelleri_listele = new OleDbDataAdapter
                ("select tcno AS[TC KİMLİK NO], ad AS[ADI], soyad AS[SOYADI], cinsiyet AS[CİNSİYETİ], dogumtarihi AS[DOĞUM TARİHİ], gorevi AS[GÖREVİ], gorevyeri AS[YER] from personeller Order By ASC", baglanti);
                DataSet dshafiza = new DataSet();
                personelleri_listele.Fill(dshafiza);
                dataGridView2.DataSource = dshafiza.Tables[0];
                baglanti.Close();
            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message, "Eyüpsultan Belediyesi Bilgi İşlem Müdürlüğü", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
;
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            kullanicilari_goster();
            personel_goster();
        }

    }
}
