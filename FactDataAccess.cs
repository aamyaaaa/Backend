using Npgsql;
using art_gallery_api.Models;
using System;
using System.Collections.Generic;

namespace art_gallery_api.Persistence
{
    public class FactDataAccess : IFactDataAccess
    {
        private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=muffy12;Database=art_gallery";

        public List<Fact> GetFacts()
        {
            var facts = new List<Fact>();
            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM public.facts", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            facts.Add(new Fact
                            {
                                FactId = Convert.ToInt32(reader["id"]),
                                Facts = reader["fact"].ToString()
                            });
                        }
                    }
                }
            }
            return facts;
        }

        public Fact GetFactById(int id)
        {
            Fact fact = null;
            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM public.facts WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            fact = new Fact
                            {
                                FactId = Convert.ToInt32(reader["id"]),
                                Facts = reader["fact"].ToString()
                            };
                        }
                    }
                }
            }
            return fact;
        }

        public void InsertFact(Fact newFact)
        {
            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("INSERT INTO public.facts (fact) VALUES (@Facts) RETURNING id", connection))
                {
                    command.Parameters.AddWithValue("@Facts", newFact.Facts);    
                    newFact.FactId = (int)command.ExecuteScalar();
                }
            }
        }

        public void UpdateFact(int id, Fact updatedFact)
        {
            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("UPDATE public.facts SET fact = @Facts WHERE id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Facts", updatedFact.Facts ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteFact(int id)
        {
            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("DELETE FROM public.facts WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
