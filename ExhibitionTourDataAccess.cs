using Npgsql;
using art_gallery_api.Models;
using System;
using System.Collections.Generic;

namespace art_gallery_api.Persistence
{
    public class ExhibitionTourDataAccess : IExhibitionTourDataAccess
    {
        private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=muffy12;Database=art_gallery";

        public List<ExhibitionTour> GetExhibitionTours()
        {
            var tours = new List<ExhibitionTour>();
            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM public.exhibition_tours", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tours.Add(new ExhibitionTour
                            {
                                TourId = Convert.ToInt32(reader["id"]),
                                TourName = reader["tourname"].ToString(),
                                StartDate = Convert.ToDateTime(reader["startdate"]),
                                EndDate = reader["enddate"] as DateTime?,
                                CreatedDate = Convert.ToDateTime(reader["createddate"]),
                                ModifiedDate = Convert.ToDateTime(reader["modifieddate"])
                            });
                        }
                    }
                }
            }
            return tours;
        }

        public ExhibitionTour GetExhibitionTourById(int id)
        {
            ExhibitionTour tour = null;
            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM public.exhibition_tours WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tour = new ExhibitionTour
                            {
                                TourId = Convert.ToInt32(reader["id"]),
                                TourName = reader["tourname"].ToString(),
                                StartDate = Convert.ToDateTime(reader["startdate"]),
                                EndDate = reader["enddate"] as DateTime?,
                                CreatedDate = Convert.ToDateTime(reader["createddate"]),
                                ModifiedDate = Convert.ToDateTime(reader["modifieddate"])
                            };
                        }
                    }
                }
            }
            return tour;
        }

        public void InsertExhibitionTour(ExhibitionTour newTour)
        {
            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("INSERT INTO public.exhibition_tours (tourname, startdate, enddate, createddate, modifieddate) VALUES (@TourName, @StartDate, @EndDate, @CreatedDate, @ModifiedDate) RETURNING id", connection))
                {
                    command.Parameters.AddWithValue("@TourName", newTour.TourName);
                    command.Parameters.AddWithValue("@StartDate", newTour.StartDate);
                    command.Parameters.AddWithValue("@EndDate", newTour.EndDate ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CreatedDate", newTour.CreatedDate);
                    command.Parameters.AddWithValue("@ModifiedDate", newTour.ModifiedDate);

                    newTour.TourId = (int)command.ExecuteScalar();
                }
            }
        }

        public void UpdateExhibitionTour(int id, ExhibitionTour updatedTour)
        {
            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("UPDATE public.exhibition_tours SET tourname = @TourName, startdate = @StartDate, enddate = @EndDate, modifieddate = @ModifiedDate WHERE id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@TourName", updatedTour.TourName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@StartDate", updatedTour.StartDate);
                    command.Parameters.AddWithValue("@EndDate", updatedTour.EndDate ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ModifiedDate", updatedTour.ModifiedDate);
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteExhibitionTour(int id)
        {
            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("DELETE FROM public.exhibition_tours WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
