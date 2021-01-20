using CommonClasses.Enums;
using CommonClasses.EventArgs;
using Engine.Models;
using System;
using System.Linq;

namespace Services
{
    public class BattleService : IDisposable
    {
        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();
        private readonly Player _player;
        private readonly Enemy _enemy;
        private bool wasKilled = false;

        public event EventHandler<CombatVictoryEventArgs> OnCombatVictory;

        public BattleService( Player player, Enemy enemy)
        {
            _player = player;
            _enemy = enemy;

            _player.OnBattleActionPerformed += OnCombatantActionPerformed;
            _player.OnKilled += OnPlayerKilled;

            _enemy.OnBattleActionPerformed += OnCombatantActionPerformed;
            _enemy.OnKilled += OnCurrentEnemyKilled;

            _messageBroker.RaiseMessage("", GameMessageTypeEnum.Info);
            _messageBroker.RaiseMessage($"Ti imbatti in un {_enemy.Name}!", GameMessageTypeEnum.BattleInfo);
        }

        public void Dispose()
        {
            _player.OnBattleActionPerformed -= OnCombatantActionPerformed;
            _player.OnKilled -= OnPlayerKilled;

            _enemy.OnBattleActionPerformed -= OnCombatantActionPerformed;
            _enemy.OnKilled -= OnCurrentEnemyKilled;
        }

        public void EvaluateBattleTurn(string attackType)
        {
            if ((_player.Speed + _player.CurrentWeapon?.WeaponSpeed) >= (_enemy.Speed + _enemy.CurrentWeapon.WeaponSpeed) && attackType == "L")
            {
                EvaluatePlayerTurn(attackType);
                if (!_enemy.IsDead)
                {
                    _enemy.UseCurrentWeaponOn(_player);
                }
            }
            else
            {
                _enemy.UseCurrentWeaponOn(_player);
                if (wasKilled)
                {
                    wasKilled = false;
                    return;
                }
                else
                {
                    EvaluatePlayerTurn(attackType);
                }
            }
            if (_player.Class != PlayerClassTypeEnum.Mago)
                _player.Restore(_player.MaximumStamina / 15, nameof(Player.CurrentStamina));
            else
                _player.Restore(_player.MaximumMana / 15, nameof(Player.CurrentMana));
        }

        private void EvaluatePlayerTurn(string attackType)
        {
            int damageToEnemy;

            // Determino il danno inflitto dal PLayer
            if (_player.CurrentWeapon == null)
            {
                _messageBroker.RaiseMessage("Non avendo equipaggiato un'arma ti scagli sul nemico a mani nude.", GameMessageTypeEnum.BattleInfo);
                damageToEnemy = _player.BaseDamage;
                _messageBroker.RaiseMessage($"Hai colpito {_enemy.Name} causando {damageToEnemy} dann{(damageToEnemy > 1 ? "i" : "o")}!", GameMessageTypeEnum.BattlePositive);
                _enemy.TakeDamage(damageToEnemy);
            }
            else
            {
                _player.UseCurrentWeaponOn(_enemy, attackType);
            }
        }

        private void OnCurrentEnemyKilled(object sender, EventArgs eventArgs)
        {
            // Se il nemico è sconfitto ottengo il loot
            _messageBroker.RaiseMessage($"Hai sconfitto {_enemy.Name}!", GameMessageTypeEnum.BattleInfo);

            _messageBroker.RaiseMessage($"Ricevi {_enemy.RewardExperiencePoints} punti esperienza!", GameMessageTypeEnum.BattleInfo);
            _player.AddExperience(_enemy.RewardExperiencePoints);

            _messageBroker.RaiseMessage($"Ricevi {_enemy.Gold} oro!", GameMessageTypeEnum.BattleInfo);
            _player.ReceiveGold(_enemy.Gold);

            foreach (GroupedItem drop in _enemy.GroupedInventory)
            {
                _messageBroker.RaiseMessage($"Ricevi {drop.Quantity} {drop.Item.Name}!", GameMessageTypeEnum.BattleInfo);
                _player.AddItemToInventory(drop);
                foreach (QuestStatus questStatus in _player.Quests)
                {
                    if (questStatus.Quest.ItemsToComplete.FirstOrDefault(e => e.Item.ItemID == drop.Item.ItemID)!= null)
                    {
                        if (_player.HasAllTheseItems(questStatus.Quest.ItemsToComplete))
                            questStatus.Status = QuestStatusEnum.Completabile;
                    }
                }

            }
            OnCombatVictory?.Invoke(this, new CombatVictoryEventArgs());
        }

        private void OnPlayerKilled(object sender, EventArgs eventArgs)
        {
            wasKilled = true;
        }

        private void OnCombatantActionPerformed(object sender, GameMessageEventArgs args)
        {
            _messageBroker.RaiseMessage(args.Message, args.Type);
        }
    }
}
