using System;
using System.IO;
using System.Windows.Forms;
using System.Threading;

/*12/4/21 Updated game so that the user can click new game, and choose between whether they want to upload the phrases or use ones already in the game.
 * The user can see the name of the file they choose to upload
 * When a file is uploaded the amount of phrases are counted, so that a random one can be choosen later.
 * the player can now enter their name and it will appear on the labels when they click begin
 * the code for getting an uploaded phrase and putting it into 2 arrays that will compare each other to see if it is solved was created
 * the empty phrase is shown on the board
 * category is displayed
 * player can get a value by clicking the spin button spin
 * if a player buys a vowel and gets it correct the player will lose $200 and is able to see the value on the board
 * 
 */
//*****still need to update settings in the menu strip******

namespace WheelOfFortune
{
    public partial class Puzzle : Form
    {
        int numOfUploadedPhrases = -1;
        string fileName;
        string category;
        string[] phraseArray;
        string playerNameTextBox;
        string[] playerNameCollection;
        string[] correctPhrase;
        string[] solvePhrase;
        int currPlayer = 0;
        int spinValue;
        int roundNum = 1;
        string displayPhrase;
        string displayPhraseCopy;
        string currLetter;
        int timeLeft = 30;
        int playerTime = 3;
        int playerWon = 0;
        string tempPhrase = "";
        string currPhrase;
        string unusedConsonants = "BCDFGHJKLMNPQRSTVWXYZ";
        string unusedVowels = "AEIOU";

        //determines if player is choosing a vowel(0) or consonant(1)
        int Letter = 0;

        //length of the phrase
        int len;

        //keeps track of scores
        int p1TotalScore = 0;
        int p2TotalScore = 0;
        int p3TotalScore = 0;

        // current round scores
        int p1CurrScore = 0;
        int p2CurrScore = 0;
        int p3CurrScore = 0;


        //is the string that displays in the box that the players will solve
        string newString;
        Random r = new Random();

        //variable determines whether there is a correct file uploaded-- 0 = not uploaded 
        int isFile = 0;
        //this variable will help decide what array to get the phrases from-- 0 = game phrases & 1 = uploaded phrases
        int isUploaded = 0;
        public Puzzle()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            hidePanels();
            startPanel.Show();
            currPlayer = 0;

        }

        //makes it easier to hide panels for each time a button is clicked
        // then we can just call hidePanels and then show the one that needs to be displayed
        private void hidePanels()
        {
            startPanel.Hide();
            puzzleLetterPanel.Hide();
            gameTypePanel.Hide();
            uploadPhrasesPanel.Hide();
            roundOneDisplayPanel.Hide();
            optionsPuzzlePanel.Hide();
            spinDisplayPanel.Hide();
            solvingPanel.Hide();
            roundSummaryPanel.Hide();
            winnersPanel.Hide();
        }

        private void getThePhrase()
        {
            //will get the next phrase depending on what round it is and whether the files are uploaded
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string line;
            int x;
            
            
            Random rndPhrases = new Random();

            if (isUploaded == 0)
            {
                //use phrases that we created

                numOfUploadedPhrases = 0;
                StreamReader sr = new StreamReader("TheGamePhrases(432).txt");
                line = sr.ReadLine();
                phraseArray = System.IO.File.ReadAllLines("TheGamePhrases(432).txt");

                while (line != null)
                {
                    numOfUploadedPhrases++;
                    //Read the next line
                    line = sr.ReadLine();
                }
                x = rndPhrases.Next(0, numOfUploadedPhrases);

                //determines what the category is so it can be displayed
                //0-69 category = Phrases
                if (x <= 70)
                {
                    category = "Phrase";
                }
                //70-82 = Places
                else if (x > 70 && x <= 83)
                {
                    category = "Places";
                }
                //83-304 = Titles
                else if (x > 83 && x <= 305)
                {
                    category = "Titles";                    
                }
                //305-343 = Events
                else if (x >305 && x <= 344)
                {
                    category = "Event";                    
                }
                //344-431 = Person
                else if (x > 344)
                {
                    category = "Person";                    
                }


                //update the category labels
                catNameLabel.Text = category;
                catLabel1.Text = category;
                catNameLabel2.Text = category;
                catNameLabel3.Text = category;


                //use the phrases that the user uploaded

                x = rndPhrases.Next(0, numOfUploadedPhrases);
                currPhrase = phraseArray[x].ToUpper();
                len = currPhrase.Length;
                correctPhrase = new string[len];
                solvePhrase = new string[len];

                //goes through and puts the phrase into an array that 
                for (int i = 0; i < len; i++)
                {
                    char c = currPhrase[i];
                    string a = c.ToString();
                    if (a == " ")
                    {
                        correctPhrase[i] = "@";
                    }
                    else
                    {
                        correctPhrase[i] = a;
                    }
                    

                    //checks if each char in the phrase is a letter
                    for (int j = 0; j < alphabet.Length; j++)
                    {
                        string s = alphabet[j].ToString();
                        if (correctPhrase[i] == s)
                        {
                            solvePhrase[i] = "_ ";
                            j = 30;
                        }
                        //if the character was not a letter
                        else if (j == 26)
                        {
                            solvePhrase[i] = s;
                        }
                        else if (correctPhrase[i] == "@")
                        {
                            solvePhrase[i] = "@";
                        }
                        else
                        {
                                solvePhrase[i] = correctPhrase[i];
                        }
                    }
                }
            }
            else if (isUploaded == 1)
            {
                //use the phrases that the user uploaded

                x = rndPhrases.Next(0, numOfUploadedPhrases);
                currPhrase = phraseArray[x].ToUpper();
                len = currPhrase.Length;
                correctPhrase = new string[len];
                solvePhrase = new string[len];

                //goes through and puts the phrase into an array that 
                for (int i = 0; i < len; i++)
                {
                    char c = currPhrase[i];
                    string a = c.ToString();
                    if (a == " ")
                    {
                        correctPhrase[i] = "@";
                    }
                    else
                    {
                        correctPhrase[i] = a;
                    }

                    //checks if each char in the phrase is a letter
                    for (int j = 0; j < alphabet.Length; j++)
                    {
                        string s = alphabet[j].ToString();
                        if (correctPhrase[i] == s)
                        {
                            solvePhrase[i] = "_ ";
                            j = 30;
                        }
                        else if (correctPhrase[i] == "@")
                        {
                            solvePhrase[i] = "@";
                        }
                        //if the character was not a letter
                        else if (j == 26)
                        {
                            solvePhrase[i] = s;
                        }
                        else
                        {
                            solvePhrase[i] = correctPhrase[i];
                        }
                    }
                }

            }


            displayPhrase = "";

            for (int i = 0; i < solvePhrase.Length; i++)
            {
                if(solvePhrase[i] == "@")
                {
                    displayPhrase = displayPhrase + "@";
                }
                else
                {
                    displayPhrase = displayPhrase + solvePhrase[i];
                }
            }

            changeStringDisplay();




        }
         private void getSpinVal()
         {
            int a = r.Next(0, 101);

            if (a < 10)
            {
                spinValue = 100;
            }
            else if (10 <= a && a < 20)
            {
                spinValue = 200;
            }
            else if (20 <= a && a < 30)
            {
                spinValue = 300;
            }
            else if (30 <= a && a < 40)
            {
                spinValue = 400;
            }
            else if (40 <= a && a < 45)
            {
                spinValue = 500;
            }
            else if (45 <= a && a < 50)
            {
                spinValue = 600;
            }

            else if (50 <= a && a < 55)
            {
                spinValue = 700;
            }

            else if (55 <= a && a < 60)
            {
                spinValue = 800;
            }
            else if (60 <= a && a < 75)
            {
                spinValue = 0;
            }

            else if (75 <= a && a < 78)
            {
                spinValue = 20000;
            }
            else if (78 <= a && a < 85)
            {
                spinValue = 1000;
            }
            else if (85 <= a && a <= 90)
            {
                spinValue = -1;
            }
            else if (90 <= a && a <= 100)
            {
                spinValue = 150;
            }
         }

        

