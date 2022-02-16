using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoComplete
{
    [TestFixture]
    public class AutoCompleteTests
    {
        [Test]
        public void Basic()
        {
            var autocompleter = new AutoCompleter();
            autocompleter.AddToSearch(new List<FullName> {
                new FullName { Name = "Анастасия" },
                new FullName { Name = "Анна" },
                new FullName { Name = "Богдан" },
                new FullName { Name = "Боря" } 
            });

            var expected = new List<string> { "Анастасия", "Анна" };
            var result = autocompleter.Search("А");
            for (var i = 0; i < result.Count; i++)
                Assert.AreEqual(expected[i], result[i]);
        }


    }
}
