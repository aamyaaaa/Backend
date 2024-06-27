using Npgsql;
using art_gallery_api.Models;
using System;
using System.Collections.Generic;

namespace art_gallery_api.Persistence
{
    public class ArtistDataAccess : IArtistDataAccess
    {
        private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=muffy12;Database=art_gallery";

        public List<Artist> GetArtists()
        {
            var artists = new List<Artist>();
            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM public.artists", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            artists.Add(new Artist
                            {
                                ArtistId = Convert.ToInt32(reader["id"]),
                                Name = reader["name"].ToString(),
                                BirthDate = reader["birthdate"] as DateTime?,
                                FamousWork = reader["famouswork"].ToString(),
                                Bio = reader["bio"].ToString(),
                                CreatedDate = Convert.ToDateTime(reader["createddate"]),
                                ModifiedDate = Convert.ToDateTime(reader["modifieddate"])
                            });
                        }
                    }
                }
            }
            return artists;
        }

        public Artist GetArtistById(int id)
        {
            Artist artist = null;
            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM public.artists WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            artist = new Artist
                            {
                                ArtistId = Convert.ToInt32(reader["id"]),
                                Name = reader["name"].ToString(),
                                BirthDate = reader["birthdate"] as DateTime?,
                                FamousWork = reader["famouswork"].ToString(),
                                Bio = reader["bio"].ToString(),
                                CreatedDate = Convert.ToDateTime(reader["createddate"]),
                                ModifiedDate = Convert.ToDateTime(reader["modifieddate"])
                            };
                        }
                    }
                }
            }
            return artist;
        }

        public void InsertArtist(Artist newArtist)
        {
            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("INSERT INTO public.artists (name, birthdate, famouswork, bio, createddate, modifieddate) VALUES (@Name, @BirthDate, @FamousWork, @Bio, @CreatedDate, @ModifiedDate) RETURNING id", connection))
                {
                    command.Parameters.AddWithValue("@Name", newArtist.Name);
                    command.Parameters.AddWithValue("@BirthDate", newArtist.BirthDate);
                    command.Parameters.AddWithValue("@FamousWork", newArtist.FamousWork);
                    command.Parameters.AddWithValue("@Bio", newArtist.Bio);
                    command.Parameters.AddWithValue("@CreatedDate", newArtist.CreatedDate);
                    command.Parameters.AddWithValue("@ModifiedDate", newArtist.ModifiedDate);

                    newArtist.ArtistId = (int)command.ExecuteScalar();
                }
            }
        }

        public void UpdateArtist(int id, Artist updatedArtist)
        {
            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("UPDATE public.artists SET name = @Name, birthdate = @BirthDate, famouswork = @FamousWork, bio = @Bio, modifieddate = @ModifiedDate WHERE id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Name", updatedArtist.Name ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@BirthDate", updatedArtist.BirthDate ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@FamousWork", updatedArtist.FamousWork ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Bio", updatedArtist.Bio ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ModifiedDate", updatedArtist.ModifiedDate);
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteArtist(int id)
        {
            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("DELETE FROM public.artists WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