        /* Computer Players : Players 2 and 3 Turn
         * 
         */

        private void otherPlayerConsonantCheck()
        {
            string s = currLetter;
            if (s == "B")
            {
                //check if there's an B
                currLetter = "B";
                Bbtn.Enabled = false;
                Bbtn2.Enabled = false;
                
            }
            else if (s == "C")
            {
                //check if there is an C
                currLetter = "C";
                Cbtn.Enabled = false;
                Cbtn2.Enabled = false;
                
            }
            else if (s == "D")
            {
                //check if there is an D
                currLetter = "D";
                Dbtn.Enabled = false;
                Dbtn2.Enabled = false;
                
            }
            else if (s == "F")
            {
                //check if there is an F
                currLetter = "F";
                Fbtn.Enabled = false;
                Fbtn2.Enabled = false;
                
            }
            else if (s == "G")
            {
                //check if there is an G
                currLetter = "G";
                Gbtn.Enabled = false;
                Gbtn2.Enabled = false;
                
            }
            else if (s == "H")
            {
                //check if there is an H
                currLetter = "H";
                Hbtn.Enabled = false;
                Hbtn2.Enabled = false;
                
            }
            else if (s == "J")
            {
                //check if there is an J
                currLetter = "J";
                Jbtn.Enabled = false;
                Jbtn2.Enabled = false;
                
            }
            else if (s == "K")
            {
                //check if there is an K
                currLetter = "K";
                Kbtn.Enabled = false;
                Kbtn2.Enabled = false;
                
            }
            else if (s == "L")
            {
                //check if there is an L
                currLetter = "L";
                Lbtn.Enabled = false;
                Lbtn2.Enabled = false;
                
            }
            else if (s == "M")
            {
                //check if there is an M
                currLetter = "M";
                Mbtn.Enabled = false;
                Mbtn2.Enabled = false;

            }
            else if (s == "N")
            {
                //check if there is an N
                currLetter = "N";
                Nbtn.Enabled = false;
                Nbtn2.Enabled = false;
                
            }
            else if (s == "P")
            {
                //check if there is an P
                currLetter = "P";
                Pbtn.Enabled = false;
                Pbtn2.Enabled = false;
                
            }
            else if (s == "Q")
            {
                //check if there is an Q
                currLetter = "Q";
                Qbtn.Enabled = false;
                Qbtn2.Enabled = false;
                
            }
            else if (s == "R")
            {
                //check if there is an R
                currLetter = "R";
                Rbtn.Enabled = false;
                Rbtn2.Enabled = false;
                
            }
            else if (s == "S")
            {
                //check if there is an S
                currLetter = "S";
                Sbtn.Enabled = false;
                Sbtn2.Enabled = false;
                
            }
            else if (s == "T")
            {
                //check if there is an T
                currLetter = "T";
                Tbtn.Enabled = false;
                Tbtn2.Enabled = false;
                
            }
            else if (s == "V")
            {
                //check if there is an V
                currLetter = "V";
                Vbtn.Enabled = false;
                Vbtn2.Enabled = false;
                
            }
            else if (s == "W")
            {
                //check if there is an W
                currLetter = "W";
                Wbtn.Enabled = false;
                Wbtn2.Enabled = false;
                
            }
            else if (s == "X")
            {
                //check if there is an X
                currLetter = "X";
                Xbtn.Enabled = false;
                Xbtn2.Enabled = false;
                
            }
            else if (s == "Y")
            {
                //check if there is an Y
                currLetter = "Y";
                Ybtn.Enabled = false;
                Ybtn2.Enabled = false;
                
            }
            else if (s == "Z")
            {
                //check if there is an Z
                currLetter = "Z";
                Zbtn.Enabled = false;
                Zbtn2.Enabled = false;
                
            }
        }

