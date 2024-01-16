using LIBRARY;

namespace APP
{
    public class Program
    {
        // Максимов Тимофей Степанович, БПИ236-1, Вариант: 12
        
        // сделай проверку на null и поиграйся с файлом. напиши имя фамилию и тд. надо чтобы успешно считывался!!
        // коммы может добавить
        public static void Main()
        {
            /* try
            {
                Methods.CheсkString();
                Console.WriteLine("Файл считан успешно!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Это файл с некорректными данными!");
            }
            Store[] objects = JsonParser.ReadJson();
            foreach (Store obj in objects)
            {
                Console.WriteLine(obj);
                Console.WriteLine();
            } */
            
            Store[] objects = new Store[10000]; // создаю массив для большого количества объектов, так как пользователь
            // может добавлять их по мере работе программы
            int indexOfObjects = 0;
            bool mainFlag = true;
            do
            {
                Console.WriteLine("Укажите номер пункта меню для запуска действия:");
                Console.WriteLine("\t1. Ввести данные через консоль или предоставить путь к файлу для чтения данных");
                Console.WriteLine("\t2. Отфильтровать данные по одному из полей");
                Console.WriteLine("\t3. Отсортировать данные по одному из полей");
                Console.WriteLine("\t4. Ввести (сохранить) данные через консоль или файл");
                Console.WriteLine("\t5. Выйти из программы");
                string numberOfPoint = Console.ReadLine();
                switch (numberOfPoint)
                {
                    case "1":
                        // Methods.SpecializationMethod(slaceAllInformation, slaceMassivData, massivData[0]);
                        break;
                    case "2":
                        // Methods.ChiefPositionMethod(slaceAllInformation, slaceMassivData, massivData[0]);
                        break;
                    case "3":
                        // Methods.DistrictMethod(slaceAllInformation, slaceMassivData, massivData[0]);
                        break;
                    case "4":
                        // ChooseRecords(allInformation, massivData, ref slaceMassivData, ref slaceAllInformation);
                        break;
                    case "5":
                        mainFlag = false;
                        break;
                    default:
                        Console.WriteLine("Введенное значение может быть от 1 до 5, как выбор пункта для запуска действия, повторите попытку.");
                        break;
                }
            } while (mainFlag);
        }
    }

}