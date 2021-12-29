using System.ComponentModel.DataAnnotations;

namespace GroceryStoreAPI.Dtos
{
    public class CustomerEditDto
    {
        [Required]
        public string Name { get; set; }

        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return false;
            }

            return true;
        }
    }
}