        private void otherPlayerVowelCheck()
        {
            if (currLetter == "A")
            {
                //check if there's an A
                currLetter = "A";
                Abtn.Enabled = false;
                Abtn2.Enabled = false;
            }
            else if (currLetter == "E")
            {
                //check if there is an E
                currLetter = "E";
                Ebtn.Enabled = false;
                Ebtn2.Enabled = false;
            }
            else if (currLetter == "I")
            {
                //check if there is an I
                currLetter = "I";
                Ibtn.Enabled = false;
                Ibtn2.Enabled = false;

            }
            else if (currLetter == "O")
            {
                //check if there is an O
                currLetter = "O";
                Obtn.Enabled = false;
                Obtn2.Enabled = false;

            }
            else if (currLetter == "U")
            {
                //check if there is an U
                currLetter = "U";
                Ubtn.Enabled = false;
                Ubtn2.Enabled = false;

            }
        }

      
        private void player2_3()
        {
            
            int z = -1;
            if(currPlayer==1)
            {

                MessageBox.Show("P2 is playing...");
              
                for (int i = 0; i < 4; i++)
                {
                    int play = r.Next(1, 6);

                    if (play == 1)
                    {
                        if (p2CurrScore >= 200)
                        {
                            //vowel
                            int x = unusedVowels.Length;
                            int y = r.Next(1, x);
                            currLetter = unusedVowels[y].ToString();
                            otherPlayerVowelCheck();
                            int a = checkForTheLetter();
                            p2CurrScore = p2CurrScore - 200;
                        }

                    }
                    else if(play == 2)
                    {
                        continue;
                    }
                    else if (play == 3)
                    {
                        //solve
                        int solve = r.Next(1, 6);
                        if (solve == 1)
                        {
                            //solves correctly
                            int theWinner = currPlayer + 1;
                            playerWhoWonDisplay.Text = "Player " + theWinner.ToString() + " Round " + roundNum.ToString();
                            int length = correctPhrase.Length;
                            string displayThePhrase = "";

                            for (int y = 0; y < length; y++)
                            {
                                displayThePhrase = displayThePhrase + correctPhrase[i];
                            }

                            displayThePhrase = displayThePhrase.Replace("@", "\n");
                            winPuzzleDisplay.Text = displayThePhrase;
                            winPuzzleDisplay.SelectAll();
                            winPuzzleDisplay.SelectionAlignment = HorizontalAlignment.Center;
                            p2CurrScore = p2CurrScore + 2000;
                            playerWon++;
                            MessageBox.Show("Player 2 Solved The Puzzle");
                            i = 5;
                            z++;
                            

                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                       
                         //guess a letter
                        int x = unusedConsonants.Length;
                        int y = r.Next(1, x);
                        currLetter = unusedConsonants[y].ToString();
                        getSpinVal();
                        if(spinValue== -1)
                        {
                            p2CurrScore = 0;
                        }
                        else
                        {
                            otherPlayerConsonantCheck();
                            int letterCount = checkForTheLetter();
                            p2CurrScore = p2CurrScore + spinValue * letterCount;
                        }
                        
                       
                    }

                }
                changeStringDisplay();
                updatePlayerScores();
            }
            else
            {
                MessageBox.Show("P3 is playing...");
                //Thread.Sleep(3000);
                //whoIsPlayingPanel.Hide();

                for (int i = 0; i < 4; i++)
                {
                    int play = r.Next(1, 6);

                    if (play == 1)
                    {
                        if (p3CurrScore >= 200)
                        {
                            //vowel
                            int x = unusedVowels.Length;
                            int y = r.Next(1, x);
                            currLetter = unusedVowels[y].ToString();
                            otherPlayerVowelCheck();
                            int a = checkForTheLetter();
                            p3CurrScore = p3CurrScore - 200;
                        }
                    }
                    else if (play == 2)
                    {
                        continue;
                    }
                    else if(play ==3)
                    {
                        //solve
                        int solve = r.Next(1, 6);
                        if (solve == 1)
                        {
                            //solves correctly
                            int theWinner = currPlayer + 1;
                            playerWhoWonDisplay.Text = "Player " + theWinner.ToString() + " Round " + roundNum.ToString();
                            int length = correctPhrase.Length;
                            string displayThePhrase = "";

                            for(int y = 0; y<length; y++)
                            {
                                displayThePhrase = displayThePhrase + correctPhrase[i];
                            }

                            displayThePhrase = displayThePhrase.Replace("@", "\n");
                            winPuzzleDisplay.Text = displayThePhrase;
                            winPuzzleDisplay.SelectAll();
                            winPuzzleDisplay.SelectionAlignment = HorizontalAlignment.Center;
                            p3CurrScore = p3CurrScore + 2000;
                            playerWon++;
                            MessageBox.Show("Player 3 Solved The Puzzle");
                            i = 5;
                            z++;

                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        
                            //guess a letter
                            int x = unusedConsonants.Length;
                            int y = r.Next(1, x);
                            currLetter = unusedConsonants[y].ToString();
                            getSpinVal();
                            if (spinValue == -1)
                            {
                                p3CurrScore = 0;
                            }
                            else
                            {
                                otherPlayerConsonantCheck();
                                int letterCount = checkForTheLetter();
                                p2CurrScore = p3CurrScore + spinValue * letterCount;
                            }


                    }

                }
                
            }
            changeStringDisplay();
            updatePlayerScores();
            if(z == 0)
            {
                z = -1;
                startANewRound();
                
            }
        }

        //puts the phrase into the text box
        private void changeStringDisplay()
        {
            
            newString = displayPhrase.Replace("@", "\n");
            solvePuzzleBox1.Text = newString;
            phraseBox.Text = newString;
            winPuzzleDisplay.Text = newString;
            //centers the text
            phraseBox.SelectAll();
            phraseBox.SelectionAlignment = HorizontalAlignment.Center;
            solvePuzzleBox1.SelectAll();
            solvePuzzleBox1.SelectionAlignment = HorizontalAlignment.Center;
            winPuzzleDisplay.SelectAll();
            winPuzzleDisplay.SelectionAlignment = HorizontalAlignment.Center;

        }

        //displays and updates the scores
        private void updatePlayerScores()
        {

            int p1DisplayScore = p1TotalScore + p1CurrScore;
            int p2DisplayScore = p2TotalScore + p2CurrScore;
            int p3DisplayScore = p3TotalScore + p3CurrScore;

            //this will set the display for the scores
            p1Score1.Text = "$" + p1DisplayScore.ToString();
             p2Score1.Text = "$" + p2DisplayScore.ToString();
             p3Score1.Text = "$" + p3DisplayScore.ToString();
             p1Score2.Text = "$" + p1DisplayScore.ToString();
             p2Score2.Text = "$" + p2DisplayScore.ToString();
             p3Score2.Text = "$" + p3DisplayScore.ToString();
            

        }

        private int checkForTheLetter()
        {
            int letterCount = 0;
            string a  ="";
            string b;
            //go through the correct array and see if there are any letters
            displayPhrase = "";
            for(int i = 0; i < len; i++)
            {
                a = correctPhrase[i];
                b = currLetter.ToString();
                if(a == b)
                {
                    letterCount++;
                    solvePhrase[i] = a;
                    displayPhrase = displayPhrase + solvePhrase[i];
                }
                else
                {
                    displayPhrase = displayPhrase + solvePhrase[i];
                }
            }

            //remove the current letter from the string
            if(a == "A" | a == "E" | a== "I" | a == "O" | a == "U")
            {
                
                for(int i = 0; i < unusedVowels.Length; i++)
                {
                    if(a == unusedVowels[i].ToString())
                    {
                        unusedVowels = unusedVowels.Remove(i);
                    }
                }
            }
            else
            {
                
                for (int i = 0; i < unusedConsonants.Length; i++)
                {
                    if (a == unusedConsonants[i].ToString())
                    {
                        unusedConsonants = unusedConsonants.Remove(i);
                    }
                }
            }
            
            return letterCount;
            
        }

        private void continueTheTurn()
        {
            hidePanels();
            optionsPuzzlePanel.Show();
            updatePlayerScores();
        }

        
        private void startGamebtn_Click(object sender, EventArgs e)
        {
            //allows the user to choose whether they want to upload the phrases or play with the already made phrases
            hidePanels();
            gameTypePanel.Show();
            
        }

        private void backToStartBtn_Click(object sender, EventArgs e)
        {
            //allows the user to go back to the game menu incase they do not want to play
            hidePanels();
            startPanel.Show();
            
        }

        private void exitGamebtn_Click(object sender, EventArgs e)
        {
            //allows the user to exit the game if they decide they do not want to play
            Application.Exit();
        }

        private void goToUpload_Click(object sender, EventArgs e)
        {
            //allows the user to go to the page to upload their phrases into the game
            hidePanels();
            uploadPhrasesPanel.Show();
        }

        private void backToGameTypeFromUpload_Click(object sender, EventArgs e)
        {
            //allows the user to go back and choose a new game type
            hidePanels();
            gameTypePanel.Show();
            
        }

        
     
        private void openFile_Click(object sender, EventArgs e)
        {
            string line;
            //trying to upload the file of phrases and get the count of how many different phrases there are
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                numOfUploadedPhrases = 0;
                StreamReader sr = new StreamReader(openFileDialog1.FileName);
                line = sr.ReadLine();
                fileName = openFileDialog1.FileName;
                phraseArray = System.IO.File.ReadAllLines(fileName);
                while (line != null)
                {
                   numOfUploadedPhrases++;
                    //Read the next line
                    line = sr.ReadLine();
                }
                nameBox.Text = fileName;
                isFile++;
                //int showing us that the files were uploaded
                isUploaded = 1;
                category = "Your Phrases";
                catNameLabel.Text= category;
            }
            else
            {
                nameBox.Text="Invaid File";
                isFile = 0;
            }
            
        }

        private void startGameBtn_Upload_Click(object sender, EventArgs e)
        {   
            if(isFile == 0)
            {
                //user enters the wrong file
                MessageBox.Show("Upload a correct file to play");
            }
            else
            {
                hidePanels();
                roundOneDisplayPanel.Show();
            }
        }

        private void startGameBtn_Loaded_Click(object sender, EventArgs e)
        {
            hidePanels();
            roundOneDisplayPanel.Show();           
        }
        
        /*starts the different rounds
         * 
         * 
         */
        private void beginRound1Btn_Click(object sender, EventArgs e)
        {
           
            playerNameTextBox = p1Naming.Text;
            //ensures that the player has entered a name for themselves
            if(playerNameTextBox == " ")
            {
                MessageBox.Show("Please Enter a Valid Name");
            }
            else
            {
                hidePanels();
                optionsPuzzlePanel.Show();

                // creates the arrays for the phrase
                 getThePhrase();

                //sets labels to the correct player
                optionsPlayer1Name.Text = playerNameTextBox;
                label29.Text = playerNameTextBox;
                firstPlaceLabel.Text = playerNameTextBox;

                //creates the array of players for the first game
                string s2 = "Player 2";
                string s3 = "Player 3";
                playerNameCollection = new string[] { playerNameTextBox, s2, s3 };

                //displays the current player at the top of the screen
                optionsCurrPlayer.Text = playerNameCollection[currPlayer];
                label25.Text = playerNameCollection[currPlayer];
                getThePhrase();
            }
        }

        private void toTheNextRound_Click(object sender, EventArgs e)
        {
            hidePanels();
            optionsPuzzlePanel.Show();
            roundNum++;
            // creates the arrays for the phrase
            getThePhrase();
            unusedConsonants = "BCDFGHJKLMNPQRSTVWXYZ";
            unusedVowels = "AEIOU";

            roundDisplay1.Text = "Round " + roundNum.ToString();
            roundDisplay2.Text = "Round " + roundNum.ToString();

            //make all the buttons available
            Abtn.Enabled = true;
            Abtn2.Enabled = true;
            Bbtn.Enabled = true;
            Bbtn2.Enabled = true;
            Cbtn.Enabled = true;
            Cbtn2.Enabled = true;
            Dbtn.Enabled = true;
            Dbtn2.Enabled = true;
            Ebtn.Enabled = true;
            Ebtn2.Enabled = true;
            Fbtn.Enabled = true;
            Fbtn2.Enabled = true;
            Gbtn.Enabled = true;
            Gbtn2.Enabled = true;
            Hbtn.Enabled = true;
            Hbtn2.Enabled = true;
            Ibtn.Enabled = true;
            Ibtn2.Enabled = true;
            Jbtn.Enabled = true;
            Jbtn2.Enabled = true;
            Kbtn.Enabled = true;
            Kbtn2.Enabled = true;
            Lbtn.Enabled = true;
            Lbtn2.Enabled = true;
            Mbtn.Enabled = true;
            Mbtn2.Enabled = true;
            Nbtn.Enabled = true;
            Nbtn2.Enabled = true;
            Obtn.Enabled = true;
            Obtn2.Enabled = true;
            Pbtn.Enabled = true;
            Pbtn2.Enabled = true;
            Qbtn.Enabled = true;
            Qbtn2.Enabled = true;
            Rbtn.Enabled = true;
            Rbtn2.Enabled = true;
            Sbtn.Enabled = true;
            Sbtn2.Enabled = true;
            Tbtn.Enabled = true;
            Tbtn2.Enabled = true;
            Ubtn.Enabled = true;
            Ubtn2.Enabled = true;
            Vbtn.Enabled = true;
            Vbtn2.Enabled = true;
            Wbtn.Enabled = true;
            Wbtn2.Enabled = true;
            Xbtn.Enabled = true;
            Xbtn2.Enabled = true;
            Ybtn.Enabled = true;
            Ybtn2.Enabled = true;
            Zbtn.Enabled = true;
            Zbtn2.Enabled = true;

            
           
           nextPlayersTurn();
           
            
            



        }
        private void EndTheGame()
        {
            hidePanels();
            winnersPanel.Show();
            roundNum = 1;
            

            if(p1TotalScore > p2TotalScore && p1TotalScore > p3TotalScore)
            {
                //p1 is first place
                firstPlaceLabel.Text = playerNameCollection[0];
                firstPlaceScore.Text = "$" + p1TotalScore.ToString();
                if(p2TotalScore> p3TotalScore)
                {
                    //p2 is second place
                    secondPlaceLabel.Text = playerNameCollection[1];
                    secondPlaceScore.Text = "$" + p2TotalScore.ToString();

                    thirdPlaceLabel.Text = playerNameCollection[2];
                    thirdPlaceScore.Text = "$" + p3TotalScore.ToString();
                }
                else
                {
                    //p3 is second place
                    secondPlaceLabel.Text = playerNameCollection[2];
                    secondPlaceScore.Text = "$" + p3TotalScore.ToString();

                    thirdPlaceLabel.Text = playerNameCollection[1];
                    thirdPlaceScore.Text = "$" + p2TotalScore.ToString();
                }
            }
            else if (p3TotalScore > p1TotalScore && p3TotalScore > p1TotalScore)
            {
                //p3 is first place
                firstPlaceLabel.Text = playerNameCollection[2];
                firstPlaceScore.Text = "$" + p3TotalScore.ToString();

                if (p2TotalScore > p1TotalScore)
                {
                    //p2 is second place
                    secondPlaceLabel.Text = playerNameCollection[1];
                    secondPlaceScore.Text = "$" + p2TotalScore.ToString();

                    thirdPlaceLabel.Text = playerNameCollection[0];
                    thirdPlaceScore.Text = "$" + p1TotalScore.ToString();
                }
                else
                {
                    //p1 is second place
                    secondPlaceLabel.Text = playerNameCollection[0];
                    secondPlaceScore.Text = "$" + p1TotalScore.ToString();

                    thirdPlaceLabel.Text = playerNameCollection[1];
                    thirdPlaceScore.Text = "$" + p2TotalScore.ToString();
                }
            }
            else
            {
                //p3 is first place
                firstPlaceLabel.Text = playerNameCollection[1];
                firstPlaceScore.Text = "$" + p2TotalScore.ToString();
                if (p1TotalScore > p3TotalScore)
                {
                    //p1 is second place
                    secondPlaceLabel.Text = playerNameCollection[0];
                    secondPlaceScore.Text = "$" + p1TotalScore.ToString();

                    thirdPlaceLabel.Text = playerNameCollection[2];
                    thirdPlaceScore.Text = "$" + p3TotalScore.ToString();
                }
                else
                {
                    //p3 is second place
                    secondPlaceLabel.Text = playerNameCollection[2];
                    secondPlaceScore.Text = "$" + p3TotalScore.ToString();

                    thirdPlaceLabel.Text = playerNameCollection[0];
                    thirdPlaceScore.Text = "$" + p1TotalScore.ToString();
                }
            }

        }

        private void startANewRound()
        {

            hidePanels();
            roundSummaryPanel.Show();
            int length = correctPhrase.Length;
            string displayThePhrase = "";

            for (int i = 0; i < length; i++)
            {
                displayThePhrase = displayThePhrase + correctPhrase[i];
            }

            displayThePhrase = displayThePhrase.Replace("@", "\n");
            winPuzzleDisplay.Text = displayThePhrase;
            winPuzzleDisplay.SelectAll();
            winPuzzleDisplay.SelectionAlignment = HorizontalAlignment.Center;

            int theWinner = currPlayer + 1;
            playerWhoWonDisplay.Text = "Player " + theWinner.ToString() + " Won Round " + roundNum.ToString() + "!";

            int winner = currPlayer;

            if(winner == 0)
            {

                //p1 is the only player that gets to keep their score
                int tempp1TotalScore = p1TotalScore + p1CurrScore;
                p1RoundScore.Text = "$" + p1TotalScore.ToString() + " + " + "$" + p1CurrScore.ToString() + " = " + "$" + tempp1TotalScore.ToString();
                p1TotalScore = p1TotalScore + p1CurrScore;
                p1CurrScore = 0;

                int tempp2TotalScore = p2TotalScore + 0;
                p2CurrScore = 0;
                p2RoundScore.Text ="$"+ p2TotalScore.ToString() + " + " + "$" + p2CurrScore.ToString() + " = " + "$" + tempp2TotalScore.ToString();

                int tempp3TotalScore = p3TotalScore + 0;
                p3CurrScore = 0;
                p3RoundScore.Text = "$" + p3TotalScore.ToString() + " + " + "$" + p3CurrScore.ToString() + " = " + "$" + tempp3TotalScore.ToString();

            }
            else if( winner == 1)
            {
                //p2 is the only player that gets to keep their score
                int tempp1TotalScore = p1TotalScore + 0;
                p1RoundScore.Text = "$" + p1TotalScore.ToString() + " + " + "$" + p1CurrScore.ToString() + " = " + "$" + tempp1TotalScore.ToString();
                p1CurrScore = 0;

                int tempp2TotalScore = p2TotalScore + p2CurrScore;
                p2RoundScore.Text = "$" + p2TotalScore.ToString() + " + " + "$" + p2CurrScore.ToString() + " = " + "$" + tempp2TotalScore.ToString();
                p2TotalScore = p2TotalScore + p2CurrScore;
                p2CurrScore = 0;

                int tempp3TotalScore = p3TotalScore + 0;
                p3CurrScore = 0;
                p3RoundScore.Text = "$" + p3TotalScore.ToString() + " + " + "$" + p3CurrScore.ToString() + " = " + "$" + tempp3TotalScore.ToString();
            }
            else
            {
                //p3 is the only player that gets to keep their score
                int tempp1TotalScore = p1TotalScore + 0;
                p1CurrScore = 0;
                p1RoundScore.Text = "$" + p1TotalScore.ToString() + " + " + "$" + p1CurrScore.ToString() + " = " + "$" + tempp1TotalScore.ToString();
                

                int tempp2TotalScore = p2TotalScore + 0;
                p2CurrScore = 0;
                p2RoundScore.Text = "$" + p2TotalScore.ToString() + " + " + "$" + p2CurrScore.ToString() + " = " + "$" + tempp2TotalScore.ToString();

                int tempp3TotalScore = p3TotalScore + p3CurrScore;
                p3RoundScore.Text = "$" + p3TotalScore.ToString() + " + " + "$" + p3CurrScore.ToString() + " = " + "$" + tempp3TotalScore.ToString();
                p3TotalScore = p3TotalScore + p3CurrScore;
                p3CurrScore = 0;

            }
            if(roundNum ==3)
            {
                toTheNextRound.Text = "See Results";
            }
            updatePlayerScores();
            
            //update the current scores to be the total--- make a winner integer and set it to the winner of the round so thatthey get the points
            //start up the new puzzle starting with the person who would have been next
            
        }
        

        //changes who is playing
        private void nextPlayersTurn()
        {
            hidePanels();
            optionsPuzzlePanel.Show();

            //decides whether to end the game or continue playing
            if (roundNum == 4)
            {
                EndTheGame();

            }
            else
            {
                if (currPlayer == 0 || currPlayer == 1)
                {
                    currPlayer++;
                    optionsCurrPlayer.Text = playerNameCollection[currPlayer];

                    player2_3();
                    if (playerWon == 0)
                    {
                        nextPlayersTurn();
                    }
                    else
                    {
                        //MessageBox.Show("Next Player will Play");
                    }
                    playerWon = 0;
                }
                else
                {

                    currPlayer = 0;
                    optionsCurrPlayer.Text = playerNameCollection[currPlayer];
                    continueTheTurn();
                }
            }
            
        }


        //spin button 
        private void button5_Click(object sender, EventArgs e)
        {

            int a = r.Next(0, 101);

            if(a < 10)
            {
                spinValue = 100;
            }
            else if(10<= a && a < 20)
            {
                spinValue = 200;
            }
            else if (20 <= a && a < 30)
            {
                spinValue = 300;
            }
            else if (30 <= a && a < 40)
            {
                spinValue = 400;
            }
            else if (40 <= a && a < 45)
            {
                spinValue = 500;
            }
            else if (45 <= a && a < 50)
            {
                spinValue = 600;
            }

            else if (50 <= a && a < 55)
            {
                spinValue = 700;
            }

            else if (55 <= a && a < 60)
            {
                spinValue = 800;
            }
            else if (60 <= a && a < 75)
            {
                spinValue = 0;
            }

            else if (75 <= a && a < 78)
            {
                spinValue = 20000;
            }
            else if (78 <= a && a < 85)
            {
                spinValue = 1000;
            }
            else if (85 <= a && a <= 90)
            {
                spinValue = -1;
            }
            else if (90 <= a && a <= 100)
            {
                spinValue = 150;
            }

            //if spinvalue 0 then player loses a turn, if negative 1 then player spun bankrupt
            if (spinValue == -1)
            {

                //bankrupt-- curr score to 0
                if (currPlayer == 0)
                {
                    p1CurrScore = 0;
                }
                else if (currPlayer == 1)
                {
                    p2CurrScore = 0;
                }
                else
                {
                    p3CurrScore = 0;
                }

                hidePanels();
                spinDisplay.Text = "BANKRUPT!";
                spinDisplayPanel.Show();
                LoseATurnbtn.Show();
                guessTheLetterBtn.Hide();
                updatePlayerScores();
                
            }
            else if(spinValue == 0)
            {
                //player loses a turn
                hidePanels();
                spinDisplay.Text = "LOSE A TURN!";
                spinDisplayPanel.Show();
                LoseATurnbtn.Show();
                guessTheLetterBtn.Hide();
               
            }
            else
            {
                hidePanels();
                spinDisplayPanel.Show();
                LoseATurnbtn.Hide();
                guessTheLetterBtn.Show();
                spinDisplay.Text = "Spin: $" + spinValue.ToString();
            }

        }

        

        /*
         * This is the area for if a player wants to select a Vowel
         * still need o make it so the player can not buy a vowel if they have less than 200
         */
        
        private void buyVowelbtn_Click(object sender, EventArgs e)
        {
            timer1.Start();
            int x = 0;
            //make sure that the current player score is over 200, so they can buy a vowel
            if(currPlayer == 0)
            {
                if(p1CurrScore < 200)
                {
                    x++;
                }
            }
            else if (currPlayer == 1)
            {
                if (p2CurrScore < 200)
                {
                    x = 2;
                }
            }
            else if (currPlayer == 2)
            {
                if (p3CurrScore < 200)
                {
                    x=2;
                }
            }


            if (x == 0)
            {
                hidePanels();
                puzzleLetterPanel.Show();
                vowelBox.Show();
                consonantBox.Hide();
                theSelectBtn.Text = "Buy a Vowel:";
                spinOrBuyLetter.Text = "Cost:";
                moneyForEachGuess.Text = "$200";
                determineGuess.Enabled = false;
                Letter = 0;
            }
            else if (x == 2)
            {
                //just skips through
            }
            else
            {
                MessageBox.Show("You need to have more than $200 from this round to buy a vowel!");
            }
            x = 0;
        }

        private void playBuyAVowel()
        {
            //take in the letter that was submitted and see if the vowel is there
            string s = enterALetterBox.Text.ToUpper();
            int letterCount = -1;
            if (s == "A")
            {
                //check if there's an A
                currLetter = "A";
                Abtn.Enabled = false;
                Abtn2.Enabled = false;
                letterCount = checkForTheLetter();
            }
            else if (s == "E")
            {
                //check if there is an E
                currLetter = "E";
                Ebtn.Enabled = false;
                Ebtn2.Enabled = false;
                letterCount = checkForTheLetter();
            }
            else if (s == "I")
            {
                //check if there is an I
                currLetter = "I";
                Ibtn.Enabled = false;
                Ibtn2.Enabled = false;
                letterCount = checkForTheLetter();
            }
            else if (s == "O")
            {
                //check if there is an O
                currLetter = "O";
                Obtn.Enabled = false;
                Obtn2.Enabled = false;
                letterCount = checkForTheLetter();
            }
            else if (s == "U")
            {
                //check if there is an U
                currLetter = "U";
                Ubtn.Enabled = false;
                Ubtn2.Enabled = false;
                letterCount = checkForTheLetter();
            }
            
            

            //checked for the letter
            if (letterCount == 0)
            {
                MessageBox.Show("There were none of the letter, " + currLetter + " in the Puzzle");
                nextPlayersTurn();
            }
            else if (letterCount > 0)
            {
                if (currPlayer == 0)
                {
                    p1CurrScore = p1CurrScore - 200;
                    updatePlayerScores();
                    continueTheTurn();
                }
                else if (currPlayer == 1)
                {
                    p2CurrScore = p2CurrScore - 200;
                    updatePlayerScores();
                    continueTheTurn();
                }
                else if (currPlayer == 2)
                {
                    p3CurrScore = p3CurrScore - 200;
                    updatePlayerScores();
                    continueTheTurn();
                }
                
            }
        }

        private void Abtn_Click(object sender, EventArgs e)
        {
            enterALetterBox.Text = "A";
            determineGuess.Enabled = true;
            
        }

        private void Ebtn_Click(object sender, EventArgs e)
        {
            enterALetterBox.Text = "E";
            determineGuess.Enabled = true;
        }

        private void Ibtn_Click(object sender, EventArgs e)
        {
            enterALetterBox.Text = "I";
            determineGuess.Enabled = true;
        }

        private void Obtn_Click(object sender, EventArgs e)
        {
            enterALetterBox.Text = "O";
            determineGuess.Enabled = true;
        }

        private void Ubtn_Click(object sender, EventArgs e)
        {
            enterALetterBox.Text = "U";
            determineGuess.Enabled = true;
        }



        // Diferentiates between buying a vowel and a consonant
        private void determineGuess_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timeLeft = 31;

            if(Letter == 0)
            {
                playBuyAVowel();
                enterALetterBox.Text = "";
                updatePlayerScores();
                changeStringDisplay();
                //check if the letter guessed solves the puzzle
                solvedInGame();
            }
            else
            {
                //playConsoant Method
                determineGuess.Enabled = false;
                guessALetter();
                enterALetterBox.Text = "";
                updatePlayerScores();
                changeStringDisplay();
                //check if the letter guessed solves the puzzle
                solvedInGame();

            }
            Letter = 0;
        }
           

