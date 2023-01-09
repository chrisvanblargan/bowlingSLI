namespace BowlingApp.Program
{
    public class BowlingProgram
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to Bowl-a-rama!");
            Game newGame = new Game();
            int max = 10;
            while (newGame.isActive)
            {
                try
                {
                    int roll = 0;
                    while(true)
                    {
                        Console.WriteLine($"Enter Roll Result(0-{max}): ");
                        string input = Console.ReadLine();
                        roll = Int32.Parse(input);
                        if(roll <= max){
                            break;
                        }
                        Console.WriteLine("That was not a valid result!");
                    }
                    max = newGame.recordRoll(roll);
                    if(newGame.isActive == false) //will return true if game is over
                    {
                        break;
                    }
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            Console.WriteLine("Press any key to exit..."); //prevent console app from auto closing
            Console.ReadKey();

        }
    }
    public class Game //stores player name, score, and array of frame objects 
    {
        public bool isActive = false;
        public int frameNumber;
        public int gameScore;
        private Frame[] frameArray = new Frame[10];
        private Scoreboard scoreboard;
        

        public Game()
        {
            isActive = true;
            frameNumber = 0;
            for(int i = 0; i < 10; i++)
            {
                frameArray[i] = new Frame(i); //setup frames
            }
            scoreboard = new Scoreboard(frameArray);
            gameScore = scoreboard.writeScore(frameNumber); //display initial scoreboard
        }
        public int recordRoll(int rollScore)   //takes input, processes the roll for a given frame, checks if the frame is complete, calls teh scoreboard, and returns pins remaining
        {
            int pinsRemaining;
            frameArray[frameNumber].processRoll(rollScore);
            if(frameArray[frameNumber].frameComplete)
            {
                pinsRemaining = 10;
                frameNumber++;
                if(isGameOver())
                {
                    frameNumber = 9;
                    isActive = false;
                }
            }else{
                if(frameArray[frameNumber].strikeInd) // frame 10 will not be complete after strike
                {
                    if(frameArray[frameNumber].rollArray[1] != 10) //check for strike on roll 2
                    {
                        pinsRemaining = 10 - frameArray[frameNumber].rollArray[1]; 
                    }else{
                        pinsRemaining = 10;
                    }
                }else{
                    if(frameArray[frameNumber].spareInd)
                    {
                        pinsRemaining = 10; //spares mean there will be a full reset for teh bonus throw in frame 10
                    }else{
                        pinsRemaining = 10 - frameArray[frameNumber].rollArray[0];
                    }
                }
            }
            gameScore = scoreboard.writeScore(frameNumber);
            return pinsRemaining;   //return pins remaining for data cleaning input
        }
        private bool isGameOver()
        {
            return frameNumber > 9;
        }
        public Frame getFrame(int i)
        {
            return frameArray[i];
        }
    }
    public class Frame//stores info about a specific frame, rolls, flag for display
    {
        public int frameNumber;
        public int[] rollArray = new int[3];
        public string[] rollStringArray = {" ", " ", " "}; //strings start as " " instead of "" for ease of formatting on the scoreboard
        public bool displayFlag = false;
        public bool frameComplete = false;

        //used for calculating scores
        public bool strikeInd = false;
        public bool spareInd = false;
        public int frameScore;


        public int rollCount = 0;
        public int rollsRemaining;


        public Frame(int number)    //every frame will have a score of 0, and 2 rolls remaining, we will add an additional roll if needed in frame 10
        {
            frameNumber = number;
            frameScore = 0;
            rollsRemaining = 2;
        }
        public int[] processRoll(int rollVal)//add roll value to array, add roll string to display array, process if completed frame
        {
            rollArray[rollCount] = rollVal;
            rollToString(rollCount, rollVal);
            if(frameNumber == 9 && (strikeInd || spareInd) && rollCount < 1)    //check if there is a strike or spare in frame 10, to add a bonus roll
            {
                rollsRemaining++;
            }
            rollsRemaining--;
            rollCount++;
            if((rollCount > 1 || rollVal == 10) && frameNumber != 9) //if 2 rolls or strike, and not last frame, frame is completed
            {
                frameComplete = true;
                rollsRemaining = 0;
            }else{
                if(frameNumber == 9)
                {
                    if(rollArray[0] + rollArray[1] < 10 && rollCount == 2)//check for bonus roll
                    {
                        frameComplete = true;
                        rollsRemaining = 0;
                    }else{
                        if(rollCount == 3)
                        {    
                            frameComplete = true;
                        }
                    }
                }
            }
            return rollArray;
        }
        private string rollToString(int rollNum, int rollVal)//processes a roll into bowling standard symbols for use in the display
        {
            if(rollVal == 10 && (rollNum == 0 || frameNumber == 9))//strike on first roll or any roll on 10
            {
                rollStringArray[rollNum] = "X";
                if(rollNum > 0)
                {
                    if(rollArray[rollNum - 1] != 10)
                    {
                        rollStringArray[rollNum] = "/";
                        spareInd = true;
                    }else{
                        strikeInd = true;
                    }
                }else{
                    strikeInd = true;
                }
                return rollStringArray[rollNum];
            }
            if(rollNum != 0)//first roll cannot be spare, so skip
            {
                if((rollArray[rollNum] + rollArray[rollNum-1] == 10) && spareInd == false)//spare, will always be here after first roll, so no out of bounds
                {
                    rollStringArray[rollNum] = "/";
                    spareInd = true;
                    return rollStringArray[rollNum];
                }
            }
            switch(rollArray[rollNum])//non-special rolls
            {
                case 0:
                    rollStringArray[rollNum] = "-";     //formatting to match bowling scoreboards
                    break;
                default:
                    rollStringArray[rollNum] = rollVal.ToString();
                    break;
            }
            return rollStringArray[rollNum];
        }
    }
    class Scoreboard //displays scores after each roll, will parse scores from numbers into typical bowling score symbols + numbers
    {
        private Frame[] frameArrayScoreboard;
        private int frameNumber;
        public Scoreboard(Frame[] frameArray)
        {
            frameArrayScoreboard = frameArray;
        }
        public int writeScore(int frameNum)
        {
            frameNumber = frameNum;
            this.displayHeader();
            return this.displayScoreRows();
        }
        private void displayHeader()
        {
            string frameNumberDisplay;
            if(frameNumber + 1 >= 10)
            {
                frameNumberDisplay = "10";
            }else{
                frameNumberDisplay = (frameNumber + 1).ToString() + " ";
            }
            Console.WriteLine("| Current Frame |   " + frameNumberDisplay +"  |Rolls Remaining|   " + frameArrayScoreboard[frameNumber].rollsRemaining + "   |");
            Console.WriteLine("| Frame |   1   |   2   |   3   |   4   |   5   |   6   |   7   |   8   |   9   |    10   |");
            Console.WriteLine("| ----- | ----- | ----- | ----- | ----- | ----- | ----- | ----- | ----- | ----- | ------- |");
        }                                             
        private int displayScoreRows()
        {
            
            string frameRow = "| Input";
            string frameOut = "";
            string scoreRow = "| Score";
            string scoreOut = "";
            int scoreReturn = 0;
            for(int i = 0; i < 10; i++) //iterate for frame, need to iterate all to draw entire scoreboard
            {
                Frame currentFrame = frameArrayScoreboard[i];
                
                
                frameOut = " |  " + currentFrame.rollStringArray[0] + " " + currentFrame.rollStringArray[1] + " "; 

                if(i == 9)
                {
                    frameOut += currentFrame.rollStringArray[2] + "  |";
                }

                frameRow += frameOut;
                currentFrame.frameScore = processScore(i, frameNumber); 
                if(i != 0)
                {
                    currentFrame.frameScore += frameArrayScoreboard[i-1].frameScore;
                }
                scoreReturn = currentFrame.frameScore;
                if(currentFrame.displayFlag)
                {
                    scoreOut = currentFrame.frameScore.ToString();
                }else{
                    scoreOut = "";
                }
                switch(scoreOut.Length)  // fix dispay format
                {
                    case 0:
                        scoreOut += "    ";
                        break;
                    case 1:
                        scoreOut += "   ";
                        break;
                    case 2:
                        scoreOut += "  ";
                        break;
                    case 3:
                        scoreOut += " ";
                        break;
                    default:
                        break;
                }
                scoreOut = " |  " + scoreOut;
                scoreRow += scoreOut;
                if(i == 9)
                {
                    scoreRow += "   |";
                }
            }

            Console.WriteLine(frameRow);
            Console.WriteLine(scoreRow);

            return scoreReturn;
        }
        //score is calculated outside of the frames so we can look forward and backward conditionally at all frames
        //will calculate if score can be displayed (strike/spare wait)
        public int processScore(int frameNum, int realtimeFrame)
        {
            int output; 
            Frame currentFrame = frameArrayScoreboard[frameNum];
            //using default frame objects lets us detect if the frame objects are empty or real data
            Frame previousFrame = new Frame(11);
            Frame nextFrame = new Frame(11);
            Frame nextNextFrame = new Frame(11);
            if(frameNum > 0)        //frame 0 will not have a previous frame
            {
                previousFrame = frameArrayScoreboard[frameNum - 1];
            }
            if(frameNum < 9)        //frame 10 will not have a next frame
            {
                nextFrame = frameArrayScoreboard[frameNum + 1];
                if(frameNum < 8)    //frame 9 will not have a next next frame
                {
                    nextNextFrame = frameArrayScoreboard[frameNum + 2];
                }
            }
            //wait logic, frame score logic
            if((currentFrame.spareInd == false && currentFrame.strikeInd == false) || frameNum == 9)
            {
                //No bonus roll
                currentFrame.frameScore = currentFrame.rollArray[0] + currentFrame.rollArray[1] + currentFrame.rollArray[2];
                if(((previousFrame.spareInd == false && previousFrame.strikeInd == false) || previousFrame.displayFlag == true) && previousFrame.rollsRemaining == 0 && currentFrame.rollsRemaining == 0) //possible cleanup
                {
                    //only display after previous frames
                    currentFrame.displayFlag = true;
                }else{
                    if(frameNum == 0 && currentFrame.spareInd == false && currentFrame.strikeInd == false && currentFrame.rollsRemaining == 0)
                    {
                        //only draw frame 0 score if not spare/strike and last roll is used
                        currentFrame.displayFlag = true;
                    }else{
                        if(frameNum == 9 && currentFrame.rollsRemaining == 0)
                        {
                            //final frame draw after all rolls used
                            currentFrame.displayFlag = true;
                        }
                    }
                }
            }else{
                if(currentFrame.strikeInd)//2 cases, 2 rolls from next frame, or 1 from next, one from one after
                {
                    if(frameNum < 8)
                    {
                        if(nextFrame.strikeInd)
                        {
                            //strike + strike means looking an extra frame ahead for 2nd bonus score
                            currentFrame.frameScore = currentFrame.rollArray[0] + nextFrame.rollArray[0] + nextNextFrame.rollArray[0];
                            if(nextNextFrame.rollCount > 0 || realtimeFrame == 10)
                            {
                                //show if final frame entered or extra frame has calculated a roll already
                                currentFrame.displayFlag = true;
                            }
                        }else{ 
                            //strike + no strike means just adding the entire next frames rolls
                            currentFrame.frameScore = currentFrame.rollArray[0] + nextFrame.rollArray[0] + nextFrame.rollArray[1];
                            if(nextFrame.rollCount > 1 || realtimeFrame > 9)//when to show 8
                            {
                                //same condition as above, shifted one roll
                                currentFrame.displayFlag = true;
                            }
                        }
                    }else{
                        //strike strike and strike no strike are the same condition on frame 9, since frame 10 cannot 
                        currentFrame.frameScore = currentFrame.rollArray[0] + nextFrame.rollArray[0] + nextFrame.rollArray[1];
                        if(nextFrame.rollCount > 1)//when to show 9 on strike
                        {
                            currentFrame.displayFlag = true;
                        }
                    }
                }else{  //spare ind by default
                    //always has this calculation, only needs to change display based on frames
                    currentFrame.frameScore = currentFrame.rollArray[0] + currentFrame.rollArray[1] + nextFrame.rollArray[0];
                    if(frameNum < 8)
                    {
                        //frame 8 spare display condition
                        if(nextFrame.rollsRemaining < 2 || (nextNextFrame.frameNumber == 11 && currentFrame.rollCount > 0))
                        {
                            currentFrame.displayFlag = true;
                        }
                    }else{
                        if(nextFrame.rollCount > 0)//when to show 9 on spare
                        {
                            currentFrame.displayFlag = true;
                        }
                    }
                }
            }
            output = currentFrame.frameScore;
            return output;
        }
    }
}