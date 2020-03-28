using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pendleton.JsonSchemaValidator.Tests
{
    [TestClass]
    public class BasicTests
    {
        [TestMethod]
        public void Validates()
        {
            // Arrange
            var schema = @"{
    ""$schema"": ""http://json-schema.org/draft-07/schema#"",
    ""title"": ""A person"",
    ""properties"": {
        ""name"": {
            ""description"": ""Name"",
            ""type"": ""string""
        },
        ""age"": {
            ""description"": ""Age of the person"",
            ""type"": ""number"",
            ""minimum"": 2,
            ""maximum"": 200
        }
    },
    ""required"": [
                 ""name"",
                 ""age""
                 ],
    ""type"": ""object""
}";

            var json = @"{""name"": ""Albert"", ""age"": 42}";
            var validator = new JsonValidator(schema);

            // Act
            var result = validator.Validate(json, out _);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void FailsValidation()
        {
            // Arrange
            var schema = @"{
    ""$schema"": ""http://json-schema.org/draft-07/schema#"",
    ""title"": ""A person"",
    ""properties"": {
        ""name"": {
            ""description"": ""Name"",
            ""type"": ""string""
        },
        ""age"": {
            ""description"": ""Age of the person"",
            ""type"": ""number"",
            ""minimum"": 2,
            ""maximum"": 200
        }
    },
    ""required"": [
                 ""name"",
                 ""age""
                 ],
    ""type"": ""object""
}";

            var json = @"{""name"": ""Albert""}";
            var validator = new JsonValidator(schema);

            // Act
            var result = validator.Validate(json, out var errors);

            // Assert
            Assert.IsFalse(result);
            Assert.IsFalse(string.IsNullOrEmpty(errors));
        }
    }
}
