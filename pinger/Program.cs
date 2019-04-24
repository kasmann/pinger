using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace pinger
{
    class Program
    {
        static void Main(string[] args)
        {
            Configuration config = new Configuration(@"d:\settings.xml");
            if (config.IsValidated)
                config.ReadXMLFile();

            Console.WriteLine("it's ok");
            
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
        private readonly XmlDocument configFile = new XmlDocument();
        public bool IsValidated;
        public Uri URL;
        public string protocol;
        public int validHTTPcode;
        public int period;
        public string logfilePath;

        public Configuration(string path)
        {
            configFile.Load(path);
            ValidateXML();
        }

        public void ValidateXML()
        {
            IsValidated = true;
            XmlElement xRoot = configFile.DocumentElement;

            if (!xRoot.HasChildNodes)
            {
                IsValidated = false;
                throw new ConfigurationException("XML-документ конфигурации не содержит данных.");
            }

            if (xRoot.SelectSingleNode("//settings/URL") == null)
            {
                IsValidated = false;
                throw new ConfigurationException("XML-документ конфигурации не содержит параметра \"URL\".");
            }
            else
            {
                string _URL = xRoot.SelectSingleNode("//settings/URL").InnerText;
                try
                {
                    Uri _URIfromURL = new Uri(_URL);
                }
                catch (UriFormatException)
                {
                    IsValidated = false;
                    throw new ConfigurationException("Неверный формат URL.\n" + _URL);
                }
            }

            if (xRoot.SelectSingleNode("//settings/protocol") == null)
            {
                IsValidated = false;
                throw new ConfigurationException("XML-документ конфигурации не содержит параметра \"protocol\".");
            }
            else
            {
                string _protocol = xRoot.SelectSingleNode("//settings/protocol").InnerText;
                switch (_protocol)
                {
                    case "ICMP": break;
                    case "TCP": break;
                    case "HTTP":
                        {
                            if (xRoot.SelectSingleNode("//settings/validHTTPcode") == null)
                            {
                                IsValidated = false;
                                throw new ConfigurationException("XML-документ конфигурации не содержит параметра \"validHTTPcode\".");
                            }
                            else
                                try
                                {
                                    Convert.ToUInt16(xRoot.SelectSingleNode("//settings/validHTTPcode").InnerText);
                                }
                                catch (InvalidCastException)
                                {
                                    IsValidated = false;
                                    throw new ConfigurationException("Неверный формат параметра \"validHTTPcode\".");
                                }
                            break;
                        }
                    default:
                        {
                            IsValidated = false;
                            throw new ConfigurationException("Протокол \"" + _protocol + "\" не поддерживается.");
                        }
                }
            }

            if (xRoot.SelectSingleNode("//settings/period") == null)
            {
                IsValidated = false;
                throw new ConfigurationException("XML-документ конфигурации не содержит параметра \"period\".");
            }
            else
            {
                try
                {
                    Convert.ToUInt16(xRoot.SelectSingleNode("//settings/period").InnerText);
                }
                catch (FormatException)
                {
                    IsValidated = false;
                    throw new ConfigurationException("Неверный формат параметра \"period\".");
                }
            }

            if (xRoot.SelectSingleNode("//settings/logfilepath") == null)
            {
                IsValidated = false;
                throw new ConfigurationException("XML-документ конфигурации не содержит параметра \"logfilepath\".");
            }
            else
            {
                string _logfilePath = xRoot.SelectSingleNode("//settings/logfilepath").InnerText;
                if (!Regex.IsMatch(_logfilePath, @"^(([a-zA-Z]:|\\)\\)?(((\.)|(\.\.)|([^\\/:\*\?""\|<>\. ](([^\\/:\*\?""\|<>\. ])|([^\\/:\*\?""\|<>] *[^\\/:\*\?""\|<>\. ]))?))\\)*[^\\/:\*\?""\|<>\. ](([^\\/:\*\?""\|<>\. ])|([^\\/:\*\?""\|<>] *[^\\/:\*\?""\|<>\. ]))?$"))
                {
                    IsValidated = false;
                    throw new ConfigurationException("Неверный формат пути к файлу логирования.\n" + _logfilePath);
                }
            }
        }

        public void ReadXMLFile()
        {
            try
            {
                XmlElement xRoot = configFile.DocumentElement;
                URL = new Uri(xRoot.SelectSingleNode("//settings/URL").InnerText);
                protocol = xRoot.SelectSingleNode("//settings/protocol").InnerText;
                if (protocol == "HTTP")
                    validHTTPcode = Convert.ToUInt16(xRoot.SelectSingleNode("//settings/validHTTPcode").InnerText);
                period = Convert.ToUInt16(xRoot.SelectSingleNode("//settings/period").InnerText);
                logfilePath = xRoot.SelectSingleNode("//settings/logfilepath").InnerText;
            }
            catch (ConfigurationException ex)
            {
                Console.WriteLine(ex.Message);
            }

            
        }
    }

    internal class ConfigurationException : Exception
    {
        public ConfigurationException(string message) : base(message) { }
    }
}
