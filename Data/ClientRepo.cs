using System;
using System.Collections.Generic;
using System.Linq;
using ClientService.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientService.Data
{
    public class ClientRepo : IClientRepo
    {

        private readonly AppDbContext _context;

        public ClientRepo(AppDbContext context)
        {
            // On initialise le context 
            _context = context;
        }

        public void CreateClient(Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            _context.Add(client);
            _context.SaveChanges();
        }

        public void DeleteClientById(int id)
        {
            var client = _context.Client.FirstOrDefault(Client => Client.Id == id);

            if (client != null)
            {
                _context.Client.Remove(client);
            }
        }

        public IEnumerable<Client> GetAllClient()
        {
            return _context.Client.ToList();
        }

        public Client GetClientById(int id)
        {
            return _context.Client.FirstOrDefault(Client => Client.Id == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >=0 );
        }

        public void UpdateClientById(int id)
        {
            var client = _context.Client.FirstOrDefault(Client => Client.Id == id);

            _context.Entry(client).State = EntityState.Modified;
        }

        public bool VerifyClientByEmail(string email)
        {
            var client = _context.Client.FirstOrDefault(Client => Client.Email == email);

            if (client != null)
            {
                return true;
            }else
            {
                return false;
            }
        }
    }
}