using System.Runtime.CompilerServices;
using System.Text;

namespace LIBRARY;

public static class Methods
{
    private enum State
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
    public static void CheсkString(string allStrings)
    {
        int indexOfLetters = 0; // индекс по проходу всего файла
        State state = State.Start; // состояние
        string nameField = "";
        
        while (indexOfLetters < allStrings.Length)
        {
            char symbol = allStrings[indexOfLetters];
            switch (symbol)
            {
                // декомпозировать код вот так?
                case '[' when state == State.Start: indexOfLetters++; state = State.Program; break;
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
                        throw new ArgumentNullException();
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
            throw new ArgumentNullException();
        }
    }

    public static void PreparationForReading(List<Store> objects)
    {
        Menu switchPreparation = new Menu(new[]
            { "\t1. Ввести данные через консоль", "\t2. Предоставить путь к файлу для чтения данных" },
            "Укажите, как вы бы хотели ввести данные:");
        switch (switchPreparation.ShowMenu())
        {
            case 1:
                ReadingThroughConsole(objects);
                break;
            case 2:
                ReadingThroughFile(objects);
                break;
        }
    }

    public static void ReadingThroughFile(List<Store> objects)
    {
        // абсолютный путь ? или просто имя файла? 
        
        Console.WriteLine("Введите имя файла (разрешение не указывайте): ");
        while (true)
        {
            try
            {
                string path = "." + Path.DirectorySeparatorChar + Console.ReadLine() + ".json"; // назначение пути
                string allStrings = File.ReadAllText(path);
                if (allStrings == null || allStrings.Length == 0) // если файл пустой или null выбрасываем исключение
                {
                    throw new ArgumentNullException();
                } 
                CheсkString(allStrings);
                JsonParser.ReadJson(allStrings, objects);
                Console.WriteLine("Файл успешно считан.");
                break;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("Файл отсутствует или его структура не соответствуют варианту/шаблону json. Повторите попытку: ");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Возникла ошибка при открытии файла, повторите попытку: ");
            }
            catch (IOException e)
            {
                Console.WriteLine("Введено некорректное название файла или он находится не в текущей директории, повторите попытку: ");
            }
            catch (Exception e)
            {
                Console.WriteLine("Возникла непредвиденная ошибка, повторите попытку: ");
            }
        }
    }

    public static void ReadingThroughConsole(List<Store> objects)
    {
        Console.WriteLine("Теперь произведите ввод по каждому объекту. После каждого объекта мы будем вас уведомлять о корректном вводе.");
        Console.WriteLine("Квадратные скобки не требуются, введите данные по объекту, как и в json файле с фигурными скобками.");
        Console.WriteLine("Вот так выглядит структура правильного ввода данных. Пожалуйста, соблюдайте ее, иначе ваши данные нельзя будет считать.");
        Console.WriteLine("{\n    \"store_id\": 3,\n    \"store_name\": \"English, Tran and Horne\",\n    \"location\": \"New Dustinview\",\n " +
                          "   \"employees\": [\n      \"Christopher Brewer\",\n      \"Jackie Reed\",\n      \"Cory Yates\",\n      \"Nicole Montoya\",\n      \"Stacy Hansen\",\n     " +
                          " \"Julie Garcia\",\n      \"Thomas Rose\",\n      \"Jeffrey Martin\"\n    ],\n    \"products\": [\n      \"especially\",\n      \"upon\",\n      \"when\",\n " +
                          "     \"before\",\n      \"various\",\n      \"entire\",\n      \"government\",\n      \"official\",\n      \"wide\",\n      \"boy\"\n    ]\n  }");
        // вот про это еще лучше у ассиста спросить!!
        Console.WriteLine("Когда объект будет введен нажмите \"CTRL\" + \"Z\"!!!");
        Console.WriteLine("Введите ваш объект (можно сразу несколько через запятую): ");
        bool flagForObject = true;
        
        while (flagForObject)
        {
            StringBuilder newObject = new StringBuilder("[");
            string line;
            do
            {
                line = Console.ReadLine();
                if (line != null)
                {
                    newObject.Append(line);
                }
            } while (line != null);
            newObject.Append("]");
            try
            {
                CheсkString(newObject.ToString());
                JsonParser.ReadJson(newObject.ToString(), objects);
                Console.WriteLine("Объект успешно считан и добавлен");
            }
            catch (Exception e)
            {
                Console.WriteLine("Введенный объект не соответствует структуре, повторите попытку: ");
                continue;
            }
            
            Menu switchWriter = new Menu(new[] { "\t1. Хочу добавить еще объектов", "\t2. Больше не хочу " +
                "добавлять объекты" }, "Хотите добавить еще объектов?");
            if (switchWriter.ShowMenu() == 1)
            {
                continue;
            }
            else
            {
                flagForObject = false;
            }
        }
    }

    public static void FilterList(List<Store> objects)
    {
        Menu switchFilter = new Menu(new[] {"\t1. store_id", "\t2. store_name", "\t3. location", "\t4. employees", "\t5. products" }, "По какому полю произвести фильтрацию?");
        switch (switchFilter.ShowMenu())
        {
            case 1:
                // FilterField("store_id", objects);
                break;
            case 2:
                // FilterField("store_name", objects);
                break;
            case 3:
                // FilterField("location", objects);
                break;
            case 4:
                // FilterField("employees", objects);
                break;
            case 5:
                // FilterField("products", objects);
                break;
        }
        
        foreach (Store obj in objects)
        {
            Console.WriteLine(obj);
            Console.WriteLine();
        }
        
        Console.WriteLine("Нажмите Enter, чтобы перейти обратно к меню...");
        Console.ReadLine();
    }
}