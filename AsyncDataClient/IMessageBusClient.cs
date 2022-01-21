using ClientService.Dtos;

namespace client_service_refwebsoftware.AsyncDataClient
{
    public interface IMessageBusClient
    {
         void UpdatedClient(ClientUpdateAsyncDto clientUpdatedDto);
    }
}