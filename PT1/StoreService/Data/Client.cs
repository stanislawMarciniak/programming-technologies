using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace StoreService.Data
{
    public class Client
    {
        private int clientID;
        private string firstName;
        private string lastName;
        private string email;

        public int ClientID => clientID;
        public string FirstName => firstName;
        public string LastName => lastName;
        public string Email => email;

        public Client(int _clientID, string _firstName, string _lastName, string _email)
        {
            clientID = _clientID;
            firstName = _firstName;
            lastName = _lastName;
            email = _email;
        }
    }
}
