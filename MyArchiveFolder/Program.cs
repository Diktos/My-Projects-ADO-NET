using System.Diagnostics;

bool ArchiveFolder(string folder, string resultFile)
{
    var proc = new Process();
    string script = $"tar -cvjf {resultFile} {folder}";
    proc.StartInfo.FileName = "cmd.exe";
    proc.StartInfo.Arguments = $"/C {script}";
    /*+".tar.bz2"*/
    proc.StartInfo.WorkingDirectory = @"C:\Users\Danylo\Desktop\My Project ADO NET\MyArchiveFolder";

    proc.Start();
    proc.WaitForExit();
    return proc.ExitCode == 0;
}
// Процес - окрема програма в окремому просторі і запускається окремо


// Usage: MyArchiveFolder <folder> <resultFile>

// MyArchiveFolder\bin\Debug\net8.0\MyArchiveFolder.exe <folder> <resultFile>

string nameFile = args[0];
string resultFile = args[1];
// cd MyArchiveFolder_тест_завдання
// MyArchiveFolder.exe SchoolDiary ArchiveFile
ArchiveFolder(@"C:\Users\Danylo\Desktop\My Project ADO NET\MyArchiveFolder\" + nameFile, resultFile + ".tar.bz2");


