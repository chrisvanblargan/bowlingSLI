using BowlingApp.Program;

namespace BowlingApp.Test
{
    public class Tests
    {
        private Game bowlingGame;
        [SetUp]
        public void Setup()
        {
            bowlingGame = new Game();
        }
        [Test]
        //[Ignore("")]
        public void ReamingPinsTest()
        {
            Assert.AreEqual(bowlingGame.recordRoll(8), 2);
            Assert.AreEqual(bowlingGame.recordRoll(1), 10); 
            Assert.AreEqual(bowlingGame.recordRoll(10), 10);    //reset pins for new frame
            Assert.AreEqual(bowlingGame.recordRoll(3), 7);
        }
        [Test]
        //[Ignore("")]
        public void FrameRollZeroTest()
        {
            bowlingGame.recordRoll(0);
            bowlingGame.recordRoll(0);
            Assert.AreEqual(bowlingGame.getFrame(0).rollArray[0], 0);
        }
        [Test]
        //[Ignore("")]
        public void FrameFrameNumTest()
        {
            bowlingGame.recordRoll(0);
            bowlingGame.recordRoll(0);
            bowlingGame.recordRoll(0);
            bowlingGame.recordRoll(0);
            Assert.AreEqual(bowlingGame.getFrame(1).frameNumber, 1);
        }
        [Test]
        //[Ignore("")]
        public void FrameStrikeTest()
        {
            bowlingGame.recordRoll(10);
            Assert.True(bowlingGame.getFrame(0).strikeInd);
        }
        [Test]
        //[Ignore("")]
        public void FrameSpareTest()
        {
            bowlingGame.recordRoll(1);
            bowlingGame.recordRoll(9);
            Assert.True(bowlingGame.getFrame(0).spareInd);
        }
        [Test]
        //[Ignore("")]
        public void FrameRollStringTest()
        {
            bowlingGame.recordRoll(1);
            bowlingGame.recordRoll(8);
            Assert.AreEqual(bowlingGame.getFrame(0).rollStringArray[0], "1");
            Assert.AreEqual(bowlingGame.getFrame(0).rollStringArray[1], "8");
        }
        [Test]
        //[Ignore("")]
        public void FrameNumberTest()
        {
            bowlingGame.recordRoll(0);
            bowlingGame.recordRoll(0);
            Assert.AreEqual(bowlingGame.frameNumber, 1);//count starts at 0
        }
        [Test]
        //[Ignore("")]
        public void GameActiveTest()
        {
            bowlingGame.recordRoll(0);
            bowlingGame.recordRoll(0);
            Assert.True(bowlingGame.isActive);//check game is active
        }
        [Test]
        //[Ignore("")]
        public void GameInactiveTest()
        {
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(10);
            Assert.False(bowlingGame.isActive);//check game is active
        }
        [Test]
        //[Ignore("")]
        public void PerfectGameTest()
        {
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(10);
            Assert.AreEqual(bowlingGame.gameScore, 300);
        }
        [Test]        
        //[Ignore("")]
        public void OopsAllSparesTest()
        {
            bowlingGame.recordRoll(1);
            bowlingGame.recordRoll(9);
            bowlingGame.recordRoll(1);
            bowlingGame.recordRoll(9);
            bowlingGame.recordRoll(1);
            bowlingGame.recordRoll(9);
            bowlingGame.recordRoll(1);
            bowlingGame.recordRoll(9);
            bowlingGame.recordRoll(1);
            bowlingGame.recordRoll(9);
            bowlingGame.recordRoll(1);
            bowlingGame.recordRoll(9);
            bowlingGame.recordRoll(1);
            bowlingGame.recordRoll(9);
            bowlingGame.recordRoll(1);
            bowlingGame.recordRoll(9);
            bowlingGame.recordRoll(1);
            bowlingGame.recordRoll(9);
            bowlingGame.recordRoll(1);
            bowlingGame.recordRoll(9);
            bowlingGame.recordRoll(1);
            Assert.AreEqual(bowlingGame.gameScore, 110);
        }
        [Test]
        //[Ignore("")]
        public void ExampleGameBonusRollTest()
        {
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(7);
            bowlingGame.recordRoll(3);
            bowlingGame.recordRoll(9);
            bowlingGame.recordRoll(0);
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(0);
            bowlingGame.recordRoll(8);
            bowlingGame.recordRoll(8);
            bowlingGame.recordRoll(2);
            bowlingGame.recordRoll(0);
            bowlingGame.recordRoll(6);
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(8);
            bowlingGame.recordRoll(1);
            Assert.AreEqual(bowlingGame.gameScore, 167);
        }
        [Test]
        //[Ignore("")]
        public void ExampleGameNoBonusTest()
        {
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(7);
            bowlingGame.recordRoll(3);
            bowlingGame.recordRoll(9);
            bowlingGame.recordRoll(0);
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(0);
            bowlingGame.recordRoll(8);
            bowlingGame.recordRoll(8);
            bowlingGame.recordRoll(2);
            bowlingGame.recordRoll(0);
            bowlingGame.recordRoll(6);
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(10);
            bowlingGame.recordRoll(8);
            bowlingGame.recordRoll(1);
            Assert.AreEqual(bowlingGame.gameScore, 146);
        }
    }
}



