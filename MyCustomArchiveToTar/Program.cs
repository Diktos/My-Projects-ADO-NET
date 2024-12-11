using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

bool ArchiveFolder(string folder, string resultFile)
{
    var proc = new Process();
    //string script = $"tar -cvjf {resultFile} {folder}";  // з + bz2 варіант
    string script = $"tar -cvf {resultFile} {folder}";
    proc.StartInfo.FileName = "cmd.exe";
    proc.StartInfo.Arguments = $"/C {script}";
    /*+".tar.bz2"*/
    proc.StartInfo.WorkingDirectory = @"C:\Users\Danylo\Desktop\My_Project_ADO_NET\MyCustomArchiveToTar";

    proc.Start();
    proc.WaitForExit();
    return proc.ExitCode == 0;
}
// Процес - окрема програма в окремому просторі і запускається окремо

// Usage: MyCustomArchiveToTar <folder> <resultFile>

// MyCustomArchiveToTar\bin\Debug\net8.0\MyCustomArchiveToTar.exe <folder> <resultFile>

string nameFile = args[0];
string resultFile = args[1];

//    C:\Users\Danylo\Desktop\My_Project_ADO_NET\MyCustomArchiveToTar\bin\Debug\net8.0\MyCustomArchiveToTar.exe SchoolDiary ArchiveFile          - команда

//ArchiveFolder(@"C:\Users\Danylo\Desktop\My_Project_ADO_NET\" + nameFile, resultFile + ".tar.bz2"); // з + bz2 варіант
ArchiveFolder(@"C:\Users\Danylo\Desktop\My_Project_ADO_NET\" + nameFile, resultFile + ".tar");

Console.WriteLine("Good job!!!");