        /*This code is for if the player is guessing a letter
         * 
         * 
         * 
         */


        private void guessALetter()
        {
            //take in the letter that was submitted and see if the vowel is there
            while (enterALetterBox.ToString() == ".")
            {
                determineGuess.Enabled = false;
            }
            determineGuess.Enabled = true;
            string s = enterALetterBox.Text.ToUpper();
            int letterCount = -1;
            if (s == "B")
            {
                //check if there's an B
                currLetter = "B";
                Bbtn.Enabled = false;
                Bbtn2.Enabled = false;
                letterCount = checkForTheLetter();
            }
            else if (s == "C")
            {
                //check if there is an C
                currLetter = "C";
                Cbtn.Enabled = false;
                Cbtn2.Enabled = false;
                letterCount = checkForTheLetter();
            }
            else if (s == "D")
            {
                //check if there is an D
                currLetter = "D";
                Dbtn.Enabled = false;
                Dbtn2.Enabled = false;
                letterCount = checkForTheLetter();
            }
            else if (s == "F")
            {
                //check if there is an F
                currLetter = "F";
                Fbtn.Enabled = false;
                Fbtn2.Enabled = false;
                letterCount = checkForTheLetter();
            }
            else if (s == "G")
            {
                //check if there is an G
                currLetter = "G";
                Gbtn.Enabled = false;
                Gbtn2.Enabled = false;
                letterCount = checkForTheLetter();
            }
            else if (s == "H")
            {
                //check if there is an H
                currLetter = "H";
                Hbtn.Enabled = false;
                Hbtn2.Enabled = false;
                letterCount = checkForTheLetter();
            }
            else if (s == "J")
            {
                //check if there is an J
                currLetter = "J";
                Jbtn.Enabled = false;
                Jbtn2.Enabled = false;
                letterCount = checkForTheLetter();
            }
            else if (s == "K")
            {
                //check if there is an K
                currLetter = "K";
                Kbtn.Enabled = false;
                Kbtn2.Enabled = false;
                letterCount = checkForTheLetter();
            }
            else if (s == "L")
            {
                //check if there is an L
                currLetter = "L";
                Lbtn.Enabled = false;
                Lbtn2.Enabled = false;
                letterCount = checkForTheLetter();
            }
            else if (s == "M")
            {
                //check if there is an M
                currLetter = "M";
                Mbtn.Enabled = false;
                letterCount = checkForTheLetter();
            }
            else if (s == "N")
            {
                //check if there is an N
                currLetter = "N";
                Nbtn.Enabled = false;
                Nbtn2.Enabled = false;
                letterCount = checkForTheLetter();
            }
            else if (s == "P")
            {
                //check if there is an P
                currLetter = "P";
                Pbtn.Enabled = false;
                Pbtn2.Enabled = false;
                letterCount = checkForTheLetter();
            }
            else if (s == "Q")
            {
                //check if there is an Q
                currLetter = "Q";
                Qbtn.Enabled = false;
                Qbtn2.Enabled = false;
                letterCount = checkForTheLetter();
            }
            else if (s == "R")
            {
                //check if there is an R
                currLetter = "R";
                Rbtn.Enabled = false;
                Rbtn2.Enabled = false;
                letterCount = checkForTheLetter();
            }
            else if (s == "S")
            {
                //check if there is an S
                currLetter = "S";
                Sbtn.Enabled = false;
                Sbtn2.Enabled = false;
                letterCount = checkForTheLetter();
            }
            else if (s == "T")
            {
                //check if there is an T
                currLetter = "T";
                Tbtn.Enabled = false;
                Tbtn2.Enabled = false;
                letterCount = checkForTheLetter();
            }
            else if (s == "V")
            {
                //check if there is an V
                currLetter = "V";
                Vbtn.Enabled = false;
                Vbtn2.Enabled = false;
                letterCount = checkForTheLetter();
            }
            else if (s == "W")
            {
                //check if there is an W
                currLetter = "W";
                Wbtn.Enabled = false;
                Wbtn2.Enabled = false;
                letterCount = checkForTheLetter();
            }
            else if (s == "X")
            {
                //check if there is an X
                currLetter = "X";
                Xbtn.Enabled = false;
                Xbtn2.Enabled = false;
                letterCount = checkForTheLetter();
            }
            else if (s == "Y")
            {
                //check if there is an Y
                currLetter = "Y";
                Ybtn.Enabled = false;
                Ybtn2.Enabled = false;
                letterCount = checkForTheLetter();
            }
            else if (s == "Z")
            {
                //check if there is an Z
                currLetter = "Z";
                Zbtn.Enabled = false;
                Zbtn2.Enabled = false;
                letterCount = checkForTheLetter();
            }
            




            if (letterCount == 0)
            {
                MessageBox.Show("There were none of the letter, " + currLetter + " in the Puzzle");
                nextPlayersTurn();
            }
            else if (letterCount > 0)
            {
                if (currPlayer == 0)
                {
                    p1CurrScore = p1CurrScore + (letterCount*spinValue);
                    continueTheTurn();
                    updatePlayerScores();
                    
                }
                else if (currPlayer == 1)
                {
                    p2CurrScore = p2CurrScore + (letterCount * spinValue);
                    updatePlayerScores();
                    continueTheTurn();
                }
                else if (currPlayer == 2)
                {
                    p3CurrScore = p3CurrScore + (letterCount * spinValue);
                    updatePlayerScores();
                    continueTheTurn();
                }


             
                
            }
        }

