# Pendleton.JsonSchemaValidator
This horrible thing is a .Net Standard 2.1 wrapper around [my fork](https://github.com/evman182/json-schema-validator) of a much better project: https://github.com/pboettch/json-schema-validator  
  
This should work on Windows and Linux right now. I'll work on Mac eventually.  
  
Right now the validator can't handle schemas containing references to schemas in extneral files or urls. Otherwise, it passes all the other non-optional draft7 tests.
  
Any other issues with the dotnet wrapper you should bring to me. Any issues with Json Schema validation, I'll do my best to help but it's not my area.
  
I'll write an actual readme and setup CI badges and stuff when I have some time.

Nuget package available. Use at your own risk: https://www.nuget.org/packages/Pendleton.JsonSchemaValidator/
