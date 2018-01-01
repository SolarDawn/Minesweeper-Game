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
        Button[,] grid = new Button[Difficulty.gridWidth, Difficulty.gridHeight];
        //Create a new instance of a public timer
        public static Timer clock = new Timer();
        //Create an array of 2 textboxes
        TextBox[] textTime = new TextBox[2];
        //Initalise public variables to be used in other forms
        public static int easyTime = 0;
        public static int mediumTime = 0;
        public static int hardTime = 0;
        public static int playerTime = 0;
        public static bool playAgain = false;
        //Initialise variables
        int x = 0, y = 0;
        int flagTotal = Difficulty.flagAmount;
        public MainGame()
        {
            var normalColour = Color.FromArgb(211, 211, 211);
            var bombColour = Color.FromArgb(212, 211, 211);
                                          //Loop used to create the new instances of the textbox
            for (int i = 0; i < textTime.Length; i++)
            {
                textTime[i] = new TextBox();
            }
            int mineAmount = Difficulty.mineAmount;
            Random random = new Random();
            int x = 0, y = 0;
            InitializeComponent();
            for (x = 0; x < grid.GetLength(0); x++)
            {
                for (y = 0; y < grid.GetLength(1); y++)
                {
                    grid[x, y] = new Button();
                    grid[x, y].SetBounds(Difficulty.buttonPosition * (x + 1), Difficulty.buttonPosition * (y + 1), Difficulty.buttonSize, Difficulty.buttonSize);
                    grid[x, y].Click += new EventHandler(ButtonClick);
                    grid[x, y].MouseDown += RightClickGrid;
                    grid[x, y].Name = (x + " " + y);
                    grid[x, y].Tag = "No-Flag";
                    grid[x, y].BackColor = normalColour;
                    Controls.Add(grid[x, y]);
                }
                y = 0;
            }
            for (int i = 0; i < mineAmount; i++)
            {
                int randomVertical = random.Next(0, Difficulty.gridHeight);
                int randomHorizontal = random.Next(0, Difficulty.gridWidth);
                if (grid[randomHorizontal, randomVertical].BackColor != bombColour)
                {
                    grid[randomHorizontal, randomVertical].BackColor = bombColour;
                }
                else
                {
                    i--;
                }
            }
        }

        private void MainGame_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            //If the variable in form2 is equal to one (Game mode easy)
            if (Difficulty.gameDifficulty == 1)
            {
                //Set size of screen to dimensions 350x400
                Size = new Size(350, 400);
                //Position the textTime boxes around the screen
                textTime[0].Top = 320;
                textTime[0].Left = 200;
                textTime[1].Top = 320;
                textTime[1].Left = 30;
            }
            else if (Difficulty.gameDifficulty == 2)
            {
                //Set size of screen to dimensions 430x500
                Size = new Size(430, 500);
                //Position the textTime boxes around the screen
                textTime[0].Top = 400;
                textTime[0].Left = 290;
                textTime[1].Top = 400;
                textTime[1].Left = 25;
            }
            else if (Difficulty.gameDifficulty == 3)
            {
                //Set size of screen to dimensions 750x500
                Size = new Size(750, 500);
                //Position the textTime boxes around the screen
                textTime[0].Top = 400;
                textTime[0].Left = 610;
                textTime[1].Top = 400;
                textTime[1].Left = 25;
            }
            //Set so both of the textboxes are read only and add them to the screen
            for (int i = 0; i < textTime.Length; i++)
            {
                textTime[i].ReadOnly = true;
                textTime[i].Font = new Font("Circular", 10, FontStyle.Bold);
                Controls.Add(textTime[i]);
            }
            //Set clock to tick every second and enable
            clock.Interval = 1000;
            clock.Enabled = true;
            clock.Tick += new EventHandler(ClockTick);
            //Set window to not be able to be changed in size by user
            FormBorderStyle = FormBorderStyle.FixedDialog;
            //Show value of how many flags can still be planted in the positions on screen
            textTime[1].Text = Convert.ToString(flagTotal);
        }

        private void ClockTick(object sender, EventArgs e)
        {
            //Show elapsed time on screen
            textTime[0].Text = Convert.ToString(playerTime);
            //If there are still blank squares on the screen
            if (Difficulty.blankSquares != 0)
            {
                //Add to the player time variable
                playerTime++;
            }
            else
            {
                //Stop the clock and reset to to 0
                clock.Stop();
                playerTime = 0;
            }

        }

        private void ButtonClick(object sender, EventArgs e)
        {
            var normalColour = Color.FromArgb(211, 211, 211);
            var bombColour = Color.FromArgb(212, 211, 211);
            var clicked = Color.White;
            var user = ((Button)sender);
            int horizontal = int.Parse(user.Name.Split()[0]);
            int vertical = int.Parse(user.Name.Split()[1]);
            if (user.BackColor == normalColour && user.Tag.ToString() == "No-Flag")
            {
                //Set it so the button has been clicked
                user.BackColor = clicked;
                //Take away 1 from the blankSquares variable and take away the returned value of the method
                Difficulty.blankSquares--;
                Difficulty.blankSquares = Difficulty.blankSquares - SurroundingMines(sender, horizontal, vertical);
                //If there are no more blank squares on the board
                if (Difficulty.blankSquares == 0)
                {
                    MessageBox.Show("You Win!");
                }
            }
            //If the square clicked is a mine and has no flag
            else if (user.BackColor.Equals(bombColour) && user.Tag.ToString() == "No-Flag")
            {
                //Play explosion sound effect
               //////////// explosion.Play();
                //Look for all mines in the grid and display them with a white background
                for (x = 0; x < grid.GetLength(0); x++)
                {
                    for (y = 0; y < grid.GetLength(1); y++)
                    {
                        //If the grid position is a bomb
                        if (grid[x, y].BackColor.Equals(bombColour))
                        {
                            //Show the mine, set the background as white and stop the clock
                            grid[x, y].BackColor = clicked;
                            ///////////////grid[x, y].Image = mine;
                            clock.Stop();
                            clock.Tick -= (ClockTick);
                        }
                    }
                }
                //Prompt user if they would like to try again
                DialogResult gameOverAnswer = MessageBox.Show("Game Over. Would you like to try again?", "Game Over", MessageBoxButtons.YesNo);
                //If the player would like to play again
                if (gameOverAnswer == DialogResult.Yes)
                {
                    //Reset clock 
                    playerTime = 0;
                    //Create new instance of form2 to take user back to difficulty setting
                    Difficulty form2 = new Difficulty();
                    Hide();
                    form2.Show();
                }
                else
                {
                    //Close program
                    Close();
                }
            }
            //If the square has no flag 
            if (user.Tag.ToString() == "No-Flag")
            {
                //Call mineCheck to detect how many mines are around
                MineCheck(sender, horizontal, vertical);
            }

            //Call imageCheck to set image to the amount of mines around the clicked button
            ImageCheck(sender, horizontal, vertical);
        }

        private void RightClickGrid(object sender, MouseEventArgs e)
        {
            var flagMarker = Properties.Resources.flagMarker;
            var clicked = Color.White;
            var bombColour = Color.FromArgb(212, 211, 211);
            var user = ((Button)sender);
            int horizontal = int.Parse(user.Name.Split()[0]);
            int vertical = int.Parse(user.Name.Split()[1]);
            if ((e.Button == MouseButtons.Right) && (user.BackColor != clicked) && (flagTotal > 0))
            {
                //If there is no flag on the square
                if (user.Tag.ToString() == "No-Flag")
                {
                    //Set a flag onto the square
                    user.Image = flagMarker;
                    user.Tag = "Flag";
                    //Take away from flagTotal and update textTime box
                    flagTotal--;
                    textTime[1].Text = Convert.ToString(flagTotal);
                }
                else
                {
                    //Take away flag from button
                    user.Image = null;
                    user.Tag = "No-Flag";
                    //Add to flagTotal and update textTime box
                    flagTotal++;
                    textTime[1].Text = Convert.ToString(flagTotal);
                }
            }
            else if (user.BackColor.Equals(bombColour) && user.Tag.ToString() == "No-Flag")
            {
                //Play explosion sound effect
                //explosion.Play();
                //Look for all mines in the grid and display them with a white background
                for (x = 0; x < grid.GetLength(0); x++)
                {
                    for (y = 0; y < grid.GetLength(1); y++)
                    {
                        //If the grid position is a bomb
                        if (grid[x, y].BackColor.Equals(bombColour))
                        {
                            //Show the mine, set the background as white and stop the clock
                            grid[x, y].BackColor = clicked;
                            // grid[x, y].Image = mine;
                            clock.Stop();
                            clock.Tick -= (ClockTick);
                        }
                    }
                }
                //Prompt user if they would like to try again
                DialogResult gameOverAnswer = MessageBox.Show("Game Over. Would you like to try again?", "Game Over", MessageBoxButtons.YesNo);
                //If the player would like to play again
                if (gameOverAnswer == DialogResult.Yes)
                {
                    //Reset clock 
                    playerTime = 0;
                    //Create new instance of form2 to take user back to difficulty setting
                    Difficulty form2 = new Difficulty();
                    Hide();
                    form2.Show();
                }
                else
                {
                    //Close program
                    Close();
                }
        
            }
            //If the square has no flag 
            if (user.Tag.ToString() == "No-Flag")
            {
                //Call mineCheck to detect how many mines are around
                MineCheck(sender, horizontal, vertical);
            }

            //Call imageCheck to set image to the amount of mines around the clicked button
           // ImageCheck(sender, horizontal, vertical);

        }

        private int ImageCheck(object sender, int horizontal, int vertical)
        {
            //Set the button clicked to a variable
            var user = ((Button)sender);
            //Depending on the value returned from mineCheck will determine the image shown
            switch (MineCheck(sender, horizontal, vertical))
            {
                case 0:
                    //No mines found
                    break;
                case 1:
                    //One mine found
                    user.Image = Properties.Resources._1;
                    break;
                case 2:
                    //Two mines found
                    user.Image = Properties.Resources._2;
                    break;
                case 3:
                    //Three mines found
                    user.Image = Properties.Resources._3;
                    break;
                case 4:
                    //Four mines found
                    user.Image = Properties.Resources._4;
                    break;
                case 5:
                    //Five mines found
                    user.Image = Properties.Resources._5;
                    break;
                case 6:
                    //Six mines found
                    user.Image = Properties.Resources._6;
                    break;
                case 7:
                    //Seven mines found
                    user.Image = Properties.Resources._7;
                    break;
                case 8:
                    //Eight mines found
                    user.Image = Properties.Resources._8;
                    break;
                default:
                    //Erroneous number of mines found
                    break;
            }
            return 0;
        }

        private int MineCheck(object sender, int horizontal, int vertical)
        {
            //Set colours of mines and surroundingMines to 0
            var bombColour = Color.FromArgb(212, 211, 211);
            int surroundingMines = 0;
            Button user = ((Button)sender);
            surroundingMines = 0;

            try
            {
                //If the button above is a bomb
                if (grid[horizontal, vertical + 1].BackColor == bombColour)
                {
                    surroundingMines++;
                }
            }
            //If a position not in the array tries to get accessed, do nothing
            catch (IndexOutOfRangeException) { }

            try
            {
                //If the button diagonally above (right) is a bomb
                if (grid[horizontal + 1, vertical + 1].BackColor == bombColour)
                {
                    surroundingMines++;
                }
            }
            //If a position not in the array tries to get accessed, do nothing
            catch (IndexOutOfRangeException) { }

            try
            {
                //If the button to the right is a bomb
                if (grid[horizontal + 1, vertical].BackColor == bombColour)
                {
                    surroundingMines++;
                }
            }
            //If a position not in the array tries to get accessed, do nothing
            catch (IndexOutOfRangeException) { }

            try
            {
                //If the button diagonally below (right) is a bomb
                if (grid[horizontal + 1, vertical - 1].BackColor == bombColour)
                {
                    surroundingMines++;
                }
            }
            //If a position not in the array tries to get accessed, do nothing
            catch (IndexOutOfRangeException) { }

            try
            {
                //If the button below is a bomb
                if (grid[horizontal, vertical - 1].BackColor == bombColour)
                {
                    surroundingMines++;
                }
            }
            //If a position not in the array tries to get accessed, do nothing
            catch (IndexOutOfRangeException) { }

            try
            {
                //If the button diagonally below (left) is a bomb
                if (grid[horizontal - 1, vertical - 1].BackColor == bombColour)
                {
                    surroundingMines++;
                }
            }
            //If a position not in the array tries to get accessed, do nothing
            catch (IndexOutOfRangeException) { }

            try
            {
                //If the button to the left is a bomb
                if (grid[horizontal - 1, vertical].BackColor == bombColour)
                {
                    surroundingMines++;
                }
            }
            //If a position not in the array tries to get accessed, do nothing
            catch (IndexOutOfRangeException) { }

            try
            {
                //If the button diagonally above (left) is a bomb
                if (grid[horizontal - 1, vertical + 1].BackColor == bombColour)
                {
                    surroundingMines++;
                }
            }
            //If a position not in the array tries to get accessed, do nothing
            catch (IndexOutOfRangeException) { }

            return surroundingMines;
        }

        private int SurroundingMines(object sender, int horizontal, int vertical)
        {
            //Set the button clicked to a variable
            Button user = ((Button)sender);
            //Set colours of mines and normal squares to variables
            var condition = MineCheck(sender, horizontal, vertical) != 0;
            var normalColour = Color.FromArgb(211, 211, 211);
            var bombColour = Color.FromArgb(212, 211, 211);
            var clicked = Color.White;
            //Set minecheck to a variable
            var imageValue = MineCheck(sender, horizontal, vertical);
            //Initialise variables
            int minesRemoved = 0;
            int minesTotalRemoved = 0;
            int range = 1;
            bool mineFound = true;
            //While there are still mines around
            while (mineFound == true)
            {
                try
                {
                    //If the button above  is not a mine and has a mine nearby
                    if (grid[horizontal, vertical + range].BackColor == normalColour && condition)
                    {
                        //Set button above as clicked and add to mineRemoved variable
                        grid[horizontal, vertical + range].BackColor = clicked;
                        mineFound = true;
                        minesRemoved++;
                    }
                    else if (MineCheck(sender, horizontal, vertical) == 0)
                    {
                        mineFound = false;
                    }
                }
                //If a position not in the array tries to get accessed, do nothing
                catch (IndexOutOfRangeException) { }

                try
                {
                    //If the button diagonally (right) above is not a mine and has a mine nearby
                    if (grid[horizontal + range, vertical + range].BackColor == normalColour && condition)
                    {
                        //Set button diagonally (right above) as clicked and add to mineRemoved variable
                        grid[horizontal + range, vertical + range].BackColor = clicked;
                        mineFound = true;
                        minesRemoved++;
                    }
                    else
                    {
                        mineFound = false;
                    }
                }
                //If a position not in the array tries to get accessed, do nothing
                catch (IndexOutOfRangeException) { }

                try
                {
                    //If the button to the right is not a mine and has a mine nearby
                    if (grid[horizontal + range, vertical].BackColor == normalColour && condition)
                    {
                        //Set button to the right as clicked and add to mineRemoved variable
                        grid[horizontal + range, vertical].BackColor = clicked;
                        mineFound = true;
                        minesRemoved++;
                    }
                    else
                    {
                        mineFound = false;
                    }
                }
                //If a position not in the array tries to get accessed, do nothing
                catch (IndexOutOfRangeException) { }

                try
                {
                    //If the button diagonally (right) below is not a mine and has a mine nearby
                    if (grid[horizontal + range, vertical - range].BackColor == normalColour && condition)
                    {
                        //Set button diagonally (right) below as clicked and add to mineRemoved variable
                        grid[horizontal + range, vertical - range].BackColor = clicked;
                        mineFound = true;
                        minesRemoved++;
                    }
                    else
                    {
                        mineFound = false;
                    }
                }
                //If a position not in the array tries to get accessed, do nothing
                catch (IndexOutOfRangeException) { }

                try
                {
                    //If the button below is not a mine and has a mine nearby
                    if (grid[horizontal, vertical - range].BackColor == normalColour && condition)
                    {
                        //Set button below as clicked and add to mineRemoved variable
                        grid[horizontal, vertical - range].BackColor = clicked;
                        mineFound = true;
                        minesRemoved++;
                    }
                    else
                    {
                        mineFound = false;
                    }
                }
                //If a position not in the array tries to get accessed, do nothing
                catch (IndexOutOfRangeException) { }

                try
                {
                    //If the button diagonally (left) below is not a mine and has a mine nearby
                    if (grid[horizontal - range, vertical - range].BackColor == normalColour && condition)
                    {
                        //Set button diagonally (left) below as clicked and add to mineRemoved variable
                        grid[horizontal - range, vertical - range].BackColor = clicked;
                        mineFound = true;
                        minesRemoved++;
                    }
                    else
                    {
                        mineFound = false;
                    }
                }
                //If a position not in the array tries to get accessed, do nothing
                catch (IndexOutOfRangeException) { }

                try
                {
                    //If the button to the right is not a mine and has a mine nearby
                    if (grid[horizontal - range, vertical].BackColor == normalColour && condition)
                    {
                        //Set button to the left as clicked and add to mineRemoved variable
                        grid[horizontal - range, vertical].BackColor = clicked;
                        mineFound = true;
                        minesRemoved++;
                    }
                    else
                    {
                        mineFound = false;
                    }
                }
                //If a position not in the array tries to get accessed, do nothing
                catch (IndexOutOfRangeException) { }

                try
                {
                    //If the button diagonally (left) above is not a mine and has a mine nearby
                    if (grid[horizontal - range, vertical + range].BackColor == normalColour && condition)
                    {
                        //Set button diagonally (left) above as clicked and add to mineRemoved variable
                        grid[horizontal - range, vertical + range].BackColor = clicked;
                        mineFound = true;
                        minesRemoved++;
                    }
                    else
                    {
                        mineFound = false;
                    }
                }
                //If a position not in the array tries to get accessed, do nothing
                catch (IndexOutOfRangeException) { }

                //Choose image depending on the variable amount
                switch (minesRemoved)
                {
                    //No Mines
                    case 0:
                        break;
                    //One Mine
                    case 1:
                        user.Image = Properties.Resources._1;
                        break;
                    //Two Mines
                    case 2:
                        user.Image = Properties.Resources._2;
                        break;
                    //Three Mines
                    case 3:
                        user.Image = Properties.Resources._3;
                        break;
                    //Four Mines
                    case 4:
                        user.Image = Properties.Resources._4;
                        break;
                    //Five Mines
                    case 5:
                        user.Image = Properties.Resources._5;
                        break;
                    //Six Mines
                    case 6:
                        user.Image = Properties.Resources._6;
                        break;
                    //Seven Mines
                    case 7:
                        user.Image = Properties.Resources._7;
                        break;
                    //Eight Mines
                    case 8:
                        user.Image = Properties.Resources._8;
                        break;
                    //Erroneous amount of mines
                    default:
                        break;
                }
                //Add amount of mines found to total then reset variable to 0
                minesTotalRemoved = minesTotalRemoved + minesRemoved;
                minesRemoved = 0;
            }
            return minesTotalRemoved;
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult answer = MessageBox.Show("All progress will be lost if exited", "Are You Sure?", MessageBoxButtons.YesNo);
            if (answer == DialogResult.Yes)
            {
                Application.Exit();
            }
            else
            {

            }
        }
    }
}

