int result = 0;

foreach (var arg in args)
{
    if (int.TryParse(arg, out int number)) // Перевіряємо чи можна перетворити аргумент на число
    {
        result += number; 
    }
}
return result;
