namespace _00_Long_operation__WF_
{
    public partial class Form1 : Form
    {
        int upLimit = 100_000;
        public Form1()
        {
            InitializeComponent();
            progressBar1.Maximum = upLimit;
        }

        private /*async*/ void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Long operation!!!");
            //await 
                LongOperation();
        }
        //private void LongOperation()
        //{
        //    decimal result = 1;
        //    for (decimal i = 0; i < upLimit; i++)
        //    {
        //        result = i * i;
        //        progressBar1.Value++;
        //        Thread.Sleep(100); //  імітація довгої роботи
        //    }
        //    MessageBox.Show("Completed!");
        //}
        private async Task LongOperation()
        {
            decimal result = 1;
            for (decimal i = 0; i < upLimit; i++)
            {
                result = i * i;
                progressBar1.Value++;
                await Task.Delay(10); //  імітація довгої роботи  ( без блокування потоку), кажемо - я зараз чекаю - можеш зайнтися іншими справами
            }
            MessageBox.Show("Completed!");
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            this.Text = $"X = {e.X}, Y = {e.Y}";
        }
    }
}
