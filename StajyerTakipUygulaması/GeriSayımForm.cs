using System.Windows.Forms;
using System;

public partial class GeriSayımForm : Form
{
    private int countdownSeconds;
    private Timer countdownTimer;

    public GeriSayımForm(int seconds)
    {
        InitializeComponent();
        countdownSeconds = seconds;
        countdownLabel.Text = countdownSeconds.ToString();
        this.Text = "Kalan Süre"; // Form başlığını burada değiştirin
        InitializeTimer();
    }

    private void InitializeTimer()
    {
        countdownTimer = new Timer();
        countdownTimer.Interval = 1000; // 1 saniye
        countdownTimer.Tick += CountdownTimer_Tick;
        countdownTimer.Start();
    }

    private void CountdownTimer_Tick(object sender, EventArgs e)
    {
        countdownSeconds--;
        countdownLabel.Text = countdownSeconds.ToString();

        if (countdownSeconds <= 0)
        {
            countdownTimer.Stop();
            MessageBox.Show("Süreniz doldu, tekrar deneyebilirsiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            this.Close(); // Formu kapat
        }
    }
}
