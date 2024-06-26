﻿using System.Text;

namespace LIBRARY
{
    public static class JsonParser
    {
        /// <summary>
        /// Состояния для парсинга json
        /// </summary>
        enum State
        {
            Field,
            ContentField,
            ContentFieldForId, // это специальное состояние для id, так как оно идет без кавычек, а как число
            ContentFiledForMassiv // состояние для полей с данными из массивов
        }

        /// <summary>
        /// Метод парсинга json файла
        /// </summary>
        /// <param name="allStrings"></param>
        /// <param name="objects"></param>
        /// <returns></returns>
        public static List<Store> ReadJson(string allStrings, List<Store> objects)
        {
            string[] information = { "", "", "" }; // сохранение информации для некоторых полей
            string[] employeesMassiv = new string[0]; // массив для всех сотрудников
            string[] productsMassiv = new string[0]; // массив для всех продуктов

            int indexOfLetters = 0; // индекс по проходу всего файла
            string nameField = "";
            string contentOfField = "";
            StringBuilder contentForMassiv = new StringBuilder(); // строим строку с данными, чтобы потом сделать массив

            State state = State.Field;
            while (indexOfLetters < allStrings.Length)
            {
                char symbol = allStrings[indexOfLetters];
                switch (state)
                {
                    case State.Field when symbol == '"': // считываем поле
                        indexOfLetters++;
                        StringBuilder field = new StringBuilder();
                        while (allStrings[indexOfLetters] != '"')
                        {
                            field.Append(allStrings[indexOfLetters]);
                            indexOfLetters++;
                        }

                        nameField = field.ToString(); // обновляем имя поля
                        indexOfLetters++;
                        if (nameField == "store_id")
                        {
                            state = State.ContentFieldForId;
                        }
                        else
                        {
                            state = State.ContentField;
                        }

                        break;
                    // считываем значение поля для int
                    case State.ContentFieldForId when symbol == ':':
                        // возможно может быть ошибка из-за \n or \r, но вроде нет
                        StringBuilder fieldContentId = new StringBuilder();
                        indexOfLetters++;
                        while (allStrings[indexOfLetters] != ',')
                        {
                            fieldContentId.Append(allStrings[indexOfLetters]);
                            indexOfLetters++;
                        }

                        indexOfLetters++;

                        contentOfField = fieldContentId.ToString().Trim(' ');
                        state = State.Field;
                        information[0] = contentOfField;
                        break;
                    // считываем поле string
                    case State.ContentField when symbol == '"':
                        StringBuilder fieldContent = new StringBuilder();
                        indexOfLetters++;
                        while (allStrings[indexOfLetters] != '"')
                        {
                            fieldContent.Append(allStrings[indexOfLetters]);
                            indexOfLetters++;
                        }

                        indexOfLetters++;

                        contentOfField = fieldContent.ToString();
                        state = State.Field;

                        if (nameField == "store_name")
                        {
                            information[1] = contentOfField;
                        }
                        else // значит это location
                        {
                            information[2] = contentOfField;
                        }

                        break;
                    // начало для массива
                    case State.ContentField when symbol == '[':
                        state = State.ContentFiledForMassiv;
                        indexOfLetters++;
                        break;
                    // считываем весь массив
                    case State.ContentFiledForMassiv when symbol == '"':
                        StringBuilder content = new StringBuilder();
                        indexOfLetters++;
                        while (allStrings[indexOfLetters] != '"')
                        {
                            content.Append(allStrings[indexOfLetters]);
                            indexOfLetters++;
                        }

                        indexOfLetters++;

                        contentForMassiv.Append(content);
                        contentForMassiv.Append(';');
                        break;
                    // считываем конец массива
                    case State.ContentFiledForMassiv when symbol == ']':
                        state = State.Field;
                        indexOfLetters++;
                        if (nameField == "employees")
                        {
                            employeesMassiv = contentForMassiv.ToString().Split(';')[..^1];
                            // обрезаем массив, так как при сплите самый последний элемент - пустота
                        }
                        else // значит это products
                        {
                            productsMassiv = contentForMassiv.ToString().Split(';')[..^1];
                        }

                        contentForMassiv = new StringBuilder(); // обнуляем
                        break;
                    // заканчиваем объект и добавляем его в класс
                    case State.Field when symbol == '}':
                        // и еще создать объект здесь надо, а также массив обнулить и все переменные обнулить, наверное
                        int id;
                        // если id не целый или null, или там лишние символы
                        if (!int.TryParse(information[0], out id))
                        {
                            id = 0; // или может здесь лучше исключение выкинуть
                        }

                        objects.Add(new Store(id, information[1], information[2], employeesMassiv,
                            productsMassiv));

                        // обнуляем все массивы для новых будущих объектов
                        information[0] = "";
                        information[1] = "";
                        information[2] = "";
                        employeesMassiv = new string[0];
                        productsMassiv = new string[0];
                        indexOfLetters++;
                        break;
                    // считываем запятую после значения поля
                    case State.ContentField when symbol == ',':
                        // вместо значения был null --> оставляем пустое значение в таком случае
                        state = State.Field;
                        indexOfLetters++;
                        break;
                    // если что проходим дальше
                    default:
                        indexOfLetters++;
                        break;
                }
            }

            return objects;
        }
        
