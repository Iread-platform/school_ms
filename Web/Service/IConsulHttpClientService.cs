using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Http;

namespace iread_school_ms.Web.Service
{
    public interface IConsulHttpClientService
    {
        Task<T> GetAsync<T>(string serviceName, string requestUri);
        AgentService GetAgentService(string serviceName);
        Task<T> PostBodyAsync<T>(string serviceName, string requestUri, Object obj);
        Task<T> PostFormAsync<T>(string serviceName, string requestUri, Dictionary<string, string> parameters,
            List<IFormFile>? attachments);
        Task<T> PostFormAsync<T>(string serviceName, string requestUri, Dictionary<string, string> parameters, Object obj);
        Task<T> PutBodyAsync<T>(string serviceName, string requestUri, Object obj);
        Task<T> PutFormAsync<T>(string serviceName, string requestUri, Dictionary<string, string> parameters, List<IFormFile>? attachments);
    }
}
