using System;
using System.Collections.Generic;
using AutoMapper;
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

        public ClientController(IMapper mapper, IClientRepo repository)
        {
            _repository = repository;
            _mapper = mapper;
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
        public ActionResult<UpdateClientDTO> UpdateClientById(int id, UpdateClientDTO clientDTO)
        {
            var clientItem = _repository.GetClientById(id);

            _mapper.Map(clientDTO, clientItem);

            if (clientItem == null)
            {
                return NotFound();
            }

            _repository.UpdateClientById(id);
            _repository.SaveChanges();

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