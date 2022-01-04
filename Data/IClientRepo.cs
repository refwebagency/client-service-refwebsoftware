using System.Collections.Generic;
using ClientService.Models;

namespace ClientService.Data
{
    public interface IClientRepo
    {
        bool SaveChanges();

        void CreateClient(Client client);

        IEnumerable<Client> GetAllClient();

        Client GetClientById(int id);

        bool VerifyClientByEmail(string email);

        void UpdateClientById(int id);

        void DeleteClientById(int id);
    }
}