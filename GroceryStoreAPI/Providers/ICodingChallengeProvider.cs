namespace GroceryStoreAPI.Providers
{
    public interface ICodingChallengeProvider
    {
        public bool ValidateArray(int[] array);

        public string GetMissingArrayElements(int[] inputArray);
    }
}
