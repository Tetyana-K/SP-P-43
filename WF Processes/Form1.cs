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
                    // де€к≥ процеси недоступн≥ Ч просто пропускаЇмо
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
    }
}
