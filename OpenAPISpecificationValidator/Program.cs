using Microsoft.OpenApi.Readers;

class Program
{
    static void Main(string[] args)
    {
        string solutionRoot = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName ?? string.Empty;
        string specsFolder = Path.Combine(solutionRoot, "specs");

        if (!Directory.Exists(specsFolder))
        {
            Console.WriteLine($"The folder '{specsFolder}' does not exist.");
            return;
        }

        //Validate Json Files -- should fail 1 file and pass 1 file
        var jsonFiles = Directory.GetFiles(specsFolder, "*.json");

        if (jsonFiles.Length == 0)
        {
            Console.WriteLine("No .json files found in the 'specs' folder.");
            return;
        }

        foreach (var file in jsonFiles)
        {
            Console.WriteLine($"Validating file: {Path.GetFileName(file)}");

            using var stream = new FileStream(file, FileMode.Open, FileAccess.Read);
            var openApiReader = new OpenApiStreamReader();
            var openApiDocument = openApiReader.Read(stream, out var diagnostic);

            if (diagnostic.Errors.Count > 0)
            {
                Console.WriteLine($"Validation failed for {Path.GetFileName(file)}:");
                foreach (var error in diagnostic.Errors)
                {
                    Console.WriteLine($"- {error.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Validation succeeded for {Path.GetFileName(file)}.");
            }
        }

        //Validate Yaml Files -- should pass 2 files
        var yamlFiles = Directory.GetFiles(specsFolder, "*.yaml");

        if (yamlFiles.Length == 0)
        {
            Console.WriteLine("No .yaml files found in the 'specs' folder.");
            return;
        }

        foreach (var file in yamlFiles)
        {
            Console.WriteLine($"Validating file: {Path.GetFileName(file)}");

            using var stream = new FileStream(file, FileMode.Open, FileAccess.Read);
            var openApiReader = new OpenApiStreamReader();
            var openApiDocument = openApiReader.Read(stream, out var diagnostic);

            if (diagnostic.Errors.Count > 0)
            {
                Console.WriteLine($"Validation failed for {Path.GetFileName(file)}:");
                foreach (var error in diagnostic.Errors)
                {
                    Console.WriteLine($"- {error.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Validation succeeded for {Path.GetFileName(file)}.");
            }
        }
    }
}
