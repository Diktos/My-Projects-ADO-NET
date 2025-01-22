
using System;
using System.IO;
using System.Net;
using System.Text;

// FTP (File Transfer Protocol) — це протокол для передавання(або обміну) файлів між КОМП'ЮТЕРАМИ в мережі.
// Він дозволяє завантажувати, вивантажувати та управляти файлами на віддаленому сервері.
// Зазвичай використовується для роботи з вебсайтами чи передавання даних.

// FTP працює на всіх популярних ОС (Windows, Linux, macOS).
// Однак сервери часто використовують Linux через стабільність і безпеку.

// FTP ще використовується в закритих мережах і для простих завдань,
// але для публічних серверів краще вибрати безпечніші протоколи.

// Замість нього частіше використовують SFTP (SSH File Transfer Protocol) або FTPS (FTP із шифруванням).
// SSH(Secure Shell) — це мережевий протокол для безпечного віддаленого доступу до СЕРВЕРІВ і передачі даних.

// SSH — це більше про безпечне керування та передачу, а FTP — про базовий обмін файлами, часто у великих обсягах.


// Визначаємо URL FTP сервера
string ftpServer = "ftp://ftp.gnu.org"; // FTP server URL
string directory = "/"; // Директорія для доступу на сервері
string userName = "anonymous"; // Ім'я користувача для серверу, логін
string password = ""; // Пароль (пустий для анонімного доступу), пароль

// Створюємо запит для FTP
FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(ftpServer + directory);
ftpWebRequest.Credentials = new NetworkCredential(userName, password); // Вказуємо облікові дані
ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails; // Метод для отримання деталей списку файлів

try
{
    // Створюємо масив для отримання даних
    var data = new byte[8000];
    FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse(); // Отримуємо відповідь від сервера
    int bytes = ftpWebResponse.GetResponseStream().Read(data, 0, data.Length); // Читаємо дані з потоку
    ftpWebResponse.Close();

    Console.WriteLine(Encoding.UTF8.GetString(data, 0, bytes)); // Виводимо список файлів на консоль

    string directoryListing = Encoding.UTF8.GetString(data, 0, bytes); // Конвертуємо отримані дані в рядок
    string[] lines = directoryListing.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries); // Розділяємо рядки на окремі частини(файли)

    foreach (var line in lines)
    {
        if (line.StartsWith("-")) // Перевіряємо, чи є це файлом, а не лінк і тд
        {
            var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); // Розділяємо інформацію про файл на окремі частини
            string fileName = parts[parts.Length - 1]; // Останній елемент - це ім'я файлу

            Console.WriteLine($"Found file: {fileName}"); // Виводимо ім'я файлу

            // Створюємо запит для завантаження файлу
            var fileRequest = (FtpWebRequest)WebRequest.Create(ftpServer + "/" + fileName);
            fileRequest.Credentials = new NetworkCredential(userName, password);
            fileRequest.Method = WebRequestMethods.Ftp.DownloadFile; // Метод для завантаження файлу

            try
            {
                // Створюємо масив для отримання файлу
                var fileData = new byte[80000];
                FtpWebResponse fileResponse = (FtpWebResponse)fileRequest.GetResponse();
                int fileBytes = fileResponse.GetResponseStream().Read(fileData, 0, fileData.Length); // Читаємо файл

                // Вказуємо шлях для збереження файлу
                string filePath = Path.Combine("C:\\Users\\Danylo\\Desktop\\My_Project_ADO_NET\\FTP(File Transfer Protocol) Client\\bin\\Debug\\net8.0", fileName);
                File.WriteAllBytes(filePath, fileData.Take(fileBytes).ToArray()); // Зберігаємо файл на диск

                fileResponse.Close();

                Console.WriteLine($"File {fileName} downloaded and saved to {filePath}"); // Виводимо повідомлення про завершення операції в позитивному значені

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading file {fileName}: {ex.Message}"); // Виводимо помилку, якщо не вдалося завантажити файл
            }
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error getting file list: {ex.Message}"); // Виводимо помилку, якщо не вдалося отримати список файлів
}
