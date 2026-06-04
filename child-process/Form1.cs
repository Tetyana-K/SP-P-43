namespace child_process
{
    public partial class Form1 : Form
    {
        private string[] args;

        public Form1(string[] args) : base() 
        {
            InitializeComponent();
            this.args = args;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (args.Length == 0)
            {
                label1.Text = $"Не отримали параметри";
                label1.ForeColor = Color.Red;
                listBox1.Visible = false; // ховаємо ListBox, якщо немає параметрів для відображення
            }
            else
            {
                label1.Text = $"Отримали параметри";
                label1.ForeColor = Color.Green;
                foreach (var arg in args)  // проходимо по кожному аргументу та додаємо його до ListBox для відображення
                {
                    listBox1.Items.Add(arg);
                }
            }
        }
    }
}
