using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SimpleApi.Repository;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleApi.Response
{
    public class CustomResponse
    {
        public async Task Write(HttpContext httpContext)
        {
            var personRepository = httpContext.RequestServices.GetRequiredService<IPersonRepository>();
            var jsonString = JsonSerializer.Serialize(personRepository.GetPeople());

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = 200;
            await httpContext.Response.WriteAsync(jsonString);
        }
    }
}