        /// <summary>
        /// Метод для записи в файл
        /// </summary>
        /// <param name="objects">Список объектов</param>
        /// <param name="flagForFile">Использовать текущий файл? true - да, false - задать новый</param>
        public static void WriteJson(List<Store> objects, bool flagForFile)
        {
            while (true)
            {
                try
                {
                    string path = "";
                    if (!flagForFile) 
                    {
                        Console.WriteLine("Введите имя файла (разрешение не указывайте): ");
                        string name = Console.ReadLine();
                        while (name == null || name.Length == 0)
                        {
                            Console.WriteLine("Введите не null и не пустое значение");
                            name = Console.ReadLine();
                        }
                        path = "." + Path.DirectorySeparatorChar + name + ".json"; // назначение пути
                    }
                    else
                    {
                        path = "." + Path.DirectorySeparatorChar + Methods.CurrentFile + ".json";
                    }
                    if (File.Exists(path))
                    {
                        Console.WriteLine("Такой файл существует, данные в нем будут перезаписаны.");
                    }
                    // поток для ПЕРЕЗАПИСИ файла
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        Console.SetOut(sw);
                        Console.WriteLine("[");
                        for (int i = 0; i < objects.Count(); i++)
                        {
                            Console.Write(Methods.ObjectToStructure(objects[i]));
                            if (i != objects.Count() - 1)
                            {
                                Console.WriteLine(",");
                            }
                        }
                        Console.Write("\n]");
                    }
                    // перенаправляем поток и устанавливаем стандартную кодировку
                    var standardOutput = new StreamWriter(Console.OpenStandardOutput());
                    standardOutput.AutoFlush = true;
                    Console.SetOut(standardOutput);
                    Console.OutputEncoding = Encoding.UTF8;
                    Console.WriteLine("Данные записаны успешно!");
                    Thread.Sleep(2500);
                    break;
                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine(
                        "Файл отсутствует или его структура не соответствуют варианту/шаблону json. Повторите попытку: ");
                    flagForFile = false;
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Возникла ошибка при открытии файла, повторите попытку: ");
                    flagForFile = false;
                }
                catch (IOException)
                {
                    Console.WriteLine(
                        "Введено некорректное название файла или он находится не в текущей директории, повторите попытку: ");
                    flagForFile = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Возникла непредвиденная ошибка, повторите попытку: ");
                    flagForFile = false;
                }
            }
        }
        
        /// <summary>
        /// Перегрузка метода для записи в консоль
        /// </summary>
        /// <param name="objects"></param>
        public static void WriteJson(List<Store> objects)
        {
            Console.OpenStandardOutput();
            foreach (Store obj in objects)
            {
                Console.WriteLine(obj);
            }
            Console.WriteLine("Нажмите Enter, чтобы перейти обратно к меню...");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
        }
    }
}