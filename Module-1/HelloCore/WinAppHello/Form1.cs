using System.Windows.Forms;

namespace WinAppHello
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void UserTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                MessageLabel.Text = UserTextBox.Text.Length > 0 ? HelloCore.Formatter.FormatToDateHello(UserTextBox.Text) : "Pls fill box!";
            }
        }
    }
}