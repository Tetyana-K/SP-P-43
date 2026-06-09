namespace _07_Thread_WF__letters_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            listBox1.MultiColumn = true; // Встановлюємо режим багатоколоночного відображення для ListBox
            //btnStart.Click += buttonStartAsync_Click;
            //btnStart.Click += btnStart_Click; // Додаємо обробник для звичайного потока, щоб порівняти з асинхронним
        }



        private void btnStart_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear(); // Очищуємо ListBox перед початком нового потока
            Thread thread = new Thread(PrintLetters);
            thread.Start();
        }
        private async void buttonStartAsync_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            // Програма не зависне, цикл виконується асинхронно
            for (char c = 'A'; c <= 'Z'; c++)
            {
                // Додаємо елемент напряму! Ніяких Invoke не треба.
                listBox1.Items.Add($"{c} in async task");

                // Замість Thread.Sleep використовуємо Task.Delay, 
                // який тимчасово звільняє потік, поки йде пауза
                await Task.Delay(200);
            }
        }
        void PrintLetters()
        {
            for (char c = 'A'; c <= 'Z'; c++)
            {
                //listBox1.Items.Add(c); // може викликати помилку, якщо це робить не головний потік
                // Правильний спосіб — використовувати Invoke() або BeginInvoke(), 
                // щоб код виконувався у головному UI-потоці

                // Перевіряємо, чи потрібен виклик Invoke (чи ми в іншому потоці)
                // Якщо true — ми у фоновому потоці. Прямий доступ заборонено
                // Використовуємо Invoke, щоб передати роботу головному потоку.
                if (listBox1.InvokeRequired)
                {
                    //this.Invoke(new Action(() => // інший потік звертається до головного потока (UI потік) для оновлення UI
                    //{
                    //    listBox1.Items.Add(c);
                    //}));
                    this.Invoke(() => // інший потік звертається до головного потока (UI потік) для оновлення UI
                    {
                        listBox1.Items.Add(c);
                    });
                }
                else // Якщо false — ми вже в головному потоці (наприклад, користувач клікнув на кнопку).
                     // Можна безпечно додавати елемент напряму.
                {
                    listBox1.Items.Add(c);
                }
                Thread.Sleep(200); // Затримка для наочності
            }
        }

    }
}
