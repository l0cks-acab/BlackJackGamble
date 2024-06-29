# BlackjackGamble

BlackjackGamble is a Rust plugin that allows players to gamble their RP (Reward Points) in a game of Blackjack using the Economics plugin. Players can place bets and play against the dealer in a user-friendly UI. **Dependant on: [Economics](https://umod.org/plugins/economics)**

## Features
- Gamble RP in a game of Blackjack.
- Configurable minimum and maximum bet amounts.
- User-friendly UI for placing bets, hitting, standing, and quitting the game.
- Permissions for players and admins.
- Admin commands to change minimum and maximum bet amounts on the go.

## Installation

1. Download the `BlackjackGamble.cs` file.
2. Place the file in your server's `oxide/plugins` directory.
3. Restart your server or use the `oxide.reload BlackjackGamble` command in the console.

## Configuration

The configuration file allows you to set the minimum and maximum bet amounts. The file will be generated in the `oxide/config` directory after the first run.

### Default Configuration

```json
{
  "MinBet": 10,
  "MaxBet": 1000
}
```

### Changing Configuration
Open the BlackjackGamble.json file in the oxide/config directory edit the values or use the in-game commands.

## Permissions
blackjackgamble.use: Allows players to use the /blackjack command and play the game.

blackjackgamble.admin: Allows admins to use admin commands to change bet limits.

## Commands
### Player Commands
- /blackjack <bet>: Start a game of Blackjack with the specified bet amount.

### Admin Commands
- /setminbet <amount>: Set the minimum bet amount.
- /setmaxbet <amount>: Set the maximum bet amount.

## Example
- Player runs the command /blackjack 100 to start a game with a bet of 100 RP.
- The UI displays the player's and dealer's cards.
- Player can choose to hit or stand.
- The game will continue until the player stands or busts.
- The result will be displayed, and the player can choose to replay or quit.

