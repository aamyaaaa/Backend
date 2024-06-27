using Npgsql;
using art_gallery_api.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace art_gallery_api.Persistence
{
    public class UserDataAccess : IUserDataAccess
    {
        private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=muffy12;Database=art_gallery";

        public List<UserModel> GetUsers()
        {
            var users = new List<UserModel>();
            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM public.users", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new UserModel
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Email = reader["email"].ToString(),
                                FirstName = reader["firstname"].ToString(),
                                LastName = reader["lastname"].ToString(),
                                PasswordHash = reader["passwordhash"].ToString(),
                                Role = reader["role"].ToString(),
                                CreatedDate = Convert.ToDateTime(reader["createddate"]),
                                ModifiedDate = Convert.ToDateTime(reader["modifieddate"])
                            });
                        }
                    }
                }
            }
            return users;
        }

        public void AddUser(UserModel newUser)
        {
            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("INSERT INTO public.users (email, firstname, lastname, passwordhash, role, createddate, modifieddate) VALUES (@Email, @FirstName, @LastName, @PasswordHash, @Role, @CreatedDate, @ModifiedDate) RETURNING id", connection))
                {
                    command.Parameters.AddWithValue("@Email", newUser.Email);
                    command.Parameters.AddWithValue("@FirstName", newUser.FirstName);
                    command.Parameters.AddWithValue("@LastName", newUser.LastName);
                    command.Parameters.AddWithValue("@PasswordHash", newUser.PasswordHash);
                    command.Parameters.AddWithValue("@Role", newUser.Role);
                    command.Parameters.AddWithValue("@CreatedDate", newUser.CreatedDate);
                    command.Parameters.AddWithValue("@ModifiedDate", newUser.ModifiedDate);

                    newUser.Id = (int)command.ExecuteScalar();
                }
            }
        }

        public void UpdateUser(UserModel updatedUser)
        {
            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("UPDATE public.users SET email = @Email, firstname = @FirstName, lastname = @LastName, passwordhash = @PasswordHash, role = @Role, modifieddate = @ModifiedDate WHERE id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Email", updatedUser.Email ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@FirstName", updatedUser.FirstName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", updatedUser.LastName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PasswordHash", updatedUser.PasswordHash ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Role", updatedUser.Role ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ModifiedDate", updatedUser.ModifiedDate);
                    command.Parameters.AddWithValue("@Id", updatedUser.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteUser(int id)
        {
            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("DELETE FROM public.users WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public UserModel GetUserById(int id)
        {
            UserModel user = null;
            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM public.users WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new UserModel
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Email = reader["email"].ToString(),
                                FirstName = reader["firstname"].ToString(),
                                LastName = reader["lastname"].ToString(),
                                PasswordHash = reader["passwordhash"].ToString(),
                                Role = reader["role"].ToString(),
                                CreatedDate = Convert.ToDateTime(reader["createddate"]),
                                ModifiedDate = Convert.ToDateTime(reader["modifieddate"])
                            };
                        }
                    }
                }
            }
            return user;
        }

        public void UpdateUser(int id, UserModel updatedUser)
        {
            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("UPDATE public.users SET email = @Email, firstname = @FirstName, lastname = @LastName, passwordhash = @PasswordHash, role = @Role, modifieddate = @ModifiedDate WHERE id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Email", updatedUser.Email ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@FirstName", updatedUser.FirstName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", updatedUser.LastName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PasswordHash", updatedUser.PasswordHash ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Role", updatedUser.Role ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ModifiedDate", updatedUser.ModifiedDate);
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<UserModel> GetAdminUsers()
        {
            var adminUsers = new List<UserModel>();
            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM public.users WHERE role = @Role", connection))
                {
                    command.Parameters.AddWithValue("@Role", "Admin");
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            adminUsers.Add(new UserModel
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Email = reader["email"].ToString(),
                                FirstName = reader["firstname"].ToString(),
                                LastName = reader["lastname"].ToString(),
                                PasswordHash = reader["passwordhash"].ToString(),
                                Role = reader["role"].ToString(),
                                CreatedDate = Convert.ToDateTime(reader["createddate"]),
                                ModifiedDate = Convert.ToDateTime(reader["modifieddate"])
                            });
                        }
                    }
                }
            }
            return adminUsers;
        }
    }
}
