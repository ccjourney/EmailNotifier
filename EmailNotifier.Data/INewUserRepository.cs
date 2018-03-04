using System.Collections.Generic;
using System.Threading.Tasks;
using EmailNotifier.Models;

namespace EmailNotifier.Data
{
    public interface INewUserRepository
    {
        Task<IEnumerable<NewUser>> GetAllUsers();
        Task CreateNewUser(NewUser newUser);
        Task UpdateNewUser(string id, NewUser newUser);
        Task<NewUser> GetNewUser(string id);
    }
}