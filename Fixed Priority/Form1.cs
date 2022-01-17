using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fixed_Priority
{
    public partial class Form1 : Form
    {
        readonly bool ru = true;
        public Form1()
        {
            InitializeComponent();
            if (!CultureInfo.CurrentUICulture.ToString().Contains("ru-RU"))
            {
                ru = false;
                label1.Text = "Process name";
                groupBox1.Text = "Priority";
                groupBox2.Text = "Every";
                Button1.Text = "Set";
                radioButton1.Text = "Normal";
                radioButton2.Text = "High";
                radioButton3.Text = "Realtime";
                radioButton4.Text = "5 sec";
                radioButton5.Text = "15 sec";
                radioButton6.Text = "30 sec";
            }
            About.Click += (s, e) => MessageBox.Show($"{(ru ? "Программа задаёт нужный приоритет для всех\nпроцессов программы и не даёт их сбросить пока программа запущена" : "The program sets the desired priority for everyone\nprogram processes and does not allow them to be reset while the program is running")}", "Fixed Priority");
            Menu1.Click += (s, e) => Close();
            NotifyIcon1.MouseClick += (s, e) => { if (e.Button == MouseButtons.Left) { Show(); WindowState = FormWindowState.Normal; } };
            Resize += (s, e) => { if (WindowState == FormWindowState.Minimized) Hide(); };
        }

        async void Set(ProcessPriorityClass p)
        {
            string name = TextBox1.Text;
            while (Process.GetProcessesByName(name).Length > 0)
            {
                Process.GetProcessesByName(name).ToList().ForEach(x => x.PriorityClass = p);
                await Task.Delay(num);
            }
            MessageBox.Show($"{(ru ? $"Процесс с названием {name} не найден.\n\nПрограмма закрывается" : $"Process with the name {name} was not found.\n\nThe program is closing")}", "Fixed Priority", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            Close();
        }

        int num;
        void Button1_Click(object sender, System.EventArgs e)
        {
            TextBox1.Text = TextBox1.Text.EndsWith(".exe") ? TextBox1.Text.Remove(TextBox1.Text.Length - 4) : TextBox1.Text;
            TextBox1.Enabled = false;
            Button1.Enabled = false;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            if (radioButton4.Checked)
                num = 5000;
            else if (radioButton5.Checked)
                num = 15000;
            else if (radioButton6.Checked)
                num = 30000;
            if (radioButton1.Checked)
                Set(ProcessPriorityClass.Normal);
            else if (radioButton2.Checked)
                Set(ProcessPriorityClass.High);
            else if (radioButton3.Checked)
                Set(ProcessPriorityClass.RealTime);
        }
    }
}
