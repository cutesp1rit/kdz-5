using System.Text;

namespace LIBRARY;

public static class Methods
{
    enum State
    {
        Start, // самое начальное состояние перед квадратной скобкой
        Program, // состояние, когда мы еще не зашли в определенный объект, то есть не прочитали фигурные скобки
        Object,
        Field,
        ContentFieldString,
        ContentFieldInt, // это специальное состояние для id, так как оно идет без кавычек, а как число
        ContentFieldMassiv, // состояние для полей с данными из массивов
        ContentFieldMassivString,
        ContentFieldMassivComma,
        Comma, // состояние, когда мы ждем запятаю (то есть окончание описания одного из полей)
        BetweenObjects,
        Finish
    }
    
    // считаю важным упомянуть, что долго рассуждал над тем, как конкретно реализовать проверку файла.
    // при написании кода все больше и больше мыслей появлялось по поводу еще больших ограничений.
    // по началу мне казалось, что достаочно будет соблюдать лишь структуру и проверять ее. но 
    // с учетом того, что относительно ключей могут быть разные значения, я пришел к выводу, что и каждый ключ в 
    // соответствии с моим файлом-примером нужно проверять на корректное значение, так как метод парсинга (который
    // позже переносится в массив объектов подстроенного класса) разработан именно под конкретные значения (иначе бы
    // было невозможно создавать объекты для класса требуемого в кдз). 
    // поэтому я опираюсь на то, что напротив store_id не может быть массива строк, а напротив location не может
    // быть интовского значения. если такое происходит -- файл некорректный
    public static void CheсkString()
    {
        // Поработать с исключениями, подключить подходящие и информативные, а не одни и те же
        string allStrings = File.ReadAllText("data_12V.json");
        if (allStrings==null || allStrings.Length == 0) // если файл пустой или null выбрасываем исключение
        {
            throw new ArgumentNullException();
        }

        int indexOfLetters = 0; // индекс по проходу всего файла
        State state = State.Start; // состояние
        string nameField = "";
        
        while (indexOfLetters < allStrings.Length)
        {
            char symbol = allStrings[indexOfLetters];
            switch (symbol)
            {
                case '[' when state == State.Start:
                    indexOfLetters++;
                    state = State.Program;
                    break;
                case '\n' or ' ' or '\r':
                    // в таком случае файл не является некорретным, поэтому просто пропускаем такие символы
                    indexOfLetters++;
                    break;
                case '{' when state == State.Program:
                    state = State.Object;
                    indexOfLetters++;
                    break;
                case '"' when state == State.Object:
                    state = State.Field;
                    indexOfLetters++;
                    StringBuilder tmp = new StringBuilder();
                    while (allStrings[indexOfLetters] != '"')
                    {
                        tmp.Append(allStrings[indexOfLetters]);
                        indexOfLetters++;
                    }

                    nameField = tmp.ToString();
                    indexOfLetters++;
                    break;
                case ':' when state == State.Field:
                    if (nameField == "store_id")
                    {
                        state = State.ContentFieldInt;
                    }
                    if (nameField == "store_name" || nameField == "location")
                    {
                        state = State.ContentFieldString;
                    }
                    if (nameField == "employees" || nameField == "products")
                    {
                        state = State.ContentFieldMassiv;
                    }
                    // если state не изменился, значит название поля неверное
                    if (state == State.Field)
                    {
                        throw new ArgumentException();
                    }
                    indexOfLetters++;
                    break;
                case char n when ( '0' <=  n &&  n <=  '9') && state == State.ContentFieldInt:
                    // такая запись приемлема
                    indexOfLetters++;
                    while ('0' <= allStrings[indexOfLetters] && allStrings[indexOfLetters] <= '9')
                    {
                        indexOfLetters++;
                    }
                    state = State.Comma;
                    break;
                case 'n' when (state == State.ContentFieldInt || state == State.ContentFieldMassiv || state == State.ContentFieldString) 
                              && allStrings[indexOfLetters+1] == 'u' && allStrings[indexOfLetters+2] == 'l' && allStrings[indexOfLetters+3] == 'l':
                    // такой исход тоже приемлем для всех трех таких состояний 
                    state = State.Comma;
                    indexOfLetters += 4;
                    break;
                case '"' when state == State.ContentFieldString:
                    indexOfLetters++;
                    while (allStrings[indexOfLetters] != '"')
                    {
                        indexOfLetters++;
                    }
                    indexOfLetters++;
                    state = State.Comma;
                    break;
                case '[' when state == State.ContentFieldMassiv:
                    indexOfLetters++;
                    state = State.ContentFieldMassivString;
                    break;
                case '"' when state == State.ContentFieldMassivString:
                    indexOfLetters++;
                    while (allStrings[indexOfLetters] != '"')
                    {
                        indexOfLetters++;
                    }
                    indexOfLetters++;
                    state = State.ContentFieldMassivComma;
                    break;
                case ',' when state == State.ContentFieldMassivComma:
                    indexOfLetters++;
                    state = State.ContentFieldMassivString;
                    break;
                case ']' when state == State.ContentFieldMassivComma:
                    indexOfLetters++;
                    state = State.Comma;
                    break;
                case ',' when state == State.Comma:
                    indexOfLetters++;
                    state = State.Object;
                    break;
                case '}' when state == State.Comma:
                    indexOfLetters++;
                    state = State.BetweenObjects;
                    break;
                case ',' when state == State.BetweenObjects:
                    indexOfLetters++;
                    state = State.Program;
                    break;
                case ']' when state == State.BetweenObjects:
                    indexOfLetters++;
                    state = State.Finish;
                    break;
                default:
                    // если попадают некорретные символы файла, неудовлетворяющие структуре, то выбрасываем исключение
                    throw new ArgumentNullException();
            }
        }
        if (state != State.Finish)
        {
            throw new AggregateException();
        }
    }

    public static void PreparationForReading()
    {
        bool switchFlag = true;
        do
        {
            Console.WriteLine("Укажите, как вы бы хотели ввести данные:");
            Console.WriteLine("\t1. Через консоль");
            Console.WriteLine("\t2. Предоставить путь к файлу для чтения данных");
            string numberOfPoint = Console.ReadLine();
            switch (numberOfPoint)
            {
                case "1":
                    ReadingThroughFile();
                    switchFlag = false;
                    break;
                case "2":
                    ReadingThroughConsole();
                    switchFlag = false;
                    break;
                default:
                    Console.WriteLine("Введенное значение может быть от 1 до 2, как выбор пункта для запуска действия, повторите попытку.");
                    break;
            }
        } while (switchFlag);
    }

    public static void ReadingThroughFile()
    {
        
    }

    public static void ReadingThroughConsole()
    {
        
    }
}