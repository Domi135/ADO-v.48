using Microsoft.Data.SqlClient;
using System.Net.NetworkInformation;
namespace ADO
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Sakila;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            Console.WriteLine("Ange skådespelarens för- och efternamn: ");
            string? actorName = Console.ReadLine();

            string query = @"SELECT title AS FilmTitle
                            FROM actor a 
                            INNER JOIN film_actor fa ON a.actor_id = fa.actor_id
                            INNER JOIN film f ON fa.film_id = f.film_id
                            WHERE (a.first_name + ' ' + a.last_name) = @ActorName";

            using (connection)
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ActorName", actorName);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                       
                        if (reader.HasRows)
                        {
                            Console.WriteLine($"\nFilmer med {actorName}: ");

                            while (reader.Read())
                            {
                                Console.WriteLine($"- {reader["FilmTitle"]}");
                            }
                        }

                        else
                        {
                            Console.WriteLine("Inga filmer hittades...");
                        }

                        Console.WriteLine("Tryck på valfri knapp för att avsluta.");
                        Console.ReadKey();
                    }
                }
            }
        }
    }
}
