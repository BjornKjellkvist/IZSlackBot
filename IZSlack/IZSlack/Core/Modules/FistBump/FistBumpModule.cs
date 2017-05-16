using IZSlack.Core.Messengers;
using IZSlack.Core.Model;
using IZSlack.Core.WebAPI;
using IZSlack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IZSlack.Core.Modules.FistBump {
    public class FistBumpModule : IModule {
        private RTMSender Sender = new RTMSender();
        public Dictionary<string, Action<string>> Commands { get; set; }
        public FistBumpModule() {
            Commands = new Dictionary<string, Action<string>> {
                { "!FistBump",  InitFistBump },
                { "!MyBumps", FetchMyAwaitedBumps}
            };
        }


        static List<GameRound> CurrentGames = new List<GameRound>();
        static List<GameRound> ArchivedGames = new List<GameRound>();

        public void InitFistBump(string payload) {
            Parser _Parser = new Parser(payload);
            string RequestPlayerId = _Parser.ParsePlayerId();
            string RequsetPlayerName = _Parser.ParsePlayerName();
            string RequestChannel = _Parser.ParseChannel();
            GameType _GameType = _Parser.ParseGameType();
            GameRound GameInProgress;
            if (_GameType == GameType.Open) {
                //Handle Open game type
                if (PlayerAlreadyInGame(RequestPlayerId, RequestChannel)) {
                    Sender.SendMessage(new RTMMessageOut {
                        channel = RequestChannel,
                        text = "You are waiting for a fist bump, you can't fist bump yourself"
                    });
                } else if (OpenGameInProgress(RequestChannel, out GameInProgress)) {
                    //Finish open game
                    FinishGame(GameInProgress, RequsetPlayerName, RequestPlayerId);
                } else {
                    //Start new game
                    StartOpenGame(new GameRound {
                        PlayerOne = RequestPlayerId,
                        Channel = RequestChannel
                    }, RequsetPlayerName);
                }
            } else if (_GameType == GameType.PlayerSpecific) {
                //Handle PlayerSpecific game type
                var TargetPlayer = _Parser.ParseTargetPlayer();
                //Finsih started specific game
                if (SpecificGameInProgress(TargetPlayer, RequestChannel, out GameInProgress)) {
                    FinishGame(GameInProgress);
                } else {
                    // start specifc game
                    StartSpecificGame(TargetPlayer, RequestPlayerId, RequsetPlayerName, RequestChannel);
                }
            }
        }

        private void FetchMyAwaitedBumps(string payload) {
            Parser _Parser = new Parser(payload);
            string PlayerId = _Parser.ParsePlayerId();
            string Channel = _Parser.ParseChannel();
            StringBuilder ListOfWaiters = new StringBuilder();
            foreach (GameRound round in CurrentGames) {
                if (round.PlayerTwo.Equals(PlayerId) && round.Channel.Equals(Channel)) {
                    ListOfWaiters.Append($"{round.PlayerOne}, ");
                }
            }
            //Make sure that we have people actually waiting for fist bumps from this user
            if (ListOfWaiters.Length > 1) {
                //Remove the last ", " from the list of waiters
                ListOfWaiters.Remove(ListOfWaiters.Length - 2, ListOfWaiters.Length);
                StringBuilder sb = new StringBuilder();
                sb.Append("You are waiting for Fist Bumps from: ")
                  .Append(ListOfWaiters.ToString());
                Sender.SendMessage(new RTMMessageOut {
                    channel = Channel,
                    text = sb.ToString()
                });
                //Tell the user the sad news that no one is waiting for fist bumps from him/her
            } else {
                Sender.SendMessage(new RTMMessageOut {
                    channel = Channel,
                    text = "No one is waiting for a fist bump from you in this channel =("
                });
            }
        }

        private void StartOpenGame(GameRound newRound, string playerName) {
            CurrentGames.Add(newRound);
            Sender.SendMessage(new RTMMessageOut {
                channel = newRound.Channel,
                text = $"{playerName} is looking for a fistbump! \nType \"!FistBump\" to bump {playerName} back!"
            });
            ConsoleMessenger.PrintSuccess($"New game started by {playerName}");
        }

        private void ArchiveGame(GameRound round) {
            ArchivedGames.Add(round);
            CurrentGames.Remove(round);
        }

        private void FinishGame(GameRound round, string requestingPlayerName = "", string requestingPlayerId = "") {
            if (!round.PlayerSpecific) {
                //finish open game
                round.PlayerTwo = requestingPlayerId;
                ArchiveGame(round);
                Sender.SendMessage(new RTMMessageOut {
                    channel = round.Channel,
                    text = $"{requestingPlayerName} Fist bumped {Users.FindUser(round.PlayerOne).name} back!"
                });
                ConsoleMessenger.PrintSuccess("Game Finished");
            } else {
                //finish specific game
                ArchiveGame(round);
                Sender.SendMessage(new RTMMessageOut {
                    channel = round.Channel,
                    text = $"{Users.FindUser(round.PlayerTwo).name} Fist bumped {Users.FindUser(round.PlayerOne).name} back!"
                });
            }
        }

        private bool PlayerAlreadyInGame(string player, string channel) {
            foreach (var round in CurrentGames) {
                if (round.Channel == channel && round.PlayerOne == player) {
                    return true;
                }
            }
            return false;
        }

        private bool SpecificGameInProgress(User targetPlayer, string channel, out GameRound round) {
            foreach (var Game in CurrentGames.Where(x => x.PlayerSpecific && x.Channel == channel)) {
                if (Game.PlayerTwo.Equals(targetPlayer.id)) {
                    round = Game;
                    return true;
                }
            }
            round = null;
            return false;
        }

        private void StartSpecificGame(User targetPlayer, string requestingPlayerId, string requestingPlayerName, string channel) {
            Channel ChannelInfo = APICalls.GetChannelInfo(channel);
            List<string> UsersInChannel = ChannelInfo.members.ToList();
            if (UsersInChannel.Contains(targetPlayer.id)) {
                GameRound newRound = new GameRound {
                    PlayerOne = requestingPlayerId,
                    PlayerTwo = targetPlayer.id,
                    Channel = channel,
                    PlayerSpecific = true
                };
                Sender.SendMessage(new RTMMessageOut {
                    channel = channel,
                    text = $"{requestingPlayerName} wants to fist bump {targetPlayer.name}"
                });
                ConsoleMessenger.PrintSuccess($"New game started by {requestingPlayerId}");
                CurrentGames.Add(newRound);
            } else {
                Sender.SendMessage(new RTMMessageOut {
                    channel = channel,
                    text = $"{targetPlayer.name} is not this channel"
                });
            }
        }

        private bool OpenGameInProgress(string channel, out GameRound gameRound) {
            foreach (var game in CurrentGames) {
                if (!game.PlayerSpecific && game.Channel == channel) {
                    gameRound = game;
                    return true;
                }
            }
            gameRound = null;
            return false;
        }

    }
}

