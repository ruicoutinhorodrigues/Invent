using Invent.Common.Models;
using Plugin.Connectivity;
using System.Threading.Tasks;


namespace Invent.Common.Services
{
    public class NetService
    {
        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Turn on your internet.",
                };
            }

            var isReachable = await CrossConnectivity.Current.IsRemoteReachable(
                "google.com");

            if (!isReachable)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Ckeck your connection.",
                };
            }

            return new Response
            {
                IsSuccess = true,
                Message = "Ok"
            };
        }
    }
}
