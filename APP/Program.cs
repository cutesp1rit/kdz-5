using LIBRARY;

namespace APP
{
    public class Program
    {
        // Максимов Тимофей Степанович, БПИ236-1, Вариант: 12
        
        // сделай проверку на null и поиграйся с файлом. напиши имя фамилию и тд. надо чтобы успешно считывался!!
        // коммы может добавить
        // проверить, что со списками все норм
        public static void Main()
        {
            Menu switchTest = new Menu(new[] {"\t1. Ввести данные через консоль или предоставить путь к файлу " +
                                              "для чтения данных", "\t2. Отфильтровать данные по одному из полей", "\t3. " +
                "Отсортировать данные по одному из полей",  "\t4. Ввести (сохранить) данные через консоль или файл", 
                "\t5. Выйти из программы"});
            Console.WriteLine(switchTest.ShowMenu() + 1);
            
            List<Store> objects = new List<Store>(150); // список для объектов Store
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