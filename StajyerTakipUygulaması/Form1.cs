using System;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace StajyerTakipUygulaması
{
    public partial class Form1 : Form
    {
        private CheckBox showPasswordCheckBox;
        private bool timerRunning = false; // Timer'ın çalışıp çalışmadığını kontrol etmek için

        public Form1()
        {
            InitializeComponent();

            // textBox1 için olayların eklenmesi
            textBox1.KeyPress += new KeyPressEventHandler(textBox1_KeyPress);
            textBox1.TextChanged += new EventHandler(textBox1_TextChanged);

            // Şifre alanının gizlenmesi için PasswordChar özelliğinin eklenmesi
            textBox2.PasswordChar = '*';

            // Parolayı göster/gizle CheckBox'ının eklenmesi ve olayların tanımlanması
            showPasswordCheckBox = new CheckBox();
            showPasswordCheckBox.Text = "Parolayı Göster";
            showPasswordCheckBox.AutoSize = true;
            showPasswordCheckBox.Location = new Point(textBox2.Right + 10, textBox2.Top);
            showPasswordCheckBox.CheckedChanged += new EventHandler(showPasswordCheckBox_CheckedChanged);
            this.Controls.Add(showPasswordCheckBox);

            this.Load += new EventHandler(Form1_Load);
        }

        // Veritabanı dosya yolu ve provider nesnesinin belirlenmesi
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.Ace.OleDb.12.0;Data Source=personel.accdb");

        // Formlar arası kullanılacak değişkenlerin tanımlanması
        public static string tcno, adi, soyadi, yetki;

        // Yerel değişkenler (yalnız bu formda geçerli değişkenler)
        int hak = 3;
        bool durum = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "EYÜP SULTAN BELEDİYESİ - BİLGİ İŞLEM MÜDÜRLÜĞÜ";
            this.AcceptButton = button2;
            label6.Text = "3";
            radioButton1.Checked = true;
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dddd, dd MMMM yyyy"; // Detaylı tarih formatı
            dateTimePicker1.Enabled = false; // Tarih seçimini devre dışı bırakır
                                             // DateTimePicker'i gizle
            dateTimePicker1.Visible = false;

            // Yeni bir Label kontrolü oluştur ve aynı pozisyona yerleştir
            Label dateLabel = new Label();
            dateLabel.Location = dateTimePicker1.Location;
            dateLabel.Size = dateTimePicker1.Size;
            dateLabel.Text = dateTimePicker1.Value.ToString("dd MMMM yyyy"); // Tarihi uygun formatta göster

            // Label kontrolünü forma ekle
            this.Controls.Add(dateLabel);


        }

        private void labelLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo("https://www.eyupsultan.bel.tr") { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bağlantı açılırken bir hata oluştu: " + ex.Message);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (hak > 0)
            {
                baglanti.Open();
                OleDbCommand selectsorgu = new OleDbCommand("select * from kullanicilar", baglanti);
                OleDbDataReader kayitokuma = selectsorgu.ExecuteReader();
                durum = false;

                while (kayitokuma.Read())
                {
                    if (radioButton1.Checked)
                    {
                        try
                        {
                            // Kullanıcı verilerini oku ve doğrula
                            if (kayitokuma["tcno"].ToString() == textBox1.Text &&
                                kayitokuma["parola"].ToString() == textBox2.Text &&
                                kayitokuma["yetki"].ToString() == "Yönetici")
                            {
                                durum = true; // Durum kontrolü
                                tcno = kayitokuma["tcno"].ToString(); // TC Kimlik No
                                adi = kayitokuma["ad"].ToString(); // Ad
                                soyadi = kayitokuma["soyad"].ToString(); // Soyad

                                // Form2'yi oluştur ve göster
                                Form2 frm2 = new Form2();
                                frm2.Show();
                                this.Hide();
                                break; // Döngüden çık
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Bir hata oluştu: " + ex.Message);
                        }

                    }
                    else if (radioButton2.Checked)
                    {
                        if (kayitokuma["tcno"].ToString() == textBox1.Text && kayitokuma["parola"].ToString() == textBox2.Text && kayitokuma["yetki"].ToString() == "Stajyer")
                        {
                            durum = true;
                            tcno = kayitokuma.GetValue(0).ToString();
                            adi = kayitokuma.GetValue(1).ToString();
                            soyadi = kayitokuma.GetValue(2).ToString();
                            this.Hide();
                            Form3 frm3 = new Form3();
                            frm3.Show();
                            break;
                        }
                    }
                }

                baglanti.Close();

                if (!durum)
                {
                    hak--;
                    label6.Text = hak.ToString();
                    if (hak == 0)
                    {
                        baglanti.Close();
                    }
                }
            }
        }

       

        private void Form1_Load_1(object sender, EventArgs e)
        {
            // DateTimePicker'i devre dışı bırak
            dateTimePicker1.Enabled = false;
            // İsteğe bağlı: DateTimePicker'in görünümünü gri yap
            dateTimePicker1.CalendarForeColor = SystemColors.GrayText;
            dateTimePicker1.CalendarMonthBackground = SystemColors.Control;
        }

        private void dateTimePicker1_MouseDown(object sender, MouseEventArgs e)
        {
            // Kullanıcı tıklamasını iptal et
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Sadece rakam girilmesine izin verir
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // 11 karakterden fazla girilmesini engeller
            if (textBox1.Text.Length > 11)
            {
                textBox1.Text = textBox1.Text.Substring(0, 11);
                textBox1.SelectionStart = textBox1.Text.Length; // İmleci sona yerleştirir
            }
        }

        private void showPasswordCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // CheckBox işaretli ise parolayı göster, değilse gizle
            if (showPasswordCheckBox.Checked)
            {
                textBox2.PasswordChar = '\0'; // Göster
            }
            else
            {
                textBox2.PasswordChar = '*'; // Gizle
            }
        }

       
    }
}
