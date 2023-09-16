using System;
using Newtonsoft.Json.Linq;


namespace main
{
    class Program
    {
        static void Main(string[] args)
        {


            bool is_online = true;
            var request = new GetRequest("https://www.cbr-xml-daily.ru/daily_json.js");
            request.Run();

            float[] exchange2 = new float[2];

            var response = request.Response;
            var json = JObject.Parse(response);

            while (is_online)
            {
                Console.WriteLine("Введите опцию:\n1 - конвертатор\n2 - Список доступных валют\n" +
                    "3 - выйти\n");
                string user_choice = Console.ReadLine();
                switch (user_choice)
                {
                    case "1":
                        Console.WriteLine("Введите название валюты, которую хотите разменять " +
                            "и ее количество: (необходимо вводить в соответствии с названием валюты в списке валют)");
                        Currency curToExchange = new Currency(Console.ReadLine().ToUpper());
                        if (checkIfExist(json, curToExchange.currencyName) == false)
                        {
                            continue;
                        }
                        string user_input = Console.ReadLine();
                        if (float.TryParse(user_input, out float currencyValue))
                        {
                            curToExchange = new Currency(curToExchange.currencyName, currencyValue);
                            Console.WriteLine("Введите название первой валюты");
                            Currency currency1 = new Currency(Console.ReadLine().ToUpper());
                            if (checkIfExist(json, currency1.currencyName) == false)
                            {
                                continue;
                            }
                            Console.WriteLine("Введите название второй валюты или знак '-'");
                            Currency currency2 = new Currency(Console.ReadLine().ToUpper());
                            if (checkIfExist(json, currency2.currencyName) == false)
                            {
                                currency1 = new Currency(currency1.currencyName, exchange(curToExchange, currency1, json));
                                Console.Write($"{curToExchange.value} {curToExchange.currencyName} = ");
                                currency1.showInfo();
                            }
                            else
                            {
                                exchange2 = exchange(curToExchange, currency1, currency2, json);
                                currency1 = new Currency(currency1.currencyName, exchange2[0]);
                                currency2 = new Currency(currency2.currencyName, exchange2[1]);

                                Console.WriteLine($"{curToExchange.value} {curToExchange.currencyName} =");
                                currency1.showInfo();
                                currency2.showInfo();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Введенна неправильная сумма");
                        }
                        Console.ReadKey();
                        Console.Clear();
                        break;

                    case "2":
                        showAllCurrencies(json);
                        break;

                    case "3":
                        is_online = false;
                        break;
                    default:
                        Console.WriteLine("Неверная операция!!!");
                        break;

                }
            }
        }


        static float getExchangeRate(JObject jsObject, string currencyName)
        {
            var Valute = jsObject["Valute"];
            var curName = Valute[currencyName];
            var curValue = curName["Value"];
            return (float)curValue;
        }

        static bool checkIfExist(JObject jsObject, string currencyName)
        {
            var Valute = jsObject["Valute"];
            var curName = Valute[currencyName];
            if (curName == null)
            { 
                Console.WriteLine("Введена неверная валюта");
                Console.WriteLine("Нажмите любую кнопку для продолжения...");
                Console.ReadKey();
                Console.Clear();
                return false;
            }   
            else return true;
        }

        static void showAllCurrencies(JObject jsObject)
        {
            var Valute = jsObject["Valute"];
            foreach (var item in Valute)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("Нажмите любую кнопку, чтобы продолжить");
            Console.ReadKey();
            Console.Clear();
        }

        static float exchange(Currency currencyToEx, Currency CurrencyName1, JObject Valute)
        {
            float exchangeValue;
            if (currencyToEx.currencyName == "RUB")
            {
                exchangeValue = currencyToEx.value / getExchangeRate(Valute, CurrencyName1.currencyName);
                return exchangeValue;
            }
            else if ( CurrencyName1.currencyName == "RUB")
            {
                exchangeValue = getExchangeRate(Valute, currencyToEx.currencyName) * currencyToEx.value;
                return exchangeValue;
            }
            else
            {
                exchangeValue = currencyToEx.value * getExchangeRate(Valute, currencyToEx.currencyName) 
                    / getExchangeRate(Valute, CurrencyName1.currencyName);
                return exchangeValue;
            } 
        }

        static float[] exchange(Currency currencyToEx, Currency CurrencyName1, Currency CurrencyName2, JObject Valute)
        {
            float[] exchangeValue = new float[2];
            if (currencyToEx.currencyName == "RUB")
            {
                exchangeValue[0] = currencyToEx.value / getExchangeRate(Valute, CurrencyName1.currencyName);
                exchangeValue[1] = currencyToEx.value / getExchangeRate(Valute, CurrencyName2.currencyName);
                return exchangeValue;
            }
            else if (CurrencyName1.currencyName == "RUB")
            {
                exchangeValue[0] = getExchangeRate(Valute, currencyToEx.currencyName) * currencyToEx.value;
                exchangeValue[1] = currencyToEx.value * getExchangeRate(Valute, currencyToEx.currencyName) 
                    / getExchangeRate(Valute, CurrencyName2.currencyName);
                return exchangeValue;
            }
            else if (CurrencyName2.currencyName == "RUB")
            {
                exchangeValue[0] = currencyToEx.value * getExchangeRate(Valute, currencyToEx.currencyName) 
                    / getExchangeRate(Valute, CurrencyName2.currencyName);
                exchangeValue[1] = getExchangeRate(Valute, currencyToEx.currencyName) * currencyToEx.value;
                return exchangeValue;
            }
            else
            {
                exchangeValue[0] = currencyToEx.value * getExchangeRate(Valute, currencyToEx.currencyName)
                    / getExchangeRate(Valute, CurrencyName1.currencyName);                
                exchangeValue[1] = currencyToEx.value * getExchangeRate(Valute, currencyToEx.currencyName)
                    / getExchangeRate(Valute, CurrencyName2.currencyName);
                return exchangeValue;
            }
        }
    }

    class Currency
    {
        public string currencyName { get; private set; }
        public float value { get; private set; }

        public Currency(string Name)
        {
            currencyName = Name;
        }

        public Currency(string Name, float Value)
        {
            currencyName = Name;
            value = Value;
        }

        public void showInfo()
        {
            Console.Write($"{value} {currencyName}\n");
        }

    }
       
}
