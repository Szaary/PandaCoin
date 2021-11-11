using System;

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
                
                Console.WriteLine("\n PandaCoin\n" +
                                  "1 - Create user\n" +
                                  "2 - Create transaction\n"+
                                  "3 - Start mining\n"+
                                  "4 - Check user wallet\n"+
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
                else if (command == "9")
                {
                    Console.WriteLine("Exiting application");
                    break;
                }

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
                Console.WriteLine("Started Mining Block: ");
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

            if(double.TryParse(amount, out var verifiedAmount))
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