        private void Bbtn_Click(object sender, EventArgs e)
        {
            enterALetterBox.Text = "B";
            determineGuess.Enabled = true;
        }

        private void Cbtn_Click(object sender, EventArgs e)
        {
            enterALetterBox.Text = "C";
            determineGuess.Enabled = true;
        }

        private void Dbtn_Click(object sender, EventArgs e)
        {
            enterALetterBox.Text = "D";
            determineGuess.Enabled = true;
        }

        private void Fbtn_Click(object sender, EventArgs e)
        {
            enterALetterBox.Text = "F";
            determineGuess.Enabled = true;
        }

        private void Gbtn_Click(object sender, EventArgs e)
        {
            enterALetterBox.Text = "G";
            determineGuess.Enabled = true;
        }

        private void Hbtn_Click(object sender, EventArgs e)
        {
            enterALetterBox.Text = "H";
            determineGuess.Enabled = true;
        }

        private void Jbtn_Click(object sender, EventArgs e)
        {
            enterALetterBox.Text = "J";
            determineGuess.Enabled = true;
        }

        private void Kbtn_Click(object sender, EventArgs e)
        {
            enterALetterBox.Text = "K ";
            determineGuess.Enabled = true;
        }

        private void Lbtn_Click(object sender, EventArgs e)
        {
            enterALetterBox.Text = "L";
            determineGuess.Enabled = true;
        }

