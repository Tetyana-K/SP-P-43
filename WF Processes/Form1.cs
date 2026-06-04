using System.Diagnostics;

namespace WF_Processes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<ProcessInfo> processes = new();
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadProcesses();
        }
        private void LoadProcesses()
        {
            processes = new List<ProcessInfo>();

            foreach (var p in Process.GetProcesses())
            {
                try
                {
                    processes.Add(new ProcessInfo
                    {
                        Id = p.Id,
                        Name = p.ProcessName,
                        Memory = Math.Round(p.PrivateMemorySize64 / 1024.0, 2),
                        //Priority = p.PriorityClass.ToString()
                        Priority = GetPriority(p)
                    });
                }
                catch
                {
                    // деякі процеси недоступні — просто пропускаємо
                }
            }

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = processes;
        }
        private string GetPriority(Process p)
        {
            try
            {
                return p.PriorityClass.ToString();
            }
            catch
            {
                return "N/A";
            }
        }

        Process process;
        int PID;
        string processName;
        private void button1_Click(object sender, EventArgs e)
        {
            process = Process.Start("mspaint.exe"); // запускаємо процес "Paint"
            PID = process.Id;
            processName = process.ProcessName;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!process.CloseMainWindow())
            {
                MessageBox.Show("Процес не має головного вікна або вже завершений.");
                return;
            }
            if(process.WaitForExit(10000)) {
                MessageBox.Show($"Процес {processName} (PID: {PID}) успішно закрито.");
            }
             else
            {
                MessageBox.Show($"Процес {process.ProcessName} (PID: {PID}  не відповідає на запит закриття. Спроба примусового завершення...");
                TryKillProcess(process);
            }
        }

        private void TryKillProcess(Process process)
        {
            if (process == null || process.HasExited)
            {
                MessageBox.Show("Процес не існує чи завершено");
                return;
            }
            try
            {
                process.Kill();
                process.WaitForExit();
                MessageBox.Show($"Процес {process.ProcessName} (PID: {process.Id}) примусово завершено.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при завершенні процесу: {ex.Message}");
            }
        }
    }
}


