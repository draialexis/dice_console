using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using Xunit;

namespace Tests.Utils_UTs
{
    public class EnumerablesTest
    {
        [Fact]
        public void TestFeedListsToDict()
        {
            // Arrange
            string str1 = "blah";
            string str2 = "blahblah";
            string str3 = "azfyoaz";

            int int1 = 5;
            int int2 = 12;
            int int3 = 3;

            Dictionary<string, int> expected = new()
            {
                { str1, int1 },
                { str2, int2 },
                { str3, int3 }
            };

            List<string> strings = new() { str2, str3 };
            List<int> ints = new() { int2, int3 };

            Dictionary<string, int> actual = new()
            {
                {str1, int1 }
            }; // we will add on top of this

            // Act

            actual = Enumerables.FeedListsToDict(actual, strings, ints);
            // Assert

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestGetDictFromLists()
        {
            // Arrange
            string str1 = "blah";
            string str2 = "blahblah";

            int int1 = 5;
            int int2 = 12;

            Dictionary<string, int> expected = new()
            {
                { str1, int1 },
                { str2, int2 }
            };

            List<string> strings = new() { str1, str2 };
            List<int> ints = new() { int1, int2 };

            // Act

            Dictionary<string, int> actual = Enumerables.GetDictFromLists(strings, ints);
            // Assert

            Assert.Equal(expected, actual);
        }
    }
}
