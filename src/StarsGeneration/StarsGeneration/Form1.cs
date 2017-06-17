using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using StarsGenerator;

namespace StarsGeneration
{
    public partial class Form1 : Form
    {
        private readonly UniverseGenerator _universeGenerator;


        public void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            _universeGenerator.Paint(e);
        }

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