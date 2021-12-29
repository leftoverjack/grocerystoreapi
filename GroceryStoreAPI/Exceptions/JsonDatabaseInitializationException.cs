using System;

namespace GroceryStoreAPI.Exceptions
{
    public class JsonDatabaseInitializationException : Exception
    {
        private static string _message = "Could not initialize json database";
        public JsonDatabaseInitializationException() : base(_message)
        {
        }
    }
}
