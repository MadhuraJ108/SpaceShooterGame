using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;


namespace SpaceShooter

{

    public partial class Form1 : Form
    {
        //we have to create our media
        WindowsMediaPlayer gameMedia; //this is the background media while playing
        WindowsMediaPlayer shootgMedia; //this is for shoot media
        WindowsMediaPlayer Explosion; //this is for sound on colliding

        PictureBox[] stars; //PictureBox array will store tge stars we see on the screen  
        int backgroundspeed; //background will move  
        int playerSpeed;

        PictureBox[] munitions; //create a table for munitions
        int MunitionSpeed;

        PictureBox[] enemies; //declare a picturebox array for enemies
        int enemiespeed; //declare a variable for enemies speed-speed the enemies will move

        PictureBox[] enemiesMunition; //declare a picturebox array for enemies munition array
        int enemiesMunitionSpeed; // create an int for enemies munition speed

        Random rnd; //to random-dynamically position stars on screen
        int score; //player's score that wil appear on screen 
        int level;
        int difficulty; //how many more enemies will shoot 
        bool pause;
        bool gameIsOver;
        public Form1()

        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) //when windows will be loaded- when game appears on screen-  

        //this function is called  
        {
            pause = false; //in the beginning the game is pause so it is set to false 
            gameIsOver = false; //as long as we are playinmg the game is not over so this is set to false 
            score = 0;
            level = 1;
            difficulty = 9; //we decrease difficulty from 9 to 0

            backgroundspeed = 4; //initialized variable. when timer is called then stars will move by 4 pixel from top to bottom   

            playerSpeed = 4;
            backgroundspeed = 4; //initialized variable. when timer is called then stars will move by 4 pixel from top to bottom  
            playerSpeed = 4;


            //code for enemies starts here
            enemiespeed = 4;//initialize the enemy spped
            //load the images
            Image enemy1 = Image.FromFile(@"asserts\\E1.png");
            Image enemy2 = Image.FromFile(@"asserts\\E2.png");
            Image enemy3 = Image.FromFile(@"asserts\\E3.png");
            Image boss1 = Image.FromFile(@"asserts\\Boss1.png");
            Image boss2 = Image.FromFile(@"asserts\\Boss2.png");
            //we create array with 10 pictureboxes because we will create 10 enemies and everytime we will create them in loop
            enemies = new PictureBox[10];
            //Initialize enemies Pictureboxes
            for (int i = 0; i < enemies.Length; i++) //we setup all values for enemies array
            {
                enemies[i] = new PictureBox(); //we create an object for each picturebox at index i
                enemies[i].Size = new Size(30, 30);//the size should be 40pixel
                enemies[i].SizeMode = PictureBoxSizeMode.Zoom;//image should be stretched so we have zoom
                enemies[i].BorderStyle = BorderStyle.None;//we dont want border
                enemies[i].Visible = false;//visbility a the beginning is false because we dont want to seethem as we start the game. They come from top to bottom
                this.Controls.Add(enemies[i]); //we add them to control tab
                enemies[i].Location = new Point((i + 1) * 32, -32);//we set an initial position , changed the coordinates here bcoz it was giving wrong output
            }
            //we couldnt set images in loop becase images are different with different colors so we set them out of loop
            //we load all images and set images for each picturebox of our enemies array
            enemies[0].Image = boss1;
            enemies[1].Image = enemy2;
            enemies[2].Image = enemy3;
            enemies[3].Image = enemy3;
            enemies[4].Image = enemy1;
            enemies[5].Image = enemy3;
            enemies[6].Image = enemy2;
            enemies[7].Image = enemy3;
            enemies[8].Image = enemy2;
            enemies[9].Image = boss2;

            //code for enemies ends here


            //code for munition starts here
            MunitionSpeed = 20; //munition speed can be 20pixels
            munitions = new PictureBox[3]; //munitions spped array will also be initialized with 3 picture boxes
            //load image for munition
            Image munition = Image.FromFile(@"asserts\munition.png");



            //next step is to set all images in picture boxes
            for (int i = 0; i < munitions.Length; i++) //we use for loopas long we havent reached the length of the munition
                                                       //then we create an object of each munition index
            {
                munitions[i] = new PictureBox();
                munitions[i].Size = new Size(8, 8);      //we define a size 8pixel by 8 pixel
                munitions[i].Image = munition; //image will be munition
                munitions[i].SizeMode = PictureBoxSizeMode.Zoom; //size mode will be zoomed
                munitions[i].BorderStyle = BorderStyle.None; //we want no border
                this.Controls.Add(munitions[i]); // we add it to the control tab
            }
            //code for munition ends here

            //code for media starts

            //Create WMP
            gameMedia = new WindowsMediaPlayer(); //we create object for those media
            shootgMedia = new WindowsMediaPlayer(); //we create object for those media
            Explosion = new WindowsMediaPlayer();//we create an object for explosion media

            //load songs
            gameMedia.URL = "songs\\GameSong.mp3";
            shootgMedia.URL = "songs\\shoot.mp3";
            Explosion.URL = "songs\\boom.mp3";

            //Setup Songs settings
            gameMedia.settings.setMode("loop", true);  //as long as the player is playing the song will keep playing in loop
            gameMedia.settings.volume = 5;
            shootgMedia.settings.volume = 1;
            Explosion.settings.volume = 6;

            gameMedia.controls.play(); // the background song will be played when the form gets loaded
            //code for media ends here

            stars = new PictureBox[15]; //here is the picturebox array  
            rnd = new Random();
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i] = new PictureBox(); //an object is created for every element in array  
                stars[i].BorderStyle = BorderStyle.None; //we dont want any border  
                stars[i].Location = new Point(rnd.Next(20, 580), rnd.Next(-10, 400));
                if (i % 2 == 1) //size and color of stars is divided into 2 types using if else  
                {
                    stars[i].Size = new Size(2, 2); //some stars have 2 pixel size  
                    stars[i].BackColor = Color.Wheat; //and wheat colour  
                }
                else
                {
                    stars[i].Size = new Size(3, 3); //some stars have 3 pixel size  
                    stars[i].BackColor = Color.DarkGray; //and dark grey colour  
                }
                this.Controls.Add(stars[i]);
            }

            //enemies munition code starts here
            enemiesMunitionSpeed = 4; //initialize the enemiesmunition speed to 4 pixel- each 20 millisec interval, munition will move forward
            enemiesMunition = new PictureBox[10]; //initialize the picture box array with 10 pictureboxes inside

            for (int i = 0; i < enemiesMunition.Length; i++)
            {
                enemiesMunition[i] = new PictureBox();//create a new picturebox
                enemiesMunition[i].Size = new Size(1, 15); //set size, changed this value
                enemiesMunition[i].Visible = false; //set visibility to false
                enemiesMunition[i].BackColor = Color.Yellow;
                int x = rnd.Next(0, 10); //gennertae  a number between 0 and 10 because we need to choose dynamically which enemy will have munition
                enemiesMunition[i].Location = new Point(enemies[x].Location.X, enemies[x].Location.Y - 10);//we set position of munition: location which is =where the enemies are 
                this.Controls.Add(enemiesMunition[i]); //we add our picturebox to controls tab
            }
            //enemies munition code end here for Form.load

        }
        private void MoveBgTimer_Tick(object sender, EventArgs e)
        {
            //here we are using 2 for loops because we have 2 cateories of speed- some stars move faster than others  
            for (int i = 0; i < stars.Length / 2; i++) //from 0 to half length of array  
            {
                stars[i].Top += backgroundspeed;//we set the position of stars  
                if (stars[i].Top >= this.Height) //here we check the stars reach the maximum height of the screen
                {
                    stars[i].Top = -stars[i].Height;//if stars reach the end of screen then we push them back to the top 
                }
            }

            for (int i = stars.Length / 2; i < stars.Length; i++)
            {
                stars[i].Top += backgroundspeed - 2; //these stars are slower than stars with upper for loop  
                if (stars[i].Top >= this.Height)
                {
                    stars[i].Top = -stars[i].Height;
                }

            }
        }
        private void LeftMoveTimer_Tick(object sender, EventArgs e)

        {
            if (Player.Left > 10) //when the player moves left check if player reach 10pixel on left side because we want space in wall and plyer 
            {
                Player.Left -= playerSpeed;
            }
        }
        //we are checking as long we are in the range 10 to 580 pixel then we can move to left or right 
        //and as long we are in the range of 10 to 400 pixel then we can move down or top 
        //By using pictureBox we cannot use the property "right" we can just use property "left" 
        //beacuse the right property is private- READ-ONLY. We dont have accessibility. we have access to left property of picturebox 
        // this is the reason- when we are moving to right we have to increase 
        //when we are moving to left we dont have to decrease 
        //We do the same for up and down because we cannot access the down property 
        private void RightMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Right < 380)
            {
                Player.Left += playerSpeed;  //when we are moving to right we have to increase 
            }
        }
        private void DownMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Top < 250)
            {
                Player.Top += playerSpeed;
            }
        }
        private void UpMOveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Top > 10)
            {
                Player.Top -= playerSpeed; //when we are moving to up side we have to increase 
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //we will check if the player has pushed up, down, left, right Key and we activate the timer for the key he has pressed 
            if (!pause) //if the game is not pause then all of below can work- to prevent timers to start when we pause the game
            {
                if (e.KeyCode == Keys.Right)
                {
                    RightMoveTimer.Start();
                }
                if (e.KeyCode == Keys.Left)
                {
                    LeftMoveTimer.Start();
                }
                if (e.KeyCode == Keys.Down)
                {
                    DownMoveTimer.Start();
                }
                if (e.KeyCode == Keys.Up)
                {
                    UpMOveTimer.Start();
                }
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //we tell the computer what he has to do when player release the button- thats why we use key up 
            //when player release the button then this part is activated and we stop the timers that were started 
            RightMoveTimer.Stop();
            LeftMoveTimer.Stop();
            DownMoveTimer.Stop();
            UpMOveTimer.Stop();
            if (e.KeyCode == Keys.Space) //if space button has been pushed
                if (!gameIsOver) //if game isnt over
                {
                    if (pause)//if the game is paused 
                    {
                        StartTimers(); //if the game is paused means the player will replay to restart so we start timers
                        label1.Visible = false; //label will dissapeear
                        gameMedia.controls.play(); //game sounds will play
                        pause = false;//pause will be set to false means game is running
                    }
                    else
                    { //if the player is playing and player pushed the space button then we implement following things: name test appear with PAUSE
                        label1.Location = new Point(this.Width / 2-70, 100);
                        //label1.Location = new Point(this.Width / 2 - 120, 150);
                        label1.Text = "PAUSED"; //we name test appear with PAUSE
                        label1.Visible = true; //set visibility to true
                        gameMedia.controls.pause(); //we stop sound
                        StopTimers(); //we stop timers
                        pause = true; //we set pause to true
                    }
                }
        }
        private void MoveMunitionTimer_Tick(object sender, EventArgs e)
        {
            shootgMedia.controls.play(); //the media sound effect will play as the munition will be in effect foe ever 20 milliseconds
            //this is a method which will be called every 20 milliseconds

            for (int i = 0; i < munitions.Length; i++) // for loop for running the munitions array to check the position of actual picture box or munition
            {
                if (munitions[i].Top > 0) //if it hasn't reached the top of the screen then the property visibility woll be set to true
                {
                    munitions[i].Visible = true;    // the property visibility woll be set to true
                    munitions[i].Top -= MunitionSpeed; //then we decrease the positions so he reaches 0
                    Collision(); //when we move the munition we check for colliusion- call the collision method to check if player-enemy or enemy-munition hits
                }
                else
                {
                    munitions[i].Visible = false;  //on reaching 0 we set the visibility to false
                    //changed th value of X to give approprite position to munition
                    munitions[i].Location = new Point(Player.Location.X + 13, Player.Location.Y - i * 30); // we bring it back to position where the player is located
                    //so the munition is always located where the player is located
                    //everythiung will be done continuously as long as player is moving
                }
            }
        }
        private void MoveEnemiesTimer_Tick(object sender, EventArgs e)
        {
            //we will call the mOveEnemies method in our timer so that each 100milliseconds we have it called
            MoveEnemies(enemies, enemiespeed); //we pass enemies array and enemiesSpeed as parameters to the method
        }
        private void MoveEnemies(PictureBox[] array, int speed)//we have created this function to move the enemies and we will call it in MoveEnemiesTimer event handler
                                                               //this function takes Picturebox Array- which are our enemies array and the spped
                                                               //we use speed parameter here because later on when the game becomes harder or player get to next level-high level
                                                               //then we can increase the speedand make game more difficult and dynamic
        {
            //here we use a really simple for loop to move from top to bottom
            for (int i = 0; i < array.Length; i++)
            {
                array[i].Visible = true; //we set visibility to teue because we want to see them
                array[i].Top += speed; //we set at top position and then we increase by 4 pixels every 100milliseconds for the enemies to move
                if (array[i].Top > this.Height)//later when the enemy reach the height of the window bring it back to thetop to move froom top to bottom
                {
                    array[i].Location = new Point((i + 1) * 50, -200);
                }
            }
        }

        private void Collision() //this method will check all collisions between munitions-enemies and player-enemies
                                 //this function Collision is called in the MoveMunitionTimer
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (munitions[0].Bounds.IntersectsWith(enemies[i].Bounds) || munitions[1].Bounds.IntersectsWith(enemies[i].Bounds) || munitions[2].Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    Explosion.controls.play(); //if there is collision between munition and enemies we play explosion sound effect

                    score += 1; //we inc the score when players munitions hit enemies
                    scorelbl.Text = (score < 10) ? "0" + score.ToString() : score.ToString(); //this is how the text will look- if it is smaller than 10, then add a 0 infront of it
                    if(score%30==0) //everytime the score is multiple of 30, we get to next level- 30,60,90- we inc level
                    {
                        level += 1;
                        levellbl.Text=(level<10)?"0"+level.ToString() : level.ToString();//how the level should look like
                        if (enemiespeed<=10 && enemiesMunitionSpeed<=10 && difficulty>=0) //if the enemyspeed is less than 10, then we can inc the enemyspeed (maximum enemy speed is 10)
                            //similarly for enemiesMunitionSpeed is less than 10, then we can inc it and dificulty has to be more than 0
                        {
                            difficulty--; //just decrease the difficulty when we haven't reached 0
                            enemiespeed++; //at the same time we inc the enemySpeed
                            enemiesMunitionSpeed++; //inc the enemyMunitionSpeed
                        }
                        if(level==10) //if player reaches level 9 then game gets over
                        {
                            GameOver("WELL DONE");
                        }
                    }

                    enemies[i].Location = new Point((i + 1) * 50, -100); //then we bring enemy back to the top where he started to come from top to bottom
                }
                if (Player.Bounds.IntersectsWith(enemies[i].Bounds)) //if there is collision btween player and enemies then 
                {
                    Explosion.settings.volume = 30;
                    Explosion.controls.play();
                    Player.Visible = false; //we makew player disappear
                    GameOver("GAME OVER");
                }
            }
        }
        private void GameOver(string str)
        {
            label1.Text = str; 
            label1.Location = new Point(100, 70); //set label position
            label1.Visible = true;
            ReplayBtn.Visible = true;
            ExitBtn.Visible = true; //exit btn appears on screen

            gameMedia.controls.stop(); //stop background sounds
            StopTimers(); //stoptimers
        }
        private void StopTimers() //stop timers
        {
            MoveBgTimer.Stop();
            MoveEnemiesTimer.Stop();
            MoveMunitionTimer.Stop();
            EnemiesMunitionTimer.Stop();
        }
        private void StartTimers() //start timers
        {
            MoveBgTimer.Start();
            MoveEnemiesTimer.Start();
            MoveMunitionTimer.Start();
        }
        private void EnemiesMunitionTimer_Tick(object sender, EventArgs e) //This wil help enemy munition to move
        {
            for (int i = 0; i < enemiesMunition.Length-difficulty; i++)
            {
                if (enemiesMunition[i].Top < this.Height) //we check the position of munition is not hight=er than our screen
                {
                    enemiesMunition[i].Visible = true; //as long as the munition is in the range it has visibility true
                    enemiesMunition[i].Top += enemiesMunitionSpeed; //if the munition has not reached the height then we increase the position with 4 pixel and it goes on
                    CollisionWithEnemiesMunition();
                }
                else
                {
                    enemiesMunition[i].Visible = false; //if munition is outside our screen then visibility is off
                    int x = rnd.Next(0, 10); //we choose which enemy will have the munition 
                    enemiesMunition[i].Location = new Point(enemies[x].Location.X + 15, enemies[x].Location.Y + 30); //we bring the munition back to position of enemy, changed te value                 
                }
            }
        }
        private void CollisionWithEnemiesMunition() //method for enemies munition and player
        {
            for (int i = 0; i < enemiesMunition.Length; i++) //for loop that runs through enemies munition table
            {
                if (enemiesMunition[i].Bounds.IntersectsWith(Player.Bounds))//chcek if bounds of enemy's munition have intersectiuon with player's bounds
                {
                    enemiesMunition[i].Visible = false; //dissapper
                    Explosion.settings.volume = 30;//sound
                    Explosion.controls.play();
                    Player.Visible = false;//player becomes invisible
                    GameOver("Game Over");

                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ReplayBtn_Click(object sender, EventArgs e)
        {
            this.Controls.Clear(); //when replay button is hit, we first clear the screen
            InitializeComponent(); //then we initailize all components
            Form1_Load(e, e); //then load it 
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }
    }
}


