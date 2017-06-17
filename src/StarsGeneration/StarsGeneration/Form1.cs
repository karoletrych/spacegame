using System;
using System.Windows.Forms;

namespace StarsGeneration
{
    public partial class Form1 : Form
    {
        private readonly UniverseGenerator _universeGenerator;

        public Form1()
        {
            InitializeComponent();
            _universeGenerator = new UniverseGenerator(pictureBox1, minDistance, newPointsCount);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }
    }
}