using System.Collections.Generic;
using System.Data;
using Data;
using Service.Models;


namespace Service
{
    public class ClientService
    {
        ClientRepository clientRepository = new ClientRepository();
        EventReturnRepository returnRepository = new ReturnEventRepository();
        EventPurchaseRepository purchaseRepository = new PurchaseEventRepository();

        public List<ClientModel> GetAllClients()
        {
            List<ClientModel> models = new List<ClientModel>();

            foreach (var client in clientRepository.GetAllClients())
            {
                models.Add(MapClientDetails(client));
            }
            return models;
        }

        public ClientModel GetClientById(int id)
        {
            Client client = clientRepository.GetClientById(id);

            return (client is null) ? null : MapClientDetails(client);
        }
        public ClientModel GetClientByCredentials(string name, string surname)
        {
            return MapClientDetails(clientRepository.GetClientByCredentials(name, surname));
        }

        public ClientModel GetLastlyAddedClient()
        {
            Client client = clientRepository.GetLastClient();

            return client == null ? null : MapClientDetails(client);
        }

        public bool AddClient(ClientModel model)
        {
            if (model == null)
            {
                return false;
            }

            clientRepository.AddClient(MapModelDetails(model));
            return true;
        }

        public bool DeleteClient(int id)
        {
            if (ClientExists(id) && HasNoEvents(id))
            {
                clientRepository.DeleteClient(id);
                return true;
            }
            return false;
        }

        public bool UpdateClient(ClientModel model)
        {
            if (ClientExists(model._id))
            {
                clientRepository.UpdateClient(MapModelDetails(model));
                return true;
            }

            return false;
        }

        public bool HasNoEvents(int id)
        {
            return purchaseRepository.GetPurchaseEventsByClientId(id).Count.Equals(0)
                   && returnRepository.GetReturnEventsByClientId(id).Count.Equals(0);
        }

        private bool ClientExists(int id)
        {
            return clientRepository.GetClientById(id) != null;
        }

        private Client MapModelDetails(ClientModel model)
        {
            return new Client()
            {
                Id = model._id,
                FirstName = model._firstName,
                LastName = model._lastName,
                Email = model._email
            };
        }

        private ClientModel MapClientDetails(Client client)
        {
            return new ClientModel()
            {
                _id = client.Id,
                _firstName = client.FirstName,
                _lastName = client.LastName,
                _email = client.Email
            };
        }
    }
}
