using System.Diagnostics;
using System.Security.Cryptography;

class Program
{
    static void Main()
    {

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        directorio();

        stopwatch.Stop();
        Console.WriteLine($"Tiempo Sincrono de CPU: {stopwatch.Elapsed}");

        stopwatch = new Stopwatch();
        stopwatch.Start();

        directorio(0);

        stopwatch.Stop();
        Console.WriteLine($"Tiempo de CPU en Paralelo: {stopwatch.Elapsed}");

    }

    public static void directorio(int sincrono = 1)
    {
        string carpeta = @"D:\Prueba"; 

        try
        {
          
            string[] archivos = Directory.GetFiles(carpeta);

            Console.WriteLine("Archivos en la carpeta:");
            if (sincrono == 1)
            {
                foreach (string archivo in archivos)
                {
                    calcula_md5(archivo);
                }
            }
            else
            {
                Parallel.ForEach(archivos, archivo =>
                {
                    calcula_md5(archivo);
                });
            }
        }
        catch (DirectoryNotFoundException)
        {
            Console.WriteLine("La carpeta no se encontró.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocurrió un error: {ex.Message}");
        }
    }

    public static void calcula_md5(string filePath)
    {      
        try
        {
            string md5Hash = GetMD5HashFromFile(filePath);
            Console.WriteLine($"MD5 del archivo {filePath}: {md5Hash}");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("El archivo no se encontró.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocurrió un error: {ex.Message}");
        }
    }
    static string GetMD5HashFromFile(string filePath)
    {
        using (var md5 = MD5.Create())
        {
            using (var stream = File.OpenRead(filePath))
            {
                byte[] hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}

