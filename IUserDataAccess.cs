using art_gallery_api.Models;
using System.Collections.Generic;

namespace art_gallery_api.Persistence
{
    public interface IUserDataAccess
    {
        List<UserModel> GetUsers();
        UserModel GetUserById(int id);
        // void InsertUser(UserModel newUser);
        void UpdateUser(int id, UserModel updatedUser);
        void DeleteUser(int id);
        List<UserModel> GetAdminUsers();
        void AddUser(UserModel newUser);
    }
}
