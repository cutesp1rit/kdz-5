namespace LIBRARY;

public class Menu
{
    private string[] _arrayCases;

    public Menu(string[] arrayCases)
    {
        _arrayCases = arrayCases;
    }

    /// <summary>
    /// возвращает пункт выбранного меню
    /// </summary>
    /// <returns></returns>
    public int ShowMenu()
    {
        int lengthArray = _arrayCases.Length;
        bool flagChoosing = true;
        int indexOfColour = 0; // изначально первая строка будет показываться как выборочной

        while (flagChoosing)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green; // устанавливаем цвет
            Console.WriteLine("С помощью перемещения стрелками вверх и вниз выберите подходящий вам пункт.");
            Console.WriteLine("Когда выберете подходящий пункт, нажмите enter.");
            Console.ResetColor(); // сбрасываем в стандартный
            for (int i = 0; i < indexOfColour; i++)
            {
                Console.WriteLine(_arrayCases[i]);
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow; // устанавливаем цвет
            Console.WriteLine(_arrayCases[indexOfColour]);
            Console.ResetColor(); // сбрасываем в стандартный
            for (int i = indexOfColour + 1; i < lengthArray; i++)
            {
                Console.WriteLine(_arrayCases[i]);
            }
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.UpArrow:
                    indexOfColour--;
                    if (indexOfColour < 0)
                    {
                        indexOfColour = 0;
                    }

                    break;
                case ConsoleKey.DownArrow:
                    indexOfColour++;
                    if (indexOfColour >= lengthArray)
                    {
                        indexOfColour = lengthArray - 1;
                    }

                    break;
                case ConsoleKey.Enter:
                    flagChoosing = false;
                    break;
            }
        }

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta; // устанавливаем цвет
        Console.Write("Вы выбрали: ");
        Console.WriteLine(_arrayCases[indexOfColour]);
        Console.ResetColor(); // сбрасываем в стандартный

        return indexOfColour;
    }
}