using LIBRARY;

namespace APP
{
    public class Program
    {
        // Максимов Тимофей Степанович, БПИ236-1, Вариант: 12
        
        // сделай проверку на null и поиграйся с файлом. напиши имя фамилию и тд. надо чтобы успешно считывался!!
        // коммы может добавить
        // проверить, что со списками все норм
        // оформить ToString в Menu
        // должно быть поле по которму я сортирую/фильтрую
        // а у тебя там файл не считывается, чек тг 
        // записывать ли null, если у нас пустое значение? да пофиг? 
        // дозапись в файл, а потом считывание
        
        public static void Main()
        {
            
            List<Store> objects = new List<Store>(150); // список для объектов Store
            // сначала запрашиваем у пользователя хотя бы какие-то объекты
            Methods.PreparationForReading(objects);
            bool mainFlag = true;
            do
            {
                Menu switchMain = new Menu(new[] {"\t1. Ввести данные через консоль или предоставить путь к файлу " +
                                                  "для чтения данных", "\t2. Отфильтровать данные по одному из полей", "\t3. " +
                    "Отсортировать данные по одному из полей",  "\t4. Ввести (сохранить) данные через консоль или файл", 
                    "\t5. Выйти из программы"}, "Чтобы вы хотели сделать?");
                switch (switchMain.ShowMenu())
                {
                    case 1:
                        Methods.PreparationForReading(objects);
                        break;
                    case 2:
                        Methods.FilterList(objects);
                        break;
                    case 3:
                        Methods.SortList(objects);
                        break;
                    case 4:
                        Methods.WriteList(objects);
                        break;
                    case 5:
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