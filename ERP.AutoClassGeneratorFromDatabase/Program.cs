using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.AutoClassGeneratorFromDatabase
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {

                // Get the connection string from a central DataManager class
                string connectionString = DataManager.ConnectionString();


                // Build the path to the SQL script located in the "Database Scripts" folder at the project root level.
                string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                string scriptFilePath = Path.Combine(projectRoot, "Database Scripts", "Create_Procedure.sql");

                // Check if the script file exists before attempting to run it
                if (File.Exists(scriptFilePath))
                {
                    // Run the SQL script on the database
                    DataManager.RunSqlScript(connectionString, scriptFilePath);
                }
                else
                {
                    // Inform the user that the script file was not found and guide them to correct the path
                    Console.WriteLine("❌ SQL script file not found at: " + scriptFilePath);
                    Console.WriteLine("➡ Please ensure the file 'Create_Procedure.sql' exists in the 'Database Scripts' folder of your project.");
                    return;
                }



                // Step 1: Fetch class definitions from the database using the stored procedure
                DataTable classTable = GetClassDefinitionsFromDatabase(connectionString);

                // Step 2: Check if any data is returned
                if (classTable.Rows.Count > 0)
                {
                    // Define the target file path where class files will be generated
                    string filePath = "D:\\ERP\\Model\\";

                    // Step 3: Generate class files based on the data and save them in the specified path
                    DataManager.GenerateClass(classTable, filePath);

                    // Notify user of successful generation
                    Console.WriteLine("Successfully generated all class");
                }
                else
                {
                    // Notify user if no class definitions were found
                    Console.WriteLine("No class definitions found.");
                }
            }
            catch (Exception ex)
            {
                // Catch any unexpected errors during the process and display them
                Console.WriteLine("Error occurred: " + ex.Message);
            }
            finally
            {
                // Wait for user input before closing the console window
                Console.ReadLine();
            }

        }

        #region GetClassDefinitionsFromDatabase
        /// <summary>
        /// Calls the stored procedure 'sp_GenerateClassProperties' to fetch 
        /// database table structures and generate corresponding C# class property definitions.
        /// </summary>
        /// <returns>A DataTable containing ClassName and PropertyTexts columns</returns>
        public static DataTable GetClassDefinitionsFromDatabase(string connectionString)
        {
            // Create a DataTable to hold the result
            DataTable classTable = new DataTable();

            // Create SQL connection and command to call the stored procedure
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GenerateClassProperties", conn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                // Specify the command type as Stored Procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // Execute the command and fill the DataTable with results
                adapter.Fill(classTable);
            }

            // Return the DataTable containing class names and property definitions
            return classTable;
        }
        #endregion

    }
}
