using System;
using System.Collections.Generic;

namespace PandaCoin
{
    public class UsersDatabase
    {
        private HashSet<string> _userAddress = new();
        
        public void CreateUser()
        {
            Console.WriteLine("Write your address: ");
            var address = Console.ReadLine();
            
            CreateUser(address);
        }
        public void CreateUser(string address)
        {
            if (_userAddress.Contains(address))
            {
                Console.WriteLine("This address is already taken.");
            }
            else
            {
                _userAddress.Add(address);
                Console.WriteLine("User created: {0}. (authentication logic etc.)", address);
            }
        }

        public bool CheckIfUserExist(string userName)
        {
            return _userAddress.Contains(userName);
        }
    }
}