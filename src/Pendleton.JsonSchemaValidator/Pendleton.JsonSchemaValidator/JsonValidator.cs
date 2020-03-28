using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using NativeLibraryManager;

namespace Pendleton.JsonSchemaValidator
{
    public class JsonValidator
    {
        [DllImport("nlohmann_json_schema_validator")]
        private static extern IntPtr NewJsonValidator(string schema);

        [DllImport("nlohmann_json_schema_validator")]
        private static extern void DeleteJsonValidator(IntPtr validator);

        [DllImport("nlohmann_json_schema_validator")]
        private static extern bool Validate(IntPtr validator, string json, StringBuilder errors);

        static JsonValidator()
        {
            var accessor = new ResourceAccessor(Assembly.GetExecutingAssembly());
            var libManager = new LibraryManager(
                Assembly.GetExecutingAssembly(),
                new LibraryItem(Platform.Windows, Bitness.x64,
                    new LibraryFile("nlohmann_json_schema_validator.dll",
                        accessor.Binary("nlohmann_json_schema_validator.dll"))));

            libManager.LoadNativeLibrary();
        }

        private readonly IntPtr _validator;

        public JsonValidator(string schema)
        {
            _validator = NewJsonValidator(schema);
        }

        public bool Validate(string json, out string errors)
        {
            var errorBuffer = new StringBuilder(100);
            bool result = Validate(_validator, json, errorBuffer);
            errors = errorBuffer.ToString();
            return result;
        }

        ~JsonValidator()
        {
            DeleteJsonValidator(_validator);
        }
    }
}
