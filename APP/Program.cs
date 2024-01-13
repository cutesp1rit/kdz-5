using LIBRARY;

namespace APP
{
    public class Program
    {
        // сделай проверку на null и поиграйся с файлом. напиши имя фамилию и тд. надо чтобы успешно считывался!!
        // коммы может добавить
        public static void Main()
        {
            Store[] objects = JsonParser.ReadJson();
            foreach (Store obj in objects)
            {
                Console.WriteLine(obj);
                Console.WriteLine();
            }
        }
    }

}