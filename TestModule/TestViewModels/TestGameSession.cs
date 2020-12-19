using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MysticAdventureRPG.ViewModels;

namespace TestModule.TestViewModels
{
    [TestClass]
    public class TestGameSession
    {
        [TestMethod]
        public void TestCreateGameSession()
        {
            GameSessionViewModel gameSession = new GameSessionViewModel();

            Assert.IsNotNull(gameSession.CurrentPlayer);
            Assert.AreEqual("Casa", gameSession.CurrentLocation.Name);
        }

        [TestMethod]
        public void TestPlayerMovesHomeAndIsCompletelyHealedOnKilled()
        {
            GameSessionViewModel gameSession = new GameSessionViewModel();

            gameSession.CurrentPlayer.TakeDamage(999);

            Assert.AreEqual("Casa", gameSession.CurrentLocation.Name);
            Assert.AreEqual(gameSession.CurrentPlayer.MaximumHitPoints, gameSession.CurrentPlayer.CurrentHitPoints);
        }
    }
}
