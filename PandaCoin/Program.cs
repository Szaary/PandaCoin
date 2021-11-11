using System;
using System.Collections.Generic;


namespace PandaCoin
{
    /// <summary>
    /// Implementacja prostego blockchaina na podstawie:
    /// https://towardsdatascience.com/blockchain-explained-using-c-implementation-fb60f29b9f07
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var userDatabase = new UsersDatabase();
            // Dodałem 3 użytkowników na start w celach testowych.
            userDatabase.CreateUser("Michal");
            userDatabase.CreateUser("Dorota");
            userDatabase.CreateUser("Filip");
            
            var blockChain = new BlockChain(proofOfWorkDifficulty: 2, miningReward: 10);
            
            var userInterface = new UserInterface(blockChain, userDatabase);
            
            userInterface.ShowUi();
        }
    }
}