        private void Mbtn_Click(object sender, EventArgs e)
        {
            enterALetterBox.Text = "M";
            determineGuess.Enabled = true;
        }

        private void Nbtn_Click(object sender, EventArgs e)
        {
            enterALetterBox.Text = "N";
            determineGuess.Enabled = true;
        }

        private void Pbtn_Click(object sender, EventArgs e)
        {
            enterALetterBox.Text = "P";
            determineGuess.Enabled = true;
        }

        private void Qbtn_Click(object sender, EventArgs e)
        {
            enterALetterBox.Text = "Q";
            determineGuess.Enabled = true;
        }

        private void Rbtn_Click(object sender, EventArgs e)
        {
            enterALetterBox.Text = "R";
            determineGuess.Enabled = true;
        }

        private void Sbtn_Click(object sender, EventArgs e)
        {
            enterALetterBox.Text = "S";
            determineGuess.Enabled = true;
        }

        private void Tbtn_Click(object sender, EventArgs e)
        {
            enterALetterBox.Text = "T";
            determineGuess.Enabled = true;
        }

        private void Vbtn_Click(object sender, EventArgs e)
        {
            enterALetterBox.Text = "V";
            determineGuess.Enabled = true;
        }

        private void Wbtn_Click(object sender, EventArgs e)
        {
            enterALetterBox.Text = "W";
            determineGuess.Enabled = true;
        }

