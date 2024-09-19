namespace Claudinessa.Services
{
    public class PhotoService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _photoDirectory;

        public PhotoService(IWebHostEnvironment environment)
        {
            _environment = environment;
            _photoDirectory = Path.Combine(_environment.WebRootPath, "Uploads");
        }

        public string SavePhotoFromBase64(string base64Image, string name)
        {
            if (string.IsNullOrEmpty(base64Image))
            {
                throw new Exception("Cadena Base64 no válida.");
            }

            try
            {
                // Convierte la cadena Base64 en bytes
                byte[] imageBytes = Convert.FromBase64String(base64Image);

                // Crea la ruta completa del archivo
                var fileName = name + ".png";
                var filePath = Path.Combine(_photoDirectory, fileName);

                // Guarda el archivo en el directorio
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    stream.Write(imageBytes, 0, imageBytes.Length);
                }

                return filePath;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar la imagen: " + ex.Message);
            }
        }
    }
}
