using System;
using System.Collections.Generic;
using System.Linq;

namespace PandaCoin
{
    public class BlockChain
    {
        private readonly int _proofOfWorkDifficulty;
        private readonly double _miningReward;
        private List<Transaction> _pendingTransactions;
        public List<Block> Chain { get; set; }

        public BlockChain(int proofOfWorkDifficulty, double miningReward)
        {
            _proofOfWorkDifficulty = proofOfWorkDifficulty;
            _miningReward = miningReward;
            _pendingTransactions = new List<Transaction>();
            Chain = new List<Block>() {CreateGenesisBlock()};
        }

        private Block CreateGenesisBlock()
        {
            var transactions = new List<Transaction> {new Transaction("", "", 0)};
            return new Block(DateTime.Now, transactions, "0");
        }

        public void CreateTransaction(Transaction transaction)
        {
            _pendingTransactions.Add(transaction);
        }

        public void MineBlock(string minerAddress)
        {
            var minerRewardTransaction = new Transaction(null, minerAddress, _miningReward);
            _pendingTransactions.Add(minerRewardTransaction);

            var block = new Block(DateTime.Now, _pendingTransactions);
            block.MineBlock(_proofOfWorkDifficulty);

            block.PreviousHash = Chain.Last().Hash;
            Chain.Add(block);
            _pendingTransactions = new List<Transaction>();
        }

        public bool IsValidChain()
        {
            for (var i = 1; i < Chain.Count; i++)
            {
                var previousBlock = Chain[i - 1];
                var currentBlock = Chain[i];

                if (currentBlock.PreviousHash != previousBlock.Hash) return false;
                if (currentBlock.Hash != currentBlock.CreateHash()) return false;
            }

            return true;
        }

        public double GetBalance(string address)
        {
            double balance = 0;
            foreach (var block in Chain)
            {
                foreach (var transaction in block.Transactions)
                {
                    if (transaction.From == address)
                    {
                        balance -= transaction.Amount;
                    }

                    if (transaction.To == address)
                    {
                        balance += transaction.Amount;
                    }
                }
            }
            return balance;
        }
    }
}