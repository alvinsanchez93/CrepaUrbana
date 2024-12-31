using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using CrepaUrbana.Models;


namespace CrepaUrbana.Services
{
    public class ProductoService
    {
        private readonly string connectionString;

        public ProductoService()
        {
            connectionString = ConfigurationManager.ConnectionStrings["CrepaUrbanaDB"].ConnectionString;
        }

        public List<Producto> ObtenerProductos()
        {
            var productos = new List<Producto>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM Productos";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productos.Add(new Producto
                            {
                                IdProducto = (int)reader["IdProducto"],
                                Nombre = reader["Nombre"].ToString(),
                                Precio = (decimal)reader["Precio"],
                                Categoria = reader["Categoria"].ToString(),
                                Stock = (int)reader["Stock"]
                            });
                        }
                    }
                }
            }
            return productos;
        }

        public void AgregarProducto(Producto producto)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "INSERT INTO Productos (Nombre, Precio, Categoria, Stock) VALUES (@Nombre, @Precio, @Categoria, @Stock)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    command.Parameters.AddWithValue("@Precio", producto.Precio);
                    command.Parameters.AddWithValue("@Categoria", producto.Categoria);
                    command.Parameters.AddWithValue("@Stock", producto.Stock);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void ActualizarProducto(Producto producto)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE Productos SET Nombre = @Nombre, Precio = @Precio, Categoria = @Categoria, Stock = @Stock WHERE IdProducto = @IdProducto";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdProducto", producto.IdProducto);
                    command.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    command.Parameters.AddWithValue("@Precio", producto.Precio);
                    command.Parameters.AddWithValue("@Categoria", producto.Categoria);
                    command.Parameters.AddWithValue("@Stock", producto.Stock);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void EliminarProducto(int idProducto)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "DELETE FROM Productos WHERE IdProducto = @IdProducto";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdProducto", idProducto);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
