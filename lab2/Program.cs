using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lab2PhoneBook
{

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
            this.name = name;
            this.address = address;
            this.phone = phone;
        }
        /// <summary>
        /// Вывод записи
        /// </summary>
        public override void printInfo()
        {
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
            this.name = name;
            this.address = address;
            this.phone = phone;
            this.fax = fax;
            this.contact = contact;
        }

        public override void printInfo()
        {
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
            this.name = name;
            this.address = address;
            this.phone = phone;
            this.birthDate = birthDate;
        }

        public override void printInfo()
        {
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
        static PhoneBook parseNote(string line)
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
            System.IO.StreamReader file = new System.IO.StreamReader("input.txt");
            int n = Convert.ToInt32(file.ReadLine());
            PhoneBook[] phoneBookDataBase = new PhoneBook[n];
            for (int i = 0; i < n; i++)
            {
                phoneBookDataBase[i] = parseNote(file.ReadLine());
            }
            return phoneBookDataBase;
        }



        static void Main(string[] args)
        {
            PhoneBook[] phoneBase = readFromFile("input.txt");
            Console.WriteLine("Телефонный справочник: ");
            foreach (PhoneBook note in phoneBase)
            {
                note.printInfo();
            }

            Console.Write("Для поиска записи введите фамилию человека или название организации: ");
            string nameForSearch = Console.ReadLine();
            foreach (PhoneBook note in phoneBase)
            {
                note.Search(nameForSearch);
            }
            Console.ReadKey();
        }
    }
}
