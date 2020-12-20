using Engine.Factories;
using Engine.Models;
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

        //[TestMethod]
        //public void TestPlayerAttackAndBeingAttacked()
        //{
        //    GameSessionViewModel gameSession = new GameSessionViewModel();

        //    gameSession.CurrentPlayer.CurrentWeapon = ItemFactory.ObtainItem(1005) as Weapon;
        //    gameSession.CurrentEnemy = EnemyFactory.GetEnemyByID(1);
        //    gameSession.EvaluateBattleTurn(null);

        //    //Assert.AreEqual(gameSession.CurrentPlayer.MaximumHitPoints, gameSession.CurrentPlayer.CurrentHitPoints);
        //}
    }
}
