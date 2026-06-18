namespace _06_02_WF_Several_tasks
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.ReadOnly = true; // Робимо textBox1 лише для читання
            textBox1.Multiline = true; // Дозволяємо багаторядковий ввід у textBox1
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Start");
            string text = richTextBox1.Text;
            string[] currentLines = richTextBox1.Lines; // дістали текст розбтий на рядки

            //Task<int> taskLen = Task.Run(() => richTextBox1.Text.Length);// поганий спосіб = змішали доступ до UI та  Task
            Task<int> taskLen = Task.Run(() => text.Length);// Завдання для показу довжини рядка
            Task<int> taskWordCount = Task.Run(() => CountWords(text)); // Завдання для підрахунку слів
            Task<int> taskLines = Task.Run(() => currentLines.Length);// text.Split("\n").Length);// // Завдання для підрахунку  числа рядків

            await Task.WhenAll(taskLen, taskWordCount, taskLines); // Очікуємо завершення усіх завдань, головний потік UI не блокується

            textBox1.Text = $"Length: {taskLen.Result}\r\nWord count: {taskWordCount.Result}";
            textBox1.AppendText($"\r\nLines: {taskLines.Result}\r\n");
        }


        private int CountWords(string text)
        {
            return text.Split(", !?-:;.\t\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length;
        }
        /*
         * UI потік:
        - читає текст
        - запускає задачі

        Task потоки:
        - рахують
        - НЕ чіпають UI
         */
    }
}
