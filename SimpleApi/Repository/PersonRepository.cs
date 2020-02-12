using SimpleApi.Models;
using System.Collections.Generic;

namespace SimpleApi.Repository
{
    public class PersonRepository : IPersonRepository
    {
        public IList<Person> GetPeople()
        {
            return new List<Person> {
                new Person { Id = 0, FullName = "Andrea Tate" },
                new Person { Id = 1, FullName = "Jimmy Pope" },
                new Person { Id = 2, FullName = "Sophie Mccormick" },
                new Person { Id = 3, FullName = "Adrian Leonard" },
                new Person { Id = 4, FullName = "Chester Lee" },
                new Person { Id = 5, FullName = "Jimmie Wilson" },
            };
        }
    }
}