        private void Xbtn_Click(object sender, EventArgs e)
        {
            enterALetterBox.Text = "X";
            determineGuess.Enabled = true;
        }

        private void Ybtn_Click(object sender, EventArgs e)
        {
            enterALetterBox.Text = "Y";
            determineGuess.Enabled = true;
        }

        private void Zbtn_Click(object sender, EventArgs e)
        {
            enterALetterBox.Text = "Z";
            determineGuess.Enabled = true;
        }

        private void guessTheLetterBtn_Click(object sender, EventArgs e)
        {
            hidePanels();
            puzzleLetterPanel.Show();
            consonantBox.Show();
            vowelBox.Hide();
            
            theSelectBtn.Text = "Select a Letter";
            spinOrBuyLetter.Text = "SPIN: ";
            moneyForEachGuess.Text = spinValue.ToString();
            determineGuess.Enabled = false;
            Letter = 1;
            timer1.Start();
            
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (timeLeft > 0)
            {
                
                timeLeft = timeLeft - 1;
                timeLabel.Text = timeLeft + " seconds";
            }
            else if (timeLeft == 0)
            {
                nextPlayersTurn();
            }
        }

        private void LoseATurnbtn_Click(object sender, EventArgs e)
        {
            nextPlayersTurn();
        }


        // this is the pass button
        private void button44_Click_1(object sender, EventArgs e)
        {
            nextPlayersTurn();
        }






        //if the player is solving the puzzle 
        /*
         * 
         * 
         * 
         */

        private void solveThePuzzlebtn_Click(object sender, EventArgs e)
        {
            hidePanels();
            solvingPanel.Show(); 
            solveBox.Text = displayPhrase.Replace("@", "\n");
            solveBox.SelectAll();
            solveBox.SelectionAlignment = HorizontalAlignment.Center;
            displayPhraseCopy = displayPhrase;
        }

        private void Abtn2_Click(object sender, EventArgs e)
        {
            letterSolve.Text = "A";
            determineGuess.Enabled = true;
        }

        private void Bbtn2_Click(object sender, EventArgs e)
        {
            letterSolve.Text = "B";
            determineGuess.Enabled = true;
        }

        private void Cbtn2_Click(object sender, EventArgs e)
        {
            letterSolve.Text = "C";
            determineGuess.Enabled = true;
        }

        private void Dbtn2_Click(object sender, EventArgs e)
        {
            letterSolve.Text = "D";
            determineGuess.Enabled = true;
        }

        private void Ebtn2_Click(object sender, EventArgs e)
        {
            letterSolve.Text = "E";
            determineGuess.Enabled = true;
        }

        private void Fbtn2_Click(object sender, EventArgs e)
        {
            letterSolve.Text = "F";
            determineGuess.Enabled = true;
        }

        private void Gbtn2_Click(object sender, EventArgs e)
        {
            letterSolve.Text = "G";
            determineGuess.Enabled = true;
        }

        private void Hbtn2_Click(object sender, EventArgs e)
        {
            letterSolve.Text = "H";
            determineGuess.Enabled = true;
        }

        private void Ibtn2_Click(object sender, EventArgs e)
        {
            letterSolve.Text = "I";
            determineGuess.Enabled = true;
        }

        private void Jbtn2_Click(object sender, EventArgs e)
        {
            letterSolve.Text = "J";
            determineGuess.Enabled = true;
        }

        private void Kbtn2_Click(object sender, EventArgs e)
        {
            letterSolve.Text = "K";
            determineGuess.Enabled = true;
        }

        private void Lbtn2_Click(object sender, EventArgs e)
        {
            letterSolve.Text = "L";
            determineGuess.Enabled = true;
        }

        private void Mbtn2_Click(object sender, EventArgs e)
        {
            letterSolve.Text = "M";
            determineGuess.Enabled = true;
        }

        private void Nbtn2_Click(object sender, EventArgs e)
        {
            letterSolve.Text = "N";
            determineGuess.Enabled = true;
        }

        private void Obtn2_Click(object sender, EventArgs e)
        {
            letterSolve.Text = "O";
            determineGuess.Enabled = true;
        }

        private void Pbtn2_Click(object sender, EventArgs e)
        {
            letterSolve.Text = "P";
            determineGuess.Enabled = true;
        }

        private void Qbtn2_Click(object sender, EventArgs e)
        {
            letterSolve.Text = "Q";
            determineGuess.Enabled = true;
        }

        private void Rbtn2_Click(object sender, EventArgs e)
        {
            letterSolve.Text = "R";
            determineGuess.Enabled = true;
        }

        private void Sbtn2_Click(object sender, EventArgs e)
        {
            letterSolve.Text = "S";
            determineGuess.Enabled = true;
        }

        private void Tbtn2_Click(object sender, EventArgs e)
        {
            letterSolve.Text = "T";
            determineGuess.Enabled = true;
        }

        private void Ubtn2_Click(object sender, EventArgs e)
        {
            letterSolve.Text = "U";
            determineGuess.Enabled = true;
        }

        private void Vbtn2_Click(object sender, EventArgs e)
        {
            letterSolve.Text = "V";
            determineGuess.Enabled = true;
        }

        private void Wbtn2_Click(object sender, EventArgs e)
        {
            letterSolve.Text = "W";
            determineGuess.Enabled = true;
        }

        private void Xbtn2_Click(object sender, EventArgs e)
        {
            letterSolve.Text = "X";
            determineGuess.Enabled = true;
        }

        private void Ybtn2_Click(object sender, EventArgs e)
        {
            letterSolve.Text = "Y";
            determineGuess.Enabled = true;
        }

        private void Zbtn2_Click(object sender, EventArgs e)
        {
            letterSolve.Text = "Z";
            determineGuess.Enabled = true;
        }

