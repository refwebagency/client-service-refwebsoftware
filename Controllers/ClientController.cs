using System;
using System.Collections.Generic;
using AutoMapper;
using client_service_refwebsoftware.AsyncDataClient;
using ClientService.Data;
using ClientService.Dtos;
using ClientService.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClientService.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ClientController : ControllerBase
    {
        private readonly IClientRepo _repository;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;

        public ClientController(IMapper mapper, IClientRepo repository, IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ReadClientDTO>> GetAllClient()
        {
            var ClientItems = _repository.GetAllClient();

            return Ok(_mapper.Map<IEnumerable<ReadClientDTO>>(ClientItems));
        }

        [HttpGet("{id}", Name = "GetClientById")]
        public ActionResult<ReadClientDTO> GetClientById(int id)
        {
            var ClientItem = _repository.GetClientById(id);

            if (ClientItem == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ReadClientDTO>(ClientItem));
        }

        [HttpGet("meet/id", Name = "GetClientByMeetId")]
        public ActionResult<IEnumerable<ReadClientDTO>> GetClientByMeetId(int id)
        {
            var ClientItems = _repository.GetClientByMeetId(id);

             if (ClientItems == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<ReadClientDTO>>(ClientItems));
        }

        [HttpPost]
        public ActionResult<CreateClientDTO> CreateClient(CreateClientDTO clientDTO)
        {
            var clientModel = _mapper.Map<Client>(clientDTO);
            
            if (_repository.VerifyClientByEmail(clientModel.Email))
            {
                return NotFound("Un compte contenant cet email est déja existant");
            }
            else
            {
                _repository.CreateClient(clientModel);
                _repository.SaveChanges();
            }

            var readClient = _mapper.Map<ReadClientDTO>(clientModel);

            return CreatedAtRoute(nameof(GetClientById), new {id = readClient.Id }, readClient);
        }

        [HttpPut("{id}", Name = "UpdateClientById")]
        public ActionResult<ReadClientDTO> UpdateClientById(int id, UpdateClientDTO clientDTO)
        {
            var clientItem = _repository.GetClientById(id);

            _mapper.Map(clientDTO, clientItem);

            if (clientItem == null)
            {
                return NotFound();
            }

            _repository.UpdateClientById(id);
            _repository.SaveChanges();
            


            // Envoie Async des Data

            try
            {
                Console.WriteLine(clientItem.Address);
              
                //var clientItemUpdated = _repository.GetClientById(id); 
                //On envoie les données du client mis à jour avec les données du DTO
                var clientUpdatedDto = _mapper.Map<ClientUpdateAsyncDto>(clientItem);

                // On dit que l'event est égal à "Client_Updated"
                clientUpdatedDto.Event = "Client_Updated";
                Console.WriteLine(clientUpdatedDto.Address);
                Console.WriteLine(clientUpdatedDto.Id);
                Console.WriteLine(clientUpdatedDto.LastName);
                Console.WriteLine(clientUpdatedDto.Event);

                // On appelle la méthode qui se trouve dans MessageBusClient
                _messageBusClient.UpdatedClient(clientUpdatedDto);
            }

            catch (System.Exception ex)
            {
                Console.WriteLine("Error: Async" + ex.Message);
            }

            return CreatedAtRoute(nameof(GetClientById), new {id = clientDTO.Id }, clientDTO);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteClientById(int id)
        {
            var clientItem = _repository.GetClientById(id);

            if (clientItem == null)
            {
                return NotFound();
            }

            _repository.DeleteClientById(clientItem.Id);
            _repository.SaveChanges();

            return NoContent();
        }
    }
}