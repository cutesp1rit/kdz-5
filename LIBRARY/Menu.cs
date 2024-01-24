namespace LIBRARY;
// класс отвечает за оформление меню-switch
public class Menu
{
    private string[] _arrayCases; // массив с пунктами
    private string _quastion; // вопрос к ним

    public Menu(string[] arrayCases, string quastion)
    {
        _arrayCases = arrayCases;
        _quastion = quastion;
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
            Console.ForegroundColor = ConsoleColor.Magenta; // устанавливаем цвет
            Console.WriteLine(_quastion);
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
                case ConsoleKey.UpArrow: // стрелка вверх
                    indexOfColour--;
                    if (indexOfColour < 0)
                    {
                        indexOfColour = 0;
                    }

                    break;
                case ConsoleKey.DownArrow: // стрелка вниз
                    indexOfColour++;
                    if (indexOfColour >= lengthArray)
                    {
                        indexOfColour = lengthArray - 1;
                    }

                    break;
                case ConsoleKey.Enter: // произошел выбор
                    flagChoosing = false;
                    break;
            }
        }

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta; // устанавливаем цвет
        Console.Write("Вы выбрали: ");
        Console.WriteLine(_arrayCases[indexOfColour]);
        Console.ResetColor(); // сбрасываем в стандартный

        return indexOfColour + 1;
    }
}