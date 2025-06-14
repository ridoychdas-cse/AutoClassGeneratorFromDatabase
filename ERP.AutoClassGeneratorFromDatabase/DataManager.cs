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
    public class DataManager
    {
        #region Database Connection String
        /// <summary>
        /// Returns the database connection string used to connect to the AIMS database.
        /// </summary>
        /// <returns>A connection string for SQL Server</returns>
        public static string ConnectionString()
        {
            // Define the SQL Server connection string (change values if needed for security).
            const string connectionString = @"Server=localhost; Database=ERP; User Id=sa; Password=123; Trusted_Connection=False; pooling=false;";
            return connectionString;
        }
        #endregion

        #region Run SQL Script

        /// <summary>
        /// Executes the entire SQL script file against the specified database connection.
        /// Reads the SQL script file content and executes it as a single batch command.
        /// </summary>
        /// <param name="connectionString">The connection string for the target SQL Server database.</param>
        /// <param name="scriptFilePath">The full file path of the SQL script to be executed.</param>
        public static void RunSqlScript(string connectionString, string scriptFilePath)
        {
            // Read the entire SQL script text from the specified file path
            string script = File.ReadAllText(scriptFilePath);

            // Open a connection to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Create a SQL command with the script text
                using (SqlCommand command = new SqlCommand(script, connection))
                {
                    // Execute the entire script as a single command
                    command.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Generate Class Files from DataTable
        /// <summary>
        /// Generates C# class files for each table based on the given DataTable.
        /// Each row in the DataTable should contain 'ClassName' and 'PropertyTexts' columns.
        /// </summary>
        /// <param name="data">A DataTable containing class metadata per row (ClassName, PropertyTexts)</param>
        /// <param name="filePath">The directory path where the generated .cs class files will be saved</param>
        public static void GenerateClass(DataTable data, string filePath)
        {
            // Loop through each row in the DataTable
            foreach (DataRow dr in data.Rows)
            {
                // Extract class name and properties text from the row
                string className = dr["ClassName"].ToString().Trim();
                string properties = dr["PropertyTexts"].ToString();

                // Generate the full class content using a helper method
                string classContent = GenerateClassContent(className, properties);

                // Construct the file name and path
                string fullPath = Path.Combine(filePath, className + ".cs");

                // Write the generated class content to the .cs file
                File.WriteAllText(fullPath, classContent);
            }
        }

        #endregion

        #region Generate Class Content

        /// <summary>
        /// Generates the full C# class content as a string based on the class name and its properties.
        /// Includes using directives, namespace, class declaration, and properly formatted properties with consistent indentation.
        /// </summary>
        /// <param name="className">The name of the class</param>
        /// <param name="PropertiesName">A formatted string of property definitions (may contain multiple lines)</param>
        /// <returns>The complete class source code as a string</returns>
        private static string GenerateClassContent(string className, string PropertiesName)
        {
            // Split the properties string into individual lines
            string[] propertyLines = PropertiesName
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Trim each line to remove any leading/trailing whitespace,
            // then add exactly 8 spaces indentation uniformly
            string formattedProperties = string.Join(
                Environment.NewLine,
                propertyLines.Select(line => "        " + line.Trim()));

            // Build the full class content string with using directives and namespace
            string classContent =
                "using Newtonsoft.Json;\r\n" +
                "using System;\r\n" +
                "using System.Collections.Generic;\r\n\r\n" +
                "namespace ERP.BOL\r\n" +
                "{\r\n" +
                "    [JsonObject]\r\n" +
                "    [Serializable]\r\n" +
                "    public class " + className + "\r\n" +
                "    {\r\n" +
                formattedProperties + "\r\n" +
                "    }\r\n" +
                "}";

            return classContent;
        }

        #endregion
    }
}
