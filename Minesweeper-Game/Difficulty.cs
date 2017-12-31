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
    public partial class Difficulty : Form
    {
        //Initalise variables to be used in other forms
        public static int gridWidth = 0;
        public static int gridHeight = 0;
        public static int mineAmount = 0;
        public static int buttonSize = 0;
        public static int buttonPosition = 0;
        public static int gameDifficulty = 0;
        public static int blankSquares = 0;
        public static int flagAmount = 0;

        public Difficulty()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
        }

        private void Difficulty_Load(object sender, EventArgs e)
        {

        }

        private void PlayGame_Click(object sender, EventArgs e)
        {
            //If the user chooses easy mode
            if (EasyRadioButton.Checked == true)
            {
                //Set values of easy mode to variables
                gridWidth = 9;
                gridHeight = gridWidth;
                mineAmount = 10;
                flagAmount = mineAmount;
                buttonSize = 30;
                buttonPosition = 30;
                gameDifficulty = 1;
                blankSquares = (gridWidth * gridHeight) - mineAmount;

            }
            //If the user chooses medium mode
            else if (MediumRadioButton.Checked == true)
            {
                //Set values of medium mode to variables
                gridWidth = 16;
                gridHeight = gridWidth;
                mineAmount = 40;
                flagAmount = mineAmount;
                buttonSize = 23;
                buttonPosition = 23;
                gameDifficulty = 2;
                blankSquares = (gridWidth * gridHeight) - mineAmount;
            }
            //If the user chooses hard mode
            else if (HardRadioButton.Checked == true)
            {
                //Set values of hard mode to variables
                gridHeight = 16;
                gridWidth = 30;
                mineAmount = 99;
                flagAmount = mineAmount;
                buttonSize = 23;
                buttonPosition = 23;
                gameDifficulty = 3;
                blankSquares = (gridWidth * gridHeight) - mineAmount;
            }
            //Create instance of form1 and start up game, hiding the difficulty screen
            MainGame mainGame = new MainGame();
            Hide();
            mainGame.Show();
        }
    }
}
