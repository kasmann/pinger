using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace pinger
{
    class Program
    {
        static void Main(string[] args)
        {
            WebRequest request = WebRequest.Create("http://ya.ru");
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    Console.WriteLine(reader.ReadToEnd());
                }
            }
            response.Close();
            Console.Read();
        }
    }

    class Pinger
    {
        public PingAnswer Answer;
        public void send() { }
        //отправляем на указанный адрес указанный пакет
    }

    class Logger
    {
        //пишем в лог
    }

    class PingAnswer
    {
        //получаем ответ запрашиваемого ресурса
        //умеем его парсить
    }

    class Query
    {
        //формируем пакет по типу протокола
    }

    class Configuration
    {
        //чтение файла конфигурации, парсинг нужных значений
        //если в строке больше одного значения, то предупреждаем, но продолжаем работать с первым
    }
}
