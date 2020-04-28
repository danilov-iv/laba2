using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;

namespace Lab2PhoneBook
{
    [XmlInclude(typeof(Person)), XmlInclude(typeof(Friend)), XmlInclude(typeof(Organization))]
    [Serializable]
    /// <summary>
    /// абстрактный класс, обозначающий запись в телефонном справочнике
    /// </summary>
    public abstract class PhoneBook
    {
        /// <summary>
        /// Поле имени
        /// </summary>
        public string name;
        /// <summary>
        /// Поле адреса
        /// </summary>
        public string address;
        /// <summary>
        /// Телефонный номер
        /// </summary>
        public string phone;

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public PhoneBook() { }
        /// <summary>
        /// Метод, выводящий на экран информацию о записи
        /// </summary>
        public abstract void printInfo();

        /// <summary>
        /// Проверка записи на соответствие введенному значению
        /// </summary>
        /// <param name="searching">Значение фамилии или названия организации, с которым происходит сравнение
        /// </param>
        public void Search(string searching)
        {
            Trace.WriteLine("PhoneBook.Search");
            if (name.Contains(searching)) printInfo();
        }
    }
    /// <summary>
    /// Персона, характеризуется именем, адресом и номером телефона
    /// </summary>
    public class Person : PhoneBook
    {
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public Person() { }
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="address">Адрес</param>
        /// <param name="phone">Телефон</param>
        public Person(string name, string address, string phone)
        {
            Trace.WriteLine("Person.Person");
            this.name = name;
            this.address = address;
            this.phone = phone;
        }
        /// <summary>
        /// Вывод записи
        /// </summary>
        public override void printInfo()
        {
            Trace.WriteLine("Person.PrintInfo");
            Console.WriteLine($"Имя: {name}\nАдрес: {address}\nТелефон: {phone}\n");
        }
    }
    /// <summary>
    /// Организация
    /// </summary>
    public class Organization : PhoneBook
    {
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public Organization() { }
        /// <summary>
        /// Факс
        /// </summary>
        public string fax;
        /// <summary>
        /// Контактное лицо
        /// </summary>
        public string contact;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="name">Имя/Название</param>
        /// <param name="address">Адрес</param>
        /// <param name="phone">Номер телефона</param>
        /// <param name="fax">Факс</param>
        /// <param name="contact">Контактное лицо</param>
        public Organization(string name, string address, string phone, string fax, string contact)
        {
            Trace.WriteLine("Organization.Organization");
            this.name = name;
            this.address = address;
            this.phone = phone;
            this.fax = fax;
            this.contact = contact;
        }

        public override void printInfo()
        {
            Trace.WriteLine("Organization.PrintInfo");
            Console.WriteLine($"Название: {name}\nАдрес: {address}\nТелефон: {phone}\nФакс: {fax}\nКонтактное лицо: {contact}\n");
        }
    }
    /// <summary>
    /// Друг
    /// </summary>
    public class Friend : PhoneBook
    {
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public Friend() { }
        /// <summary>
        /// Дата рождения
        /// </summary>
        public string birthDate;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="address">Адрес</param>
        /// <param name="phone">Номер телефона</param>
        /// <param name="birthDate">Дата рождения</param>
        public Friend(string name, string address, string phone, string birthDate)
        {
            Trace.WriteLine("Friend.Friend");
            this.name = name;
            this.address = address;
            this.phone = phone;
            this.birthDate = birthDate;
        }

        public override void printInfo()
        {
            Trace.WriteLine("Friend.PrintInfo");
            Console.WriteLine($"Имя: {name}\nАдрес: {address}\nТелефон: {phone}\nДата рождения: {birthDate}\n");
        }
    }
    class Program
    {
        /// <summary>
        /// Преобразование строки из файла в экземпляр класса
        /// </summary>
        /// <param name="line">Преобразуемая строка</param>
        /// <returns></returns>
        static PhoneBook parseRec(string line)
        {
            string[] element = line.Split(';');
            switch (element[0])
            {
                case "person":
                    return new Person(element[1], element[2], element[3]);
                case "organization":
                    return new Organization(element[1], element[2], element[3], element[4], element[5]);
                case "friend":
                    return new Friend(element[1], element[2], element[3], element[4]);
                default: return new Person(" ", " ", " ");
            }
        }

        /// <summary>
        /// Построковое чтение из файла
        /// </summary>
        /// <param name="fileName">Название файла для чтения</param>
        /// <returns></returns>
        static PhoneBook[] readFromFile(string fileName)
        {
            StreamReader file = new StreamReader("input.txt", Encoding.Default);
            int n = Convert.ToInt32(file.ReadLine());
            PhoneBook[] phoneBookDB = new PhoneBook[n];
            for (int i = 0; i < n; i++)
            {
                phoneBookDB[i] = parseRec(file.ReadLine());
            }
            return phoneBookDB;
        }

        static void Main(string[] args)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(PhoneBook[]));
            PhoneBook[] phoneBase;
            try
            {
                phoneBase = readFromFile("input.txt");
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Не найден исходный файл:");
                Console.WriteLine(e.FileName);
                return;
            }

            Console.WriteLine("Телефонный справочник: \n");
            foreach (PhoneBook record in phoneBase)
            {
                record.printInfo();
            }

            Console.Write("Поиск в базе по фамилии/названию: ");
            string searching = Console.ReadLine();
            foreach (PhoneBook record in phoneBase)
            {
                record.Search(searching);
            }

            using (FileStream fs = new FileStream("base.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, phoneBase);
            }

            Console.ReadKey();
        }

    }
}
