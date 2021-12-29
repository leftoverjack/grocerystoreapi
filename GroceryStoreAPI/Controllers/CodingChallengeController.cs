using System;
using GroceryStoreAPI.Providers;
using Microsoft.AspNetCore.Mvc;

namespace GroceryStoreAPI.Controllers
{
    [Route("api/codingchallenge")]
    [ApiController]
    public class CodingChallengeController : ControllerBase
    {
        private readonly ICodingChallengeProvider _codingChallengeProvider;

        public CodingChallengeController(ICodingChallengeProvider codingChallengeProvider)
        {
            if (codingChallengeProvider == null)
            {
                throw new ArgumentNullException(nameof(codingChallengeProvider));
            }

            _codingChallengeProvider = codingChallengeProvider;
        }

        /// <summary>
        /// Takes an input array and returns a string containing all the elements between 1 and 99 that are missing from the array
        /// </summary>
        /// <param name="inputArray">a sorted array containing some or all of the integers between 1 and 99 inclusive</param>
        /// <returns>a string containing all the integers between 1 and 99 not present in the input array</returns>
        [HttpPost]
        public ActionResult<string> GetMissingElements(int[] inputArray)
        {
            if (inputArray == null)
            {
                return Ok("1-99");
            }

            if (inputArray.Length == 0)
            {
                return Ok("1-99");
            }

            if (!_codingChallengeProvider.ValidateArray(inputArray))
            {
                return BadRequest("input array is invalid");
            }

            var result = _codingChallengeProvider.GetMissingArrayElements(inputArray);

            return Ok(result);
        }
    }
}