        private void solveSubmit_Click(object sender, EventArgs e)
        {
            currLetter = letterSolve.Text;
            //replace the underscore with the letter entered, let the player submit all of the letters
            // check if there are anymore underscores, if not then check if the phrase is correct
            changeSolveDisplay();

            int count1 = countTheUnderscores();
            solveBox.Text = tempPhrase.Replace("@", "\n");
            solveBox.SelectAll();
            solveBox.SelectionAlignment = HorizontalAlignment.Center;
            if (count1 == 0)
            {
                
                bool y = compareTheStrings();
                if (y == true)
                {
                    //thePlayer won the round
                    //**** reset tempPhrase
                    int theWinner = currPlayer + 1;
                    playerWhoWonDisplay.Text = "Player " + theWinner.ToString() + " Won Round " + roundNum.ToString() + "!";

                    newString = displayPhraseCopy.Replace("@", "\n");
                    winPuzzleDisplay.Text = newString;
                    winPuzzleDisplay.SelectAll();
                    winPuzzleDisplay.SelectionAlignment = HorizontalAlignment.Center;
                    MessageBox.Show("You solved the puzzle!");
                    startANewRound();
                }
                else
                {
                    //continue the game

                    //as if the player passed
                    displayPhraseCopy = displayPhrase;
                    MessageBox.Show("You did not solve the puzzle correctly!");
                    nextPlayersTurn();
                }
            }
            

        }

        private bool compareTheStrings()
        {
            //these two strings will be built by only putting the letters of the displaying phrases 
            string checkPhrase= "";
            string changedPhrase = "";

            // will return true if the player solved and false if not
            bool check;

            int x = displayPhraseCopy.Length;

            for (int i = 0; i < x; i++)
            {
                if (displayPhraseCopy[i] == ' ' || displayPhraseCopy[i] == '@')
                {
                    //skip through so that the spaces and @ are not used when comparing
                    continue;
                }
                else
                {
                    changedPhrase = changedPhrase + displayPhraseCopy[i];
                }
            }
            //puts the correctPhrase into a string
            int y = currPhrase.Length;
            for (int i = 0; i < y; i++)
            {
                char c = currPhrase[i];
                string a = c.ToString();
                if (a == " "|| a =="@")
                {
                    //skip through
                    continue;
                }
                else
                {
                    checkPhrase = checkPhrase + c;
                }
            }


            //checks if the phrases are the same
            if(changedPhrase==checkPhrase)
            {
                check = true;
            }
            else
            {
                check = false;
            }
            return check;
        }

        private void changeSolveDisplay()
        {
            
            string a = "";
            tempPhrase = "";
            int c = 0;
            int x = len;
            a = currLetter.ToString();

            //counts the total to make a temp array of that size
            for (int i = 0; i < x; i++)
            { 
                if(displayPhraseCopy[i] == ' ')
                {
                    x++;
                }
            }

            
            string[] theTempPhrase = new string[x];

            //go through the correct array and see if there are any letters
            for (int i = 0; i < x; i++)
            {
                if (displayPhraseCopy[i] == '_' && c == 0)
                {
                    //change tempPhrase
                    theTempPhrase[i] = a.ToString();


                    c++;
                }
                else
                {
                    theTempPhrase[i] = displayPhraseCopy[i].ToString();

                }
                tempPhrase = tempPhrase + theTempPhrase[i];
            }
            displayPhraseCopy = tempPhrase;
        }
        private int countTheUnderscores()
        {
            int x = len;
            for (int i = 0; i < x; i++)
            {
                if (displayPhraseCopy[i] == ' ')
                {
                    x++;
                }
            }
            int count = 0;
            for(int i = 0; i<x; i++)
            {
                string a = tempPhrase[i].ToString();
                if (a == "_")
                {
                    count++;
                }
            }
            return count;
        }


        private void solvedInGame()
        {
            int x = displayPhrase.Length;
            //counts all the underscores to see if the puzzle is solved during the game
            int count1 = 0;
            for (int i = 0; i < x; i++)
            {
                string a = displayPhrase[i].ToString();
                if (a == "_")
                {
                    count1++;
                }
            }

            if (count1 == 0)
            {
                startANewRound();
               
            }
        }

        private void exitGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //allows the user to quit
            Application.Exit();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //brings the player back to the beginning
            hidePanels();
            startPanel.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //this starts a new game once the game is over
            hidePanels();
            startPanel.Show();
            p1CurrScore = 0;
            p2CurrScore = 0;
            p3CurrScore = 0;
            p1TotalScore = 0;
            p2TotalScore = 0; ;
            p3TotalScore = 0;
            currPlayer = 0;
            toTheNextRound.Text = "START THE NEXT ROUND!";
            updatePlayerScores();
            determineGuess.Enabled = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //closes the application
            Application.Exit();
        }

        private void guessHelpBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You have the options to spin the wheel, buy a vowel, pass to the next player, or solve the puzzle. \n If you" +
                " decide to spin the wheel you will get a random dollar amount, bankrupt or lose a turn. If you spin the wheel and get a dollar " +
                "amount you will get the opportunity to choose a consonant letter that you think is in the puzzle. If the letter you select is in the puzzle " +
                "you will get the dollar amount you spun multiplied by the amount of times the letter is in in the puzzle and your turn will continue." +
                " If the letter is not in the puzzle then the next player will get to play. If you get a lose a turn, the next player will play.If you get " +
                "bankrupt you lose all the money that you earned that round. \n If you choose buy a vowel, you will get to choose a vowel you think is in the " +
                "puzzle. Regardless if the letter is in the puzzle you will lose $200; if the letter is in the puzzle you continue your turn, if it is not it " +
                "is the next players turn. \n When you think you know the answer to the puzzle you choose solve, if you are correct you win $2000 and the round is over," +
                " otherwise the game continues.");
        }

        private void solveHelpBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Click the letter that you believe is the next available spot in the puzzle(the first “_” in the puzzle). \n" +
                "Then click submit and your letter will appear in the blank spot. Continue to do this until there are no more blank spots(or “_”)" +
                " in the puzzle. \n Once you click submit you can not go back!");
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            MessageBox.Show(" The Wheel of Fortune Game consists of three rounds of puzzles where you will be playing against 2 other computer players. \n" +
                "You may either upload your own phrases (the more you upload the less likely there will be a repeat) or play with phrases already uploaded to the game. \n" +
                "For each round there will be a word puzzle to solve (similar to hangman), when it is your turn you will have the options to spin the wheel, buy a vowel, " +
                "pass to the next player, or solve the puzzle. You will have the options to spin the wheel, buy a vowel, pass to the next player, or solve the puzzle. \n If you" +
                " decide to spin the wheel you will get a random dollar amount, bankrupt or lose a turn. If you spin the wheel and get a dollar " +
                "amount you will get the opportunity to choose a consonant letter that you think is in the puzzle. If the letter you select is in the puzzle " +
                "you will get the dollar amount you spun multiplied by the amount of times the letter is in in the puzzle and your turn will continue." +
                " If the letter is not in the puzzle then the next player will get to play. If you get a lose a turn, the next player will play.If you get " +
                "bankrupt you lose all the money that you earned that round. \n If you choose buy a vowel, you will get to choose a vowel you think is in the " +
                "puzzle. Regardless if the letter is in the puzzle you will lose $200; if the letter is in the puzzle you continue your turn, if it is not it " +
                "is the next players turn. \n When you think you know the answer to the puzzle you choose solve, if you are correct you win $2000 and the round is over," +
                " otherwise the game continues. \n At the end of each round only the person who solved the puzzle will get to keep their money, the person with the most " +
                "amount of money at the end of 3 rounds wins the game!");
        }
    }


    
}