using System;
using System.Collections.Generic;

namespace PandaCoin
{
    public class UserInterface
    {
        private readonly BlockChain _blockChain;
        private readonly UsersDatabase _usersDatabase;

        public UserInterface(BlockChain blockChain, UsersDatabase usersDatabase)
        {
            _blockChain = blockChain;
            _usersDatabase = usersDatabase;
        }

        public void ShowUi()
        {
            while (true)
            {
                Console.WriteLine("Is blockchain valid: {0}", _blockChain.IsValidChain());

                Console.WriteLine("\n PandaCoin - is blockchain valid: {0}", _blockChain.IsValidChain() +
                                                                             "\n1 - Create user\n" +
                                                                             "2 - Create transaction\n" +
                                                                             "3 - Start mining\n" +
                                                                             "4 - Check user wallet\n" +
                                                                             "5 - Print Chain\n" +
                                                                             "6 - Try To Cheat\n" +
                                                                             "7 - Change Mine Difficulty\n"+
                                                                             "9 - Exit");

                var command = Console.ReadLine();

                if (command == "1")
                {
                    _usersDatabase.CreateUser();
                }
                else if (command == "2")
                {
                    CreateTransaction();
                }
                else if (command == "3")
                {
                    MineBlock();
                }
                else if (command == "4")
                {
                    CheckUserWallet();
                }
                else if (command == "5")
                {
                    PrintChain();
                }
                else if (command == "6")
                {
                    TryToCheat();
                }
                else if (command == "7")
                {
                    ChangeMineDifficulty();
                }
                else if (command == "9")
                {
                    Console.WriteLine("Exiting application");
                    break;
                }
            }
        }

        private void ChangeMineDifficulty()
        {
            Console.WriteLine("Current mine difficulty is: {0}. Set new difficulty to: ", _blockChain.GetCurrentDifficulty());
            var difficulty = Console.ReadLine();
            
            if (int.TryParse(difficulty, out var verifiedDifficulty))
            {
                _blockChain.SetMineDifficulty(verifiedDifficulty);
            }
        }

        private void TryToCheat()
        {
            if (_blockChain.Chain.Count > 1)
            {
                _blockChain.Chain[1].Transactions = new List<Transaction>()
                {
                    new("", "Michal", 3000)
                };
            }
            else
            {
                Console.WriteLine("Blockchain is still empty.");
            }
        }

        private void PrintChain()
        {
            foreach (var block in _blockChain.Chain)
            {
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("Hash: {0} ", block.Hash);
                Console.WriteLine("Previous Hash: {0}\n", block.PreviousHash);

                foreach (var transaction in block.Transactions)
                {
                    Console.WriteLine("Transaction From: {0} To: {1} Amount {2}",
                        transaction.From, transaction.To, transaction.Amount);
                }

                Console.WriteLine("--------------------------------------------------\n");
            }
        }

        private void CheckUserWallet()
        {
            Console.WriteLine("Write user address: ");
            var user = Console.ReadLine();
            var wallet = _blockChain.GetBalance(user);
            Console.WriteLine("User currently have: {0}", wallet);
        }

        private void MineBlock()
        {
            Console.WriteLine("Write miner address: ");
            var miner = Console.ReadLine();
            if (_usersDatabase.CheckIfUserExist(miner))
            {
                Console.WriteLine("Started Mining Block with difficulty: {0}.", _blockChain.GetCurrentDifficulty());
                _blockChain.MineBlock(miner);
                Console.WriteLine("BALANCE of the miner changed to: {0}", _blockChain.GetBalance(miner));
            }
            else
            {
                Console.WriteLine("Miner does not exist in miner database.");
            }
        }

        private void CreateTransaction()
        {
            Console.WriteLine("Write sender address: ");
            var from = Console.ReadLine();
            Console.WriteLine("Write recipient address: ");
            var to = Console.ReadLine();
            Console.WriteLine("Write amount: ");
            var amount = Console.ReadLine();

            if (double.TryParse(amount, out var verifiedAmount))
            {
                Console.WriteLine(_blockChain.CreateTransaction(new Transaction(@from, to, verifiedAmount))
                    ? "Transaction added to pending Transactions."
                    : "User don't have enough money.");
            }
            else
            {
                Console.WriteLine("Amount is not a number.");
            }
        }
    }
}