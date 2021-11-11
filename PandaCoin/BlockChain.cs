using System;
using System.Collections.Generic;
using System.Linq;

namespace PandaCoin
{
    /// <summary>
    /// Klasa tworząca blockchain i sterująca transakcjami.
    /// </summary>
    public class BlockChain
    {
        /// <summary>
        /// Trudność stworzenia nowego bloku. Zawiera informację
        /// jak dużo zer powinno znajdować się na początku bloku.
        /// </summary>
        private readonly int _proofOfWorkDifficulty;

        /// <summary>
        /// Wielkość nagrody (w transakcji), jaką otrzyma kopiący blok po wykopaniu kryptowaluty.
        /// Jest to stała wartość, w praktyce wysyłający transakcję dodaje od siebie nagrodę, by
        /// przekonać kopiących do dołączenia jego transakcji w kopanym bloku. 
        /// </summary>
        private readonly double _miningReward;

        /// <summary>
        /// Lista transakcji, jakie zostaną dołączone do tworzonego nowego bloku.
        /// Zawiera również nagrodę w postaci Transakcji dla kopiącego, który
        /// wykopał blok.
        /// </summary>
        private List<Transaction> _pendingTransactions;

        /// <summary>
        /// Lista Bloków, która znajduje się w blockchainie.
        /// </summary>
        public List<Block> Chain { get; set; }

        /// <summary>
        /// Konstruktor Blockchainu. Tworzy pierwszy blok blockchainu z pustym wcześniejszym blokiem i
        /// pustą listą transakcji.
        /// </summary>
        /// <param name="proofOfWorkDifficulty"></param>
        /// <param name="miningReward"></param>
        public BlockChain(int proofOfWorkDifficulty, double miningReward)
        {
            _proofOfWorkDifficulty = proofOfWorkDifficulty;
            _miningReward = miningReward;
            _pendingTransactions = new List<Transaction>();
            Chain = new List<Block>() {CreateGenesisBlock()};
        }

        /// <summary>
        /// Funkcja tworząca pierwszy blok w blockchainie.
        /// </summary>
        /// <returns>Pierwszy blok blockchainu.</returns>
        private Block CreateGenesisBlock()
        {
            var transactions = new List<Transaction> {new Transaction("", "", 0)};
            return new Block(DateTime.Now, transactions, "0");
        }


        /// <summary>
        /// Funkcja tworząca transakcję, która zostanie dodana do listy oczekujących do wykopania.
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns>True, jeśli użytkownik ma środki na koncie do wykonania transakcji.</returns>
        public bool CreateTransaction(Transaction transaction)
        {
            if (GetBalance(transaction.From) - transaction.Amount >= 0)
            {
                _pendingTransactions.Add(transaction);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Funkcja używana do wykopania nowego bloku. Tworzy transakcję dla kopiącego jako nagrodę, a
        /// następnie tworzymy blok i wylicza dla niego hash. 
        ///
        /// W realnej wersji blockchainu zanim nasz blok zostanie dodany do listy przechodzi walidację i spośród
        /// możliwych wykopanych bloków wybierany jest najdłuższy. (zawiera np. najwięcej transakcji). 
        /// </summary>
        /// <param name="minerAddress">Adres użytkownika kopiącego blok</param>
        public void MineBlock(string minerAddress)
        {
            CreateReward(minerAddress);

            var block = new Block(DateTime.Now, _pendingTransactions, Chain.LastOrDefault()?.Hash);
            block.MineBlock(_proofOfWorkDifficulty);

            AddBlockToChain(block);
        }
        /// <summary>
        /// Funkcja tworzy nagrodę dla osoby, która wykopie blok i dodawana jest do listy oczekujących
        /// transakcji.
        /// </summary>
        /// <param name="minerAddress">Adres użytkownika kopiącego blok</param>
        private void CreateReward(string minerAddress)
        {
            var minerRewardTransaction = new Transaction(null, minerAddress, _miningReward);
            _pendingTransactions.Add(minerRewardTransaction);
        }
        
        /// <summary>
        /// Jeśli wykopiemy prawidłowy hash dodajemy hash wczesniejszego bloku do zmiennej
        /// PreviousHash nowo wykopanemu blokowi i dodajemy nasz blok
        /// do listy bloków w blockchainie. Na koniec czyścimy listę oczekujących transakcji,
        /// ponieważ zawarte one już są w wykopanym bloku.
        /// </summary>
        /// <param name="block">wykopany blok</param>
        private void AddBlockToChain(Block block)
        {
            block.PreviousHash = Chain.Last().Hash;
            Chain.Add(block);
            _pendingTransactions = new ();
        }

        /// <summary>
        /// Funkcja sprawcza, czy blockchain jest prawidłowy. Dla każdego bloku w chainie
        /// sprawdzane jest, czy wcześniejszy i poprzedni blok mają takie same hashe jak te stworzone przy
        /// utworzeniu bloku. 
        /// </summary>
        /// <returns>True jeśli blok jest prawidłowy</returns>
        public bool IsValidChain()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                var previousBlock = Chain[i - 1];
                var currentBlock = Chain[i];


                if (currentBlock.PreviousHash != previousBlock.Hash)
                {
                    Console.WriteLine("FOR DEBUG PURPOSES!");
                    Console.WriteLine(currentBlock.PreviousHash + " - Current Block Previous Hash");
                    Console.WriteLine(previousBlock.Hash + " - Previous Block Hash");
                    return false;
                }

                if (currentBlock.Hash != currentBlock.CreateHash())
                {
                    Console.WriteLine("FOR DEBUG PURPOSES!");
                    Console.WriteLine(currentBlock.Hash + " - Current Block Hash");
                    Console.WriteLine(currentBlock.CreateHash() + " - Current Block CreateHash");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Wylicza wartość porfela użytkownika. Na podstawie identyfikatora
        /// funkcja sprawdza każdy blok w chainie, czy transakcje zawierają adres użytkownika
        /// i jeśli tak wyliczany jest balans portfela.
        /// </summary>
        /// <param name="address">Adres użytkownika kopiącego blok</param>
        /// <returns></returns>
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