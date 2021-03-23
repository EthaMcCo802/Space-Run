using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Space_Run
{
    //Ethan McComb
    //Mr. T
    //A space racing game which pins two players against each other in an ultimate battle to see who can get the most points
    public partial class Form1 : Form
    {
        int player1X = 120;
        int player1Y = 500;
        int player1Score = 0;

        int player2X = 450;
        int player2Y = 500;
        int player2Score = 0;

        int playerWidth = 15;
        int playerHeight = 15;
        int playerSpeed = 10;

        List<int> ballX = new List<int>();
        List<int> ballY = new List<int>();
        int ballSpeed = 6;
        int ballWidth = 10;
        int ballHeight = 10;

        bool wDown = false;
        bool sDown = false;
        bool upArrowDown = false;
        bool downArrowDown = false;

        List<int> Direction = new List<int>();

        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush blueBrush = new SolidBrush(Color.Blue);
        SolidBrush redBrush = new SolidBrush(Color.Red);

        Random randGen = new Random();

        string gameState = "waiting";

        SoundPlayer space = new SoundPlayer(Properties.Resources.Space);
        SoundPlayer go = new SoundPlayer(Properties.Resources.Go);
        SoundPlayer crash = new SoundPlayer(Properties.Resources.Crash);
        SoundPlayer winner = new SoundPlayer(Properties.Resources.Winner);
        SoundPlayer ding = new SoundPlayer(Properties.Resources.Ding);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                //Makes the keys only work when pressed
                #region Move Keys
                //The keys for player control
                case Keys.W:
                    wDown = true;
                    space.Play();
                    break;
                case Keys.S:
                    sDown = true;
                    space.Play();
                    break;
                case Keys.Up:
                    upArrowDown = true;
                    space.Play();
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    space.Play();
                    break;
                #endregion
                #region Space Key
                //Starts the game
                case Keys.Space:
                    if (gameState == "waiting" || gameState == "over")
                    {

                        GameInitialize();

                    }
                    break;
                #endregion
                #region Escape Key
                //Exits the game
                case Keys.Escape:
                    if (gameState == "waiting" || gameState == "over")

                    {

                        Application.Exit();

                    }
                    break;
                    #endregion
            }
        }

        public void GameInitialize()
        {
            //Starts the game
            go.Play();

            titleLabel.Text = "";

            subTitleLabel.Text = "";

            gameTimer.Enabled = true;

            gameState = "running";

            p1ScoreLabel.Text = $"0";
            p2ScoreLabel.Text = $"0";

            player1Score = 0;
            player2Score = 0;
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            #region Waiting
            //This state is used in the beginning of the game
            if (gameState == "waiting")

            {
                titleLabel.Text = "SPACE RUN";

                subTitleLabel.Text = "Press Space Bar to Start or Escape to Exit";

                p1ScoreLabel.Text = "";
                p2ScoreLabel.Text = "";
            }
            #endregion
            #region Running
            //This state is used while the game is running
            else if (gameState == "running")
            {
                for (int i = 0; i < ballY.Count; i++)
                {
                    e.Graphics.FillEllipse(whiteBrush, ballX[i], ballY[i], ballWidth, ballHeight);
                }
                e.Graphics.FillRectangle(redBrush, player1X, player1Y, playerWidth, playerHeight);
                e.Graphics.FillRectangle(blueBrush, player2X, player2Y, playerWidth, playerHeight);
                e.Graphics.FillRectangle(whiteBrush, 285, 0, 10, 800);

                p1Label.Text = "USSR";
                p2Label.Text = "NASA";
            }
            #endregion
            #region Over
            //This state is used when the game ends
            else if (gameState == "over")
            {
                if (player1Score == 3)
                {                    
                    titleLabel.Text = "USSR Wins!";
                    titleLabel.ForeColor = Color.Red;
                    subTitleLabel.Text = "Press Space Bar to Start or Escape to Exit";
                }
                else if (player2Score == 3)
                {
                    titleLabel.Text = "NASA Wins!";
                    titleLabel.ForeColor = Color.Blue;
                    subTitleLabel.Text = "Press Space Bar to Start or Escape to Exit";
                }
            }
            #endregion
        }
        private void gameTimer_Tick(object sender, EventArgs e)

        {
            #region Astroid Direction
            //Randomizes the starting locations of the asteroids 
            for (int i = 0; i < ballX.Count(); i++)
            {
                ballX[i] += Direction[i];

                if (ballX[i] < 0)
                {
                    Direction[i] = Direction[i] * -1;
                }
                if (ballX[i] > 590)
                {
                    Direction[i] = Direction[i] * -1;
                }
            }

            if (ballY.Count == 0)
            {
                for (int i = 0; i < 50; i++)
                {
                    ballY.Add(randGen.Next(100, 450));
                    ballX.Add(randGen.Next(100, 450));
                    int ranDirect = randGen.Next(0, 2);
                    if(ranDirect == 1)
                    {
                        Direction.Add(ballSpeed);
                    }
                    else
                    {
                        Direction.Add(ballSpeed * -1);
                    }
                }
            }
            #endregion
            #region Move Players
            //Moves player 1
            if (wDown == true && player1Y > 0)
            {
                player1Y -= playerSpeed;
            }

            if (sDown == true && player1Y < this.Height - playerHeight)
            {
                player1Y += playerSpeed;
            }

            //Moves player 2 
            if (upArrowDown == true && player2Y > 0)
            {
                player2Y -= playerSpeed;
            }

            if (downArrowDown == true && player2Y < this.Height - playerHeight)
            {
                player2Y += playerSpeed;
            }
            #endregion
            #region Collisions
            //creates Rectangles of objects on screen to be used for collision detection 
            Rectangle player1Rec = new Rectangle(player1X, player1Y, playerWidth, playerHeight);
            Rectangle player2Rec = new Rectangle(player2X, player2Y, playerWidth, playerHeight);

            for (int i = 0; i < ballX.Count; i++)
            {
                Rectangle ballRec = new Rectangle(ballX[i], ballY[i], ballWidth, ballHeight);
                if (player1Rec.IntersectsWith(ballRec))
                {
                    player1Y = 500;
                    player1X = 120;
                    crash.Play();
                }
                else if (player2Rec.IntersectsWith(ballRec))
                {
                    player2Y = 500;
                    player2X = 450;
                    crash.Play();
                }
            }
            #endregion
            #region Points
            //If a player reaches the end then it adds a point
            if (player1Y <= 3)
            {
                player1Score++;
                player1X = 125;
                player1Y = 500;
                p1ScoreLabel.Text = $"{player1Score}";
                ding.Play();
            }
            else if (player2Y <= 3)
            {
                player2Score++;
                player2X = 425;
                player2Y = 500;
                p2ScoreLabel.Text = $"{player2Score}";
                ding.Play();
            }
            #endregion
            #region Winner
            //Determines who the winner is
            if (player1Score == 3)
            {
                gameState = "over";
                winner.Play();
            }
            else if (player2Score == 3)
            {
                gameState = "over";
                winner.Play();
            }
            #endregion
            Refresh();
        }
    }
}
