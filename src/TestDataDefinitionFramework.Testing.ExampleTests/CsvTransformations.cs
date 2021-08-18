using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace TestDataDefinitionFramework.Testing.ExampleTests
{
    [Binding]
    public class CsvTransformations
    {
        [StepArgumentTransformation]
        public IReadOnlyList<string> CsvListToStringArray(string csvList)
        {
            return csvList.Split(",")
                .Select(s => s.Trim())
                .ToArray();
        }
    }
}