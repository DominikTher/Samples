using SimpleApi.Models;
using System.Collections.Generic;

namespace SimpleApi.Repository
{
    public interface IPersonRepository
    {
        IList<Person> GetPeople();
    }
}
