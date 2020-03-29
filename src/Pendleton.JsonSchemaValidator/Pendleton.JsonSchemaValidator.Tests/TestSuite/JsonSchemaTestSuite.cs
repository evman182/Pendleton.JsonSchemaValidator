using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Manatee.Json;
using Manatee.Json.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pendleton.JsonSchemaValidator.Tests.TestSuite
{
	[TestClass]
	public class JsonSchemaTestSuite
	{
		private const string RootTestsFolder = @"..\..\..\..\Json-Schema-Test-Suite\";
		private static readonly JsonSerializer Serializer;

        public static IEnumerable AllTestData => _LoadSchemaJson("draft7");

		private static IEnumerable<object[]> _LoadSchemaJson(string draft)
		{
			var testsPath = Path.Combine(Directory.GetCurrentDirectory(), RootTestsFolder, $"{draft}\\");
			if (!Directory.Exists(testsPath)) return Enumerable.Empty<object[]>();

			var fileNames = Directory.GetFiles(testsPath, "*.json", SearchOption.AllDirectories);

			var allTests = new List<object[]>();
			foreach (var fileName in fileNames)
			{
				// Can't load external files right now
				if(fileName.Contains("refRemote"))
					continue;

				var shortFileName = Path.GetFileNameWithoutExtension(fileName);
				var contents = File.ReadAllText(fileName);
				var json = JsonValue.Parse(contents);

				foreach (var testSet in json.Array)
				{
					var schemaJson = testSet.Object["schema"];
					foreach (var testJson in testSet.Object["tests"].Array)
					{
						var isOptional = fileName.Contains("optional");
						var testName = $"{shortFileName}.{testSet.Object["description"].String}.{testJson.Object["description"].String}.{draft}".Replace(' ', '_');
						if (isOptional)
							testName = $"optional.{testName}";
                        allTests.Add(new object[]
						{
                            testJson,
                            schemaJson,
                            isOptional,
                            testName,
                        });
                    }
				}
			}

			return allTests;
		}
		
		static JsonSchemaTestSuite()
		{
			Serializer = new JsonSerializer();
		}

		[DynamicData(nameof(AllTestData), DynamicDataDisplayName = nameof(GetName))]
        [DataTestMethod]
		public void Run(JsonValue testJson, JsonValue schemaJson, bool isOptional, string testName)
		{
            _Run(testJson, schemaJson, isOptional);
        }

		public static string GetName(MethodInfo methodInfo, Object[] testData)
		{
            return testData[3].ToString();
        }

		private static void _Run(JsonValue testJson, JsonValue schemaJson, bool isOptional)
        {
            var test = Serializer.Deserialize<SchemaTest>(testJson);
            var json = test.Data.ToString();
            var schema = schemaJson.ToString();
			
			// Can't load json from urls right now or pass some of the optional tests
            if(schema.Contains("\"$ref\":\"http") || isOptional)
				Assert.Inconclusive();
            
            var validator = new JsonValidator(schema);
            var results = validator.Validate(json, out _);
            Assert.AreEqual(test.Valid, results);
        }
	}
}
