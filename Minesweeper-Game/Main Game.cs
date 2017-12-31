using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper_Game
{
    public partial class MainGame : Form
    {
        public MainGame()
        {
            InitializeComponent();
        }

        private void MainGame_Load(object sender, EventArgs e)
        {
            ChangePageLayout();   
        }

        private void ChangePageLayout()
        {
            int x = 0, y = 0;
            Size = new Size(460, 500);
            Button[,] grid = new Button[9, 9];
            for(x = 0; x < grid.GetLength(0); x++)
            {
                for (y = 0; y < grid.GetLength(1); y++)
                {
                    grid[x, y] = new Button();
                    grid[x, y].SetBounds(40*(x+1), 40*(y+1), 40, 40);
                    grid[x, y].Click += new EventHandler(ButtonClick);
                    Controls.Add(grid[x, y]);
                }
                y = 0;
            }
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            MessageBox.Show("Click!");
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult answer = MessageBox.Show("All progress will be lost if exited", "Are You Sure?", MessageBoxButtons.YesNo);
            if(answer == DialogResult.Yes)
            {
                Application.Exit();
            } else
            {
                
            }
        }
    }
}
