using System.Collections.Generic;

namespace GroceryStoreAPI.Providers
{
    public class CodingChallengeProvider : ICodingChallengeProvider
    {
        public string GetMissingArrayElements(int[] inputArray)
        {
            var outputElements = new List<string>();
            var testValue = 1;

            for(int i = 0; i < inputArray.Length; i++)
            {
                var currentElement = inputArray[i];

                if (testValue < currentElement)
                {
                    outputElements.Add(currentElement - testValue > 1 ? $"{testValue}-{currentElement - 1}" : $"{testValue}");
                }

                testValue = currentElement + 1;
            }

            if (testValue < 99)
            {
                outputElements.Add($"{testValue}-99");
            }

            else if (testValue == 99)
            {
                // last element in input array was 98
                outputElements.Add("99");
            }

            return string.Join(",", outputElements);
        }

        /// <summary>
        /// Checks that all elements in the array are in the range [1-99]
        /// </summary>
        /// <param name="array">bool indicating validity of input</param>
        /// <returns></returns>
        public bool ValidateArray(int[] array)
        {
            var currentValue = 0;
            foreach(var i in array)
            {
                if (i < 1 || i > 99)
                {
                    return false;
                }

                if (i < currentValue)
                {
                    return false;
                }

                currentValue = i;
            }

            return true;
        }
    }
}
