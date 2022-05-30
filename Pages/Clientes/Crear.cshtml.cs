using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace _220529PruebaTecnica.Pages.Clientes
{
    public class CrearModel : PageModel
    {
        public ClienteInfo clienteInfo = new ClienteInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            clienteInfo.id = Request.Form["id"];
            clienteInfo.nombres = Request.Form["nombres"];
            clienteInfo.apellidos = Request.Form["apellidos"];
            clienteInfo.telefono = Request.Form["telefono"];
            clienteInfo.correo = Request.Form["correo"];
            clienteInfo.departamento = Request.Form["departamento"];
            clienteInfo.ciudad = Request.Form["ciudad"];
            clienteInfo.edad = Request.Form["edad"];

            if (clienteInfo.nombres.Length == 0 || clienteInfo.correo.Length == 0 || clienteInfo.apellidos.Length == 0)
            {
                errorMessage = "Todos los campos son obligatorios";
                return;
            }

            //save the new client into the database
            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=SalonesEmpresariales;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO clientes " +
                                 "(nombres, apellidos, correo, departamento, ciudad) VALUES " +
                                 "(@nombres, @apellidos, @correo, @departamento, @ciudad);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@nombres", clienteInfo.nombres);
                        command.Parameters.AddWithValue("@apellidos", clienteInfo.apellidos);
                        command.Parameters.AddWithValue("@correo", clienteInfo.correo);
                        command.Parameters.AddWithValue("@departamento", clienteInfo.departamento);
                        command.Parameters.AddWithValue("@ciudad", clienteInfo.ciudad);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            clienteInfo.nombres = ""; clienteInfo.apellidos = ""; clienteInfo.correo = ""; clienteInfo.departamento = ""; clienteInfo.ciudad = "";
            successMessage = "Nuevo Cliente Creado Exitosamente";

            Response.Redirect("/Clientes/Index");
        }
    }
}