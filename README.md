##### Debug:
- Fix UI not opening
- Show debug message in console for errors and cUI drops

##### To add:
- Better cUI
- Icons
- Clean up code
- Add config to change minimum bet and debug toggle.

# BlackjackGamble

BlackjackGamble is a Rust plugin that allows players to gamble their RP in a game of Blackjack using the Economics plugin, made specifically for royalgamerz.

## Features

- Players can bet their RP in a game of Blackjack.
- Simple and intuitive user interface for playing the game.
- Commands to hit, stand, and quit the game.
- Automatic handling of game results, including busts, draws, and wins.

## Installation

1. Download the `BlackjackGamble.cs` file.
2. Place the `BlackjackGamble.cs` file into your `oxide/plugins` directory.
3. Ensure that the Economics plugin is installed and configured correctly.

## Commands

### Chat Commands

- `/blackjack <bet>`: Start a new game of Blackjack with the specified bet amount.

### Console Commands

- `blackjack.hit`: Draw another card.
- `blackjack.stand`: Stand and let the dealer play.
- `blackjack.quit`: Quit the current game.

## Usage

1. To start a game, use the `/blackjack <bet>` command in the chat, replacing `<bet>` with the amount of RP you want to bet.
2. The game will display your cards, the dealer's cards, and your options.
3. Use the provided buttons to hit (draw another card) or stand (let the dealer play).
4. The game will automatically determine the winner and update your RP balance accordingly.
5. You can quit the game at any time by using the `Quit` button.
