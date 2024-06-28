using Oxide.Core;
using Oxide.Core.Plugins;
using Oxide.Game.Rust.Cui;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Oxide.Plugins
{
    [Info("BlackjackGamble", "herbs.acab", "1.2.0")]
    [Description("Allows players to gamble their RP in a game of Blackjack using the Economics plugin")]

    public class BlackjackGamble : RustPlugin
    {
        [PluginReference]
        private Plugin Economics;

        private Dictionary<ulong, BlackjackGame> activeGames = new Dictionary<ulong, BlackjackGame>();

        private class BlackjackGame
        {
            public List<int> playerCards;
            public List<int> dealerCards;
            public int bet;
            public bool isActive;

            public BlackjackGame(int bet)
            {
                this.bet = bet;
                playerCards = new List<int>();
                dealerCards = new List<int>();
                isActive = true;
                DealInitialCards();
            }

            private void DealInitialCards()
            {
                playerCards.Add(DrawCard());
                playerCards.Add(DrawCard());
                dealerCards.Add(DrawCard());
                dealerCards.Add(DrawCard());
            }

            public int DrawCard()
            {
                System.Random rnd = new System.Random();
                return rnd.Next(1, 11);
            }

            public int GetPlayerTotal()
            {
                return playerCards.Sum();
            }

            public int GetDealerTotal()
            {
                return dealerCards.Sum();
            }
        }

        [ChatCommand("blackjack")]
        private void StartBlackjack(BasePlayer player, string command, string[] args)
        {
            if (args.Length != 1 || !int.TryParse(args[0], out int bet))
            {
                player.ChatMessage("Usage: /blackjack <bet>");
                return;
            }

            var balance = (double)Economics.Call("Balance", player.UserIDString);
            if (balance < bet)
            {
                player.ChatMessage("You don't have enough RP to place that bet.");
                return;
            }

            if (activeGames.ContainsKey(player.userID))
            {
                player.ChatMessage("You are already in a game of Blackjack.");
                return;
            }

            Economics.Call("Withdraw", player.UserIDString, (double)bet);
            activeGames[player.userID] = new BlackjackGame(bet);
            ShowBlackjackUI(player);
        }

        private void ShowBlackjackUI(BasePlayer player)
        {
            var game = activeGames[player.userID];
            CuiElementContainer container = new CuiElementContainer();

            string panel = container.Add(new CuiPanel
            {
                Image = { Color = "0.1 0.1 0.1 0.8" },
                RectTransform = { AnchorMin = "0.3 0.3", AnchorMax = "0.7 0.7" },
                CursorEnabled = true
            }, "Overlay", "BlackjackUI");

            container.Add(new CuiLabel
            {
                Text = { Text = "Blackjack", FontSize = 20, Align = UnityEngine.TextAnchor.MiddleCenter, Color = "1 1 1 1" },
                RectTransform = { AnchorMin = "0 0.85", AnchorMax = "1 1" }
            }, panel);

            container.Add(new CuiLabel
            {
                Text = { Text = $"Your Cards: {string.Join(", ", game.playerCards)} (Total: {game.GetPlayerTotal()})", FontSize = 16, Align = UnityEngine.TextAnchor.MiddleCenter, Color = "0.8 0.8 0.8 1" },
                RectTransform = { AnchorMin = "0.1 0.7", AnchorMax = "0.9 0.8" }
            }, panel);

            container.Add(new CuiLabel
            {
                Text = { Text = $"Dealer's Cards: {string.Join(", ", game.dealerCards)} (Total: {game.GetDealerTotal()})", FontSize = 16, Align = UnityEngine.TextAnchor.MiddleCenter, Color = "0.8 0.8 0.8 1" },
                RectTransform = { AnchorMin = "0.1 0.5", AnchorMax = "0.9 0.6" }
            }, panel);

            container.Add(new CuiButton
            {
                Button = { Command = "blackjack.hit", Color = "0.2 0.6 0.2 0.8" },
                RectTransform = { AnchorMin = "0.3 0.2", AnchorMax = "0.45 0.3" },
                Text = { Text = "Hit", FontSize = 14, Align = UnityEngine.TextAnchor.MiddleCenter, Color = "1 1 1 1" }
            }, panel);

            container.Add(new CuiButton
            {
                Button = { Command = "blackjack.stand", Color = "0.6 0.2 0.2 0.8" },
                RectTransform = { AnchorMin = "0.55 0.2", AnchorMax = "0.7 0.3" },
                Text = { Text = "Stand", FontSize = 14, Align = UnityEngine.TextAnchor.MiddleCenter, Color = "1 1 1 1" }
            }, panel);

            container.Add(new CuiButton
            {
                Button = { Command = "blackjack.quit", Color = "0.6 0.2 0.2 0.8" },
                RectTransform = { AnchorMin = "0.8 0.9", AnchorMax = "0.95 0.95" },
                Text = { Text = "Quit", FontSize = 14, Align = UnityEngine.TextAnchor.MiddleCenter, Color = "1 1 1 1" }
            }, panel);

            CuiHelper.AddUi(player, container);
        }

        [ConsoleCommand("blackjack.hit")]
        private void HitCommand(ConsoleSystem.Arg arg)
        {
            BasePlayer player = arg.Player();
            if (player == null || !activeGames.ContainsKey(player.userID)) return;

            var game = activeGames[player.userID];
            game.playerCards.Add(game.DrawCard());

            if (game.GetPlayerTotal() > 21)
            {
                player.ChatMessage("Bust! You lose.");
                CuiHelper.DestroyUi(player, "BlackjackUI");
                activeGames.Remove(player.userID);
                return;
            }

            ShowBlackjackUI(player);
        }

        [ConsoleCommand("blackjack.stand")]
        private void StandCommand(ConsoleSystem.Arg arg)
        {
            BasePlayer player = arg.Player();
            if (player == null || !activeGames.ContainsKey(player.userID)) return;

            var game = activeGames[player.userID];

            while (game.GetDealerTotal() < 17)
            {
                game.dealerCards.Add(game.DrawCard());
            }

            DetermineWinner(player);
            CuiHelper.DestroyUi(player, "BlackjackUI");
        }

        [ConsoleCommand("blackjack.quit")]
        private void QuitCommand(ConsoleSystem.Arg arg)
        {
            BasePlayer player = arg.Player();
            if (player == null || !activeGames.ContainsKey(player.userID)) return;

            player.ChatMessage("You have quit the game.");
            CuiHelper.DestroyUi(player, "BlackjackUI");
            activeGames.Remove(player.userID);
        }

        private void DetermineWinner(BasePlayer player)
        {
            var game = activeGames[player.userID];

            if (game.GetDealerTotal() > 21 || game.GetPlayerTotal() > game.GetDealerTotal())
            {
                player.ChatMessage("You win!");
                Economics.Call("Deposit", player.UserIDString, (double)(game.bet * 2));
            }
            else if (game.GetPlayerTotal() == game.GetDealerTotal())
            {
                player.ChatMessage("It's a draw!");
                Economics.Call("Deposit", player.UserIDString, (double)game.bet);
            }
            else
            {
                player.ChatMessage("You lose.");
            }

            activeGames.Remove(player.userID);
        }
    }
}
